using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private PrefabCube _cube;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 10;

    private ObjectPool<PrefabCube> _pool;

    private void Awake()
    {
        CreatePool();
    }

    private void Start()
    {
        InvokeRepeating(nameof(SpawnCubes), 0.0f, _repeatRate);
    }

    private void CreatePool()
    {
        _pool = new ObjectPool<PrefabCube>(
            createFunc: () => Instantiate(_cube),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) => Destroy(cube.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
            );
    }

    private void ActionOnGet(PrefabCube cube)
    {
        cube.transform.position = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position;
        cube.GetComponent<Rigidbody>().velocity = Vector3.zero;
        cube.InitializeAction(ReturnObjectInPool);
        cube.gameObject.SetActive(true);
    }

    private void SpawnCubes()
    {
        _pool.Get();
    }

    private void ReturnObjectInPool(PrefabCube cube)
    {
        _pool.Release(cube);
    }
}