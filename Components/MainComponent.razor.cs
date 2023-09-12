using Microsoft.AspNetCore.Components;
using TicTacToe.Constants;

namespace TicTacToe.Components;

public partial class MainComponent : ComponentBase
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;
    private void StartGame(GameType gameType)
    {
        NavigationManager.NavigateTo($"/game/{gameType}");
    }
}
