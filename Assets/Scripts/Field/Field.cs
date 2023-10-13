using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    private readonly List<List<int>> _digits = new();

    public int RowCount => _digits.Count;

    public int ColumnCount => _digits[0].Count;

    public int Get(int row, int column) => _digits[row][column];

    public void Add(List<int> row)
    {
        _digits.Add(row);
    }
}
