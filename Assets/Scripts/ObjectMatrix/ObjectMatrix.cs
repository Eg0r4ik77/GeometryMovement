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

    private readonly UniqueObjectSpawner _objectsSpawner;
    private readonly UniqueObject[,] _objects;
    
    private Vector2Int _center;
    private Field _field = new();

    public Action<List<Vector2Int>> Shifted;

    public ObjectMatrix(ObjectMatrixConfig config, UniqueObjectSpawner objectsSpawner)
    {
        _size = config.Size;
        _distanceBetweenObjects = config.DistanceBetweenObjects;
        _types = config.Types;
        _undefinedObjectType = config.UndefinedObjectType;
        _objectsSpawner = objectsSpawner;
        
        _objects = new UniqueObject[_size, _size];
    }

    public void AttachToField(Field field)
    {
        _field = field;
        
        ValidateSize();
        SetObjectsFromRandomCenter();
    }

    public void ShiftMatrixByStep(int rowStep, int columnStep)
    {
        UpdateObjects(_center.y - rowStep, _center.x + columnStep);
    }

    private void SetObjectsFromRandomCenter()
    {
        int rowIndex = Random.Range(0, _field.RowCount);
        int columnIndex = Random.Range(0, _field.ColumnCount);
        
        UpdateObjects(rowIndex, columnIndex);
    }

    private void UpdateObjects(int fieldCenterRow, int fieldCenterColumn)
    {
        SetCenter(fieldCenterRow, fieldCenterColumn);

        List<Vector2Int> newPositions = new();
        int halfSize = _size / 2;

        for (int i = -halfSize; i <= halfSize; i++)
        {
            for (int j = -halfSize; j <= halfSize; j++)
            {
                int newRow = GetCycledIndex(fieldCenterRow + i, _field.RowCount);
                int newColumn = GetCycledIndex(fieldCenterColumn + j, _field.ColumnCount);
                
                int digit = _field.Get(newRow, newColumn);
                int matrixRow = halfSize + i;
                int matrixColumn = halfSize + j;
                Vector3 spawnPosition = new Vector3(_distanceBetweenObjects * j, 0, -_distanceBetweenObjects * i);

                newPositions.Add(new Vector2Int(newRow, newColumn));
                TrySpawnObject(digit, matrixRow, matrixColumn, spawnPosition);
            }
        }
        
        Shifted?.Invoke(newPositions);
    }

    private void SetCenter(int fieldRow, int fieldColumn)
    {
        _center.y = GetCycledIndex(fieldRow, _field.RowCount);
        _center.x = GetCycledIndex(fieldColumn, _field.ColumnCount);
    }

    private void TrySpawnObject(int digit, int matrixRow, int matrixColumn, Vector3 position)
    {
        UniqueObjectType newObjectType = GetTypeByDigit(digit);
        UniqueObject currentObject = _objects[matrixRow, matrixColumn];

        if(currentObject && currentObject.InUse)
            currentObject.Destroy();

        UniqueObject newObject = _objectsSpawner.Spawn(newObjectType,  position);
        _objects[matrixRow, matrixColumn] = newObject;
    }

    private UniqueObjectType GetTypeByDigit(int digit)
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

        return newObjectType;
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
