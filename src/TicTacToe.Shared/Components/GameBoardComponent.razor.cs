using Microsoft.AspNetCore.Components;
using TicTacToe.Shared.Constants;
using TicTacToe.Shared.Core;

namespace TicTacToe.Shared.Components;

public partial class GameBoardComponent : ComponentBase
{
    [Parameter]
    public GameType GameType { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;
    
    private TicTacToeBoard _board = new TicTacToeBoard();
    private Player _currentTurn = Player.Undefined;

    protected override void OnInitialized()
    {
        _currentTurn = Player.X;
    }
    
    private void GoBack()
    {
        NavigationManager.NavigateTo($"{NavigationManager.BaseUri}/");
    }

    private void BoxClicked(int row, int col)
    {
        if (_board.IsGameOver() || _board.GetCellPlayer(row, col) is Player.O or Player.X)
        {
            return;
        }

        _board.SetCell(row, col, _currentTurn);
        if (_board.IsGameOver())
        {
            return;
        }

        NextTurn();
    }
    private void NextTurn()
    {
        switch (GameType)
        {
            case GameType.AI:
                _currentTurn = _currentTurn == Player.X ? Player.O : Player.X;
                var (row, col) = _board.GetAiNextMove(_currentTurn);
                _board.SetCell(row, col, _currentTurn);
                _currentTurn = _currentTurn == Player.X ? Player.O : Player.X;
                break;
            case GameType.TwoPlayer:
                _currentTurn = _currentTurn == Player.X ? Player.O : Player.X;
                break;
        }
    }

    private string IsWinnerCell(int row, int col)
    {
        return _board.IsGameOver() && _board.IsWinnerCell(row, col) ? "winning-cell" : "";
    }
    private void RestartGame()
    {
        _board.Clear();
        _currentTurn = Player.X;
    }
}
