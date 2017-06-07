﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum InputState
{
	Character,
	Inventory,
	ActionBox,
	Dialogue,
	DialogueBox,
	Settings,
	Map,
	Cutscene,
	NoInput
}



public class GameManager : MonoBehaviour
{
	
	// Singleton //

	public static GameManager instance { get; protected set; }

	void Awake ()
	{		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //


	public static bool actionBoxActive = false;
	public static bool textBoxActive = false;
	public static bool inventoryOpen = false;
	public static bool settingsOpen = false;
	public static bool mapOpen = false;

	public static bool dialogueTreeBoxActive = false;

	public static Dictionary<string,Color> speakerColorMap;

	public static Room roomToLoad;
	public Dictionary<string,Room> stringRoomMap = new Dictionary<string, Room> ();

	public static UserData userData;
	public static GameData gameData;

	public static Dictionary<string,GameObject> stringPrefabMap;

	public InputState inputState = InputState.Character;



	// Use this for initialization

	public void Initialize ()
	{
		
		CreateRooms ();

		if (roomToLoad == null) 
		{
			roomToLoad = stringRoomMap ["test1"];
		}

		speakerColorMap = new Dictionary<string, Color> ();

		speakerColorMap.Add("Daniel", Color.white);
		speakerColorMap.Add("geM", Color.magenta);
		speakerColorMap.Add("llehctiM", Color.cyan);
		speakerColorMap.Add("Stella", Color.red);

		if (stringPrefabMap == null) 
		{
			LoadPrefabs ();
		}
	}


	// Update is called once per frame

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.R)) 
		{
			CreateNewData ();
		}

		if (Input.GetKeyDown (KeyCode.B)) 
		{
			RoomManager.instance.myRoom.myMirrorRoom.inTheShadow = !RoomManager.instance.myRoom.myMirrorRoom.inTheShadow;
			RoomManager.instance.SwitchObjectByShadowState(false);
		}
	}

	public void CreateRooms ()
	{
		Object[] myTextAssets = Resources.LoadAll ("Jsons/Rooms");

		foreach (TextAsset txt in myTextAssets) 
		{
			Room myRoom;
			myRoom = JsonUtility.FromJson<Room> (txt.text);

			// Adding room to dictionary

			stringRoomMap.Add (myRoom.myName, myRoom);					
		}
	}


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


	public void CreateInventoryItemData()
	{
		if (gameData != null) 
		{
			return;
		}

		gameData = new GameData ();
	}



	/* ----- SAVING AND LOADING ----- */


	public void CreateUserData ()
	{
		if (userData != null) 
		{
			//Debug.Log ("playerData is not null");
			return;
		}

		if (PlayerPrefs.HasKey ("PlayerData")) 
		{			
			//Debug.Log ("Loading data from memory");
			userData = JsonUtility.FromJson<UserData> (PlayerPrefs.GetString ("PlayerData"));

			foreach (PlayerData playerData in userData.playerDataList) 
			{
				foreach (InventoryItem item in playerData.inventory.items) 
				{
					item.Initialize ();
				}
			}

		} else {
			
			CreateNewData ();
		}
	}


	public void CreateNewData ()
	{
		Debug.Log ("Creating new data");

		userData = new UserData ();

		foreach (Player player in PlayerManager.playerList) 
		{
			userData.playerDataList.Add (new PlayerData (player.myName));
		}

		SaveData ();
	}


	public void SaveData ()
	{		
		Debug.Log ("Saving data");

		if (userData != null) 
		{
			string data = JsonUtility.ToJson (userData);
			PlayerPrefs.SetString ("PlayerData", data);

			Debug.Log ("data " + data);
		}
	}


}
