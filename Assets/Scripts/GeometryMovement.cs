using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

// в файле строк < _size.x или столбцов < size.y

public class GeometryMovement : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Vector2Int _size; //пока только на 3x3
    [SerializeField] private List<Material> _materials;

    // property blocks
    //private MeshRenderer _meshRenderer;

    private List<List<int>> _field = new();
    private GameObject[,] _prefabs;
    private int[,] _currentField;

    private bool _blocked;

    private int _currentRow;
    private int _currentColumn;
    
    private void Awake()
    {
        //_meshRenderer = new MeshRenderer();
    }

    private void Update()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        
        if (_blocked || (vertical == 0 && horizontal == 0))
            return;

        print($"Step: {vertical} {horizontal}");
        
        TryUpdate(_currentRow - (int)vertical, _currentColumn + (int)horizontal);
    }

    private void Start()
    {
        _currentField = new int[_size.x, _size.y];
        _prefabs = new GameObject[_size.x, _size.y];
        Execute();
    }

    private void Execute()
    {
        string filename = "file.txt";
        using StreamReader reader = new StreamReader(filename);
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            List<int> list = line
                .Select(number => Convert.ToInt32(Char.GetNumericValue(number)))
                .ToList();
            _field.Add(list);
        }

        InstantiateCubes();
        
        _currentRow = Random.Range(0, _field.Count);
        _currentColumn = Random.Range(0, _field[0].Count);
        
        UpdateField(_currentRow, _currentColumn);
    }

    private void InstantiateCubes()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                _prefabs[i, j] = Instantiate(_prefab, new Vector3(2 * j, 0, -2 * i), Quaternion.identity);
            }
        }
    }

    private void UpdateField(int row, int column)
    {
        string str = "";
        // если 3x3
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int normalized_i = ((row+i) % 3 + 3) % 3;
                int normalized_j = ((column+j) % 8 + 8) % 8;

                if (i == 0 && j == 0)
                {
                    _currentRow = normalized_i;
                    _currentColumn = normalized_j;
                    print($"Center: {_currentRow} {_currentColumn}");
                }

                _prefabs[1+i, 1+j].GetComponent<MeshRenderer>().material
                    = _materials[_field[normalized_i][normalized_j] - 1];
            }
        }
    }

    private async void TryUpdate(int row, int column)
    {
        UpdateField(row, column);
        
        _blocked = true;
        await Task.Delay(250);
        _blocked = false;
    }
}
