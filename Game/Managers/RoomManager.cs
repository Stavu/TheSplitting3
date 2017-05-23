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


			if (myRoom.myMirrorRoom.inTheShadow == true) 
			{
				// SHADOW ROOM

				myRoom.myMirrorRoom.myFurnitureList_Shadow.ForEach (furn => EventsHandler.Invoke_cb_furnitureChanged (furn));
				myRoom.myMirrorRoom.myTileInteractionList_Shadow.ForEach (tileInt => EventsHandler.Invoke_cb_tileInteractionChanged (tileInt));


			} else {

				// MIRROR ROOM

				myRoom.myFurnitureList.ForEach (furn => EventsHandler.Invoke_cb_furnitureChanged (furn));
				myRoom.myTileInteractionList.ForEach (tileInt => EventsHandler.Invoke_cb_tileInteractionChanged (tileInt));
			}


			// PERSISTENT INTERACTABLES

			myRoom.myMirrorRoom.myFurnitureList_Persistant.ForEach (furn => EventsHandler.Invoke_cb_furnitureChanged (furn));

			myRoom.myCharacterList.ForEach (character => {
				EventsHandler.Invoke_cb_characterChanged (character);
				nameSpeakerMap.Add (character.myName, character);
			});

			myRoom.myMirrorRoom.myTileInteractionList_Persistant.ForEach (tileInt => EventsHandler.Invoke_cb_tileInteractionChanged (tileInt));

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

		GameObject obj = new GameObject (room.myName);

		obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load <Sprite> ("Sprites/Rooms/" + room.bgName);

		obj.transform.position = new Vector3 (room.myWidth/2f, 0, 0);

		obj.GetComponent<SpriteRenderer> ().sortingLayerName = Constants.room_layer;

		obj.transform.SetParent (this.transform);
	}






}
