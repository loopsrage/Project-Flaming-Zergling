using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class Bullet : MonoBehaviour
{
	const float HIT_DISTANCE = 0.3f;
	public float rotateSpeed = 4;
	public float moveSpeed = 5;
	EnemyUnit target;
	Tower shooter;
	bool isTracking = false;

	public void Init(Tower _shooter, EnemyUnit _target)
	{
		shooter = _shooter;
		target = _target;
		isTracking = true;
	}
		
	private void HitDestroy()
	{
		Destroy (this.gameObject);
	}

	private void NoHitDestroy()
	{
		Destroy (this.gameObject);
	}

	void Awake()
	{
	}

	void Update()
	{
		if (!isTracking) {
			return;
		}
		if (target == null || !target.gameObject.activeInHierarchy) {
			NoHitDestroy ();
			return;
		}	
		transform.LookAt (target.transform.position);
		//transform.rotation = Quaternion.Slerp(transform.rotation,
		//	Quaternion.LookRotation(target.transform.position - transform.position), rotateSpeed*Time.deltaTime);
		
		transform.position += transform.forward * moveSpeed * Time.deltaTime;
		if (Vector3.Distance (transform.position, target.transform.position) < HIT_DISTANCE) {
			shooter.BulletHit (target);
			HitDestroy ();
		}
	}
}
