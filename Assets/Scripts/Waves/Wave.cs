using BasicUtilities;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Splines;

//TODO: change Vector2 spawnPoints to a spawnPoint target to put them on the spline.


[Serializable]
public class WaveData
{
	public Enemy[] enemies;
	public float duration;
	public int[] rhythm;
	public int rhythmRepetitions;
	public SplineContainer[] spawnPoints;
	public Wave ToWave(CancellationToken token)
	{
		return new Wave(enemies, duration, rhythm, rhythmRepetitions, spawnPoints, token);
	}

	public (Enemy enemy, int count)[] CountEnemies()
	{
		int[] enemyCounter = new int[enemies.Length];

		for (int i = 0; i < rhythm.Length; i++)
		{
			enemyCounter[rhythm[i]]++;
		}

		var ec = new (Enemy enemy, int count)[enemies.Length];
		for (int i = 0; i < ec.Length; i++)
		{
			ec[i] = (enemies[i], enemyCounter[i] * rhythmRepetitions);
		}

		return ec;
	}
}
public class Wave
{
	private Queue<Enemy> _enemiesToSpawn;
	private float _duration;
	private CancellationToken _token;
	private SplineContainer[] _spawnPoints;
	public Wave(Enemy[] enemyTypes, float duration, int[] rhythm, int rhythmRepetitions, SplineContainer[] spawnPoints, CancellationToken token)
	{
		int total = rhythm.Length * rhythmRepetitions;
		
		_enemiesToSpawn = new Queue<Enemy>(total);
		_duration = duration / total;
		_token = token;
		_spawnPoints = spawnPoints;

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
