using MauiApp1.Models;

namespace MauiApp1.Services;

public class BotPlayer : IBotPlayer
{
    private readonly Random _random = new();

    public int ChooseMove(Board board)
    {
        var emptyCells = new List<int>();
        for (var i = 0; i < 9; i++)
        {
            if (board.GetCell(i) == CellValue.Empty)
                emptyCells.Add(i);
        }

        return emptyCells[_random.Next(emptyCells.Count)];
    }
}
