using Microsoft.AspNetCore.Components;
using System.Security.Cryptography;

namespace TicTacToe.Components;

public partial class GameBoardComponent : ComponentBase
{
    private List<string> _events = new List<string>() { $"Started @ {DateTime.Now}"};
    private string?[,] _board = new string[3, 3];
    private string? _winner = null;
    private bool _gameOver = false;

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

        return true;
    }
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
                return true;
            }
        }
        return false;
    }
    
    private bool CheckDiagonals(string player)
    {
        return (_board[0, 0] == player && _board[1, 1] == player && _board[2, 2] == player) ||
               (_board[0, 2] == player && _board[1, 1] == player && _board[2, 0] == player);

    }

}
