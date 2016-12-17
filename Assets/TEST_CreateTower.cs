using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_CreateTower : MonoBehaviour
{
	public BuilderManager bm;
	public GameObject towerPrefab;


	void OnGUI()
	{
		if (GUI.Button (new Rect (0, 0, 100, 50), "Build")) {
			GameObject go = Instantiate (towerPrefab);
			bm.StartBuilding (go);
		}
	}
}
