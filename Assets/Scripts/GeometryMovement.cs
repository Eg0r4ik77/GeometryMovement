using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class GeometryMovement : MonoBehaviour
{
    [SerializeField] private string _fileName = "file.txt";
    
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _matrixSize = 3;

    [SerializeField] private Color _undefinedDigitColor = Color.white;
    [SerializeField] private List<Color> _colors;

    [SerializeField] private int _inputDelayMillis = 250;
    
    private MaterialPropertyBlock _materialPropertyBlock;

    private readonly List<List<int>> _field = new();
    private GameObject[,] _prefabs;

    private bool _blocked;

    private Vector2Int _matrixCenter;

    private Vector2Int _fieldSize;
    
    private static readonly int ShaderColorId = Shader.PropertyToID("_Color");

    private void Start()
    {
        _materialPropertyBlock = new();
        _prefabs = new GameObject[_matrixSize, _matrixSize];
        Execute();
    }

    private void Update()
    {
        float rawStep = Input.GetAxisRaw("Vertical");
        float columnStep = Input.GetAxisRaw("Horizontal");
        
        if (_blocked || (rawStep == 0 && columnStep == 0))
            return;
        
        TryUpdate(_matrixCenter.y - (int)rawStep, _matrixCenter.x + (int)columnStep);
    }

    private void Execute()
    {
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
                throw new Exception($"{column}: the raw must not be empty");
            }
                
            if (_field.Count > 0 && _field[0].Count != line.Length)
            {
                throw new Exception($"{column}: the raws must have the same length");
            }
                
            List<int> list = line
                .Select(number => Convert.ToInt32(Char.GetNumericValue(number)))
                .ToList();
                    
            _field.Add(list);
        }
        
        _fieldSize = new Vector2Int(_field[0].Count, _field.Count);

        if (_matrixSize % 2 == 0)
        {
            throw new Exception("the matrix size must be an odd number");
        }
        
        if (_matrixSize > _fieldSize.x)
        {
            throw new Exception("There are more matrix columns than field columns");
        }
        
        if (_matrixSize > _fieldSize.y)
        {
            throw new Exception("There are more matrix rows than field rows");
        }

        InstantiateCubes();

        int rowIndex = Random.Range(0, _fieldSize.y);
        int columnIndex = Random.Range(0, _fieldSize.x);
        
        UpdateField(rowIndex, columnIndex);
    }

    private void InstantiateCubes()
    {
        int halfSize = _matrixSize / 2;
        
        for (int i = -halfSize; i <= halfSize; i++)
        {
            for (int j = -halfSize; j <= halfSize; j++)
            {
                _prefabs[halfSize+i, halfSize+j] =
                    Instantiate(_prefab, new Vector3(2 * j, 0, -2 * i), Quaternion.identity);
            }
        }
    }

    private async void TryUpdate(int row, int column)
    {
        UpdateField(row, column);
        
        _blocked = true;
        await Task.Delay(_inputDelayMillis);
        _blocked = false;
    }
    
    private void UpdateField(int row, int column)
    {
        int halfSize = _matrixSize / 2;
        
        for (int i = -halfSize; i <= halfSize; i++)
        {
            for (int j = -halfSize; j <= halfSize; j++)
            {
                int normalized_i = ((row+i) % _fieldSize.y + _fieldSize.y) % _fieldSize.y;
                int normalized_j = ((column+j) % _fieldSize.x + _fieldSize.x) % _fieldSize.x;

                if (i == 0 && j == 0)
                {
                    _matrixCenter.y = normalized_i;
                    _matrixCenter.x = normalized_j;
                }

                int digit = _field[normalized_i][normalized_j];
                Color newColor;
                
                if (digit > _colors.Count)
                {
                    Debug.LogError($"Color for {digit} is not defined");
                    newColor = _undefinedDigitColor;
                }
                else
                {
                    newColor = _colors[digit - 1];
                }
                
                _materialPropertyBlock.SetColor(ShaderColorId, newColor);
                _prefabs[halfSize + i, halfSize + j].GetComponent<MeshRenderer>().SetPropertyBlock(_materialPropertyBlock);
            }
        }
    }
}
