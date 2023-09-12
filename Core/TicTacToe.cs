using TicTacToe.Constants;

namespace TicTacToe.Core;

public class Cell
{
    public Player Player { get; set; }
    public Cell()
    {
        Player = Player.Undefined;
    }
}
public class TicTacToeBoard
{
    private readonly Cell[,] _cells;
    public TicTacToeBoard()
    {
        _cells = new Cell[3, 3];
        InitializeBoard();
    }
    private void InitializeBoard()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                _cells[row, col] = new Cell();
            }
        }
    }

    public Player GetCellPlayer(int row, int cel)
        => _cells[row, cel].Player;
}
