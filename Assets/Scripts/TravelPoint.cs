using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class TravelPoint : MonoBehaviour 
{
	public TravelPoint nextPoint;
	public NavMeshAgent agent;

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

	void Awake()
	{
		agent = GetComponent<NavMeshAgent> ();
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
