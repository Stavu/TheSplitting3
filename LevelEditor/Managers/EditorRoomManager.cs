﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using UnityEngine.SceneManagement;



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
	GameObject roomBackground;

	public Dictionary<Furniture,GameObject> furnitureGameObjectMap;
	public Dictionary<Character,GameObject> characterGameObjectMap;

	public static Dictionary<string,GameObject> stringPrefabMap;

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


		// loading the prefabs

		if (stringPrefabMap == null) 
		{
			LoadPrefabs ();
		}


	}



	// Load Prefabs

	public void LoadPrefabs()
	{

		stringPrefabMap = new Dictionary<string, GameObject> ();

		GameObject[] furnitureArray = Resources.LoadAll<GameObject> ("Prefabs/Furniture");
		GameObject[] characterArray = Resources.LoadAll<GameObject> ("Prefabs/Characters");

		foreach (GameObject obj in furnitureArray) 
		{
			stringPrefabMap.Add (obj.name, obj);
		}

		foreach (GameObject obj in characterArray) 
		{
			stringPrefabMap.Add (obj.name, obj);

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


	public void Update()
	{

		if(Input.GetKeyDown(KeyCode.H))
		{
			Debug.Log ("room name " + room.myName);
		}


	}

	// adding background image 


	public void InitializeRoom(string name = "abandoned_lobby_bg")
	{

		//Debug.Log ("SetRoomBackground");

		if (roomToLoad == null) 
		{
			CreateNewRoomFromSprite (name);
		}

		room = LoadRoom (roomToLoad);
			
		EventsHandler.Invoke_cb_editorNewRoomCreated (room);

		// Creating room object

		SetBackgroundObject (room);

		Utilities.AdjustOrthographicCamera (room);

	}



	// Create New Room

	public void CreateNewRoomFromSprite(string name)
	{
		
		Sprite roomSprite = Resources.Load <Sprite> ("Sprites/Rooms/" + name);

		int myWidth = (int)roomSprite.bounds.size.x;
		int myHeight = (int)roomSprite.bounds.size.y;

		Room room = CreateEmptyRoom (myWidth, myHeight);
		room.bgName = name;	

		roomToLoad = JsonUtility.ToJson (room);

		SceneManager.LoadScene ("LevelEditor");

	}



	// PI tiles 

	public void SetPICoords(Tile tile)
	{
		Coords coords = new Coords (tile.x, tile.y);

		PhysicalInteractable chosenPhysicalInteractable = InspectorManager.instance.GetChosenPI ();

		for (int i = 0; i < chosenPhysicalInteractable.currentGraphicState.coordsList.Count; i++)
		{			
			if ((chosenPhysicalInteractable.currentGraphicState.coordsList[i].x == coords.x) && (chosenPhysicalInteractable.currentGraphicState.coordsList[i].y == coords.y)) 
			{
				chosenPhysicalInteractable.currentGraphicState.coordsList.RemoveAt (i);
				tile.myFurniture  = null;
				tile.myCharacter  = null;
				EventsHandler.Invoke_cb_tileLayoutChanged ();

				return;
			}
		}


		// Get furniture out of old tiles if there wasn't a coords list before

		if (chosenPhysicalInteractable.currentGraphicState.coordsList.Count == 0) 
		{
			List<Tile> oldTileList = EditorRoomManager.instance.room.GetMyTiles (EditorRoomManager.instance.room.MyGrid, chosenPhysicalInteractable.mySize, chosenPhysicalInteractable.x, chosenPhysicalInteractable.y);

			foreach (Tile oldTile in oldTileList) 
			{
				if (chosenPhysicalInteractable is Furniture) 
				{
					oldTile.myFurniture = null;
				}

				if (chosenPhysicalInteractable is Character) 
				{
					oldTile.myCharacter = null;
				}
			}		
		}

		chosenPhysicalInteractable.currentGraphicState.coordsList.Add (coords);

		if (chosenPhysicalInteractable is Furniture) 
		{
			tile.myFurniture = (Furniture)chosenPhysicalInteractable;
		}

		if (chosenPhysicalInteractable is Character) 
		{
			tile.myCharacter = (Character)chosenPhysicalInteractable;
		}


		EventsHandler.Invoke_cb_tileLayoutChanged ();

	}




	// Room background 

	public void ChangeRoomBackground(string name = "abandoned_lobby_bg")
	{

		if (room.RoomState == RoomState.Real) 
		{
			room.bgName = name;
		
		} else {

			if (room.myMirrorRoom.inTheShadow == true) 
			{
				room.myMirrorRoom.bgName_Shadow = name;

			} else {
				
				room.bgName = name;
			}
		}

		SetBackgroundObject (room);
	}



	public void SetBackgroundObject(Room room)
	{

		if (roomBackground != null) 
		{
			Destroy (roomBackground);			
		}	
	
		SpriteRenderer sr;	
		Sprite roomSprite;

		if (room.RoomState == RoomState.Real) 
		{
			roomBackground = new GameObject (room.myName);
			sr = roomBackground.AddComponent<SpriteRenderer> ();
			roomSprite = Resources.Load <Sprite> ("Sprites/Rooms/" + room.bgName);
			sr.flipX = room.bgFlipped;
		
		} else {

			if (room.myMirrorRoom.inTheShadow == true) 
			{
				roomBackground = new GameObject (room.myName + "_shadow");
				sr = roomBackground.AddComponent<SpriteRenderer> ();
				roomSprite = Resources.Load <Sprite> ("Sprites/Rooms/" + room.myMirrorRoom.bgName_Shadow);
				sr.flipX = room.myMirrorRoom.bgFlipped_Shadow;

			} else {

				roomBackground = new GameObject (room.myName);
				sr = roomBackground.AddComponent<SpriteRenderer> ();
				roomSprite = Resources.Load <Sprite> ("Sprites/Rooms/" + room.bgName);
				sr.flipX = room.bgFlipped;
			}
		}

		sr.sprite = roomSprite;
		roomBackground.transform.position = new Vector3 (room.myWidth/2f, 0, 0);
		sr.sortingLayerName = Constants.room_layer;
		roomBackground.transform.SetParent (this.transform);

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
	
		Tile tile = room.MyGrid.GetTileAt (interactable.x, interactable.y);
		Tile tileNew = room.MyGrid.GetTileAt (x, interactable.y);

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
		Tile tile = room.MyGrid.GetTileAt (interactable.x, interactable.y);
		Tile tileNew = room.MyGrid.GetTileAt (interactable.x, y);

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

		EventsHandler.Invoke_cb_tileLayoutChanged ();
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

		EventsHandler.Invoke_cb_tileLayoutChanged ();

	}




	// ------- GRAPHIC STATE ------- //


	public void ChangeInteractableCurrentGraphicState(string state, PhysicalInteractable interactable)
	{
		foreach (GraphicState graphicState in interactable.graphicStates) 
		{
			Debug.Log (graphicState.graphicStateName + "=>" + state);
			if (graphicState.graphicStateName == state) 
			{
				Debug.Log ("found state");

				// clean previous graphic state

				EditorRoomManager.instance.room.ChangePIInTiles (interactable, graphicState);
				interactable.currentGraphicState = graphicState;

				// go to animation state

				GameObject obj = GetPhysicalInteractableGameObject (interactable);
				Animator animator = obj.GetComponent<Animator> ();
				animator.PlayInFixedTime (state);

				EventsHandler.Invoke_cb_tileLayoutChanged ();
				return;
			}
		}

		Debug.LogError ("I shouldn't be here.");
	}




	// ------ FRAME SIZE ------ //


	public void ChangeInteractableFrameWidth(float width, PhysicalInteractable interactable)
	{
		width = Mathf.Abs (width);
		interactable.currentGraphicState.frameExtents.x = width;

		EventsHandler.Invoke_cb_tileLayoutChanged ();
	}


	public void ChangeInteractableFrameHeight(float height, PhysicalInteractable interactable)
	{
		height = Mathf.Abs (height);
		interactable.currentGraphicState.frameExtents.y = height;

		EventsHandler.Invoke_cb_tileLayoutChanged ();
	}


	// FRAME OFFSETS //

	public void ChangeInteractableFrameOffsetX(float offsetX, PhysicalInteractable interactable)
	{
		interactable.currentGraphicState.frameOffsetX = offsetX;

		EventsHandler.Invoke_cb_tileLayoutChanged ();
	}



	public void ChangeInteractableFrameOffsetY(float offsetY, PhysicalInteractable interactable)
	{
		interactable.currentGraphicState.frameOffsetY = offsetY;

		EventsHandler.Invoke_cb_tileLayoutChanged ();
	}




	// ---- LOAD ROOM ---- //

	public Room LoadRoom(string roomString)
	{
		Debug.Log ("LoadRoom");

		Room tempRoom = JsonUtility.FromJson<Room> (roomString);
		tempRoom.myGrid = new Grid (tempRoom.myWidth, tempRoom.myHeight);


		// MIRROR ROOM

		if (tempRoom.RoomState == RoomState.Mirror)
		{
			tempRoom.myMirrorRoom.shadowGrid = new Grid (tempRoom.myWidth, tempRoom.myHeight);

			if (tempRoom.myMirrorRoom.inTheShadow == true) 
			{
				
				foreach (Furniture furn in tempRoom.myMirrorRoom.myFurnitureList_Shadow) 
				{
					if (furn.graphicStates != null) 
					{
						furn.currentGraphicState = furn.graphicStates [0];
					
					} else {

						Debug.LogError ("graphicStates is null");
					}
				}

				foreach (TileInteraction tileInt in tempRoom.myMirrorRoom.myTileInteractionList_Shadow) 
				{
					Tile tileShadow = tempRoom.myMirrorRoom.shadowGrid.GetTileAt (tileInt.x, tileInt.y);
					tileShadow.myTileInteraction = tileInt;

				}

			} else {

				foreach (Furniture furn in tempRoom.myFurnitureList) 
				{
					if (furn.graphicStates != null) 
					{
						furn.currentGraphicState = furn.graphicStates [0];

					} else {

						Debug.LogError ("graphicStates is null");
					}
				}

				foreach (TileInteraction tileInt in tempRoom.myTileInteractionList) 
				{
					Tile tile = tempRoom.MyGrid.GetTileAt (tileInt.x, tileInt.y);
					tile.myTileInteraction = tileInt;
				}
			}



			// Persistant Interactables 

			foreach (Furniture furn in tempRoom.myMirrorRoom.myFurnitureList_Persistant) 
			{
				if (furn.graphicStates != null) 
				{
					furn.currentGraphicState = furn.graphicStates [0];

				} else {

					Debug.LogError ("graphicStates is null");
				}
			}

			foreach (TileInteraction tileInt in tempRoom.myMirrorRoom.myTileInteractionList_Persistant) 
			{				
				Tile tile = tempRoom.MyGrid.GetTileAt (tileInt.x, tileInt.y);
				tile.myTileInteraction = tileInt;

				Tile tileShadow = tempRoom.myMirrorRoom.shadowGrid.GetTileAt (tileInt.x, tileInt.y);
				tileShadow.myTileInteraction = tileInt;

			}

			foreach (Character character in tempRoom.myCharacterList) 
			{
				Tile tile = tempRoom.MyGrid.GetTileAt (character.x, character.y);
				tile.myCharacter = character;

				Tile tileShadow = tempRoom.myMirrorRoom.shadowGrid.GetTileAt (character.x, character.y);
				tileShadow.myCharacter = character;

			}

		} else {
			
			
			// REAL ROOM 

			foreach (Furniture furn in tempRoom.myFurnitureList) 
			{				
				if (furn.graphicStates != null) 
				{
					furn.currentGraphicState = furn.graphicStates [0];

				} else {

				Debug.LogError ("graphicStates is null");
				}
			}


			foreach (Character character in tempRoom.myCharacterList) 
			{
				Tile tile = tempRoom.MyGrid.GetTileAt (character.x, character.y);
				tile.myCharacter = character;
			}


			foreach (TileInteraction tileInt in tempRoom.myTileInteractionList) 
			{
				Tile tile = tempRoom.MyGrid.GetTileAt (tileInt.x, tileInt.y);
				tile.myTileInteraction = tileInt;
			}
		}

		return tempRoom;

	}



	public GameObject GetPhysicalInteractableGameObject(PhysicalInteractable physicalInteractable)
	{
		GameObject obj = null;

		if (physicalInteractable is Furniture) 
		{
			Furniture furn = (Furniture)physicalInteractable;
			obj = furnitureGameObjectMap [furn];
		}

		if (physicalInteractable is Character) 
		{
			Character character = (Character)physicalInteractable;
			obj = characterGameObjectMap [character];
		}

		return obj;
	}



}
