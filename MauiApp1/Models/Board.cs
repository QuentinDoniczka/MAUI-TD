namespace MauiApp1.Models;

public class Board
{
    private static readonly int[][] WinPatterns =
    [
        [0, 1, 2], [3, 4, 5], [6, 7, 8], // rows
        [0, 3, 6], [1, 4, 7], [2, 5, 8], // columns
        [0, 4, 8], [2, 4, 6]              // diagonals
    ];

    private readonly CellValue[] _cells = new CellValue[9];

    public CellValue GetCell(int index) => _cells[index];

    public bool Play(int index, CellValue player)
    {
        if (_cells[index] != CellValue.Empty)
            return false;

        _cells[index] = player;
        return true;
    }

    public CellValue CheckWinner()
    {
        foreach (var pattern in WinPatterns)
        {
            var a = _cells[pattern[0]];
            if (a != CellValue.Empty && a == _cells[pattern[1]] && a == _cells[pattern[2]])
                return a;
        }

        return CellValue.Empty;
    }

    public bool IsFull => _cells.All(c => c != CellValue.Empty);

    public void Reset()
    {
        Array.Fill(_cells, CellValue.Empty);
    }
}
