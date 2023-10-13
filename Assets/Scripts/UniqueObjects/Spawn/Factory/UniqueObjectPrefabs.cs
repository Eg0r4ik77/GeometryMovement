using UnityEngine;

[CreateAssetMenu(menuName = "UniqueObjects/Prefabs")]
public class UniqueObjectPrefabs : ScriptableObject
{
    [field:SerializeField] public UniqueObject WhiteCube { get; private set; }
    [field:SerializeField] public UniqueObject RedCube { get; private set; }
    [field:SerializeField] public UniqueObject YellowCube { get; private set; }
    [field:SerializeField] public UniqueObject BlueCube { get; private set; }
    [field:SerializeField] public UniqueObject PurpleCube{ get; private set; }
}