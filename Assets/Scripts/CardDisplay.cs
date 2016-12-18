using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CardDisplay : MonoBehaviour
{
	public GameObject heldImage;

	private Sprite emptyImg;
	private DeckManager deck;
	private Image img;
	private bool isHeld = false;
	private Card myCard;

	public void Clear()
	{
		UnHold ();
		img.sprite = emptyImg;
		myCard = null;
	}

	public void Hold()
	{
		isHeld = true;
		heldImage.SetActive (true);
		deck.HoldCard (myCard);
	}
		
	public void SetCard(Card c)
	{
		myCard = c;
		img.sprite = c.sprite;
	}

	public void ToggleHold()
	{
		if (isHeld) {
			UnHold ();
		} else {
			Hold ();
		}
	}

	public void UnHold()
	{
		isHeld = false;
		heldImage.SetActive (false);
		deck.UnHoldCard(myCard);
	}


	void Awake()
	{
		img = GetComponent<Image> ();
		emptyImg = img.sprite;
	}

	void Start()
	{
		deck = DeckManager.instance;

	}

}
