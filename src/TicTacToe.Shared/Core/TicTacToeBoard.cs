using System.Security.Cryptography;
using TicTacToe.Shared.Constants;

namespace TicTacToe.Shared.Core;

public class TicTacToeBoard
{
    private readonly Cell[,] _cells;
    private List<(int Row, int Col)> _winningMoves = default!;

    public TicTacToeBoard()
    {
        _cells = new Cell[3, 3];
        InitializeBoard();
    }
    public string? Winner { get; private set; }

    private void InitializeBoard()
    {
        Winner = null;
        _winningMoves = new List<(int Row, int Col)>();
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                _cells[row, col] = new Cell();
            }
        }
    }

    public Player? GetCellPlayer(int row, int col)
    {
        var player = _cells[row, col].Player;
        return player is Player.Undefined ? null : player;
    }

    public bool IsWinnerCell(int row, int col)
        => _winningMoves.Contains((row, col));

    public bool IsGameOver()
    {
        var winningPlayer = GetWinningPlayer();

        if (winningPlayer is not null)
        {
            Winner = winningPlayer.ToString();
            return true;
        }
        if (CheckTie())
        {
            Winner = "Tie";
            return true;
        }

        return false;
    }

    private Player? GetWinningPlayer()
    {
        if (CheckWinner(Player.X))
        {
            return Player.X;
        }

        if (CheckWinner(Player.O))
        {
            return Player.O;
        }

        return null;
    }
    public void SetCell(int row, int col, Player player)
        => _cells[row, col].Player = player;

    private bool CheckTie()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (_cells[row, col].Player == Player.Undefined)
                {
                    return false;
                }
            }
        }
        _winningMoves.Clear();
        return true;
    }

    public (int row, int col) GetAiNextMove(Player player)
    {
        (int? row, int? col) = GetWinningMove(player);
        if ((row, col) != (null, null)) return (row!.Value, col!.Value);

        (row, col) = GetWinningMove(player == Player.O ? Player.X : Player.O);
        if ((row, col) == (null, null))
        {
            (row, col) = RandomTurn();
        }

        return (row!.Value, col!.Value);
    }
    private (int row, int col) RandomTurn()
    {
        int row = RandomNumberGenerator.GetInt32(0, 3);
        int col = RandomNumberGenerator.GetInt32(0, 3);
        return _cells[row, col].Player == Player.Undefined ? (row, col) : RandomTurn();
    }

    private (int? row, int? col) GetWinningMove(Player player)
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (_cells[row, col].Player == Player.Undefined && IsWinningMove(player, row, col))
                {
                    return (row, col);
                }
            }
        }

        return (null, null);
    }
    private bool IsWinningMove(Player player, int row, int col)
    {
        _cells[row, col].Player = player;
        bool isWinningMove = CheckWinner(player);
        _cells[row, col].Player = Player.Undefined; // Reset the move regardless of win or not
        return isWinningMove;
    }

    private bool CheckWinner(Player player)
    {
        return CheckRows(player) || CheckColumns(player) || CheckDiagonals(player);
    }

    private bool CheckRows(Player player)
    {
        for (int row = 0; row < 3; row++)
        {
            if (_cells[row, 0].Player == player && _cells[row, 1].Player == player && _cells[row, 2].Player == player)
            {
                _winningMoves.Clear();
                _winningMoves.Add((row, 0));
                _winningMoves.Add((row, 1));
                _winningMoves.Add((row, 2));
                return true;
            }
        }
        return false;
    }

    private bool CheckColumns(Player player)
    {
        for (int col = 0; col < 3; col++)
        {
            if (_cells[0, col].Player == player && _cells[1, col].Player == player && _cells[2, col].Player == player)
            {
                _winningMoves.Clear();
                _winningMoves.Add((0, col));
                _winningMoves.Add((1, col));
                _winningMoves.Add((2, col));

                return true;
            }
        }
        return false;
    }

    private bool CheckDiagonals(Player player)
    {

        if (_cells[0, 0].Player == player && _cells[1, 1].Player == player && _cells[2, 2].Player == player)
        {
            _winningMoves.Clear();
            _winningMoves.Add((0, 0));
            _winningMoves.Add((1, 1));
            _winningMoves.Add((2, 2));
            return true;
        }
        if (_cells[0, 2].Player == player && _cells[1, 1].Player == player && _cells[2, 0].Player == player)
        {
            _winningMoves.Clear();
            _winningMoves.Add((0, 2));
            _winningMoves.Add((1, 1));
            _winningMoves.Add((2, 0));
            return true;
        }

        return false;

    }
    public void Clear()
    {
        InitializeBoard();
    }
}
