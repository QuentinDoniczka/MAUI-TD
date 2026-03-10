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

        _viewModel.ResetCommand.Execute(null);
        ResetButtons();
    }

    private void OnCellClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button) return;

        var index = _buttons.IndexOf(button);
        var symbol = _viewModel.Play(index);

        if (symbol != null)
            button.Text = symbol;
    }

    private void ResetButtons()
    {
        _viewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(GameViewModel.IsGameOver) && !_viewModel.IsGameOver)
            {
                foreach (var btn in _buttons)
                    btn.Text = string.Empty;
            }
        };
    }
}
