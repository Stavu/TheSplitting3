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




	public void OnTilesSelected(Tile tile)
	{
		/*
		if (tileList.Count == 0) {
			return;
		}
		*/

		if (tile == null) {
			return;
		}


		switch (mode) 
		{
		
			case Mode.inspect:

				InspectTiles (tile);
				return;

			
			case Mode.buildFurniture:

				EditorRoomManager.editorFurnitureHandler.PlaceFurniture (tile, furnitureName);

				break;


			case Mode.buildTileInteraction:

				EditorRoomManager.editorTileInteractionHandler.PlaceTileInteraction (tile);

				break;
		
		}


		// return to inspect mode

		mode = Mode.inspect;

	}





	public void InspectTiles(Tile tile)
	{

		Debug.Log ("Inspect tiles");


		Furniture currentFurniture = null;
		TileInteraction currentTileInteraction = null;


		currentFurniture = tile.myFurniture;
		currentTileInteraction = tile.myTileInteraction;


		InspectorManager.instance.chosenTileInteraction = currentTileInteraction;
		InspectorManager.instance.chosenFurniture = currentFurniture;



		if (currentFurniture == null) 
		{
			Debug.Log ("furniture = null");

		}

		if (currentTileInteraction == null) 
		{

			Debug.Log ("tileInteraction = null");
		}

	}




}
