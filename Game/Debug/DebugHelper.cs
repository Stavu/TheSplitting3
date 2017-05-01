﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;




public class DebugHelper : MonoBehaviour {



	// Singleton //

	public static DebugHelper instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

	}

	// Singleton //




	public GameObject roomSelect; 
	Dropdown dropDownMenu;



	// Use this for initialization
	public void Initialize () {
		

		CreateRoomSelect ();


	}


	
	// Update is called once per frame
	void Update () 
	{
		
	}




	public void CreateRoomSelect()
	{	

		roomSelect = Instantiate (roomSelect);
		dropDownMenu = roomSelect.GetComponentInChildren<Dropdown>();
		List<string> roomNameList = new List<string> ();

		foreach (string roomString in GameManager.instance.stringRoomMap.Keys) 
		{

			roomNameList.Add (roomString);
			//Debug.Log (roomString);
			
		}

		dropDownMenu.AddOptions (roomNameList);

		// what room are we in? 

		Room currentRoom = RoomManager.instance.myRoom;

		for (int i = 0; i < dropDownMenu.options.Count; i++) 
		{

			if (dropDownMenu.options [i].text == currentRoom.myName) 
			{
				dropDownMenu.value = i;			
			
			}

		}

		dropDownMenu.onValueChanged.AddListener (MoveToRoom);

	}




	// move to room


	public void MoveToRoom(int roomNum)
	{

		string roomName = dropDownMenu.options [roomNum].text;

		GameManager.roomToLoad = GameManager.instance.stringRoomMap [roomName];

		SceneManager.LoadScene(SceneManager.GetActiveScene().name);

	}





}
