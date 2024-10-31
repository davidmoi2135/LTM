using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chess
{
    public class TCPClient
    {
        private TcpClient tcpClient;
        private NetworkStream stream;

        public TCPClient(string serverIP, int serverPort)
        {
            tcpClient = new TcpClient();
            ConnectToServer(serverIP, serverPort).Wait();
        }

        private async Task ConnectToServer(string serverIP, int serverPort)
        {
            try
            {
                await tcpClient.ConnectAsync(IPAddress.Parse(serverIP), serverPort);
                stream = tcpClient.GetStream();
                Console.WriteLine("Connected to the server.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
            }
        }

        public async Task<string> RegisterAsync(string username, string password)
        {
            string request = $"REGISTER {username} {password}";
            return await SendRequestAsync(request);
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            string request = $"LOGIN {username} {password}";
            return await SendRequestAsync(request);
        }

        public async Task<string> FindMatchAsync()
        {
            string response = await SendRequestAsync("FIND_MATCH");

            if (response.StartsWith("WAITING"))
            {
                Console.WriteLine("Waiting for a match...");
                await ListenForMatchFoundAsync(); // Lắng nghe thông báo tìm thấy trận đấu
            }

            return response;
        }

        public async Task<string> SendMoveAsync(string from, string to)
        {
            string request = $"MOVE {from} {to}";
            return await SendRequestAsync(request);
        }

        private async Task<string> SendRequestAsync(string request)
        {
            try
            {
                if (!tcpClient.Connected)
                {
                    throw new InvalidOperationException("Client is not connected to the server.");
                }

                byte[] data = Encoding.UTF8.GetBytes(request + "\n");
                await stream.WriteAsync(data, 0, data.Length);
                await stream.FlushAsync();

                return await ReceiveResponseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending request: {ex.Message}");
                return "ERROR";
            }
        }

        public async Task<string> ReceiveResponseAsync()
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error receiving response: {ex.Message}");
                return "ERROR";
            }
        }

        public async Task ListenForMatchFoundAsync()
        {
            try
            {
                while (true)
                {
                    string message = await ReceiveResponseAsync();
                    if (message.StartsWith("MATCH_FOUND"))
                    {
                        bool isWhite = message.Contains("WHITE");
                        // Mở form bàn cờ trên UI thread
                        OpenChessBoardForm(isWhite);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error receiving match found: {ex.Message}");
            }
        }

        private void OpenChessBoardForm(bool isWhite)
        {
            if (Application.OpenForms.Count > 0)
            {
                Application.OpenForms[0].Invoke(new Action(() =>
                {
                    ChessBoardForm boardForm = new ChessBoardForm(this, isWhite.ToString());
                    boardForm.Show();
                }));
            }
            else
            {
                Console.WriteLine("Error: No open form to invoke UI changes.");
            }
        }

        public void Close()
        {
            stream?.Close();
            tcpClient?.Close();
            Console.WriteLine("Disconnected from server.");
        }
    }
}