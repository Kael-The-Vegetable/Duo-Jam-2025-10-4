using BasicUtilities;
using System.Collections.Generic;
using UnityEngine;

public abstract class WaveManager<T> : Singleton<T> where T : Component
{
	protected Wave[] waves;
	public Dictionary<Enemy, MonoObjectPool<Enemy>> Pools { get; } = new();

	protected override void Initialize() { }

	public void AddPool(Enemy enemy, int count)
	{
		Pools.TryAdd(enemy, new MonoObjectPool<Enemy>(enemy, transform, count, 10 * count));
	}
}
