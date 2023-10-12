using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectMatrix : MonoBehaviour
{
    [SerializeField] private int _size = 3;
    [SerializeField] private float _distanceBetweenObjects = 2f;
    
    private Vector2Int _center;

    [SerializeField] private GameObject _undefinedDigitObject;
    [SerializeField] private List<GameObject> _prefabs;
    
    private Field _field = new();
    
    private GameObject[,] _objects;

    [SerializeField] private FieldReader _fieldReader;

    private void Start()
    {
        _objects = new GameObject[_size, _size];
        
        Create();
    }

    private void Create()
    {
        _field = _fieldReader.ReadFrom();

        ValidateSize();
        SetRandomCenter();
    }

    public void ShiftMatrixByStep(int rowStep, int columnStep)
    {
        UpdateObjects(_center.y - rowStep, _center.x + columnStep);
    }

    private void SetRandomCenter()
    {
        int rowIndex = Random.Range(0, _field.RowCount);
        int columnIndex = Random.Range(0, _field.ColumnCount);
        
        UpdateObjects(rowIndex, columnIndex);
    }

    private void UpdateObjects(int row, int column)
    {
        int halfSize = _size / 2;
        
        for (int i = -halfSize; i <= halfSize; i++)
        {
            for (int j = -halfSize; j <= halfSize; j++)
            {
                int newRow = GetCycledIndex(row + i, _field.RowCount);
                int newColumn = GetCycledIndex(column + j, _field.ColumnCount);

                if (i == 0 && j == 0)
                {
                    _center.y = newRow;
                    _center.x = newColumn;
                }

                int digit = _field.Get(newRow, newColumn);

                TrySetObject(digit, halfSize, i, j);
            }
        }
    }

    private void TrySetObject(int digit, int halfSize, int i, int j)
    {
        GameObject newObject;

        if (digit > _prefabs.Count)
        {
            Debug.LogError($"Object for {digit} is not defined");
            newObject = _undefinedDigitObject;
        }
        else
        {
            newObject = _prefabs[digit - 1];
        }

        Destroy(_objects[halfSize + i, halfSize + j]);
        _objects[halfSize + i, halfSize + j] =
            Instantiate(newObject, 
                new Vector3(_distanceBetweenObjects * j, 0, -_distanceBetweenObjects * i), 
                Quaternion.identity);
    }
    
    private void ValidateSize()
    {
        if (_size % 2 == 0)
        {
            throw new Exception("the matrix size must be an odd number");
        }

        if (_size > _field.ColumnCount)
        {
            throw new Exception("There are more matrix columns than field columns");
        }

        if (_size > _field.RowCount)
        {
            throw new Exception("There are more matrix rows than field rows");
        }
    }

    private int GetCycledIndex(int index, int count) => (index % count + count) % count;
}
