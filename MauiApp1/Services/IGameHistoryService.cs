using MauiApp1.Models;

namespace MauiApp1.Services;

public interface IGameHistoryService
{
    Task<GameHistory> GetHistoryAsync();
    Task RecordResultAsync(GameResult result);
}
