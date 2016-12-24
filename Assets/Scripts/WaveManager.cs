using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour 
{
	// HP Growth
	const float HP_LINEAR_GROWTH = 1.2f;
	const float HP_EXP_GROWTH = 1.85f;
	const float HP_BASE = 9;

	// Armor Growth
	const float ARMOR_GROWTH = 0.03f;
	const float ARMOR_BASE = -1;

	// Spawns 
	const int SPAWNS_PER_WAVE = 25;
	const float TIME_BETWEEN_SPAWNS = 0.75f;

	// Round Info
	private int roundNum = 0;
	private int roundHp = 1;
	private int roundArmor = 0;
	public Text roundNumTextDisplay;

	// HP Displays
	private EnemyHPBarPool hpBarPool;

	// Game Manager
	private PathManager pathMgr;

	// Enemy Prefab
	public GameObject enemyPrefab;

	// Wave Information
	private int enemiesAlive = 0;
	private int enemiesLeaked = 0;
	private List<EnemyUnit> spawnedEnemies;
	public Text leakedTextDisplay;

	// End Wave Event
	public class WaveEvent : UnityEvent{}
	public WaveEvent onWaveFinish = new WaveEvent();




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
		leakedTextDisplay.text = "Leaks: " + enemiesLeaked.ToString ();
	}

	public void StartWave ()
	{
		// Incrememt wave number
		roundNum++;

		// Show Wave Count
		ShowWaveCount();
		ShowLeakCount ();

		// get wave healt/armor
		CalculateEnemyAttributes ();

		// Count Enemies
		enemiesAlive = SPAWNS_PER_WAVE;

		// Start spawning
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
		
	private void ShowLeakCount()
	{
		leakedTextDisplay.transform.parent.gameObject.SetActive (true);
	}

	private void ShowWaveCount()
	{
		roundNumTextDisplay.text = "Round: " + roundNum.ToString ();
		roundNumTextDisplay.transform.parent.gameObject.SetActive (true);
	}

	private IEnumerator SpawnWave()
	{
		// Get First Point in Path
		TravelPoint firstPoint = GameManager.instance.pathMgr.GetFirstPoint();
		Transform spawnPoint = GameManager.instance.pathMgr.startPoint;

		// Spawn each enemy for this wave
		spawnedEnemies = new List<EnemyUnit>();
		for (int i = 0; i < SPAWNS_PER_WAVE; ++i) {
			// Create Enemy
			GameObject go = Instantiate (enemyPrefab, spawnPoint.position, Quaternion.identity);
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
			eu.SetDestination (firstPoint);
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

	void Start()
	{
		
	}

	void Update()
	{
		
	}
		
}
