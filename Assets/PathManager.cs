using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour 
{
	public GameObject travelPointPrefab;
	public Transform startPoint;
	public TravelPointArea[] areas;
	private TravelPoint[] travelPoints;


	public void SetupPath()
	{
		CreatePaths (areas);
	}

	private void CreatePaths(TravelPointArea[] travelAreas)
	{
		// Create travel point array
		travelPoints = new TravelPoint[travelAreas.Length];

		// Create random spot in each area and assign 
		TravelPoint previousPoint = null;//new TravelPoint();
		// Traverse backwards and assign next points
		for (int i = travelPoints.Length - 1; i >= 0; --i) {
			TravelPoint tp = CreateTravelPointFromArea (travelAreas [i]);
			if (i != travelPoints.Length) {
				tp.nextPoint = previousPoint;
			}
			previousPoint = travelPoints [i] = tp;
		}
	}

	public TravelPoint GetFirstPoint()
	{
		if (travelPoints == null) {
			return null;
		}
		return travelPoints [0];
	}


	private TravelPoint CreateTravelPointFromArea(TravelPointArea area)
	{
		Vector3 travelPointLocation =  area.GetTravelPointLocation ();
		GameObject go = Instantiate (travelPointPrefab, travelPointLocation, Quaternion.identity);
		TravelPoint tp = go.GetComponent<TravelPoint> ();
		if (tp == null) {
			throw new MissingComponentException ("Travel point prefab doesnt have a travel point component");
		}
		return tp;
	}

}
