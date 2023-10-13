using UnityEngine;

public abstract class MonoBehaviourObjectsPool : Pool
{
    private readonly Transform _rootTransform;

    protected MonoBehaviourObjectsPool(Transform rootTransform)
    {
        _rootTransform = rootTransform;
    }

    protected void AttachToRoot(Transform objectTransform)
    {
        objectTransform.SetParent(_rootTransform);
        objectTransform.position = Vector3.zero;
    }
}