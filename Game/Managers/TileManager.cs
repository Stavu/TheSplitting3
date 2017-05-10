using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {



	// Singleton //

	public static TileManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //



	public GameObject TilePrefab;
	public Dictionary<Tile,GameObject> tileGameObjectMap;



	// Use this for initialization
	public void Initialize () 
	{	
		EventsHandler.cb_furnitureChanged += ColorTiles;
		EventsHandler.cb_characterChanged += ColorTiles;
		EventsHandler.cb_tileInteractionChanged += ColorTiles;

		EventsHandler.cb_roomCreated += CreateTileObject;

		tileGameObjectMap = new Dictionary<Tile, GameObject>();
					
	}


	public void OnDestroy()
	{
		EventsHandler.cb_furnitureChanged -= ColorTiles;
		EventsHandler.cb_characterChanged -= ColorTiles;
		EventsHandler.cb_tileInteractionChanged -= ColorTiles;

		EventsHandler.cb_roomCreated -= CreateTileObject;
	
	}



	// Update is called once per frame

	void Update () 
	{
		
	}


	void CreateTileObject(Room myRoom)
	{

		GameObject tiles = new GameObject ("Tiles");


		foreach (Tile tile in myRoom.myGrid.gridArray) 
		{
			GameObject obj = Instantiate (TilePrefab, tiles.transform);

			obj.transform.position = new Vector3 (tile.x,tile.y,0);
			obj.GetComponent<SpriteRenderer> ().sortingLayerName = Constants.room_layer;

			tileGameObjectMap.Add(tile, obj);


			// adding object to hirarchy under TileManager

		//	obj.transform.SetParent (this.transform);
			obj.name = "Tile " + tile.x + "," + tile.y;


		}

	}



	// Color furniture tiles inside game

	public void ColorTiles(Interactable interactable)
	{

		//Debug.Log ("ColorTiles");


		List<Tile> InteractableTiles = RoomManager.instance.myRoom.GetMyTiles(interactable.mySize, interactable.x, interactable.y);


		// light the tiles


		foreach (Tile tile in InteractableTiles) {

			TileManager.instance.tileGameObjectMap [tile].GetComponent<SpriteRenderer> ().color = new Color (0.1f, 0.3f, 0.2f, 0.4f);

		}		


	}





}
