using System;
using UnityEngine;

public class UniqueObject : MonoBehaviour, IPoolObject
{
    public Action<UniqueObject> ReturnedToPool;
    
    public bool InUse { get; set; }
    public void Clear()
    {
        gameObject.SetActive(false);
    }

    public void Destroy()
    {
        ReturnedToPool?.Invoke(this);
    }
}