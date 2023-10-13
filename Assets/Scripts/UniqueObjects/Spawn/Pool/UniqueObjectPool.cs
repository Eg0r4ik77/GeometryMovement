using UnityEngine;

public class UniqueObjectPool : MonoBehaviourObjectsPool
{
    private readonly UniqueObjectType _type;
    private readonly UniqueObjectFactory _uniqueObjectFactory;

    public UniqueObjectPool(UniqueObjectType type, Transform rootTransform, UniqueObjectFactory uniqueObjectFactory) 
        : base(rootTransform)
    {
        _type = type;
        _uniqueObjectFactory = uniqueObjectFactory;
    }

    protected override IPoolObject Create()
    {
        UniqueObject uniqueObject = _uniqueObjectFactory.Create(_type);

        uniqueObject.ReturnedToPool += Release;
        
        AttachToRoot(uniqueObject.transform);
        Release(uniqueObject);

        return uniqueObject;
    }
}