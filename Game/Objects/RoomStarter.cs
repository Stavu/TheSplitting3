using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStarter : MonoBehaviour {


	// Singleton //

	public static RoomStarter instance { get; protected set; }

	void Awake ()
	{		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //




	// Use this for initialization

	public void Initialize () 
	{
		EventsHandler.cb_entered_room += StartRoom;
	}
	

	void OnDestroy () 
	{
		EventsHandler.cb_entered_room -= StartRoom;
	}


	// Update is called once per frame

	void Update () 
	{

	}




	// -------- START ROOM -------- // 


	public void StartRoom(Room room)
	{

		// adding the room to rooms visited list

		bool firstTimeinRoom = GameManager.playerData.AddToRoomsVisited (room.myName);

		switch (room.myName) 
		{
			case "abandoned_lobby":

				InteractionManager.instance.DisplayDialogueOption("enter_room_abandoned_lobby");

				break;


			case "abandoned_lobby_mirror":

				if (firstTimeinRoom) 
				{					
					InteractionManager.instance.DisplayDialogueOption ("enter_room_abandoned_lobby");
				}

				break;


			case "abandoned_wing_outside_shadow":

				break;


			case "abandoned_wing_outside":

				break;

		}
	}


}
