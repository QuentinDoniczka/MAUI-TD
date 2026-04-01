using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Models;
using MauiApp1.Services;

namespace MauiApp1.ViewModels;

public partial class GameViewModel : ObservableObject
{
    private readonly Board _board = new();
    private readonly IBotPlayer _botPlayer;
    private readonly IGameHistoryService _historyService;
    private readonly Random _random = new();

    [ObservableProperty]
    private string _statusText = "Tour de X";

    [ObservableProperty]
    private CellValue _currentPlayer = CellValue.X;

    [ObservableProperty]
    private bool _isGameOver;

    [ObservableProperty]
    private string _historyText = "Joueur : 0 | Bot : 0 | Nul : 0";

    public ObservableCollection<string> Cells { get; } = new(Enumerable.Repeat(string.Empty, 9));

    private CellValue _botSymbol = CellValue.O;

    public GameViewModel(IBotPlayer botPlayer, IGameHistoryService historyService)
    {
        _botPlayer = botPlayer;
        _historyService = historyService;
        _ = LoadHistoryAsync();
    }

    [RelayCommand]
    private void PlayCell(string indexStr)
    {
        if (CurrentPlayer == _botSymbol) return;

        var index = int.Parse(indexStr);
        if (Play(index) != null)
            PlayBotIfNeeded();
    }

    private string? Play(int index)
    {
        if (IsGameOver)
            return null;

        if (!_board.Play(index, CurrentPlayer))
            return null;

        var symbol = CurrentPlayer == CellValue.X ? "X" : "O";
        Cells[index] = symbol;

        var winner = _board.CheckWinner();
        if (winner != CellValue.Empty)
        {
            StatusText = $"{winner} gagne !";
            IsGameOver = true;
            var result = winner == _botSymbol ? GameResult.BotWin : GameResult.PlayerWin;
            _ = RecordAndUpdateHistoryAsync(result);
            return symbol;
        }

        if (_board.IsFull)
        {
            StatusText = "Match nul !";
            IsGameOver = true;
            _ = RecordAndUpdateHistoryAsync(GameResult.Draw);
            return symbol;
        }

        CurrentPlayer = CurrentPlayer == CellValue.X ? CellValue.O : CellValue.X;
        StatusText = $"Tour de {CurrentPlayer}";
        return symbol;
    }

    private void PlayBotIfNeeded()
    {
        if (IsGameOver || CurrentPlayer != _botSymbol) return;

        var moveIndex = _botPlayer.ChooseMove(_board);
        Play(moveIndex);
    }

    [RelayCommand]
    private void Reset()
    {
        _board.Reset();
        for (var i = 0; i < 9; i++)
            Cells[i] = string.Empty;
        CurrentPlayer = CellValue.X;
        _botSymbol = _random.Next(2) == 0 ? CellValue.X : CellValue.O;
        StatusText = "Tour de X";
        IsGameOver = false;
        PlayBotIfNeeded();
    }

    private async Task LoadHistoryAsync()
    {
        var history = await _historyService.GetHistoryAsync();
        HistoryText = FormatHistory(history);
    }

    private async Task RecordAndUpdateHistoryAsync(GameResult result)
    {
        await _historyService.RecordResultAsync(result);
        await LoadHistoryAsync();
    }

    private static string FormatHistory(GameHistory history)
    {
        return $"Joueur : {history.PlayerWins} | Bot : {history.BotWins} | Nul : {history.Draws}";
    }
}
