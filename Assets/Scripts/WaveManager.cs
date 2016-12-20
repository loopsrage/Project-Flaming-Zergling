using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour 
{
	const float HP_LINEAR_GROWTH = 1.2f;
	const float HP_EXP_GROWTH = 1.85f;
	const float HP_BASE = 10;
	const float ARMOR_GROWTH = 0.03f;
	const float ARMOR_BASE = -1;
	const int SPAWNS_PER_WAVE = 25;
	const float TIME_BETWEEN_SPAWNS = 0.75f;
	public int roundNum = 0;
	private int roundHp = 1;
	private int roundArmor = 0;

	private EnemyHPBarPool hpBarPool;

	public GameObject enemyPrefab;
	public Transform spawnPos;

	public class WaveEvent : UnityEvent{}
	public WaveEvent onWaveFinish = new WaveEvent();

	private int enemiesAlive = 0;
	[SerializeField]
	private int enemiesLeaked = 0;
	private List<EnemyUnit> spawnedEnemies;

	public void EnemyDied(EnemyUnit eu)
	{
		enemiesAlive--;
		if (enemiesAlive <= 0) {
			EndWave ();
		}
	}

	public void EnemyLeaked()
	{
		enemiesLeaked++;
	}

	public void StartWave ()
	{
		roundNum++;
		StartCoroutine (SpawnWave ());
	}

	private void CalculateEnemyAttributes()
	{
		roundHp = Mathf.FloorToInt(HP_LINEAR_GROWTH * Mathf.Pow (roundNum, HP_EXP_GROWTH) + HP_BASE);
		roundArmor = Mathf.FloorToInt (Mathf.Exp (ARMOR_GROWTH * roundNum) + ARMOR_BASE);
	}

	private IEnumerator CleanWave()
	{
		// Clean up dead enemies
		for (int i = 0; i < spawnedEnemies.Count; ++i) {
			EnemyUnit eu = spawnedEnemies [i];
			spawnedEnemies.RemoveAt (i--);
			Destroy (eu.gameObject);
		}
		yield return null;
	}

	private void EndWave()
	{
		// Clean wave on another thread
		StartCoroutine (CleanWave());

		// Report the wave as finished
		onWaveFinish.Invoke ();

	}

	private IEnumerator SpawnWave()
	{
		// get wave healt/armor
		CalculateEnemyAttributes ();
		// Count Enemies
		enemiesAlive = SPAWNS_PER_WAVE;
		// Spawn each wave
		spawnedEnemies = new List<EnemyUnit>();
		for (int i = 0; i < SPAWNS_PER_WAVE; ++i) {
			// Create Enemy
			GameObject go = Instantiate (enemyPrefab, spawnPos.position, Quaternion.identity);
			EnemyUnit eu = go.GetComponent<EnemyUnit> ();
			if (eu == null) {
				throw new MissingComponentException("no enemy unit found on enemy prefab");
			}
			// Add to spawned enemys
			spawnedEnemies.Add (eu);
			// Listen for death
			eu.onDeath.AddListener (EnemyDied);
			// Set wave attributes
			eu.hp = roundHp;
			eu.armor = roundArmor;
			// Set Path
			eu.SetDestination (GameObject.FindObjectOfType<TravelPoint> ());
			// Call enemy constructor
			eu.Create ();
			// Assign hp bar
			hpBarPool.AssignHpBarToEnemy(eu);
			// Wait 
			yield return new WaitForSeconds (TIME_BETWEEN_SPAWNS);
		}
	}


	void Awake()
	{
		hpBarPool = GameObject.FindObjectOfType<EnemyHPBarPool> ();
	}

	void Update()
	{
		
	}
		
}
