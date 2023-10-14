using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class FieldReader
{
    public Action<string> ErrorThrown;
    
    public Field ReadFrom(string fileName)
    {
        using StreamReader reader = new StreamReader(fileName);
        Field field = new();

        ValidateFileForEmptiness(reader);

        while (reader.ReadLine() is {} line)
        {
            int rowNumber = field.RowCount + 1;
            
            ValidateRow(line, rowNumber, field);
            List<int> row = ConvertLineToDigitsRow(line);
            field.Add(row);
        }

        return field;
    }

    private void ValidateFileForEmptiness(StreamReader reader)
    {
        if (reader.Peek() == -1)
        {
            ThrowException("File must not be empty");
        }
    }
    
    private List<int> ConvertLineToDigitsRow(string line)
    {
        List<int> row = line
            .Select(number => Convert.ToInt32(Char.GetNumericValue(number)))
            .ToList();
        return row;
    }

    private void ValidateRow(string row, int rowNumber, Field field)
    {
        ValidateForPositiveDigits(row, rowNumber);
        ValidateForNonEmptyRow(row, rowNumber);
        ValidateForSameRowsLength(row, rowNumber, field);
    }

    private void ValidateForPositiveDigits(string line, int rowNumber)
    {
        foreach (char character in line)
        {
            if (!(char.IsDigit(character) && character > '0'))
            {
                ThrowException($"({rowNumber}): All characters must be positive digits");
            }
        }
    }
    
    private void ValidateForNonEmptyRow(string line, int rowNumber)
    {
        if (line.Length == 0)
        {
            ThrowException($"({rowNumber}): The row must not be empty");
        }
    }
    
    private void ValidateForSameRowsLength(string line, int rowNumber, Field field)
    {
        if (field.RowCount > 0 && field.ColumnCount != line.Length)
        {
            ThrowException($"({rowNumber}): The rows must have the same length");
        }
    }

    private void ThrowException(string message)
    {
        ErrorThrown?.Invoke(message);
        throw new Exception(message);
    }
}