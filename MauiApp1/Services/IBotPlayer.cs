using MauiApp1.Models;

namespace MauiApp1.Services;

public interface IBotPlayer
{
    int ChooseMove(Board board);
}
