using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameLineHandler : MonoBehaviour {



	List<GameObject> lineContainer = new List<GameObject>();




	// Use this for initialization
	void Start () 
	{
		
		EventsHandler.cb_tileLayoutChanged += CreateLines;

	}



	void OnDestroy()
	{

		EventsHandler.cb_tileLayoutChanged -= CreateLines;

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
		Debug.Log ("center" + positionList [0]);


		Vector3[] posArray = new Vector3[4];

		/*
		for (int i = 1; i < positionList; i++) 
		{
			posArray [i-1] = positionList [i];
		}
		*/


		//lr.SetPositions (posArray);


	}





	public void CreateLines()
	{

		lineContainer.ForEach (obj => Destroy (obj));
		lineContainer.Clear ();

		foreach (Furniture furn in EditorRoomManager.instance.furnitureGameObjectMap.Keys) 
		{

			GameObject lineObj = new GameObject ("frameline_" + furn.myName);
			lineObj.transform.SetParent (this.transform);

			LineRenderer lr = lineObj.AddComponent<LineRenderer> ();
			lr.loop = true;
			lr.positionCount = 4;
			lr.widthMultiplier = 0.1f;


			List<Vector3> positionList = Utilities.EditorGetPhysicalInteractableFrameBounds (furn);
			Debug.Log ("center" + positionList [0]);


			Vector3[] posArray = new Vector3[4];


			for (int i = 1; i < positionList.Count; i++) 
			{
				posArray [i - 1] = positionList [i] + positionList [0] + new Vector3 (furn.frameOffsetX, furn.frameOffsetY,-5);
			}


			lr.SetPositions (posArray);

			lineContainer.Add (lineObj);

		}





	}




}
