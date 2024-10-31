using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chess
{
    public partial class MatchGame : Form
    {
        TCPClient client;
        public MatchGame(TCPClient client)
        {
            InitializeComponent();
            this.client = client;
        }

        private async void btn_find_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Finding match...";

            // Gửi yêu cầu tìm trận đến server
            string matchResponse = await client.FindMatchAsync();

            if (matchResponse.StartsWith("WAITING"))
            {
                lblStatus.Text = "Waiting for another player...";
                await ListenForMatchAsync();
                // await ListenForMatchAsync(); // Bắt đầu lắng nghe phản hồi về trận đấu
            }
            else if (matchResponse.StartsWith("MATCH_FOUND"))
            {
                lblStatus.Text = "Tim thay tran";
                OpenChessBoard(matchResponse);
            }
            else
            {
                lblStatus.Text = matchResponse; // Hiển thị lỗi nếu có
            }
        }
        private async Task ListenForMatchAsync()
        {
            while (true)
            {
                // Đọc phản hồi từ server về việc tìm trận
                string response = await client.FindMatchAsync();

                if (response.StartsWith("MATCH_FOUND"))
                {
                    OpenChessBoard(response);
                    break;
                }
                else
                {
                    lblStatus.Text = response;
                }
            }
        }
        private void OpenChessBoard(string matchResponse)
        {
            // Kiểm tra xem phản hồi có hợp lệ không
            if (string.IsNullOrEmpty(matchResponse) || !matchResponse.StartsWith("MATCH_FOUND"))
            {
                MessageBox.Show("Error: Invalid match response.");
                return;
            }

            // Lấy màu sắc của người chơi
            string[] parts = matchResponse.Split(' ');

            if (parts.Length < 2)
            {
                MessageBox.Show("Error: Match response format is invalid.");
                return;
            }

            string playerColor = parts[1]; // "WHITE" hoặc "BLACK"
            MessageBox.Show($"Match found! You are playing as {playerColor}");

            // Mở form bàn cờ
            ChessBoardForm chessBoard = new ChessBoardForm(client, playerColor);
            this.Hide(); // Ẩn form tìm trận
            chessBoard.ShowDialog();
            this.Show();
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            // Ngắt kết nối khi form đóng
            client.Close();
        }
    }
}
