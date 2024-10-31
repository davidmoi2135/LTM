using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum PieceColor
{
    White,
    Black
}

public enum PieceType
{
    Pawn,
    Rook,
    Knight,
    Bishop,
    Queen,
    King
}
public abstract class ChessPiece
{
    public PieceColor Color { get; private set; }
    public PieceType Type { get; private set; }

    protected ChessPiece(PieceColor color, PieceType type)
    {
        Color = color;
        Type = type;
    }

    // Phương thức để kiểm tra nước đi hợp lệ (cần được triển khai trong các lớp con)
    public abstract bool IsValidMove(int startRow, int startCol, int endRow, int endCol, ChessPiece[,] board);
}
