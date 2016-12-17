using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CardDisplay : MonoBehaviour
{
	private Image img;

	public void SetCard(Card c)
	{
		img.sprite = c.sprite;
	}

	void Awake()
	{
		img = GetComponent<Image> ();
	}

}
