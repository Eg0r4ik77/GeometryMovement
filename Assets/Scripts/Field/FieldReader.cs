using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class FieldReader
{
    public static Field ReadFrom(string fileName)
    {
        using StreamReader reader = new StreamReader(fileName);
        Field field = new();

        if (IsEmptyFile(reader))
            return null;
        
        while (reader.ReadLine() is {} line)
        {
            int rowNumber = field.RowCount + 1;
            
            ValidateRow(line, rowNumber, field);
            List<int> row = ConvertLineToDigitsRow(line);
            field.Add(row);
        }

        return field;
    }

    private static bool IsEmptyFile(StreamReader reader)
    {
        if (reader.Peek() == -1)
        {
            Debug.LogError("File must not be empty");
            return true;
        }

        return false;
    }
    
    private static List<int> ConvertLineToDigitsRow(string line)
    {
        List<int> row = line
            .Select(number => Convert.ToInt32(Char.GetNumericValue(number)))
            .ToList();
        return row;
    }

    private static void ValidateRow(string row, int rowNumber, Field field)
    {
        ValidateForPositiveDigits(row, rowNumber);
        ValidateForNonEmptyRow(row, rowNumber);
        ValidateForSameRowsLength(row, rowNumber, field);
    }

    private static void ValidateForPositiveDigits(string line, int rowNumber)
    {
        foreach (char character in line)
        {
            if (!(char.IsDigit(character) && character > '0'))
            {
                throw new Exception($"{rowNumber}: all characters must be positive digits");
            }
        }
    }
    
    private static void ValidateForNonEmptyRow(string line, int rowNumber)
    {
        if (line.Length == 0)
        {
            throw new Exception($"{rowNumber}: the row must not be empty");
        }
    }
    
    private static void ValidateForSameRowsLength(string line, int rowNumber, Field field)
    {
        if (field.RowCount > 0 && field.ColumnCount != line.Length)
        {
            throw new Exception($"{rowNumber}: the rows must have the same length");
        }
    }
}