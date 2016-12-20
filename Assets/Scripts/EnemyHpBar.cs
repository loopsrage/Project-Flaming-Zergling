using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class EnemyHpBar : MonoBehaviour 
{
	private Slider slide;
	private EnemyUnit target;
	private bool isTracking = false;
	private float maxHp = 1;

	public void Init(EnemyUnit _target)
	{
		target = _target;
		maxHp = target.hp;
		target.onDeath.AddListener ((EnemyUnit eu) => DestroyMe());
		isTracking = true;
	}

	private void DestroyMe()
	{
		isTracking = false;
		// TODO: disable these objects instead of destroy to allow pooling
		Destroy (this.gameObject);
	}

	void Awake()
	{
		slide = GetComponent<Slider> ();
	}

	void Update () 
	{
		if (isTracking) {
			slide.value = (float)target.hp / maxHp;
			if (target.hp <= 0) {
				DestroyMe ();
			}
			transform.position = Camera.main.WorldToScreenPoint (target.transform.position);
		}
	}
}
