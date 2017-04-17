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

		Debug.Log ("Assign Furniture Image");

		myFurniture.myPos = new Vector3 (myFurniture.x + myFurniture.mySize.x/2, myFurniture.y, 0);

		GameObject obj = new GameObject (myFurniture.myName);
		obj.transform.SetParent (this.transform);


		SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();

		sr.sprite = Resources.Load<Sprite> ("Sprites/Furniture/" + myFurniture.myName); 

		obj.transform.position = new Vector3 (myFurniture.myPos.x + myFurniture.offsetX, myFurniture.myPos.y + 0.5f + myFurniture.offsetY, myFurniture.myPos.z);

		Debug.Log ("object position" + myFurniture.myName + obj.transform.position + sr.sprite.bounds);


		// sorting order 

		sr.sortingOrder = -myFurniture.y;
	
		if (myFurniture.walkable == true) 
		{
			sr.sortingOrder = (int) -(myFurniture.y + myFurniture.mySize.y);

		}

		sr.sortingLayerName = Constants.furniture_character_layer;


		furnitureGameObjectMap.Add (myFurniture, obj);




	}





}
