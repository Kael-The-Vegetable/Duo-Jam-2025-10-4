using BasicUtilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
	private class GameObjectPool
	{
		private readonly GameObject _prefab;
		public ObjectPool<GameObject> Pool { get; }

		public GameObjectPool(GameObject prefab, int size = 50, int maxSize = 1000)
		{
			_prefab = prefab;
			Pool = new ObjectPool<GameObject>(
				CreatePooledObject, GetFromPool, ReturnToPool, DestroyPooledObject,
				true, size, maxSize);
		}

		#region Pool Funcs
		private GameObject CreatePooledObject()
		{
			GameObject newObject = Instantiate(_prefab);
			return newObject;
		}
		private void GetFromPool(GameObject pooledObject) => pooledObject.SetActive(true);
		private void ReturnToPool(GameObject pooledObject) => pooledObject.SetActive(false);
		private void DestroyPooledObject(GameObject pooledObject) => Destroy(pooledObject);
		#endregion
	}

	private readonly Dictionary<string, GameObjectPool> _pools = new Dictionary<string, GameObjectPool>();
	protected override void Initialize()
	{
		
	}

	public void AddPool(string poolName, GameObject poolObject, int poolSize)
	{
		_pools.Add(poolName, new GameObjectPool(poolObject, poolSize));
	}

	public GameObject Get(string poolName)
	{
		if (_pools.TryGetValue(poolName, out GameObjectPool pool))
		{
			return pool.Pool.Get();
		}
		return null;
	}
	public void Release(string poolName, GameObject obj)
	{
		if (_pools.TryGetValue(poolName, out GameObjectPool pool))
		{
			pool.Pool.Release(obj);
		}
	}
}
