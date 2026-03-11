using MauiApp1.ViewModels;

namespace MauiApp1;

public partial class MainPage : ContentPage
{
    private readonly GameViewModel _viewModel;
    private readonly List<Button> _buttons = [];

    public MainPage(GameViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;

        _buttons = BoardGrid.Children.Cast<Button>().ToList();

        _viewModel.GameReset += OnGameReset;
        _viewModel.ResetCommand.Execute(null);
    }

    private void OnCellClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button) return;
        if (_viewModel.IsBotTurn) return;

        var index = _buttons.IndexOf(button);
        var symbol = _viewModel.Play(index);

        if (symbol != null)
        {
            button.Text = symbol;
            TryPlayBot();
        }
    }

    private void TryPlayBot()
    {
        var result = _viewModel.PlayBot();
        if (result == null) return;

        var (index, symbol) = result.Value;
        _buttons[index].Text = symbol;
    }

    private void OnGameReset()
    {
        foreach (var btn in _buttons)
            btn.Text = string.Empty;

        TryPlayBot();
    }
}
