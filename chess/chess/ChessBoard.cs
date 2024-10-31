using chess.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ChessBoard
{
    public ChessPiece[,] Board { get; } = new ChessPiece[8, 8];

    public ChessBoard()
    {
        InitializeBoard();
    }
    private void InitializeBoard()
    {
        // Đặt quân tốt
        for (int i = 0; i < 8; i++)
        {
            Board[1, i] = new Pawn(PieceColor.Black);
            Board[6, i] = new Pawn(PieceColor.White);
        }

        // Đặt các quân khác
        Board[0, 0] = Board[0, 7] = new Rook(PieceColor.Black);
        Board[7, 0] = Board[7, 7] = new Rook(PieceColor.White);
        Board[0, 1] = Board[0, 6] = new Knight(PieceColor.Black);
        Board[7, 1] = Board[7, 6] = new Knight(PieceColor.White);
        Board[0, 2] = Board[0, 5] = new Bishop(PieceColor.Black);
        Board[7, 2] = Board[7, 5] = new Bishop(PieceColor.White);
        Board[0, 3] = new Queen(PieceColor.Black);
        Board[7, 3] = new Queen(PieceColor.White);
        Board[0, 4] = new King(PieceColor.Black);
        Board[7, 4] = new King(PieceColor.White);
    }
    public void Reset()
    {

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Board[i, j] = null;
            }
        }

        InitializeBoard();
    }

    public bool IsValidMove(int startX, int startY, int endX, int endY, ChessPiece[,] board)
    {
        ChessPiece piece = Board[startX, startY];
        return piece != null && piece.IsValidMove(startX, startY, endX, endY, Board);
    }

}
