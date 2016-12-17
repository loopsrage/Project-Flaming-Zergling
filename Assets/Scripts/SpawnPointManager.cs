using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{

	private void CreateSpawnPoints()
	{
		
	}

	private SpawnPointArea[] GetSpawnPoints()
	{
		return GameObject.FindObjectsOfType<SpawnPointArea> ();		
	}
}
