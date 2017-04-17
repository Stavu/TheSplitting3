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


		foreach (Furniture myRoomObject in myRoom.myFurnitureList) 
		{

			EventsHandler.Invoke_cb_furnitureChanged (myRoomObject);
			
		}

	}




	
	// Update is called once per frame

	void Update () {

		
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

		obj.transform.position = new Vector3 (0, 0, 0);

		obj.GetComponent<SpriteRenderer> ().sortingLayerName = Constants.room_layer;

		obj.transform.SetParent (this.transform);
	}






}
