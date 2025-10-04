using BasicUtilities;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Wave
{
	private Queue<Enemy> _enemiesToSpawn;
	private float _duration;
	private CancellationToken _token;
	public Wave(Enemy[] enemyTypes, float duration, int[] rhythm, int rhythmRepetitions, Vector2[] spawnPoints, CancellationToken token)
	{
		int total = rhythm.Length * rhythmRepetitions;
		
		_enemiesToSpawn = new Queue<Enemy>(total);
		_duration = duration / total;
		_token = token;

		int r = -1; // location in rhythm
		for (int i = 0; i < total; i++)
		{
			_enemiesToSpawn.Enqueue(enemyTypes[rhythm[++r % rhythm.Length]]);
		}
	}
	public Wave(Queue<Enemy> enemiesToSpawn, float duration, CancellationToken token)
	{
		_enemiesToSpawn = enemiesToSpawn;
		_duration = duration / enemiesToSpawn.Count;
		_token = token;
	}

	public void SpawnEnemy()
	{
		if (_enemiesToSpawn.TryDequeue(out Enemy enemy))
		{

			OneShotTimer.Delay(_duration, SpawnEnemy, _token);
		}
	}
}
