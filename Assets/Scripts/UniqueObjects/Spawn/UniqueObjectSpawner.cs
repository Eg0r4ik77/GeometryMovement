using UnityEngine;
using Zenject;

public class UniqueObjectSpawner
{
    private readonly UniqueObjectPoolsProvider _poolsProvider;

    [Inject]
    public UniqueObjectSpawner(UniqueObjectPoolsProvider poolsProvider)
    {
        _poolsProvider = poolsProvider;
    }

    public UniqueObject Spawn(UniqueObjectType type, Vector3 position)
    {
        UniqueObjectPool pool = _poolsProvider.Get(type);
        UniqueObject uniqueObject = (UniqueObject)pool.Get();

        uniqueObject.transform.position = position;
        uniqueObject.gameObject.SetActive(true);

        return uniqueObject;
    }
}    