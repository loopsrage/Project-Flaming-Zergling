using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathManager : MonoBehaviour 
{
	public GameObject travelPointPrefab;

	public TravelPointArea[] areas;
	private TravelPoint[] travelPoints;
	private List<NavMeshPath> _paths;
	public List<NavMeshPath> paths { get { return _paths; } }

	public TravelPoint startPoint {get{ return travelPoints [0];}}

	/// <summary>
	/// Checks if the path is blocked.
	/// </summary>
	/// <returns><c>true</c>, if path is blocked<c>false</c> otherwise.</returns>
	public bool CheckPathBlocked()
	{
		GetPaths ();

		for (int i = 0; i < _paths.Count; ++i) {
			if (_paths[i].status != NavMeshPathStatus.PathComplete) {
				return false;
			}
		}
		return true;
	}

	public Vector3 GetStartPoint()
	{
		return startPoint.transform.position;
	}

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

		GetPaths ();
	}

	private void GetPaths()
	{
		// Check start to point 1, then point 1 to point n... 
		NavMeshAgent pos_i = startPoint.agent;
		_paths = new List<NavMeshPath>(travelPoints.Length){};

		for (int i = 1; i < travelPoints.Length; ++i) {
			_paths.Add(new NavMeshPath ());
			pos_i.CalculatePath ( travelPoints [i].transform.position, _paths[i-1]);	
			pos_i = travelPoints [i].agent;
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
