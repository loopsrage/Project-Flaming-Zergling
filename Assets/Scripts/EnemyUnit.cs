using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
	public TravelPoint travelPoint;
	public NavMeshAgent agent;

	public void SetDestination(TravelPoint tp)
	{
		agent.SetDestination (tp.transform.position);
	}

	void Awake()
	{
		agent = GetComponent<NavMeshAgent> ();
	}

	void Start()
	{
		if (travelPoint != null) {
			SetDestination (travelPoint);
		}
	}


}
