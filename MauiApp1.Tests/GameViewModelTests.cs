using MauiApp1.Services;
using MauiApp1.Tests.Fakes;
using MauiApp1.ViewModels;
using Xunit;

namespace MauiApp1.Tests;

public class GameViewModelTests
{
    private static GameViewModel CreateGameViewModel(params int[] botMoves)
    {
        var fakeBotPlayer = new FakeBotPlayer(new Queue<int>(botMoves));
        var gameHistoryService = new GameHistoryService();
        return new GameViewModel(fakeBotPlayer, gameHistoryService);
    }

    [Fact]
    public void PlayerWins()
    {
        var gameViewModel = CreateGameViewModel(3, 4);

        gameViewModel.PlayCellCommand.Execute("0");
        gameViewModel.PlayCellCommand.Execute("1");
        gameViewModel.PlayCellCommand.Execute("2");

        Assert.True(gameViewModel.IsGameOver);
        Assert.Equal("X gagne !", gameViewModel.StatusText);
    }

    [Fact]
    public void BotWins()
    {
        var gameViewModel = CreateGameViewModel(3, 4, 5);

        gameViewModel.PlayCellCommand.Execute("0");
        gameViewModel.PlayCellCommand.Execute("1");
        gameViewModel.PlayCellCommand.Execute("6");

        Assert.True(gameViewModel.IsGameOver);
        Assert.Equal("O gagne !", gameViewModel.StatusText);
    }

    [Fact]
    public void Draw()
    {
        var gameViewModel = CreateGameViewModel(1, 3, 6, 8);

        gameViewModel.PlayCellCommand.Execute("0");
        gameViewModel.PlayCellCommand.Execute("2");
        gameViewModel.PlayCellCommand.Execute("4");
        gameViewModel.PlayCellCommand.Execute("5");
        gameViewModel.PlayCellCommand.Execute("7");

        Assert.True(gameViewModel.IsGameOver);
        Assert.Equal("Match nul !", gameViewModel.StatusText);
    }

    [Fact]
    public void PlayOnOccupiedCellOrAfterGameOver_IsIgnored()
    {
        var gameViewModel = CreateGameViewModel(3, 4);

        gameViewModel.PlayCellCommand.Execute("0");
        gameViewModel.PlayCellCommand.Execute("0");
        Assert.Equal("X", gameViewModel.Cells[0]);
        Assert.False(gameViewModel.IsGameOver);

        gameViewModel.PlayCellCommand.Execute("1");
        gameViewModel.PlayCellCommand.Execute("2");
        gameViewModel.PlayCellCommand.Execute("5");
        Assert.Equal(string.Empty, gameViewModel.Cells[5]);
    }

    [Fact]
    public void History_UpdatesAfterGames()
    {
        var gameViewModel = CreateGameViewModel(3, 4);

        gameViewModel.PlayCellCommand.Execute("0");
        gameViewModel.PlayCellCommand.Execute("1");
        gameViewModel.PlayCellCommand.Execute("2");

        Assert.Equal("Joueur : 1 | Bot : 0 | Nul : 0", gameViewModel.HistoryText);
    }

    [Fact]
    public void Reset_ClearsGameState()
    {
        var fakeBotPlayer = new FakeBotPlayer(new Queue<int>([3, 4, 5]));
        var gameHistoryService = new GameHistoryService();
        var gameViewModel = new GameViewModel(fakeBotPlayer, gameHistoryService);

        gameViewModel.PlayCellCommand.Execute("0");
        gameViewModel.PlayCellCommand.Execute("1");
        gameViewModel.PlayCellCommand.Execute("2");

        gameViewModel.ResetCommand.Execute(null);

        Assert.False(gameViewModel.IsGameOver);
        Assert.Contains("Tour de", gameViewModel.StatusText);
    }
}
