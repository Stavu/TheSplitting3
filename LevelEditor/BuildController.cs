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
			//Debug.Log (_furnitureName);
		}
	}




	public enum Mode
	{

		buildFurniture,
		buildCharacter,
		buildTileInteraction,
		inspect

	}

	public Mode mode = Mode.inspect;





	// Use this for initialization
	public void Initialize () 
	{
		EventsHandler.cb_editorTilesSelected += OnTilesSelected;

	}


	public void OnDestroy()
	{
		EventsHandler.cb_editorTilesSelected -= OnTilesSelected;

	}


	
	// Update is called once per frame
	void Update () 
	{
		
	}



	// ---- TILES SELECTED ---- //




	public void OnTilesSelected(List<Tile> tileList)
	{

		if (tileList.Count == 0) {
			return;
		}

		if (tileList [0] == null) {
			return;
		}


		switch (mode) 
		{
		
			case Mode.inspect:

				InspectTiles (tileList);
				return;

			
			case Mode.buildFurniture:

				EditorRoomManager.instance.PlaceFurniture (tileList [0], furnitureName);

				break;


			case Mode.buildTileInteraction:

				EditorRoomManager.instance.PlaceTileInteraction (tileList [0]);

				break;
		
		}


		// return to inspect mode

		mode = Mode.inspect;

	}





	public void InspectTiles(List<Tile> tileList)
	{
		
		Furniture currentFurniture = null;
		TileInteraction currentTileInteraction = null;

		currentFurniture = tileList [0].myFurniture;
		currentTileInteraction = tileList [0].myTileInteraction;

		InspectorManager.instance.chosenFurniture = currentFurniture;
		InspectorManager.instance.chosenTileInteraction = currentTileInteraction;

	}




}
