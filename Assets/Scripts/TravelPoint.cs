using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelPoint : MonoBehaviour 
{
	public TravelPoint nextPoint;

	private void Exit(EnemyUnit e)
	{
		WaveManager wm = GameObject.FindObjectOfType<WaveManager> ();
		wm.EnemyLeaked ();
		e.Die ();
	}

	private void SetEnemyPointToNext(EnemyUnit e)
	{
		e.SetDestination (nextPoint);
	}

	void OnTriggerEnter(Collider other)
	{
		EnemyUnit e = other.GetComponent<EnemyUnit> ();
		if (e == null) {
			return;
		}

		if (nextPoint == null) {
			Exit (e);
		} else {
			SetEnemyPointToNext (e);
		}

	}
}
