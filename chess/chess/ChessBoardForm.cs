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
    public partial class ChessBoardForm : Form
    {
        private const int BoardSize = 8;
        private Button[,] boardButtons = new Button[BoardSize, BoardSize];
        private TCPClient client;
        private string playerColor;
        private string selectedFromSquare = null;
        
        public ChessBoardForm(TCPClient client, string playerColor)
        {
            InitializeComponent();
            this.client = client;
            this.playerColor = playerColor;
            lblPlayerColor.Text = $"You are playing as: {playerColor}";

            InitializeChessBoard();
        }
        private void InitializeChessBoard()
        {
            int buttonSize = 50; // Kích thước mỗi ô cờ
            this.ClientSize = new Size(BoardSize * buttonSize, BoardSize * buttonSize + 50);
            lblPlayerColor = new Label
            {
                Text = $"You are playing as: {playerColor}",
                Location = new Point(10, BoardSize * buttonSize + 10),
                AutoSize = true
            };
            this.Controls.Add(lblPlayerColor);

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    Button squareButton = new Button
                    {
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(col * buttonSize, row * buttonSize),
                        BackColor = (row + col) % 2 == 0 ? Color.White : Color.Gray,
                        Tag = $"{(char)('a' + col)}{BoardSize - row}" // Tag chứa vị trí ô, vd: "a8", "e2"
                    };

                    squareButton.Click += SquareButton_Click;
                    boardButtons[row, col] = squareButton;
                    this.Controls.Add(squareButton);
                }
            }
        }
        private async void SquareButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            string clickedSquare = clickedButton.Tag.ToString();

            if (selectedFromSquare == null)
            {
                // Chọn ô xuất phát
                selectedFromSquare = clickedSquare;
                clickedButton.BackColor = Color.Yellow; // Đánh dấu ô đã chọn
            }
            else
            {
                // Chọn ô đích và gửi nước đi
                string from = selectedFromSquare;
                string to = clickedSquare;

                string moveResponse = await client.SendMoveAsync(from, to);

                if (moveResponse.StartsWith("SUCCESS"))
                {
                    lblPlayerColor.Text = $"Move sent: {from} to {to}";
                    UpdateChessBoard(from, to); // Cập nhật giao diện bàn cờ
                }
                else
                {
                    MessageBox.Show(moveResponse); // Hiển thị lỗi nếu có
                }

                // Đặt lại ô xuất phát
                ResetBoardHighlights();
                selectedFromSquare = null;
            }
        }

        private void UpdateChessBoard(string from, string to)
        {
            // Tìm ô bắt đầu và ô kết thúc
            var (fromRow, fromCol) = ConvertSquareToIndices(from);
            var (toRow, toCol) = ConvertSquareToIndices(to);

            // Di chuyển quân cờ từ ô 'from' sang ô 'to'
            boardButtons[toRow, toCol].Text = boardButtons[fromRow, fromCol].Text;
            boardButtons[fromRow, fromCol].Text = "";
        }

        private (int row, int col) ConvertSquareToIndices(string square)
        {
            int col = square[0] - 'a';
            int row = BoardSize - int.Parse(square[1].ToString());
            return (row, col);
        }

        private void ResetBoardHighlights()
        {
            // Đặt lại màu nền của các ô
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    boardButtons[row, col].BackColor = (row + col) % 2 == 0 ? Color.White : Color.Gray;
                }
            }
        }

        private void ChessBoardForm_Load(object sender, EventArgs e)
        {

        }
    }
}
