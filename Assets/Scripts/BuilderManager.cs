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
	private GridOverlay overlay;

	private bool isBuilding = false;
	private bool isBlocked = false;

	private GameObject currentStatusObject;
	private Tower currentBuilding;

	private float afterStartWaitTime = 0.2f;
	private float afterStartWaitElapsed = 0.0f;

	public class BuildEvent : UnityEvent{}
	public BuildEvent onBuild = new BuildEvent ();


	public void StartBuilding(GameObject objToBuild)
	{
		afterStartWaitElapsed = 0;
		currentBuilding = objToBuild.GetComponent<Tower>();
		currentStatusObject = OKInidicatorObj;
		OKInidicatorObj.SetActive (true);
		isBuilding = true;
		confirmBtn.SetActive (true);
		currentBuilding.SetState (Tower.state.Placement);
		UpdateIndicatorPosition (Vector3.zero);
		overlay.enabled = true;
	}

	private void BuildObject()
	{
		isBuilding = false;
		OKInidicatorObj.SetActive (false);
		BlockedIndicitaroObj.SetActive (false);
		currentBuilding.Build ();
		currentBuilding = null;
		overlay.enabled = false;
		onBuild.Invoke ();
	}

	public void PlaceBuilding()
	{
		confirmBtn.SetActive (false);
		BuildObject ();
	}
		
	public void SetBlocked()
	{
		isBlocked = true;
		OKInidicatorObj.SetActive (false);
		BlockedIndicitaroObj.SetActive (true);
		currentStatusObject = BlockedIndicitaroObj;
	}

	public void SetUnBlocked()
	{
		isBlocked = false;
		OKInidicatorObj.SetActive (true);
		BlockedIndicitaroObj.SetActive (false);
		currentStatusObject = OKInidicatorObj;
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

	void Awake()
	{
		overlay = GameObject.FindObjectOfType<GridOverlay> ();
	}

	void Update()
	{
		if (afterStartWaitElapsed < afterStartWaitTime) {
			afterStartWaitElapsed += Time.deltaTime;
			return;
		}
		if (isBuilding) {
			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor) {
				// Check if being Blocked
				Ray r =	Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (r, out hit)) {
					// Update Position
					UpdateIndicatorPosition (hit.point);
				}


				// Check Build
				if (Input.GetMouseButtonDown (0) && !isBlocked) {
					PlaceBuilding ();
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
