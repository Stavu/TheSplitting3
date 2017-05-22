﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;



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



	// Handlers

	public static EditorFurnitureHandler editorFurnitureHandler;
	public static EditorCharacterHandler editorCharacterHandler;
	public static EditorTileInteractionHandler editorTileInteractionHandler;


	public Room room; 
	public Dictionary<Furniture,GameObject> furnitureGameObjectMap;
	public Dictionary<Character,GameObject> characterGameObjectMap;


	public static string roomToLoad;
	public static bool loadRoomFromMemory = false;



	public void Initialize ()
	{

		if (editorFurnitureHandler == null) 
		{
			editorFurnitureHandler = gameObject.AddComponent<EditorFurnitureHandler> ();
			editorFurnitureHandler.Initialize ();
		}

		if (editorCharacterHandler == null) 
		{
			editorCharacterHandler = gameObject.AddComponent<EditorCharacterHandler> ();
			editorCharacterHandler.Initialize ();
		}

		if (editorTileInteractionHandler == null) 
		{
			editorTileInteractionHandler = gameObject.AddComponent<EditorTileInteractionHandler> ();
			editorTileInteractionHandler.Initialize ();

		}

	}


	// Create Empty Room

	public Room CreateEmptyRoom(int myWidth, int myHeight)
	{

		Room tempRoom = new Room (myWidth,myHeight);

		furnitureGameObjectMap = new Dictionary<Furniture, GameObject> ();
		characterGameObjectMap = new Dictionary<Character, GameObject> ();

		return tempRoom;

	}


	// Create Empty Mirror Room

	public RoomMirror CreateEmptyRoomMirror(int myWidth, int myHeight)
	{

		RoomMirror tempRoom = new RoomMirror (myWidth,myHeight);

		furnitureGameObjectMap = new Dictionary<Furniture, GameObject> ();
		characterGameObjectMap = new Dictionary<Character, GameObject> ();

		return tempRoom;

	}



	// Creating a new flipped room in the editor (helper)

	public Room CreateFlippedRoom (Room room)
	{


		Room newRoom;
		newRoom.bgName = room.bgName;
		newRoom.bgFlipped = !room.bgFlipped;

		newRoom.myGrid = new Grid (newRoom.myWidth, newRoom.myHeight);


		// the new room's state - if the old room is real then the new one is mirror, and vice versa

		if (room.roomState == RoomState.Real) 
		{
			newRoom.roomState = RoomState.Mirror;
		
		} else {

			newRoom.roomState = RoomState.Real;
		}


		if (newRoom.roomState == RoomState.Real) 
		{
			newRoom = CreateEmptyRoom (room.myWidth, room.myHeight);

		} else {
			
			newRoom = CreateEmptyRoomMirror (room.myWidth, room.myHeight);
		}


		// Interactables

		foreach (Furniture furn in room.myFurnitureList) 
		{			
			Furniture flippedFurn = new Furniture (furn.myName, (room.myGrid.myWidth - 1 - furn.x - ((int)furn.mySize.x - 1)), furn.y);

			flippedFurn.offsetX = flippedFurn.imageFlipped ? furn.offsetX : -furn.offsetX;
			flippedFurn.offsetY = furn.offsetY;
			flippedFurn.mySize = furn.mySize;
			flippedFurn.walkable = furn.walkable;

			newRoom.myFurnitureList.Add (flippedFurn);

			Tile tile = newRoom.myGrid.GetTileAt (flippedFurn.x, flippedFurn.y);
			tile.myFurniture = flippedFurn;

			flippedFurn.imageFlipped = !furn.imageFlipped;

		}

		newRoom.myFurnitureList.ForEach (furn => Debug.Log (furn.myName));


		foreach (Character character in room.myCharacterList) 
		{			
			Character flippedCharacter = new Character (character.myName, (room.myGrid.myWidth - 1 - character.x - ((int)character.mySize.x - 1)), character.y);

			flippedCharacter.offsetX = character.offsetX;
			flippedCharacter.offsetY = character.offsetY;
			flippedCharacter.mySize = character.mySize;
			flippedCharacter.walkable = character.walkable;

			newRoom.myCharacterList.Add (flippedCharacter);

			Tile tile = newRoom.myGrid.GetTileAt (flippedCharacter.x, flippedCharacter.y);
			tile.myCharacter = flippedCharacter;
		}


		foreach (TileInteraction tileInt in room.myTileInteractionList) 
		{		
			TileInteraction flippedTileInteraction = new TileInteraction ((room.myGrid.myWidth - 1 - tileInt.x - ((int)tileInt.mySize.x - 1)), tileInt.y);
		
			flippedTileInteraction.mySize = tileInt.mySize;
			flippedTileInteraction.walkable = tileInt.walkable;

			newRoom.myTileInteractionList.Add (flippedTileInteraction);

			Tile tile = newRoom.myGrid.GetTileAt (flippedTileInteraction.x, flippedTileInteraction.y);
			tile.myTileInteraction = flippedTileInteraction;
		}





		return newRoom;


	}

	 






	// adding background image 


	GameObject roomBackground;

	public void SetRoomBackground(string name = "abandoned_lobby_bg")
	{

		Debug.Log ("SetRoomBackground");

		if (roomBackground != null) 
		{
			Destroy (roomBackground);			
		}	


		Sprite roomSprite;

		if (loadRoomFromMemory == false) 
		{

			Debug.Log ("new room");

			roomSprite = Resources.Load <Sprite> ("Sprites/Rooms/" + name);

			int myWidth = (int)roomSprite.bounds.size.x;
			int myHeight = (int)roomSprite.bounds.size.y;

			room = CreateEmptyRoom (myWidth, myHeight);
			room.bgName = name;	

					
		} else {
			
			Debug.Log ("old room");

			room = LoadRoom (roomToLoad);
			roomSprite = Resources.Load <Sprite> ("Sprites/Rooms/" + room.bgName );



		}
			
		EventsHandler.Invoke_cb_editorNewRoomCreated (room);



		// Creating room object

		GameObject obj = new GameObject (room.myName);

		SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
		sr.sprite = roomSprite;
		sr.flipX = room.bgFlipped;

		obj.transform.position = new Vector3 (room.myWidth/2f, 0, 0);
		sr.sortingLayerName = Constants.room_layer;
		obj.transform.SetParent (this.transform);

		roomBackground = obj;

		Utilities.AdjustOrthographicCamera (room);

	}



	// SERIALIZE ROOM //


	public string SerializeRoom ()
	{

		string roomString = JsonUtility.ToJson (room);
		//Debug.Log ("roomString" + roomString);
		//Debug.Log(Application.persistentDataPath); 

		string path = ("Assets/Resources/Jsons/Rooms/" + room.myName + ".json");

		using (FileStream fs = new FileStream(path, FileMode.Create))
		{
			using (StreamWriter writer = new StreamWriter(fs))
			{
				writer.Write(roomString);
			}
		}

		UnityEditor.AssetDatabase.Refresh ();

		return roomString;

	}




	// CHANGING INTERACTABLE //


	public void ChangeInteractableWidth(int width, Interactable interactable)
	{

		interactable.mySize = new Vector2 (width, interactable.mySize.y);

		if (interactable is Furniture) 
		{
			Furniture furn = (Furniture)interactable;
			furnitureGameObjectMap [furn].transform.position = new Vector3 (furn.x + furn.mySize.x / 2 + furn.offsetX, furn.y + 0.5f + furn.offsetY, 0);

		}

		if (interactable is Character) 
		{

			Character character = (Character)interactable;
			characterGameObjectMap[character].transform.position = new Vector3 (character.x + character.mySize.x / 2 + character.offsetX, character.y + 0.5f + character.offsetY, 0);

		}

		EventsHandler.Invoke_cb_tileLayoutChanged ();

	}



	public void ChangeInteractableHeight(int height, Interactable interactable)
	{

		interactable.mySize = new Vector2 (interactable.mySize.x, height);

		if (interactable is Furniture) 
		{
			Furniture furn = (Furniture)interactable;
			furnitureGameObjectMap[furn].transform.position = new Vector3 (furn.x + furn.mySize.x/2 + furn.offsetX, furn.y + 0.5f + furn.offsetY, 0);

		}

		if (interactable is Character) 
		{

			Character character = (Character)interactable;
			characterGameObjectMap[character].transform.position = new Vector3 (character.x + character.mySize.x/2 + character.offsetX, character.y + 0.5f + character.offsetY, 0);

		}

		EventsHandler.Invoke_cb_tileLayoutChanged ();

	}



	public void ChangeInteractableTileX(int x, Interactable interactable)
	{

		if (interactable == null) 
		{

			Debug.Log ("interactable is null");
		}


	
		Tile tile = room.myGrid.GetTileAt (interactable.x, interactable.y);
		Tile tileNew = room.myGrid.GetTileAt (x, interactable.y);

		if (tileNew == null) 
		{			
			return;
		}




		if (interactable is Furniture) 
		{
			
			Furniture furn = (Furniture)interactable;

			tile.myFurniture = null;
			furn.x = x;

			tileNew.myFurniture = furn;

			// Game object
			furnitureGameObjectMap [furn].transform.position = new Vector3 (furn.x + furn.mySize.x / 2 + furn.offsetX, furn.y + 0.5f + furn.offsetY, 0);

		
		} else if (interactable is Character) 
		{	

			Character character = (Character)interactable;

			tile.myCharacter = null;
			character.x = x;

			tileNew.myCharacter = character;

			// Game object
			characterGameObjectMap [character].transform.position = new Vector3 (character.x + character.mySize.x / 2 + character.offsetX, character.y + 0.5f + character.offsetY, 0);


		} else if (interactable is TileInteraction)		
		{			

			tile.myTileInteraction = null;
			interactable.x = x;

			tileNew.myTileInteraction = (TileInteraction)interactable;

		}

		EventsHandler.Invoke_cb_tileLayoutChanged ();


	}



	public void ChangeInteractableTileY(int y, Interactable interactable)
	{

		Tile tile = room.myGrid.GetTileAt (interactable.x, interactable.y);
		Tile tileNew = room.myGrid.GetTileAt (interactable.x, y);


		if (tileNew == null) 
		{			
			return;
		}

		if (interactable is Furniture) 
		{

			Furniture furn = (Furniture)interactable;

			tile.myFurniture = null;
			furn.y = y;

			tileNew.myFurniture = furn;

			furnitureGameObjectMap [furn].transform.position = new Vector3 (furn.x + furn.mySize.x / 2 + furn.offsetX, furn.y + 0.5f + furn.offsetY, 0);
	
		} else if (interactable is Character) 
		{	

			Character character = (Character)interactable;

			tile.myCharacter = null;
			character.y = y;

			tileNew.myCharacter = character;

			// Game object
			characterGameObjectMap [character].transform.position = new Vector3 (character.x + character.mySize.x / 2 + character.offsetX, character.y + 0.5f + character.offsetY, 0);


		}
		else if (interactable is TileInteraction) 
		{
			
			tile.myTileInteraction = null;
			interactable.y = y;

			tileNew.myTileInteraction = (TileInteraction)interactable;

		}

		EventsHandler.Invoke_cb_tileLayoutChanged ();


	}



	// OFFSETS //

	public void ChangeInteractableOffsetX(float offsetX, Interactable interactable)
	{
		
		if (interactable is Furniture) 
		{
			Furniture furn = (Furniture)interactable;
			furn.offsetX = offsetX;

			furnitureGameObjectMap [furn].transform.position = new Vector3 (furn.x + furn.mySize.x / 2 + furn.offsetX, furn.y + 0.5f + furn.offsetY, 0);

		} else if (interactable is Character) 
		{
			Character character = (Character)interactable;
			character.offsetX = offsetX;

			characterGameObjectMap [character].transform.position = new Vector3 (character.x + character.mySize.x / 2 + character.offsetX, character.y + 0.5f + character.offsetY, 0);

		} 

		//EventsHandler.Invoke_cb_tileLayoutChanged ();
	}



	public void ChangeInteractableOffsetY(float offsetY, Interactable interactable)
	{


		if (interactable is Furniture) 
		{
			Furniture furn = (Furniture)interactable;
			furn.offsetY = offsetY;
		
			furnitureGameObjectMap[furn].transform.position = new Vector3 (furn.x + furn.mySize.x/2 + furn.offsetX, furn.y + 0.5f + furn.offsetY, 0);

		} else if (interactable is Character) 
		{
			Character character = (Character)interactable;
			character.offsetY = offsetY;

			characterGameObjectMap[character].transform.position = new Vector3 (character.x + character.mySize.x/2 + character.offsetX, character.y + 0.5f + character.offsetY, 0);

		} 

		//EventsHandler.Invoke_cb_tileLayoutChanged ();

	}








	// ---- LOAD ROOM ---- //

	public Room LoadRoom(string roomString)
	{


		Room tempRoom = JsonUtility.FromJson<Room> (roomString);

		tempRoom.myGrid = new Grid (tempRoom.myWidth, tempRoom.myHeight);

		foreach (Furniture furn in tempRoom.myFurnitureList) 
		{
			Tile tile = tempRoom.myGrid.GetTileAt (furn.x, furn.y);
			tile.myFurniture = furn;

		}


		foreach (Character character in tempRoom.myCharacterList) 
		{
			Tile tile = tempRoom.myGrid.GetTileAt (character.x, character.y);
			tile.myCharacter = character;

		}


		foreach (TileInteraction tileInt in tempRoom.myTileInteractionList) 
		{
			Tile tile = tempRoom.myGrid.GetTileAt (tileInt.x, tileInt.y);
			tile.myTileInteraction = tileInt;

		}



		return tempRoom;

	}



}
