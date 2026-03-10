using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Models;

namespace MauiApp1.ViewModels;

public partial class GameViewModel : ObservableObject
{
    private readonly Board _board = new();

    [ObservableProperty]
    private string _statusText = "Tour de X";

    [ObservableProperty]
    private CellValue _currentPlayer = CellValue.X;

    [ObservableProperty]
    private bool _isGameOver;
    public string? Play(int index)
    {
        if (IsGameOver)
            return null;

        if (!_board.Play(index, CurrentPlayer))
            return null;

        var symbol = CurrentPlayer == CellValue.X ? "X" : "O";

        var winner = _board.CheckWinner();
        if (winner != CellValue.Empty)
        {
            StatusText = $"{winner} gagne !";
            IsGameOver = true;
            return symbol;
        }

        if (_board.IsFull)
        {
            StatusText = "Match nul !";
            IsGameOver = true;
            return symbol;
        }

        CurrentPlayer = CurrentPlayer == CellValue.X ? CellValue.O : CellValue.X;
        StatusText = $"Tour de {CurrentPlayer}";
        return symbol;
    }

    [RelayCommand]
    private void Reset()
    {
        _board.Reset();
        CurrentPlayer = CellValue.X;
        StatusText = "Tour de X";
        IsGameOver = false;
    }
}
