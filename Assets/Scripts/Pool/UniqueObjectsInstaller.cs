using UnityEngine;
using Zenject;

public class UniqueObjectsInstaller : MonoInstaller
{
    [SerializeField] private Transform _root;
    [SerializeField] private UniqueObjectPrefabs _prefabs;
    
    public override void InstallBindings()
    {
        Container
            .Bind<UniqueObjectFactory>()
            .AsSingle()
            .WithArguments(_prefabs);

        Container
            .Bind<UniqueObjectPoolsProvider>()
            .AsSingle()
            .WithArguments(_root);

        Container
            .Bind<UniqueObjectSpawner>()
            .AsSingle();
    }
}