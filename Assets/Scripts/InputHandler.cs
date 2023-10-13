using System;
using System.Threading.Tasks;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private int _inputDelayMillis = 250;
    
    private bool _blocked;

    private ObjectMatrix _objectMatrix;

    private PlayerInput _input;

    private void Awake()
    {
        _input = new();
    }

    private void OnEnable()
    {
       _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private async void Update()
    {
        Vector2 input = _input.Field.Move.ReadValue<Vector2>();

        int rowStep = (int)input.y;
        int columnStep = (int)input.x;

        if (rowStep == 0 && columnStep == 0)
            return;
            
        await TryShiftMatrixByStep(rowStep,  columnStep);
    }

    public void SetObjectMatrix(ObjectMatrix matrix)
    {
        _objectMatrix = matrix;
    }

    
    private async Task TryShiftMatrixByStep(int rowStep, int columnStep)
    {
        if (_blocked)
            return;
        
        _objectMatrix.ShiftMatrixByStep(rowStep,  columnStep);
        
        _blocked = true;
        await Task.Delay(_inputDelayMillis);
        _blocked = false;
    }
}