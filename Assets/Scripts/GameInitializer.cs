using UnityEngine;
using Zenject;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private string _fileName = "file.txt";
    [SerializeField] private ObjectMatrixConfig _matrixConfig;
    [SerializeField] private InputHandler _inputHandler;

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
        Field field = FieldReader.ReadFrom(_fileName);
        ObjectMatrix matrix = new ObjectMatrix(_matrixConfig, _spawner);
        
        matrix.AttachToField(field);
        _inputHandler.SetObjectMatrix(matrix);
    }
}