using SQLite;

namespace MauiApp1.Models;

public class GameResultEntry
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public GameResult Result { get; set; }

    public DateTime PlayedAt { get; set; }
}
