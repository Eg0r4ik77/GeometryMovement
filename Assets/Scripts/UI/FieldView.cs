using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class FieldView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    private Field _field;
    private ObjectMatrix _matrix;

    public void SetField(Field field, ObjectMatrix matrix)
    {
        _field = field;
        _matrix = matrix;
        matrix.Shifted += UpdateView;
        ResetView();
    }

    private void UpdateView(List<Vector2Int> positions)
    {
        string text = "";
        
        if (positions.IndexOf(Vector2Int.zero) != -1)
        {
            text += $"<color=green>{_field.Get(0, 0)}</color>";
        }
        else
        {
            text += $"{_field.Get(0, 0)}";   
        }
        
        for (int row = 0; row < _field.RowCount; row++)
        {
            for (int column = 0; column < _field.ColumnCount; column++)
            {
                if(row == 0 && column == 0)
                    continue;

                if (positions.FirstOrDefault(position => position.x == row && position.y == column) != default)
                {
                    text += $"<color=green>{_field.Get(row, column)}</color>";
                }
                else
                {
                    text += $"{_field.Get(row, column)}";   
                }
            }
            text += "\n";
        }

        _textMeshPro.text = text;
    }

    private void ResetView()
    {
        string text = "";
        for (int row = 0; row < _field.RowCount; row++)
        {
            for (int column = 0; column < _field.ColumnCount; column++)
            {
                text += $"{_field.Get(row, column)}";
            }

            text += "\n";
        }

        _textMeshPro.text = text;
    }
}
