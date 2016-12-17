using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum SuitEnum {
	diamonds,
	hearts,
	spades,
	clubs,
	count,
}

public enum RankEnum {
	ace,two,three,four,five,six,seven,eight,nine,ten,
	jack,
	queen,
	king,
	count,
	any,
	blank,
	joker_face,
}



[System.Serializable]
public class Card 
{
	public RankEnum rank;
	public SuitEnum suit;
	public Sprite sprite;
	public string name
	{
		get{ return  Card.GetName (rank, suit);}
	}
	public static string GetName(RankEnum r, SuitEnum s)
	{
		return r.ToString () + "_" + s.ToString ();
	}
	new	public string ToString()
	{
		return name;
	}
}

public class DeckManager : MonoBehaviour
{
	const int NUM_CARDS = 65;
	public static DeckManager instance;

	public CardDisplay[] displays;

	public Card[] cardSprites = new Card[NUM_CARDS];

	public Dictionary<string, Card> cards = new Dictionary<string, Card>();

	public Hand currentHand = new Hand ();

	public void AddCard(Card c)
	{
		currentHand.AddCard (c);
		displays [currentHand.cards.Count - 1].SetCard (c);
	}

	public void DrawFive()
	{
		currentHand = new Hand ();

//		// TEMP
//		AddCard (cards ["five_diamonds"]);
//		AddCard (cards ["five_spades"]);
//		AddCard (cards ["four_diamonds"]);
//		AddCard (cards ["five_hearts"]);
//		AddCard (cards ["three_clubs"]);
//		currentHand.DetectHand ();
//		return;
//		// TEMP

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
			int suit = Random.Range (0, (int)SuitEnum.count);
			int rank = Random.Range (0, (int)RankEnum.count);
			string name = Card.GetName ((RankEnum)rank, (SuitEnum)suit);
			Card c = cards [name];
			if (!newCards.Contains (c)) {
				newCards.Add (c);
			}
		}
		return newCards;
			
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
	}

}
