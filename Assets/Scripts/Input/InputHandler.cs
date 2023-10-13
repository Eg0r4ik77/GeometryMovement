using System.Threading.Tasks;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private int _inputDelayMillis = 250;
    
    private PlayerInput _input;
    private bool _movementInputBlocked;

    private ObjectMatrix _objectMatrix;
    
    public void SetObjectMatrix(ObjectMatrix matrix)
    {
        _objectMatrix = matrix;
    }
    
    private void Awake()
    {
        _input = new PlayerInput();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void Update()
    {
        if(_objectMatrix != null)
            HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 movementInput = _input.Field.Move.ReadValue<Vector2>();
        Vector2Int matrixShiftStep = Vector2Int.RoundToInt(movementInput);

        if (matrixShiftStep == Vector2Int.zero)
            return;
            
        TryShiftMatrixByStep(matrixShiftStep.y,  matrixShiftStep.x);
    }
    
    private void TryShiftMatrixByStep(int rowStep, int columnStep)
    {
        if (_movementInputBlocked)
            return;
        
        _objectMatrix.ShiftMatrixByStep(rowStep,  columnStep);

        BlockMovementInputForTime();
    }

    private async void BlockMovementInputForTime()
    {
        _movementInputBlocked = true;
        await Task.Delay(_inputDelayMillis);
        _movementInputBlocked = false;
    }
}