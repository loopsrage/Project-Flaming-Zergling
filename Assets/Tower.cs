using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Tower : MonoBehaviour 
{
	private BuilderManager bm;

	public enum state {
		Placement,
		Active,
		Idle
	}

	public state currentState = state.Idle;

	private Collider col;


	public void SetState(state s)
	{
		currentState = s;
		if (s == state.Placement) {
			ToggleCollider (true);
		} else {
			ToggleCollider (false);
		}

	}

	private void ToggleCollider(bool on)
	{
		col.isTrigger = on;
	}




	void UpdateForStatePlacement()
	{
		
	}

	void UpdateForStateActive()
	{
	}

	void UpdateForStateIdle()
	{
		
	}

	void Awake()
	{
		col = GetComponent<Collider> ();
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
