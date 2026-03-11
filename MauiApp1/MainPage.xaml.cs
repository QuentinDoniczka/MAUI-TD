using MauiApp1.Models;

namespace MauiApp1;

public partial class MainPage : ContentPage
{
    private readonly Board _board = new();
    private readonly List<Button> _buttons;
    private CellValue _currentPlayer = CellValue.X;
    private bool _isGameOver;

    public MainPage()
    {
        InitializeComponent();
        _buttons = BoardGrid.Children.Cast<Button>().ToList();
    }

    private void OnCellClicked(object? sender, EventArgs e)
    {
        if (_isGameOver) return;
        if (sender is not Button button) return;

        var index = _buttons.IndexOf(button);

        if (!_board.Play(index, _currentPlayer))
            return;

        button.Text = _currentPlayer == CellValue.X ? "X" : "O";

        var winner = _board.CheckWinner();
        if (winner != CellValue.Empty)
        {
            StatusLabel.Text = $"{winner} gagne !";
            _isGameOver = true;
            return;
        }

        if (_board.IsFull)
        {
            StatusLabel.Text = "Match nul !";
            _isGameOver = true;
            return;
        }

        _currentPlayer = _currentPlayer == CellValue.X ? CellValue.O : CellValue.X;
        StatusLabel.Text = $"Tour de {_currentPlayer}";
    }

    private void OnResetClicked(object? sender, EventArgs e)
    {
        _board.Reset();
        _currentPlayer = CellValue.X;
        _isGameOver = false;
        StatusLabel.Text = "Tour de X";

        foreach (var btn in _buttons)
            btn.Text = string.Empty;
    }
}
