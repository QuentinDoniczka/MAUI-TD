using MauiApp1.Models;
using SQLite;

namespace MauiApp1.Services;

public class SqliteGameHistoryService : IGameHistoryService
{
    private readonly SQLiteAsyncConnection _db;

    public SqliteGameHistoryService(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);
        _db.CreateTableAsync<GameResultEntry>().Wait();
    }

    public async Task<GameHistory> GetHistoryAsync()
    {
        var entries = await _db.Table<GameResultEntry>().ToListAsync();

        return new GameHistory
        {
            PlayerWins = entries.Count(e => e.Result == GameResult.PlayerWin),
            BotWins = entries.Count(e => e.Result == GameResult.BotWin),
            Draws = entries.Count(e => e.Result == GameResult.Draw)
        };
    }

    public async Task RecordResultAsync(GameResult result)
    {
        var entry = new GameResultEntry
        {
            Result = result,
            PlayedAt = DateTime.UtcNow
        };

        await _db.InsertAsync(entry);
    }
}
