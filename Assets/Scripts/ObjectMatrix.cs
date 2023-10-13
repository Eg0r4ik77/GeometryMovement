using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectMatrix
{
    private readonly int _size;
    private readonly float _distanceBetweenObjects;

    private readonly List<UniqueObjectType> _types;
    private readonly UniqueObjectType _undefinedObjectType;

    private readonly UniqueObject[,] _objects;
    
    private Vector2Int _center;


    private Field _field = new();
    private UniqueObjectSpawner _spawner;
    

    public ObjectMatrix(ObjectMatrixConfig config, UniqueObjectSpawner spawner)
    {
        _size = config.Size;
        _distanceBetweenObjects = config.DistanceBetweenObjects;
        _types = config.Types;
        _undefinedObjectType = config.UndefinedObjectType;

        _spawner = spawner;
        
        _objects = new UniqueObject[_size, _size];
    }

    public void AttachToField(Field field)
    {
        _field = field;
        
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
        UniqueObjectType newObjectType;

        if (digit > _types.Count)
        {
            Debug.LogError($"Object for {digit} is not defined");
            newObjectType = _undefinedObjectType;
        }
        else
        {
            newObjectType = _types[digit - 1];
        }

        UniqueObject oldObject = _objects[halfSize + i, halfSize + j];

        if(oldObject != null && oldObject.InUse)
            oldObject.Destroy();

        Vector3 spawnPosition = new Vector3(_distanceBetweenObjects * j, 0, -_distanceBetweenObjects * i);
        UniqueObject newObject = _spawner.Spawn(newObjectType,  spawnPosition);
        _objects[halfSize + i, halfSize + j] = newObject;
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
