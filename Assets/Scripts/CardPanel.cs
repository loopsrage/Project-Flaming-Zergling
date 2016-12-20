using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardPanel : MonoBehaviour 
{
	const string DRAW_TEXT = "Draw";
	public Button redrawBtn;
	public Text redrawText;

	public CardDisplay[] displays;

	public class CardPanelEvent : UnityEvent<Hand>{}
	public CardPanelEvent onPlayHand = new CardPanelEvent();

	public void AddCard(Card c, int index)
	{
		displays [index].SetCard (c);
	}

	public void ClearDisplays()
	{
		for (int i = 0; i < displays.Length; ++i) {
			displays [i].Clear ();
		}
	}

	public void FirstDraw()
	{
		DeckManager.instance.DrawFive ();
		if (DeckManager.instance.numDrawsPurchased > 0) {
			redrawBtn.enabled = true;
		} else {
			redrawBtn.enabled = false;
		}
		UpdateDrawText ();
	}

	public void PlayHand()
	{
		Hand playedHand = DeckManager.instance.PlayHand ();
		onPlayHand.Invoke (playedHand);
	}

	public void Redraw()
	{
		DeckManager.instance.Redraw ();
		UpdateDrawText ();
		if (DeckManager.instance.numDrawsPurchased <= 0) {
			redrawBtn.enabled = false;
		}
	}



	private void UpdateDrawText()
	{
		redrawText.text = DRAW_TEXT + " x" + DeckManager.instance.numDrawsPurchased.ToString();
	}

	void Start()
	{
		
	}
}
