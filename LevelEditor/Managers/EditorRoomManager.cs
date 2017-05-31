using System.Collections;
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




	// adding background image 


	public void InitializeRoom(string name = "abandoned_lobby_bg")
	{

		Debug.Log ("SetRoomBackground");


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




	// ------ FRAME SIZE ------ //



	public void ChangeInteractableFrameWidth(float width, PhysicalInteractable interactable)
	{
		width = Mathf.Abs (width);
		interactable.frameExtents.x = width;

		EventsHandler.Invoke_cb_tileLayoutChanged ();
	}


	public void ChangeInteractableFrameHeight(float height, PhysicalInteractable interactable)
	{
		height = Mathf.Abs (height);
		interactable.frameExtents.y = height;

		EventsHandler.Invoke_cb_tileLayoutChanged ();
	}


	// FRAME OFFSETS //

	public void ChangeInteractableFrameOffsetX(float offsetX, PhysicalInteractable interactable)
	{
		interactable.frameOffsetX = offsetX;

		EventsHandler.Invoke_cb_tileLayoutChanged ();
	}



	public void ChangeInteractableFrameOffsetY(float offsetY, PhysicalInteractable interactable)
	{
		interactable.frameOffsetY = offsetY;

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
					Tile tileShadow = tempRoom.myMirrorRoom.shadowGrid.GetTileAt (furn.x, furn.y);
					tileShadow.myFurniture = furn;

				}

				foreach (TileInteraction tileInt in tempRoom.myMirrorRoom.myTileInteractionList_Shadow) 
				{
					Tile tileShadow = tempRoom.myMirrorRoom.shadowGrid.GetTileAt (tileInt.x, tileInt.y);
					tileShadow.myTileInteraction = tileInt;

				}

			} else {

				foreach (Furniture furn in tempRoom.myFurnitureList) 
				{
					Tile tile = tempRoom.MyGrid.GetTileAt (furn.x, furn.y);
					tile.myFurniture = furn;

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

				Tile tile = tempRoom.MyGrid.GetTileAt (furn.x, furn.y);
				tile.myFurniture = furn;

				Tile tileShadow = tempRoom.myMirrorRoom.shadowGrid.GetTileAt (furn.x, furn.y);
				tileShadow.myFurniture = furn;

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
				Tile tile = tempRoom.MyGrid.GetTileAt (furn.x, furn.y);
				tile.myFurniture = furn;
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



}
