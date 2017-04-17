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

		EventsHandler.cb_roomCreated += CreateTileObject;

		tileGameObjectMap = new Dictionary<Tile, GameObject>();
					
	}


	public void OnDestroy()
	{
		
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

}
