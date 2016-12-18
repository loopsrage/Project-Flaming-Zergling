using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDB : MonoBehaviour
{
	public static TowerDB instance;

	public GameObject highCardTowerPrefab;
	public GameObject pairTowerPrefab;
	public GameObject twoPairTowerPrefab;
	public GameObject threeKindTowerPrefab;
	public GameObject straightTowerPrefab;
	public GameObject flushTowerPrefab;
	public GameObject fullHouseTowerPrefab;
	public GameObject fourKindTowerPrefab;
	public GameObject straightFlushTowerPrefab;
	public GameObject royalFlushTowerPrefab;

	public GameObject GetTowerPrefabByHand(HandEnum h)
	{
		switch (h) {

		case HandEnum.highCard:
			return highCardTowerPrefab;
		case HandEnum.pair:
			return pairTowerPrefab;
		case HandEnum.twoPair:
			return twoPairTowerPrefab;
		case HandEnum.threeOfAKind:
			return threeKindTowerPrefab;
		case HandEnum.straight:
			return straightTowerPrefab;
		case HandEnum.flush:
			return flushTowerPrefab;
		case HandEnum.fullHouse:
			return fullHouseTowerPrefab;
		case HandEnum.fourOfAKind:
			return fourKindTowerPrefab;
		case HandEnum.straightFlush:
			return straightFlushTowerPrefab;
		case HandEnum.royalFlush:
			return royalFlushTowerPrefab;
		default:
			return highCardTowerPrefab;
		}
	}

	private void SetInstance()
	{
		if (TowerDB.instance == null) {
			TowerDB.instance = this;
			DontDestroyOnLoad (this.gameObject);
		} else if (TowerDB.instance != this) {
			Destroy (this.gameObject);
		}
	}

	void Awake()
	{
		SetInstance ();
	}

}
