using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace chess
{
    internal class TCPServer
    {
        public enum PieceColor
        {
            White,
            Black
        }

        private TcpListener tcpListener;
        private Queue<TcpClient> waitingPlayers = new Queue<TcpClient>();
        private const string connectionString = "Data Source=players.db;Version=3;";
        private List<TcpClient> players = new List<TcpClient>();
        private List<PieceColor> playerColors = new List<PieceColor>();
        private int currentPlayerIndex = 0;

        public TCPServer(int localPort)
        {
            tcpListener = new TcpListener(IPAddress.Any, localPort);
            CreateDatabase();
        }

        public async Task StartAsync()
        {
            tcpListener.Start();
            Console.WriteLine("Server started, waiting for connections...");

            while (true)
            {
                TcpClient client = await tcpListener.AcceptTcpClientAsync();
                Console.WriteLine($"Client connected: {client.Client.RemoteEndPoint}");
                _ = HandleClientAsync(client); // Xử lý mỗi client trong một task riêng biệt
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            try
            {
                while (client.Connected)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string request = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                    Console.WriteLine($"Received: {request}");
                    string response = ProcessRequest(request, client);

                    byte[] responseData = Encoding.UTF8.GetBytes(response + "\n");
                    await stream.WriteAsync(responseData, 0, responseData.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                if (client.Connected)
                {
                    client.Close();
                    RemovePlayer(client);
                }
                Console.WriteLine("Client disconnected.");
            }
        }

        private string ProcessRequest(string request, TcpClient client)
        {
            string[] parts = request.Split(' ');
            string command = parts[0].ToUpper(); // Chuyển sang chữ hoa để so sánh không phân biệt chữ hoa chữ thường

            switch (command)
            {
                case "REGISTER":
                    return parts.Length >= 3 ? RegisterPlayer(parts[1], parts[2]) : "ERROR: Invalid request format";
                case "LOGIN":
                    return parts.Length >= 3 ? LoginPlayer(parts[1], parts[2]) : "ERROR: Invalid request format";
                case "FIND_MATCH":
                    return FindMatch(client);
                case "MOVE":
                    return parts.Length >= 3 ? HandleMove(parts[1], parts[2], client) : "ERROR: Invalid move format";
                default:
                    return "ERROR: Unknown command";
            }
        }

        private string RegisterPlayer(string username, string password)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO players (username, password) VALUES (@username, @password)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    try
                    {
                        command.ExecuteNonQuery();
                        return "SUCCESS: Registered";
                    }
                    catch (SQLiteException ex)
                    {
                        // Lỗi khóa duy nhất (unique constraint)
                        if (ex.ResultCode == SQLiteErrorCode.Constraint)
                        {
                            return "ERROR: Username already exists";
                        }
                        return "ERROR: Database error: " + ex.Message;
                    }
                }
            }
        }

        private string LoginPlayer(string username, string password)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM players WHERE username = @username AND password = @password AND isOnline = 0";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    long count = (long)command.ExecuteScalar();
                    if (count > 0)
                        {
                            string updateQuery = "UPDATE players SET isOnline = 1 WHERE username = @username";
                            using (var updateCommand = new SQLiteCommand(updateQuery, connection))
                            {
                                updateCommand.Parameters.AddWithValue("@username", username);
                                updateCommand.ExecuteNonQuery();
                            }
                            return "Login successful";
                        }
                        else
                        {
                            return "Invalid username or password, or user is already online";
                        }
                }
            }
        }

        private string FindMatch(TcpClient client)
        {
            lock (waitingPlayers)
            {
                // Kiểm tra xem client đã có trong danh sách người chơi hay chưa
                if (players.Contains(client))
                {
                    return "ERROR: Already in a match";
                }

                waitingPlayers.Enqueue(client);
                if (waitingPlayers.Count >= 2)
                {
                    TcpClient player1 = waitingPlayers.Dequeue();
                    TcpClient player2 = waitingPlayers.Dequeue();
                    players.Add(player1);
                    players.Add(player2);
                    playerColors.Clear();
                    playerColors.Add(PieceColor.White);
                    playerColors.Add(PieceColor.Black);

                    SendMessage(player1, "MATCH_FOUND WHITE");
                    SendMessage(player2, "MATCH_FOUND BLACK");

                    currentPlayerIndex = 0; // Đặt người chơi đầu tiên làm người bắt đầu
                    return "SUCCESS: Match started";
                }
                else
                {
                    return "WAITING: Finding match...";
                }
            }
        }

        private string HandleMove(string from, string to, TcpClient client)
        {
            if (!IsCurrentPlayer(client))
                return "ERROR: Not your turn";

            TcpClient opponent = GetOpponent(client);
            if (opponent != null)
                SendMessage(opponent, $"MOVE {from} {to}");

            UpdateCurrentPlayer();
            return "SUCCESS: Move sent";
        }

        private bool IsCurrentPlayer(TcpClient client)
        {
            return players[currentPlayerIndex] == client;
        }

        private TcpClient GetOpponent(TcpClient client)
        {
            return players.FirstOrDefault(p => p != client);
        }

        private void UpdateCurrentPlayer()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        }

        private async void SendMessage(TcpClient client, string message)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message + "\n");
                NetworkStream stream = client.GetStream();
                await stream.WriteAsync(data, 0, data.Length);
                await stream.FlushAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
        }

        private void CreateDatabase()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS players (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        username TEXT NOT NULL UNIQUE,
                        password TEXT NOT NULL
                    );";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                AddAdminUserIfNotExists(connection);
            }
        }

        private void AddAdminUserIfNotExists(SQLiteConnection connection)
        {
            string checkQuery = "SELECT COUNT(*) FROM players WHERE username = 'admin'";
            using (var command = new SQLiteCommand(checkQuery, connection))
            {
                long count = (long)command.ExecuteScalar();
                if (count == 0)
                {
                    string insertQuery = "INSERT INTO players (username, password) VALUES ('admin', '123')";
                    using (var insertCommand = new SQLiteCommand(insertQuery, connection))
                    {
                        insertCommand.ExecuteNonQuery();
                        Console.WriteLine("Admin user added.");
                    }
                }
            }
        }

        private void RemovePlayer(TcpClient client)
        {
            lock (waitingPlayers)
            {
                if (waitingPlayers.Contains(client))
                {
                    waitingPlayers = new Queue<TcpClient>(waitingPlayers.Where(p => p != client));
                }

                players.Remove(client);
            }
        }
    }
}
