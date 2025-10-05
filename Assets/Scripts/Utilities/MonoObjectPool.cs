using BasicUtilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MonoObjectPool<T> where T : Component
{
	private readonly T _prefab;
	private readonly Transform _poolParent;
	public ObjectPool<T> Pool { get; }

	public MonoObjectPool(T prefab, Transform parent, int size = 50, int maxSize = 1000)
	{
		_prefab = prefab;
		_poolParent = parent;
		Pool = new ObjectPool<T>(
			CreatePooledObject, GetFromPool, ReturnToPool, DestroyPooledObject,
			true, size, maxSize);
	}

	#region Pool Funcs
	private T CreatePooledObject()
	{
		return GameObject.Instantiate<T>(_prefab, new InstantiateParameters { parent = _poolParent });
	}
	private void GetFromPool(T pooledObject) => pooledObject.gameObject.SetActive(true);
	private void ReturnToPool(T pooledObject) => pooledObject.gameObject.SetActive(false);
	private void DestroyPooledObject(T pooledObject) => GameObject.Destroy(pooledObject);
	#endregion
}
