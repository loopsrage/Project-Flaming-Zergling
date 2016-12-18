using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Tower : MonoBehaviour 
{
	private BuilderManager bm;
	public GameObject targetFinderPrefab;
	public GameObject bulletPrefab;
	public int atkSpeed;
	public int atkDmg;
	public float range;
	private float attackWaitTime {get{ return 4 - atkSpeed;}}
	private float nextAttackTime = 0;

	public enum state {
		Placement,
		Active,
		Idle
	}

	public state currentState = state.Idle;

	private Collider col;
	private TargetFinder tf;



	public EnemyUnit currentTarget;
	public List<EnemyUnit> possibleTargets = new List<EnemyUnit>();

	public void Build()
	{
		SetState (state.Active);
		tf.Init (this);
	}

	public void BulletHit(EnemyUnit eu)
	{
		Debug.Log ("HIT");
		eu.TakeDamage (atkDmg);
	}

	public void TargetEnteredArea(Collider other)
	{
		if (other.tag == "enemy") {
			// Set Target If it doesn't exist
			EnemyUnit eu = other.GetComponent<EnemyUnit>();
			if (eu == null) { 
				return;
			}
			possibleTargets.Add (eu);
			if (currentTarget == null) {
				SetTarget (eu);
			}
		}
	}
	public void TargetLeftArea(Collider other)
	{
		if (other.tag == "enemy") {
			// If this is our current target aquire a new one
			EnemyUnit eu = other.GetComponent<EnemyUnit>();
			if (eu == null) {
				return;
			}
			if (possibleTargets.Contains (eu)) {
				possibleTargets.Remove (eu);
			}
			if (currentTarget == eu) {
				FindNextTarget ();
			}
		}
	}

	private void Attack()
	{
		// Create bullet
		GameObject go = Instantiate (bulletPrefab, transform.position, transform.rotation);
		Bullet bill = go.GetComponent<Bullet> ();
		bill.Init (this, currentTarget);
		nextAttackTime = Time.time + attackWaitTime;
	}

	private void FindNextTarget()
	{
		currentTarget = null;
		if (possibleTargets.Count > 0) {
			SetTarget(possibleTargets[0]);
		}
	}

	public void SetState(state s)
	{
		currentState = s;
		if (s == state.Placement) {
			ToggleCollider (true);
		} else {
			ToggleCollider (false);
		}

	}

	private void SetTarget(EnemyUnit target)
	{
		currentTarget = target;
	}

	private void ToggleCollider(bool on)
	{
		col.isTrigger = on;
		tf.col.enabled = !on;
	}

	void UpdateForStatePlacement()
	{
		
	}

	void UpdateForStateActive()
	{
		if (currentTarget != null && Time.time >= nextAttackTime) {
			Attack ();
		}
			
	}

	void UpdateForStateIdle()
	{
		
	}

	void Awake()
	{
		col = GetComponent<Collider> ();
		GameObject go = Instantiate (targetFinderPrefab, transform.position, transform.rotation);
		tf = go.GetComponent<TargetFinder> ();
	}

	void Start()
	{
		bm = GameObject.FindObjectOfType<BuilderManager> ();

	}

	void Update()
	{
		Invoke ("UpdateForState" + currentState.ToString (), 0.0f);
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "tower" || other.tag == "travelPoint") {
			Debug.Log ("Trigger Enter");
			bm.SetBlocked ();
		} 
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "tower" || other.tag == "travelPoint") {
			Debug.Log ("TriggerExit");
			bm.SetUnBlocked ();
		} 
	}

}
