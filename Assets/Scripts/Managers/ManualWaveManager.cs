using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualWaveManager : WaveManager<ManualWaveManager>
{
	[SerializeField] private WaveData[] _waveData;

	protected override void Initialize()
	{
		waves = new Wave[_waveData.Length];

		// Find number of enemies needed to make pools for.
		Dictionary<Enemy, int> enemyCounts = new();
		for (int i = 0; i < _waveData.Length; i++)
		{
			WaveData waveData = _waveData[i];
			waves[i] = waveData.ToWave(destroyCancellationToken);
			var ec = waveData.CountEnemies();

			for (int j = 0; j < ec.Length; j++)
			{
				var (enemy, count) = ec[j];
				if (!enemyCounts.ContainsKey(enemy))
				{
					enemyCounts.Add(enemy, count);
				}
				else if (enemyCounts[enemy] < count)
				{
					enemyCounts[enemy] = count;
				}
			}
		}

		// Adding pools
		foreach (var key in enemyCounts.Keys)
		{
			AddPool(key, enemyCounts[key]);
			Pools[key].Pool.Get();
		}
	}
}
