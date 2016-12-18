using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public DrawPanel cardPanel;

	public DeckManager deckManager;

	public TowerDB towers;

	public BuilderManager builder;

	public WaveManager waveMgr;

	public void HandPlayed(HandEnum h) 
	{
		cardPanel.gameObject.SetActive (false);
		GameObject towerPrefab = towers.GetTowerPrefabByHand (h);
		builder.onBuild.AddListener (EndBuild);
		GameObject go = Instantiate (towerPrefab);
		builder.StartBuilding (go);
		WaveManager wavemgr = GameObject.FindObjectOfType<WaveManager> ();
		wavemgr.onWaveFinish.AddListener (EndWave);
	}


	private void EndBuild()
	{
		builder.onBuild.RemoveListener (EndBuild);
		waveMgr.SpawnWave ();
	}

	private void EndWave()
	{
		cardPanel.gameObject.SetActive (true);
		cardPanel.FirstDraw ();
		deckManager.numDrawsPurchased += 1;
	}


	private void SetInstance()
	{
		if (GameManager.instance == null) {
			GameManager.instance = this;
			DontDestroyOnLoad (this.gameObject);
		} else if (GameManager.instance != this) {
			Destroy (this.gameObject);
		}
	}

	void Awake()
	{
		SetInstance ();
	}

	void Start()
	{
		cardPanel = GameObject.FindObjectOfType<DrawPanel> ();
		deckManager = DeckManager.instance;
		towers = TowerDB.instance;
		builder = GameObject.FindObjectOfType<BuilderManager> ();
		waveMgr = GameObject.FindObjectOfType<WaveManager> ();

		cardPanel.FirstDraw ();
	}
}
