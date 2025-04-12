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
        _winningMoves = [];
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
        Player player = _cells[row, col].Player;
        return player is Player.Undefined ? null : player;
    }

    public bool IsWinnerCell(int row, int col)
        => _winningMoves.Contains((row, col));

    public bool IsGameOver()
    {
        Player? winningPlayer = GetWinningPlayer();

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

        if ((row, col) == (null, null))
        {
            (row, col) = FindBestMove(player);
        }

        return (row!.Value, col!.Value);
    }

    private (int row, int col) FindBestMove(Player player)
    {
        int bestScore = int.MinValue;
        (int row, int col) bestMove = (-1, -1);

        // Iterate through all cells to find the best move
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                // Check if the cell is empty
                if (_cells[row, col].Player == Player.Undefined)
                {
                    // Make the move
                    _cells[row, col].Player = player;
                    // Call Minimax recursively and choose the maximum value
                    int score = Minimax(_cells, 0, false, player);
                    // Undo the move
                    _cells[row, col].Player = Player.Undefined;

                    // Update the best score and best move if the current move is better
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = (row, col);
                    }
                }
            }
        }

        return bestMove;
    }

    private int Minimax(Cell[,] board, int depth, bool isMaximizing, Player player)
    {
        // Check for a winner and return a score
        Player? winner = GetWinningPlayer();
        if (winner == player) return 10 - depth; // AI wins
        if (winner == (player == Player.X ? Player.O : Player.X)) return depth - 10; // Opponent wins
        if (CheckTie()) return 0; // Tie

        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            // Iterate through all cells
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    // Check if the cell is empty
                    if (board[row, col].Player == Player.Undefined)
                    {
                        // Make the move
                        board[row, col].Player = player;
                        // Call Minimax recursively and choose the maximum value
                        int score = Minimax(board, depth + 1, false, player);
                        // Undo the move
                        board[row, col].Player = Player.Undefined;
                        // Update the best score
                        bestScore = Math.Max(score, bestScore);
                    }
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            // Iterate through all cells
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    // Check if the cell is empty
                    if (board[row, col].Player == Player.Undefined)
                    {
                        // Make the move
                        board[row, col].Player = player == Player.X ? Player.O : Player.X;
                        // Call Minimax recursively and choose the minimum value
                        int score = Minimax(board, depth + 1, true, player);
                        // Undo the move
                        board[row, col].Player = Player.Undefined;
                        // Update the best score
                        bestScore = Math.Min(score, bestScore);
                    }
                }
            }
            return bestScore;
        }
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
