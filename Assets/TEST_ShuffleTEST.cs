using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_ShuffleTEST : MonoBehaviour {

	void OnGUI()
	{
		if (GUI.Button (new Rect (0, Screen.height / 2, 100, 50), "Redraw")) {
			DeckManager.instance.DrawFive ();
		}
	}
}
