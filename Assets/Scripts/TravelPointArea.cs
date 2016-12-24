using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class TravelPointArea : MonoBehaviour 
{
	private BoxCollider box;
	public float groundYPos = 0;

	public Vector3 GetTravelPointLocation()
	{
		float centerX = box.transform.position.x;
		float centerZ = box.transform.position.z;

		float xPos = centerX + Random.Range (-box.bounds.size.x/2, box.bounds.size.x/2);
		float xPosRounded = Mathf.RoundToInt(Mathf.MoveTowards (xPos, centerX, 0.5f));
		float zPos = Mathf.FloorToInt (centerZ + Random.Range (-box.bounds.size.z/2, box.bounds.size.z/2) + 0.5f);
		float zPosRounded = Mathf.RoundToInt(Mathf.MoveTowards (zPos, centerZ, 0.5f));
		float yPos = groundYPos;

		Debug.Log (xPos.ToString () + " -- " + xPosRounded.ToString ());
		Debug.Log (zPos.ToString () + " -- " + zPosRounded.ToString ());

		return new Vector3 (xPos, yPos, zPos);
	}


	void Awake()
	{
		box = GetComponent<BoxCollider> ();
	}
}
