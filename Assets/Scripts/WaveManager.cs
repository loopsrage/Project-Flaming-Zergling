using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class WaveManager : MonoBehaviour 
{
	public GameObject enemyPrefab;
	public Transform spawnPos;

	public class WaveEvent : UnityEvent{}
	public WaveEvent onWaveFinish = new WaveEvent();

	public int enemiesAlive = 0;
	private bool waveStarted = false;

	public void SpawnWave()
	{
		GameObject go = Instantiate (enemyPrefab, spawnPos.position, Quaternion.identity);
		go.GetComponent<EnemyUnit> ().SetDestination (GameObject.FindObjectOfType<TravelPoint> ());
		enemiesAlive = 1;
		waveStarted = true;
	}

	void Update()
	{
		if (enemiesAlive <= 0 && waveStarted) {
			waveStarted = false;
			onWaveFinish.Invoke ();
		}
	}
		
}
