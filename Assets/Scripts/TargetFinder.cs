using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TargetFinder : MonoBehaviour 
{
	public SphereCollider col;
	public Tower t;

	void Awake()
	{
		col = GetComponent<SphereCollider> ();
		col.enabled = false;
	}
	public void Init(Tower _t)
	{
		t = _t;
		col.radius = t.range;
		transform.position = t.transform.position;
		col.enabled = true;
	}

	void OnTriggerEnter(Collider other)
	{
		if (t != null) {
			t.TargetEnteredArea (other);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (t != null) {
			t.TargetLeftArea (other);
		}
	}
}
