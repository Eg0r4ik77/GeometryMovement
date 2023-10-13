using Zenject;

public class UniqueObjectFactory
{
    private readonly DiContainer _diContainer;
    private readonly UniqueObjectPrefabs _prefabs;
    
    public UniqueObjectFactory(DiContainer diContainer, UniqueObjectPrefabs prefabs)
    {
        _diContainer = diContainer;
        _prefabs = prefabs;
    }
    
    public UniqueObject Create(UniqueObjectType type)
    {
        UniqueObject prefab = Get(type);
        UniqueObject instance = _diContainer.InstantiatePrefabForComponent<UniqueObject>(prefab);

        return instance;
    }

    private UniqueObject Get(UniqueObjectType type)
    {
        return type switch
        {
            UniqueObjectType.WhiteCube => _prefabs.WhiteCube,
            UniqueObjectType.RedCube => _prefabs.RedCube,
            UniqueObjectType.YellowCube => _prefabs.YellowCube,
            UniqueObjectType.BlueCube => _prefabs.BlueCube,
            UniqueObjectType.PurpleCube => _prefabs.PurpleCube,
            _ => null
        };
    }    
}
