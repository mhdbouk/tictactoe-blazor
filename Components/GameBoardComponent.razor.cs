using Microsoft.AspNetCore.Components;
using System.Security.Cryptography;
using TicTacToe.Constants;

namespace TicTacToe.Components;

public partial class GameBoardComponent : ComponentBase
{
    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;
    [Parameter]
    public GameType GameType { get; set; }

    private List<string> _events = new List<string>() { $"Started @ {DateTime.Now}"};
    private string?[,] _board = new string[3, 3];
    private List<(int Row, int Col)> _winningMoves = new List<(int Row, int Col)>();
    private string? _winner = null;
    private bool _gameOver = false;

    private void GoBack()
    {
        NavigationManager.NavigateTo("/");
    }
    
    private string IsWinnerCell(int row, int col)
    {
        if (!_gameOver)
        {
            return string.Empty;
        }

        if (CheckWinner(row, col))
        {
            return "winning-cell";
        }

        return string.Empty;
    }
    
    private void BoxClicked(int row, int col)
    {
        if (_board[row, col]?.Length > 0)
        {
            _events.Add($"Box already clicked at {row},{col} with Value {_board[row, col]}");
            return;
        }

        if (_gameOver)
        {
            return;
        }
        
        _board[row, col] = "X";
        _events.Add($"Box clicked at {row},{col}");
        if (IsGameOver())
        {
            
            return;
        }
        
        NextTurn();
    }

    private bool IsGameOver()
    {
        var winningPlayer = GetWinningPlayer();

        if (!string.IsNullOrEmpty(winningPlayer))
        {
            _winner = $"{winningPlayer} Wins!";
            _events.Add(_winner);
            _gameOver = true;
            return true;
        }
        if (CheckTie())
        {
            _winner = "Tie!";
            _events.Add(_winner);
            _gameOver = true;
            return true;
        }

        return false;
    }
    
    private string? GetWinningPlayer()
    {
        if (CheckWinner("X"))
        {
            return "X";
        }

        if (CheckWinner("O"))
        {
            return "O";
        }

        return null;
    }
    
    private void NextTurn()
    {
        if (IsGameOver())
        {
            return;
        }

        var (row, col) = GetWinningMove("O");
        if ((row, col) == (null, null))
        {
            (row, col) = GetWinningMove("X");
            if ((row, col) == (null, null))
            {
                (row, col) = RandomTurn();
            }
        }
        
        _events.Add($"AI placed O at {row},{col}");
        _board[row!.Value, col!.Value] = "O";

        IsGameOver();
    }
    
    private (int? row, int? col) GetWinningMove(string player)
    {
        for (var row = 0; row < 3; row++)
        {
            for (var col = 0; col < 3; col++)
            {
                if (_board[row, col] == null && IsWinningMove(player, row, col))
                {
                    return (row, col);
                }
            }
        }

        return (null, null);
    }

    private bool IsWinningMove(string player, int row, int col)
    {
        _board[row, col] = player;
        var isWinningMove = CheckWinner(player);
        _board[row, col] = null; // Reset the move regardless of win or not
        return isWinningMove;
    }

    private (int row, int col) RandomTurn()
    {
        var row = RandomNumberGenerator.GetInt32(0, 3);
        var col = RandomNumberGenerator.GetInt32(0, 3);
        return _board[row, col] == null ? (row, col) : RandomTurn();
    }

    private bool CheckTie()
    {
        for (var row = 0; row < 3; row++)
        {
            for (var col = 0; col < 3; col++)
            {
                if (_board[row, col] == null)
                {
                    return false;
                }
            }
        }
        _winningMoves.Clear();
        return true;
    }

    private bool CheckWinner(int row, int col)
        => _winningMoves.Contains((row, col));

    private bool CheckWinner(string player)
    {
        return CheckRows(player) || CheckColumns(player) || CheckDiagonals(player);
    }
    
    private bool CheckRows(string player)
    {
        for (var row = 0; row < 3; row++)
        {
            if (_board[row, 0] == player && _board[row, 1] == player && _board[row, 2] == player)
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
    
    private bool CheckColumns(string player)
    {
        for (var col = 0; col < 3; col++)
        {
            if (_board[0, col] == player && _board[1, col] == player && _board[2, col] == player)
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
    
    private bool CheckDiagonals(string player)
    {

        if (_board[0, 0] == player && _board[1, 1] == player && _board[2, 2] == player)
        {
            _winningMoves.Clear();
            _winningMoves.Add((0, 0));
            _winningMoves.Add((1, 1));
            _winningMoves.Add((2, 2));
            return true;
        }
        if (_board[0, 2] == player && _board[1, 1] == player && _board[2, 0] == player)
        {
            _winningMoves.Clear();
            _winningMoves.Add((0, 2));
            _winningMoves.Add((1, 1));
            _winningMoves.Add((2, 0));
            return true;
        }

        return false;

    }

}
