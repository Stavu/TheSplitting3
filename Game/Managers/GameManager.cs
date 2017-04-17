using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {



	public static bool actionBoxActive = false;
	public static bool textBoxActive = false;
	public Color danielColor;
	public Color geMColor;
	public Color llehctiMColor;
	public Color StellaColor;

	public static Room roomToLoad;


	public Dictionary<string,Room> stringRoomMap = new Dictionary<string, Room> ();




	// Singleton //

	public static GameManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //



	// Use this for initialization
	public void Initialize () {
		
		CreateRooms ();
		if (roomToLoad == null) 
		{
			roomToLoad = stringRoomMap ["abandoned_wing_outside_shadow"];

		}

	}



	// Update is called once per frame
	void Update () 
	{
		
	}



	public void CreateRooms()
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





}
