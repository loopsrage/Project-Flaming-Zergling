using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathVisualizer : MonoBehaviour 
{
	public LineRenderer[] lines = new LineRenderer[3];

	public void DrawPaths(List<NavMeshPath> paths)
	{
		for (int i = 0; i < paths.Count; ++i) {
			DrawPath (paths [i], lines[i]);
		}
	}

	public void DrawPath(NavMeshPath path, LineRenderer line)
	{
		if (path.corners.Length < 1) { //if the path has 1 or no corners, there is no need
			Debug.Log("oh");
			return;
		}



		line.numPositions = path.corners.Length; //set the array of positions to the amount of corners

		for(var i = 0; i < path.corners.Length; i++){
			line.SetPosition(i, path.corners[i]); //go through each corner and set that to the line renderer's position
		}
	}



	public T CopyComponent<T>(T original, GameObject destination) where T : Component
	{
		System.Type type = original.GetType();
		Component copy = destination.AddComponent(type);
		System.Reflection.FieldInfo[] fields = type.GetFields();
		foreach (System.Reflection.FieldInfo field in fields)
		{
			field.SetValue(copy, field.GetValue(original));
		}
		return copy as T;
	}

	void Awake()
	{

	}
}
