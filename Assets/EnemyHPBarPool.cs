using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyHPBarPool : MonoBehaviour 
{
	public GameObject hpBarPrefab;


	public void AssignHpBarToEnemy(EnemyUnit eu)
	{
		EnemyHpBar hpBar = CreateHPBar ();
		hpBar.Init (eu);
	}

	private EnemyHpBar CreateHPBar()
	{
		// TODO: pool these objects instead of create/destroy
		GameObject go = Instantiate (hpBarPrefab, this.transform);
		EnemyHpBar hpBar = go.GetComponent<EnemyHpBar> ();
		if (hpBar == null) {
			throw new MissingComponentException ("Hp bar prefab has no enemy hp bar");
		}
		return hpBar;
	}
}
