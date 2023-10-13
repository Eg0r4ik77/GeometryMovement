using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class MatrixOnFieldView : MonoBehaviour
{
    private Field _field;
    private ObjectMatrix _matrix;

    private TextMeshProUGUI _textMeshPro;

    public void Initialize(Field field, ObjectMatrix matrix)
    {
        _field = field;
        SetObjectMatrix(matrix);
        ResetView();
    }

    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    private void OnDestroy()
    {
        if (_matrix != null)
            _matrix.Shifted -= UpdateView;
    }

    private void SetObjectMatrix(ObjectMatrix matrix)
    {
        if (_matrix != null)
            _matrix.Shifted -= UpdateView;

        _matrix = matrix;
        _matrix.Shifted += UpdateView;
    }

    private void UpdateView(List<Vector2Int> positions)
    {
        StringBuilder stringBuilder = new StringBuilder();

        for (int row = 0; row < _field.RowCount; row++)
        {
            for (int column = 0; column < _field.ColumnCount; column++)
            {
                int digit = _field.Get(row, column);
                string activeDigitString = $"<color=green>{digit}</color>";
                bool isDigitActive = positions.IndexOf(new Vector2Int(row, column)) != -1;
                
                string digitString = isDigitActive
                                    ? activeDigitString
                                    : digit.ToString();
                stringBuilder.Append(digitString);
            }
            stringBuilder.Append("\n");
        }

        _textMeshPro.text = stringBuilder.ToString();
    }

    private void ResetView()
    {
        StringBuilder stringBuilder = new StringBuilder();
        
        for (int row = 0; row < _field.RowCount; row++)
        {
            for (int column = 0; column < _field.ColumnCount; column++)
            {
                int digit = _field.Get(row, column);
                stringBuilder.Append(digit);
            }

            stringBuilder.Append("\n");
        }

        _textMeshPro.text = stringBuilder.ToString();
    }
}
