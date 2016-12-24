using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.Events;

public class EnemyUnit : MonoBehaviour
{
	public TravelPoint travelPoint;
	public NavMeshAgent agent;

	public int hp = 4;
	public int armor = 0;

	public bool isDead = false;

	public class EnemyEvent : UnityEvent<EnemyUnit>{}
	public EnemyEvent onDeath = new EnemyEvent();

	public void Create()
	{
		
	}

	public void Die()
	{
		isDead = true;
		onDeath.Invoke (this);
		this.gameObject.SetActive (false);
	}

	public void SetDestination(TravelPoint tp)
	{
		agent.SetDestination (tp.transform.position);
	}

	public bool TakeDamage(int dmg)
	{
		if (isDead) {
			return false;
		}
		dmg = (dmg-armor > 0) ? dmg-armor : 1;
		hp -= dmg;
		if (hp <= 0) {
			Die ();
			return true;
		}
		return false;
			
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
