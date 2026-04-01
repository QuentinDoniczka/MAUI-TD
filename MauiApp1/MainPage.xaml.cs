using System.Collections.Specialized;
using MauiApp1.ViewModels;

namespace MauiApp1;

public partial class MainPage : ContentPage
{
    private readonly Button[] _cellButtons;

    public MainPage(GameViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        _cellButtons = [Cell0, Cell1, Cell2, Cell3, Cell4, Cell5, Cell6, Cell7, Cell8];

        viewModel.Cells.CollectionChanged += OnCellsChanged;
        viewModel.ResetCommand.Execute(null);
    }

    private void OnCellsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Replace && e.NewStartingIndex >= 0)
        {
            _cellButtons[e.NewStartingIndex].Text = e.NewItems?[0]?.ToString() ?? string.Empty;
            return;
        }

        var cells = (IList<string>)sender!;
        for (var i = 0; i < _cellButtons.Length; i++)
            _cellButtons[i].Text = cells[i];
    }
}
