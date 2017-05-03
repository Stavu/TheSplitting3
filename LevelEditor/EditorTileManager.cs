using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorTileManager : MonoBehaviour {



	// Singleton //

	public static EditorTileManager instance { get; protected set;}

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

	GameObject tilesParent;




	// Use this for initialization
	public void Initialize () {


		EventsHandler.cb_editorNewRoomCreated += CreateTileObject;
		EventsHandler.cb_editorFurniturePlaced += ColorTiles;
		EventsHandler.cb_editorFurnitureChanged += ColorTiles;

		tileGameObjectMap = new Dictionary<Tile, GameObject>();

	}


	public void OnDestroy()
	{

		EventsHandler.cb_editorNewRoomCreated -= CreateTileObject;
		EventsHandler.cb_editorFurniturePlaced -= ColorTiles;
		EventsHandler.cb_editorFurnitureChanged -= ColorTiles;

	}




	// Update is called once per frame
	void Update () {

	}


	void CreateTileObject(Room myRoom)
	{		
		
		// Destroy old tiles


		foreach (GameObject obj in tileGameObjectMap.Values) 
		{
			
			Destroy (obj);
		}

		if (tilesParent != null) 
		{
			Destroy (tilesParent);
		}


		tileGameObjectMap.Clear ();



		// Create new tiles


		tilesParent = new GameObject ("Tiles");

		//Debug.Log ("tiles");

		foreach (Tile tile in myRoom.myGrid.gridArray) 
		{
			GameObject obj = Instantiate (TilePrefab, tilesParent.transform);

			obj.transform.position = new Vector3 (tile.x,tile.y,0);
			obj.GetComponent<SpriteRenderer> ().sortingLayerName = Constants.tiles_layer;

			tileGameObjectMap.Add(tile, obj);


			// adding object to hirarchy under TileManager

			//	obj.transform.SetParent (this.transform);
			obj.name = "Tile " + tile.x + "," + tile.y;


		}

	}



	public void ColorTiles(Furniture tempFurn)
	{

		//Debug.Log ("ColorTiles");


		foreach (GameObject obj in tileGameObjectMap.Values) 
		{

			obj.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0.1f);
			
		}

		foreach (Furniture furn in EditorRoomManager.instance.room.myFurnitureList) 
		{

			for (int x = 0; x < furn.mySize.x; x++) 
			{
				for (int y = 0; y < furn.mySize.y; y++) 
				{
					Tile tile = EditorRoomManager.instance.room.myGrid.GetTileAt (furn.x + x, furn.y + y);
					tileGameObjectMap [tile].GetComponent<SpriteRenderer> ().color = new Color (0.3f, 0.4f, 0.5f, 0.4f);

				}
			}
		}
	}
}
