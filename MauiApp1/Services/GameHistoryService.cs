using MauiApp1.Models;

namespace MauiApp1.Services;

public class GameHistoryService : IGameHistoryService
{
    private readonly GameHistory _history = new();

    public Task<GameHistory> GetHistoryAsync()
    {
        return Task.FromResult(_history);
    }

    public Task RecordResultAsync(GameResult result)
    {
        switch (result)
        {
            case GameResult.PlayerWin:
                _history.PlayerWins++;
                break;
            case GameResult.BotWin:
                _history.BotWins++;
                break;
            case GameResult.Draw:
                _history.Draws++;
                break;
        }

        return Task.CompletedTask;
    }
}
