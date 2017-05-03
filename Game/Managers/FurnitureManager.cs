using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FurnitureManager : MonoBehaviour {


	// Singleton //

	public static FurnitureManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //



	public Dictionary<Furniture,GameObject> furnitureGameObjectMap;



	//public GameObject FurniturePrefab;




	// Use this for initialization

	public void Initialize () 
	{
		
		EventsHandler.cb_furnitureChanged += ColorTiles;
		EventsHandler.cb_furnitureChanged += CreateFurnitureGameObject;

		furnitureGameObjectMap = new Dictionary<Furniture, GameObject> ();

	}


	public void OnDestroy()
	{
	
		EventsHandler.cb_furnitureChanged -= ColorTiles;
		EventsHandler.cb_furnitureChanged -= CreateFurnitureGameObject;
			
	}




	// Update is called once per frame

	void Update () {
		
	}


	public void ColorTiles(Furniture myFurniture)
	{
		
		//Debug.Log ("ColorTiles");


		List<Tile> FurnitureTiles = RoomManager.instance.myRoom.GetFurnitureTiles(myFurniture);


		// light the tiles

	
		foreach (Tile tile in FurnitureTiles) {

			TileManager.instance.tileGameObjectMap [tile].GetComponent<SpriteRenderer> ().color = new Color (0.1f, 0.3f, 0.2f, 0.4f);

		}		


	}



	public void CreateFurnitureGameObject (Furniture myFurniture)
	{
		
		GameObject obj = Utilities.CreateFurnitureGameObject (myFurniture, this.transform);

		furnitureGameObjectMap.Add (myFurniture, obj);

	}





}
