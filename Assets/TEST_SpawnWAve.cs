using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_SpawnWAve : MonoBehaviour 
{
	public GameObject enemyPrefab;
	public Transform spawnPos;

	void OnGUI()
	{
		if (GUI.Button (new Rect (Screen.width - 100, 0, 100, 50), "Spawn guy")) {
			GameObject go = Instantiate (enemyPrefab, spawnPos.position, Quaternion.identity);
			go.GetComponent<EnemyUnit> ().SetDestination (GameObject.FindObjectOfType<TravelPoint> ());
		}
	}
}
