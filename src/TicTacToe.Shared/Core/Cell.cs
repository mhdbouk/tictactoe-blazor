using TicTacToe.Shared.Constants;

namespace TicTacToe.Shared.Core;

public class Cell
{
    public Player Player { get; set; }
    public Cell()
    {
        Player = Player.Undefined;
    }
}
