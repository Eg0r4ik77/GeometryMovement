using TMPro;
using UnityEngine;

public class FileErrorMessageView : MonoBehaviour
{
    [SerializeField] private MatrixOnFieldView _matrixOnFieldView;
    
    private const string RestartGameMessage = "Fix the file errors and restart the game";
    
    private FieldReader _fieldReader;
    private ObjectMatrix _matrix;

    private TextMeshProUGUI _textMeshPro;

    public void Initialize(FieldReader fieldReader, ObjectMatrix matrix)
    {
        SetFieldReader(fieldReader);
        SetObjectMatrix(matrix);
    }

    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    private void OnDestroy()
    {
        if (_matrix != null)
            _matrix.ErrorThrown -= UpdateView;
        
        if (_fieldReader != null)
            _fieldReader.ErrorThrown -= UpdateView;
    }

    private void SetObjectMatrix(ObjectMatrix matrix)
    {
        if (_matrix != null)
            _matrix.ErrorThrown -= UpdateView;

        _matrix = matrix;
        _matrix.ErrorThrown += UpdateView;
    }
    
    private void SetFieldReader(FieldReader fieldReader)
    {
        if (_fieldReader != null)
            _fieldReader.ErrorThrown -= UpdateView;

        _fieldReader = fieldReader;
        _fieldReader.ErrorThrown += UpdateView;
    }

    private void UpdateView(string message)
    {
        _textMeshPro.text = $"{message}\n{RestartGameMessage}";
    }
}