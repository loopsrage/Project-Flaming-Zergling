using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuilderManager : MonoBehaviour 
{
	const float GRID_SIZE = 1.0f;
	public string towerTag = "tower";

	public GameObject OKStatusIndicatorPrefab;
	public GameObject BLOCKEDStatusIndicatorPrefab;

	private GameObject OKInidicatorObj;
	private GameObject BlockedIndicitaroObj;

	public GameObject confirmBtn;

	private bool isBuilding = false;
	private bool isBlocked = false;

	private GameObject currentStatusObject;

	private float afterStartWaitTime = 0.2f;
	private float afterStartWaitElapsed = 0.0f;
	private GameObject currentBuilding;

	public class BuildEvent : UnityEvent{}
	public BuildEvent onBuild = new BuildEvent ();


	public void StartBuilding(GameObject objToBuild)
	{
		afterStartWaitElapsed = 0;
		currentBuilding = objToBuild;
		currentBuilding.GetComponent<Collider> ().enabled = false;
		currentStatusObject = OKInidicatorObj;
		OKInidicatorObj.SetActive (true);
		isBuilding = true;
		confirmBtn.SetActive (true);

	}

	private void BuildObject()
	{
		isBuilding = false;
		OKInidicatorObj.SetActive (false);
		BlockedIndicitaroObj.SetActive (false);
		currentBuilding.GetComponent<Collider> ().enabled = true;
		currentBuilding = null;
		onBuild.Invoke ();
	}

	private void ConfirmBuild()
	{
		OKInidicatorObj.SetActive(false);
		BlockedIndicitaroObj.SetActive (false);
		isBuilding = false;
	}

	public void PlaceBuilding()
	{
		confirmBtn.SetActive (false);
		BuildObject ();
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
		if (afterStartWaitElapsed < afterStartWaitTime) {
			afterStartWaitElapsed += Time.deltaTime;
			return;
		}
		if (isBuilding) {
			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXEditor) {
				// Check if being Blocked
				Ray r =	Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (r, out hit)) {
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
					UpdateIndicatorPosition (hit.point);
				}

				// Check Build
				if (Input.GetMouseButtonDown (0) && !isBlocked) {
					ConfirmBuild ();
				}
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
