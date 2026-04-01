using MauiApp1.Models;
using MauiApp1.Services;

namespace MauiApp1.Tests.Fakes;

public class FakeBotPlayer(Queue<int> moves) : IBotPlayer
{
    public int ChooseMove(Board board) => moves.Dequeue();
}
