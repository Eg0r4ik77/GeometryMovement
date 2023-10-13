using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UniqueObjects/MatrixConfig")]
public class ObjectMatrixConfig : ScriptableObject
{
    [field:SerializeField] public int Size { get; private set; }
    [field:SerializeField] public float DistanceBetweenObjects { get; private set; }
    [field:SerializeField] public List<UniqueObjectType> Types { get; private set; }
    [field:SerializeField] public UniqueObjectType UndefinedObjectType { get; private set; }
}