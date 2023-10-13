using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UniqueObjectPoolsProvider
{
    private readonly Transform _root;
    private readonly UniqueObjectFactory _factory;
    
    private readonly Dictionary<UniqueObjectType, UniqueObjectPool> _pools = new();

    [Inject]
    public UniqueObjectPoolsProvider(Transform root, UniqueObjectFactory factory)
    {
        _root = root;
        _factory = factory;
    }

    public UniqueObjectPool Get(UniqueObjectType type)
    {
        if (!_pools.ContainsKey(type))
        {
            UniqueObjectPool pool = new UniqueObjectPool(type, _root, _factory);
            _pools[type] = pool;
        }
        
        return _pools[type];
    }
}