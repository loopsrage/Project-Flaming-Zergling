using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawPanel : MonoBehaviour 
{
	const string DRAW_TEXT = "Draw";
	public Button redrawBtn;
	public Text redrawText;

	public CardDisplay[] displays;

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
			UpdateDrawText ();
		}
	}

	public void PlayHand()
	{
		GameManager.instance.HandPlayed(DeckManager.instance.PlayHand ());

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
		redrawText.text = DRAW_TEXT + " x" + DeckManager.instance.numDrawsPurchased;
	}

	void Start()
	{
		FirstDraw ();
	}
}
