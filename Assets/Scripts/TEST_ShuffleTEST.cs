using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_ShuffleTEST : MonoBehaviour {
	public bool hasDrawn = false;
	private string drawText = "Draw Again";

	void OnGUI()
	{
		if (GUI.Button (new Rect (0, Screen.height / 2, 100, 50), drawText + " x" + DeckManager.instance.numDrawsPurchased.ToString ())) {
			drawText = "Redraw";
			if (!hasDrawn) {
				DeckManager.instance.DrawFive ();
				hasDrawn = true;
			} else {
				DeckManager.instance.Redraw ();
			}
		}

		if (hasDrawn) {
			if (GUI.Button (new Rect (Screen.width - 100, Screen.height / 2, 100, 50), "Cash In")) {
				HandEnum hand = DeckManager.instance.PlayHand ();
				print (hand.ToString ());
			}
		}
	}


}
