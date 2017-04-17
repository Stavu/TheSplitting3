using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour {



	// Singleton //

	public static BuildController instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

	}

	// Singleton //



	string _furnitureName;

	public string furnitureName 
	{ 
		get 
		{
			return _furnitureName;
		}

		set 
		{
			_furnitureName = value;
			Debug.Log (_furnitureName);
		}
	}




	public enum Mode
	{

		buildFurniture,
		buildCharacter,
		inspect

	}

	public Mode mode = Mode.inspect;





	// Use this for initialization
	public void Initialize () 
	{
		EventsHandler.cb_editorTilesSelected += BuildFurniture;

	}


	public void OnDestroy()
	{
		EventsHandler.cb_editorTilesSelected -= BuildFurniture;

	}


	
	// Update is called once per frame
	void Update () {
		
	}


	public void BuildFurniture(List<Tile> tileList)
	{

		//Debug.Log ("BuildFurniture");


		if (mode == Mode.inspect) 
		{
			InspectFurniture (tileList);
			return;
		}

		if (tileList.Count == 0) {
			return;
		}

		if (tileList [0] == null) {
			return;
		}


		EditorRoomManager.instance.PlaceFurniture (tileList [0], furnitureName);



		// return to inspect mode

		mode = Mode.inspect;

	}



	public void InspectFurniture(List<Tile> tileList)
	{
		
		if (mode != Mode.inspect) 
		{
			return;
		}


		Furniture currentFurniture = null;

		if (tileList.Count > 0) 
		{

			currentFurniture = tileList [0].myFurniture;
			
		} 


		InspectorManager.instance.chosenFurniture = currentFurniture;


	}


}
