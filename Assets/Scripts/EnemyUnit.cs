using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
	public TravelPoint travelPoint;
	public NavMeshAgent agent;

	public int hp = 5;
	public int armor = 0;

	public void Die()
	{
		Destroy (this.gameObject);
	}

	public void SetDestination(TravelPoint tp)
	{
		agent.SetDestination (tp.transform.position);
	}

	public void TakeDamage(int dmg)
	{
		dmg = (dmg-armor > 0) ? dmg-armor : 0;
		hp -= dmg;
		if (hp <= 0) {
			Die ();
		}
			
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
