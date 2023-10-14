using System.IO;
using UnityEngine;
using Zenject;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private string _fileName = "file.txt";
    [SerializeField] private ObjectMatrixConfig _matrixConfig;
    
    [SerializeField] private InputHandler _inputHandler;
    
    [SerializeField] private MatrixOnFieldView _matrixOnFieldView;
    [SerializeField] private FileErrorMessageView _errorMessageView;
    
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
        FieldReader fieldReader = new FieldReader();
        ObjectMatrix matrix = new ObjectMatrix(_matrixConfig, _spawner);
        
        _errorMessageView.Initialize(fieldReader, matrix);

        string filePath = Path.Combine(Application.streamingAssetsPath, _fileName);
        Field field = fieldReader.ReadFrom(filePath);
        
        _matrixOnFieldView.Initialize(field, matrix);

        matrix.AttachField(field);
       _inputHandler.SetObjectMatrix(matrix);
    }
}