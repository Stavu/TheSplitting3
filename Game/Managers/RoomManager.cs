using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;



public class RoomManager : MonoBehaviour {


	// Singleton //

	public static RoomManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //


	public Room myRoom;
	public Dictionary <string,ISpeaker> nameSpeakerMap;

	public GameObject bgObject;
	public GameObject bgObject_Shadow;



	public void Initialize ()
	{
	
		//EventsHandler.cb_furnitureChanged += OnFurnitureChanged;
			
	}

	public void OnDestroy()
	{
	
		//EventsHandler.cb_furnitureChanged -= OnFurnitureChanged;

	
	}


	// Use this for initialization

	public void BuildRoom () {


		CreateRoom ();
		nameSpeakerMap = new Dictionary<string, ISpeaker> ();

		if (myRoom.RoomState == RoomState.Real) 
		{

			// REAL ROOM

			myRoom.myFurnitureList.ForEach (furn => EventsHandler.Invoke_cb_furnitureChanged (furn));

			myRoom.myCharacterList.ForEach (character => {
				EventsHandler.Invoke_cb_characterChanged (character);
				nameSpeakerMap.Add (character.myName, character);
			});

			myRoom.myTileInteractionList.ForEach (tileInt => EventsHandler.Invoke_cb_tileInteractionChanged (tileInt));


		} else {


			// SHADOW ROOM

			myRoom.myMirrorRoom.myFurnitureList_Shadow.ForEach (furn => EventsHandler.Invoke_cb_furnitureChanged (furn));
			myRoom.myMirrorRoom.myTileInteractionList_Shadow.ForEach (tileInt => EventsHandler.Invoke_cb_tileInteractionChanged (tileInt));


			// MIRROR ROOM

			myRoom.myFurnitureList.ForEach (furn => EventsHandler.Invoke_cb_furnitureChanged (furn));
			myRoom.myTileInteractionList.ForEach (tileInt => EventsHandler.Invoke_cb_tileInteractionChanged (tileInt));


			// PERSISTENT INTERACTABLES

			myRoom.myMirrorRoom.myFurnitureList_Persistant.ForEach (furn => EventsHandler.Invoke_cb_furnitureChanged (furn));

			myRoom.myCharacterList.ForEach (character => {
				EventsHandler.Invoke_cb_characterChanged (character);
				nameSpeakerMap.Add (character.myName, character);
			});

			myRoom.myMirrorRoom.myTileInteractionList_Persistant.ForEach (tileInt => EventsHandler.Invoke_cb_tileInteractionChanged (tileInt));

			SwitchObjectByShadowState ();

		}



		// adding the player to the speaker map

		nameSpeakerMap.Add (PlayerManager.myPlayer.myName, PlayerManager.myPlayer);

		GameManager.instance.inputState = InputState.Character; //FIXME

	}

	
	// Update is called once per frame

	void Update () 
	{

		
	}


	void CreateRoom()	
	{
		//Debug.Log ("created grid");
	
		myRoom = new Room(GameManager.roomToLoad);
		EventsHandler.Invoke_cb_roomCreated (myRoom);
		Utilities.AdjustOrthographicCamera (myRoom);

		CreateRoomObject (myRoom);
	}


	public void CreateRoomObject(Room room)
	{

		bgObject = new GameObject (room.myName);

		bgObject.AddComponent<SpriteRenderer> ().sprite = Resources.Load <Sprite> ("Sprites/Rooms/" + room.bgName);
		bgObject.transform.position = new Vector3 (room.myWidth/2f, 0, 0);

		bgObject.GetComponent<SpriteRenderer> ().sortingLayerName = Constants.room_layer;
		bgObject.transform.SetParent (this.transform);


		if (myRoom.myMirrorRoom != null) 
		{		
			bgObject_Shadow = new GameObject (room.myName + "_shadow");

			bgObject_Shadow.AddComponent<SpriteRenderer> ().sprite = Resources.Load <Sprite> ("Sprites/Rooms/" + room.myMirrorRoom.bgName_Shadow);
			bgObject_Shadow.transform.position = new Vector3 (room.myWidth/2f, 0, 0);

			bgObject_Shadow.GetComponent<SpriteRenderer> ().sortingLayerName = Constants.room_layer;
			bgObject_Shadow.transform.SetParent (this.transform);
		}
	}




	// -- SWITCH BETWEEN SHADOW AND MIRROR -- //

	public void SwitchObjectByShadowState()
	{

		List<SpriteRenderer> fadeInSprites = new List<SpriteRenderer> ();
		List<SpriteRenderer> fadeOutSprites = new List<SpriteRenderer> ();


		if (myRoom.myMirrorRoom.inTheShadow == true) 
		{

			fadeOutSprites.Add (bgObject.GetComponent<SpriteRenderer>());
			fadeInSprites.Add (bgObject_Shadow.GetComponent<SpriteRenderer>());

			foreach (Furniture furn in myRoom.myFurnitureList) 
			{
				SpriteRenderer sr = FurnitureManager.instance.furnitureGameObjectMap [furn].GetComponent<SpriteRenderer>();
				fadeOutSprites.Add (sr);
			}


			foreach (Furniture furn in myRoom.myMirrorRoom.myFurnitureList_Shadow) 
			{
				SpriteRenderer sr = FurnitureManager.instance.furnitureGameObjectMap [furn].GetComponent<SpriteRenderer>();
				fadeInSprites.Add (sr);
			}

		} else {

			fadeOutSprites.Add (bgObject_Shadow.GetComponent<SpriteRenderer>());
			fadeInSprites.Add (bgObject.GetComponent<SpriteRenderer>());

			foreach (Furniture furn in myRoom.myMirrorRoom.myFurnitureList_Shadow) 
			{
				SpriteRenderer sr = FurnitureManager.instance.furnitureGameObjectMap [furn].GetComponent<SpriteRenderer>();
				fadeOutSprites.Add (sr);
			}

			foreach (Furniture furn in myRoom.myFurnitureList) 
			{
				SpriteRenderer sr = FurnitureManager.instance.furnitureGameObjectMap [furn].GetComponent<SpriteRenderer>();
				fadeInSprites.Add (sr);
			}

		}

		StartCoroutine (Utilities.FadeBetweenSprites(fadeOutSprites,fadeInSprites));

	}



}
