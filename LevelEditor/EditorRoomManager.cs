using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;




public class EditorRoomManager : MonoBehaviour {


	// Singleton //

	public static EditorRoomManager instance = null;

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //


	public Room room; 


	public Dictionary<Furniture,GameObject> furnitureGameObjectMap;


	public void CreateEmptyRoom(int myWidth, int myHeight)
	{
		
		room = new Room (myWidth,myHeight);
		room.myFurnitureList = new List<Furniture> ();

		furnitureGameObjectMap = new Dictionary<Furniture, GameObject> ();

		EventsHandler.Invoke_cb_editorNewRoomCreated (room);
		Utilities.AdjustOrthographicCamera (room);


	}


	 

	// adding background image 


	GameObject roomBackground;

	public void SetRoomBackground(string name)
	{
		//Debug.Log ("set room bg");

		if (roomBackground != null) 
		{

			Destroy (roomBackground);		
		
		}


		Sprite roomSprite = Resources.Load <Sprite> ("Sprites/Rooms/" + name);

		int myWidth = (int) roomSprite.bounds.size.x;
		int myHeight = (int) roomSprite.bounds.size.y;


		CreateEmptyRoom (myWidth,myHeight);
		room.bgName = name;

		GameObject obj = new GameObject (room.myName);

		obj.AddComponent<SpriteRenderer> ().sprite = roomSprite;
		obj.transform.position = new Vector3 (0, 0, 0);
		obj.GetComponent<SpriteRenderer> ().sortingLayerName = Constants.room_layer;
		obj.transform.SetParent (this.transform);

		roomBackground = obj;


	}



	// Placing Furniture

	public void PlaceFurniture(Tile tile, string furnitureName)
	{


		if(furnitureName == null)
		{
			return;
		}
			

		if (tile.myFurniture != null)
		{
			room.myFurnitureList.Remove (tile.myFurniture);

			Destroy (furnitureGameObjectMap [tile.myFurniture]);
			furnitureGameObjectMap.Remove (tile.myFurniture);
		}



		// create frutniture


		Furniture furn = new Furniture (furnitureName, tile.x, tile.y);

		room.myFurnitureList.Add (furn);

		Sprite furnitureSprite = Resources.Load <Sprite> ("Sprites/Furniture/" + furnitureName);



		// set default size

		furn.mySize = new Vector2 (Mathf.Ceil(furnitureSprite.bounds.size.x), 1f);



		// create furniture object 

		GameObject obj = new GameObject (furnitureName);

		obj.AddComponent<SpriteRenderer> ().sprite = furnitureSprite;


		// set object position 

		obj.transform.position = new Vector3 (furn.x + furn.mySize.x/2 + furn.offsetX, furn.y + 0.5f + furn.offsetY, 0);



		// set object layer

		obj.GetComponent<SpriteRenderer> ().sortingLayerName = Constants.furniture_character_layer;
		obj.transform.SetParent (this.transform);

		furnitureGameObjectMap.Add (furn, obj);

		tile.myFurniture = furn;

		EventsHandler.Invoke_cb_editorFurniturePlaced (furn);
	

	}



	/* CHANGING FURNITURE */


	public void ChangeFurnitureWidth(int width, Furniture furn)
	{

		furn.mySize = new Vector2 (width, furn.mySize.y);
		furnitureGameObjectMap[furn].transform.position = new Vector3 (furn.x + furn.mySize.x/2 + furn.offsetX, furn.y + 0.5f + furn.offsetY, 0);

		EventsHandler.Invoke_cb_editorFurnitureChanged (furn);


	}



	public void ChangeFurnitureHeight(int height, Furniture furn)
	{

		furn.mySize = new Vector2 (furn.mySize.x, height);
		furnitureGameObjectMap[furn].transform.position = new Vector3 (furn.x + furn.mySize.x/2 + furn.offsetX, furn.y + 0.5f + furn.offsetY, 0);


		EventsHandler.Invoke_cb_editorFurnitureChanged (furn);


	}



	public void ChangeFurnitureTileX(int x, Furniture furn)
	{
		
	
		Tile tile = room.myGrid.GetTileAt (furn.x, furn.y);
		Tile tileNew = room.myGrid.GetTileAt (x, furn.y);

		if (tileNew == null) 
		{			
			return;
		}

		tile.myFurniture = null;
		furn.x = x;

		tileNew.myFurniture = furn;



		furnitureGameObjectMap[furn].transform.position = new Vector3 (furn.x + furn.mySize.x/2 + furn.offsetX, furn.y + 0.5f + furn.offsetY, 0);

		EventsHandler.Invoke_cb_editorFurnitureChanged (furn);


	}



	public void ChangeFurnitureTileY(int y, Furniture furn)
	{

		Tile tile = room.myGrid.GetTileAt (furn.x, furn.y);
		Tile tileNew = room.myGrid.GetTileAt (furn.x, y);


		if (tileNew == null) 
		{			
			return;
		}


		tile.myFurniture = null;
		furn.y = y;

		tileNew.myFurniture = furn;


		furnitureGameObjectMap[furn].transform.position = new Vector3 (furn.x + furn.mySize.x/2 + furn.offsetX, furn.y + 0.5f + furn.offsetY, 0);

		EventsHandler.Invoke_cb_editorFurnitureChanged (furn);


	}



	public void ChangeFurnitureOffsetX(float offsetX, Furniture furn)
	{

		furn.offsetX = offsetX;

		furnitureGameObjectMap[furn].transform.position = new Vector3 (furn.x + furn.mySize.x/2 + furn.offsetX, furn.y + 0.5f + furn.offsetY, 0);

		EventsHandler.Invoke_cb_editorFurnitureChanged (furn);
	}



	public void ChangeFurnitureOffsetY(float offsetY, Furniture furn)
	{

		furn.offsetY = offsetY;

		furnitureGameObjectMap[furn].transform.position = new Vector3 (furn.x + furn.mySize.x/2 + furn.offsetX, furn.y + 0.5f + furn.offsetY, 0);

		EventsHandler.Invoke_cb_editorFurnitureChanged (furn);

	}


	public string SerializeRoom ()
	{

		string roomString = JsonUtility.ToJson (room);
		Debug.Log ("roomString" + roomString);

		return roomString;

	}



}
