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
