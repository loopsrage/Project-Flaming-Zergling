using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HandEnum {
	highCard,
	pair,
	twoPair,
	threeOfAKind,
	straight,
	flush,
	fullHouse,
	fourOfAKind,
	straightFlush,
	royalFlush
}

[System.Serializable]
public class Hand
{
	public List<Card> cards = new List<Card>(5);
	public HandEnum type;

	private bool isRoyal;
	private Dictionary<int,int> pairs;
	private List<Card> currentBest;

	public Hand()
	{
	}

	public Hand(List<Card> c)
	{
		cards = c;
	}

	public void AddCard(Card c)
	{
		cards.Add (c);
	}

	public HandEnum DetectHand()
	{
		type = HandEnum.highCard;
		GetPairs();

		bool isFlush = CheckFlush ();
		bool isStraight = CheckStraight ();

		HandEnum pairsHand = CheckPairs ();

		// **** Royal Flush *****
		if (isFlush && isStraight) {
			type = (isRoyal) ? HandEnum.royalFlush : HandEnum.straightFlush;

			// **** Four of a Kind *****
		} else if (pairsHand == HandEnum.fourOfAKind) {
			type = HandEnum.fourOfAKind;

			// **** Full House *****
		} else if (pairsHand == HandEnum.fullHouse) {
			type = HandEnum.fullHouse;

			// **** Flush *****
		} else if (isFlush) {
			type = HandEnum.flush;

			// **** Straight *****
		} else if (isStraight) {
			type = HandEnum.straight;

			// **** Three of a kind *****
		} else if (pairsHand == HandEnum.threeOfAKind) {
			type = HandEnum.threeOfAKind;

			// **** Two Pair *****
		} else if (pairsHand == HandEnum.twoPair) {
			type = HandEnum.twoPair;

			// **** Pair *****
		} else if (pairsHand == HandEnum.pair) {
			type = HandEnum.pair;
		}

		return type;
	}

	public HandEnum GetBetterHand(HandEnum h1, HandEnum h2)
	{
		return ((int)h1 > (int) h2) ? h1 : h2;
	}

	public List<Card> GetWinningCards()
	{
		return new List<Card> ();

	}

	private bool CheckFlush()
	{
		SuitEnum suit = cards[0].suit;
		for (int i = 0; i < cards.Count; ++i) {
			if (cards [i].suit != suit) {
				return false;
			}
		}
		return true;
	}

	private HandEnum CheckPairs()
	{
		HandEnum returnType = HandEnum.highCard;
		List<int> keys = new List<int>(pairs.Keys);

		// Check 4/3/2 of a kind
		for (int i = 0; i < keys.Count; ++i) {
			if (pairs [keys [i]] == 4) { 
				returnType = GetBetterHand (HandEnum.fourOfAKind, returnType);
			} else if (pairs [keys [i]] == 3) {
				returnType = GetBetterHand (HandEnum.threeOfAKind, returnType);
			} else {
				returnType = GetBetterHand (HandEnum.pair, returnType);
			}
		}

		// Check 2 pair
		if (pairs.Count > 1) {
			bool fullHouse = ((pairs[keys[0]] == 3 && pairs[keys[1]] == 2) || (pairs[keys[1]] == 3 && pairs[keys[0]] == 2));
			if (fullHouse) {
				returnType = GetBetterHand (HandEnum.fullHouse, returnType);
			} else {
				returnType =  GetBetterHand (HandEnum.twoPair, returnType);
			}
		} 

		return returnType;
	}

	private bool CheckStraight()
	{
		List<int> sortedHandValues = GetSortedCardRanks ();

		// Walk our hand
		bool isStraight = WalkHandForAStraight(sortedHandValues);

		// Check Ace Edge case. [10,j,q,k,a]
		if (!isStraight && sortedHandValues [0] == (int)RankEnum.ace) {
			
			// Swap ace to the back
			sortedHandValues [0] = sortedHandValues[sortedHandValues.Count-1];
			sortedHandValues [sortedHandValues.Count - 1] = (int)RankEnum.king + 1;
			sortedHandValues.Sort ();

			isStraight = WalkHandForAStraight (sortedHandValues);
			if (isStraight) {
				isRoyal = true;
			}
		}

		return isStraight;
	}

	private void GetPairs()
	{
		pairs = new Dictionary<int, int> ();
		List<int> sortedHandValues = GetSortedCardRanks ();

		for (int i = 0; i < sortedHandValues.Count; ++i) {
			for (int j = i+1; j < sortedHandValues.Count; ++j) {
				if (sortedHandValues [i] == sortedHandValues [j]) {
					if (pairs.ContainsKey(sortedHandValues[i])) {
						pairs [sortedHandValues [i++]] += 1;
					} else {
						pairs.Add(sortedHandValues[i++], 2);
					}
				}
			}
		}
	}

	private List<int> GetSortedCardRanks()
	{
		// Sort our hand
		List<int> sortedHandValues = new List<int>(5);
		for (int i = 0; i < cards.Count; ++i) {
			Card c = cards [i];
			RankEnum r = c.rank;
			sortedHandValues.Add ((int)r);
		}
		sortedHandValues.Sort ();
		return sortedHandValues;
	}

	private bool WalkHandForAStraight(List<int> sortedHandValues)
	{
		int lastVal = sortedHandValues[0];
		for (int i = 1; i < sortedHandValues.Count; ++i) {
			if (sortedHandValues [i] != lastVal + 1) { 
				return false;
			}
			lastVal = sortedHandValues [i];
		}
		return true;
	}




}
