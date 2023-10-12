using System.Threading.Tasks;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private ObjectMatrix _objectMatrix;
    [SerializeField] private int _inputDelayMillis = 250;
    
    private bool _blocked;
    
    private async void Update()
    {
        float rowStep = Input.GetAxisRaw("Vertical");
        float columnStep = Input.GetAxisRaw("Horizontal");

        if (rowStep == 0 && columnStep == 0)
            return;
            
        await TryShiftMatrixByStep((int)rowStep,  (int)columnStep);
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