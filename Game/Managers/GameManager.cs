using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum InputState
{

	Character,
	Inventory,
	ActionBox,
	Dialogue,
	Settings

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
	public Color danielColor;
	public Color geMColor;
	public Color llehctiMColor;
	public Color StellaColor;


	public static Room roomToLoad;
	public Dictionary<string,Room> stringRoomMap = new Dictionary<string, Room> ();


	public static PlayerData playerData;
	public static InventoryItemData inventoryItemData;

	public InputState inputState = InputState.Character;




	// Use this for initialization

	public void Initialize ()
	{

		//EventsHandler.cb_spacebarPressed += SetInputState;

		CreateRooms ();

		if (roomToLoad == null) 
		{
			roomToLoad = stringRoomMap ["abandoned_wing_outside_shadow"];

		}

	}



	// Update is called once per frame

	void Update ()
	{

		if (Input.GetKeyDown (KeyCode.R)) {

			CreateNewData ();

		}

			

	}



	public void CreateRooms ()
	{
		

		Object[] myTextAssets = Resources.LoadAll ("Jsons/Rooms");

		foreach (TextAsset txt in myTextAssets) {
			Room myRoom;
			myRoom = JsonUtility.FromJson<Room> (txt.text);

			// Adding room to dictionary

			stringRoomMap.Add (myRoom.myName, myRoom);
			
		}


	}

	public void CreateInventoryItemData()
	{

		if (inventoryItemData != null) 
		{
			return;
		}

		inventoryItemData = new InventoryItemData ();


	}





	/* ----- SAVING AND LOADING ----- */



	public void CreatePlayerData ()
	{

		if (playerData != null) 
		{
			//Debug.Log ("playerData is not null");
			return;
		}


		if (PlayerPrefs.HasKey ("PlayerData")) 
		{
			
			//Debug.Log ("Loading data from memory");
			playerData = JsonUtility.FromJson<PlayerData> (PlayerPrefs.GetString ("PlayerData"));

			//Debug.Log(PlayerPrefs.GetString ("PlayerData"));
			foreach (InventoryItem item in playerData.inventory.items) 
			{
				item.Initialize ();
			}


		} else {
			
			CreateNewData ();

		}
	}



	public void CreateNewData ()
	{

		Debug.Log ("Creating new data");

		playerData = new PlayerData ();

		SaveData ();

	}



	public void SaveData ()
	{
		
		Debug.Log ("Saving data");

		if (playerData != null) 
		{

			string data = JsonUtility.ToJson (playerData);
			PlayerPrefs.SetString ("PlayerData", data);

			Debug.Log ("data " + data);

		}

	}


}
