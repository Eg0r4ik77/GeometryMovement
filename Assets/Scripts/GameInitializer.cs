using System.IO;
using UnityEngine;
using Zenject;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private string _fileName = "file.txt";
    [SerializeField] private ObjectMatrixConfig _matrixConfig;
    
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private MatrixOnFieldView _matrixOnFieldView;
    
    private UniqueObjectSpawner _spawner;

    [Inject]
    private void Construct(UniqueObjectSpawner spawner)
    {
        _spawner = spawner;
    }
    
    private void Start()
    { 
        InitializeGame();   
    }

    private void InitializeGame()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, _fileName);
        Field field = FieldReader.ReadFrom(filePath);
        
        ObjectMatrix matrix = new ObjectMatrix(_matrixConfig, _spawner);
        
        _matrixOnFieldView.Initialize(field, matrix);
        
        matrix.AttachToField(field);
       _inputHandler.SetObjectMatrix(matrix);
    }
}