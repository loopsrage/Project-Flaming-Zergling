using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game manager.
/// In charge of flow.
/// 1. Show card panel
/// 2. Place tower
/// 3. Start Wave
/// 4. Wave Results
/// 	4.a If still alive
/// 	4.b Repeat
/// 	4.c If dead
/// 	4.d end
/// 5. End Game in Defeat or Victory
/// </summary>
public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public CardPanel cardPanel;

	public DeckManager deckManager;

	public TowerDB towers;

	public BuilderManager builder;

	public WaveManager waveMgr;

	public void HandPlayed(Hand h) 
	{
		// Disable the Card Panel
		cardPanel.gameObject.SetActive (false);
		// Get Tower based on hand type
		GameObject towerPrefab = towers.GetTowerPrefabByHand (h.type);
		// Create tower
		GameObject go = Instantiate (towerPrefab);
		// Start Building
		builder.StartBuilding (go);
	}
		
	private void EndBuild()
	{
		waveMgr.StartWave ();;
	}

	private void EndGame()
	{
		// End game
		Application.Quit();
	}

	private void EndWave()
	{
		// Check Death
		if (JudgeDeath ()) {
			EndGame ();
			return;
		}
		// Show wave Results
		ShowResults();

		// Start Next Round
		StartRound ();

	}
		
	private bool JudgeDeath()
	{
		// TODO: win conditions
		return false;
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

	private void ShowResults()
	{
		// TODO: Display panel for X seconds
	}

	private void StartRound()
	{
		cardPanel.gameObject.SetActive (true);
		deckManager.PurchaseFreeDraw ();
		cardPanel.FirstDraw ();
	}

	void Awake()
	{
		SetInstance ();
	}

	void Start()
	{
		// Get Modules / Managers
		deckManager = DeckManager.instance;
		towers = TowerDB.instance;
		builder = GameObject.FindObjectOfType<BuilderManager> ();
		waveMgr = GameObject.FindObjectOfType<WaveManager> ();
		cardPanel = GameObject.FindObjectOfType<CardPanel> ();

		// Add Event Listeners
		waveMgr.onWaveFinish.AddListener (EndWave);
		cardPanel.onPlayHand.AddListener (HandPlayed);
		builder.onBuild.AddListener (EndBuild);

		// TODO: put this somewhere else
		StartRound();
	}
}
