using Microsoft.AspNetCore.Components;
using TicTacToe.Shared.Constants;

namespace TicTacToe.Shared.Components;

public partial class MainComponent : ComponentBase
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;
    private void StartGame(GameType gameType)
    {
        NavigationManager.NavigateTo($"{NavigationManager.BaseUri}/game/{gameType}");
    }
}
