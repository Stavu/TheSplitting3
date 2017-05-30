using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameLineHandler : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{

		EventsHandler.cb_editorFurnitureModelChanged += CreateLine;


	}



	void OnDestroy()
	{

		EventsHandler.cb_editorFurnitureModelChanged -= CreateLine;
	}

	
	// Update is called once per frame
	void Update () 
	{
		
	}




	// Draw Line


	public void CreateLine(Furniture furn)
	{

		GameObject lineObj = new GameObject ("frameline_" + furn.myName);
		lineObj.transform.SetParent (this.transform);

		LineRenderer lr = lineObj.AddComponent<LineRenderer> ();
		lr.loop = true;
		lr.positionCount = 4;

		List<Vector3> positionList = Utilities.GetPhysicalInteractableFrameBounds (furn);

		Vector3[] posArray = new Vector3[4];

		/*
		for (int i = 1; i < positionList; i++) 
		{
			posArray [i-1] = positionList [i];
		}

		lr.SetPositions (posArray);
		*/

	}



}
