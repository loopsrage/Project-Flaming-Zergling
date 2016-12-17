using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderManager : MonoBehaviour 
{
	const float GRID_SIZE = 1.0f;
	public string towerTag = "tower";

	public GameObject OKStatusIndicatorPrefab;
	public GameObject BLOCKEDStatusIndicatorPrefab;

	private GameObject OKInidicatorObj;
	private GameObject BlockedIndicitaroObj;

	private bool isBuilding = false;
	private bool isBlocked = false;

	private GameObject currentStatusObject;


	private GameObject currentBuilding;


	public void StartBuilding(GameObject objToBuild)
	{
		currentBuilding = objToBuild;
		currentBuilding.GetComponent<Collider> ().enabled = false;
		currentStatusObject = OKInidicatorObj;
		OKInidicatorObj.SetActive (true);
		isBuilding = true;

	}

	private void BuildObject()
	{
		isBuilding = false;
		OKInidicatorObj.SetActive (false);
		BlockedIndicitaroObj.SetActive (false);
		currentBuilding.GetComponent<Collider> ().enabled = true;
		currentBuilding = null;
	}

	private void SwapStatusIndicator()
	{
		isBlocked = !isBlocked;
		OKInidicatorObj.SetActive (!isBlocked);
		BlockedIndicitaroObj.SetActive (isBlocked);
		currentStatusObject = (isBlocked) ? BlockedIndicitaroObj : OKInidicatorObj;
	}

	private void UpdateIndicatorPosition(Vector3 mousePos)
	{
		var x = Mathf.Round (mousePos.x / GRID_SIZE) * GRID_SIZE;
		var z = Mathf.Round (mousePos.z / GRID_SIZE) * GRID_SIZE;
		var y = 0;
		currentStatusObject.transform.position = new Vector3 (x, y, z);
		currentBuilding.transform.position = new Vector3 (x, y, z);
	}



	void Update()
	{
		if (isBuilding) {
			// Check if being Blocked
			Ray r =	Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(r, out hit)) {
				if (hit.collider.tag == towerTag) {
					if (!isBlocked) {
						SwapStatusIndicator ();
					}
				} else {
					if (isBlocked) {
						SwapStatusIndicator ();
					}
				}
				// Update Position
				UpdateIndicatorPosition(hit.point);
			}

			// Check Build
			if (Input.GetMouseButtonDown (0) && !isBlocked) {
				BuildObject ();
			}

		}
	}

	void Start()
	{
		// Create ok Indicator
		OKInidicatorObj = Instantiate (OKStatusIndicatorPrefab);
		OKInidicatorObj.SetActive (false);

		// Create blocked Indicator
		BlockedIndicitaroObj = Instantiate (BLOCKEDStatusIndicatorPrefab);
		BlockedIndicitaroObj.SetActive (false);
	}

}
