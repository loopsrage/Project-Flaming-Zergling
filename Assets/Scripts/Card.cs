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
	public bool held;
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