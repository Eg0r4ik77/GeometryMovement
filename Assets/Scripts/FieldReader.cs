using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class FieldReader : MonoBehaviour
{
    [SerializeField] private string _fileName = "file.txt";

    public Field ReadFrom()
    {
        Field field = new();
        using StreamReader reader = new StreamReader(_fileName);
        string line;
        int column = 0;
        while ((line = reader.ReadLine()) != null)
        {
            column++;
            foreach (char character in line)
            {
                if (!(char.IsDigit(character) && character > '0'))
                {
                    throw new Exception($"{column}: all characters must be positive digits");
                }
            }

            if (line.Length == 0)
            {
                throw new Exception($"{column}: the row must not be empty");
            }

            if (field.RowCount > 0 && field.ColumnCount != line.Length)
            {
                throw new Exception($"{column}: the rows must have the same length");
            }

            List<int> raw = line
                .Select(number => Convert.ToInt32(Char.GetNumericValue(number)))
                .ToList();

            field.Add(raw);
        }

        return field;
    }
}