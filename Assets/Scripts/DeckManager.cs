using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
	const int NUM_CARDS = 65;
	public static DeckManager instance;
	private CardPanel drawPanel;

	public int numDrawsPurchased = 3;

	public Card[] cardSprites = new Card[NUM_CARDS];

	public Dictionary<string, Card> cards = new Dictionary<string, Card>();

	public Hand currentHand = new Hand ();

	public void AddCard(Card c)
	{
		currentHand.AddCard (c);
		drawPanel.AddCard (c, currentHand.cards.Count-1);
	}

	public void AddCardAtPos(Card c, int index)
	{
		currentHand.AddCardAt (c, index);
		drawPanel.AddCard (c, index);
	}

	public void HoldCard(Card c)
	{
		c.held = true;
	}

	public Hand PlayHand()
	{
		Hand playedHand = currentHand;
		drawPanel.ClearDisplays ();
		// TODO: abstract line below to a close or clean function
		currentHand = new Hand ();
		return playedHand;
	}

	public void DrawFive()
	{
		currentHand = new Hand ();

		List<Card> newCards = Get5RandomCards ();
		for (int i = 0; i < newCards.Count; ++i) {
			AddCard (newCards[i]);
		}
		currentHand.DetectHand ();
	}

	public List<Card> Get5RandomCards()
	{
		List<Card> newCards = new List<Card> (5);
		while (newCards.Count < 5) {
			Card c = GetRandomCard ();
			if (!newCards.Contains (c)) {
				newCards.Add (c);
			}
		}
		return newCards;
			
	}

	public Card GetRandomCard()
	{
		int suit = Random.Range (0, (int)SuitEnum.count);
		int rank = Random.Range (0, (int)RankEnum.count);
		string name = Card.GetName ((RankEnum)rank, (SuitEnum)suit);
		return cards [name];
	}

	public void Redraw()
	{
		if (numDrawsPurchased > 0) {
			numDrawsPurchased--;
			for (int i = 0; i < currentHand.cards.Count; ++i) {
				if (currentHand.cards [i].held == false) {
					Card replacement = GetRandomCard ();
					while (currentHand.cards.Contains (replacement)) { 
						replacement = GetRandomCard ();
					}
					AddCardAtPos (replacement, i);
				}
			}
			currentHand.DetectHand ();
		} else {
			
		}
	}

	public void PurchaseFreeDraw()
	{
		numDrawsPurchased += 1;
	}

	public void PurchaseRedraw()
	{
		// TODO: Check funds
		numDrawsPurchased++;
	}

	public void UnHoldCard(Card c)
	{
		c.held = false;
	}



	private void SetCards()
	{
		for (int i = 0; i < cardSprites.Length; ++i) {
			cards.Add (cardSprites [i].name, cardSprites [i]);
		}
	}

	private void SetInstance()
	{
		if (DeckManager.instance == null) {
			DeckManager.instance = this;
			DontDestroyOnLoad (this.gameObject);
		} else if (DeckManager.instance != this) {
			Destroy (this.gameObject);
		}
	}

	void Start()
	{
		

	}

	void Awake()
	{
		SetInstance ();
		SetCards ();
		drawPanel = GameObject.FindObjectOfType<CardPanel> ();
	}

}
