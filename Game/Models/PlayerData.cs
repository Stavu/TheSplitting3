using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]

public class PlayerData {


	public string currentRoom;
	public Inventory inventory;


	List<string> gameEventsList;
	public List<string> roomsVisitedList;


	public PlayerData()
	{
		inventory = new Inventory ();
		gameEventsList = new List<string> ();
		roomsVisitedList = new List<string> ();
	}



	// check if item exists

	public bool CheckIfItemExists(string itemName)
	{
		foreach (InventoryItem item in inventory.items) 
		{
			if (item.fileName == itemName) 
			{
				return true;
			}
		}

		return false;
	}



	// check if event exists

	public bool CheckIfEventExists(string eventName)
	{
		return gameEventsList.Contains (eventName);
	}



	// check if character exists

	public bool CheckIfCharacterExistsInRoom(string characterName)
	{
		foreach (Character character in RoomManager.instance.myRoom.myCharacterList) 
		{
			if (character.myName == characterName) 
			{
				return true;
			}
		}

		return false;
	}



	/// <summary>
	/// Adds to rooms visited.
	/// </summary>
	/// <returns><c>true</c>, if room didn't exist in roomsVisited, <c>false</c> otherwise.</returns>
	/// <param name="roomName">Room name.</param>

	public bool AddToRoomsVisited(string roomName)
	{
		if (roomsVisitedList.Contains (roomName) == false) 
		{
			Debug.Log ("first time");
			roomsVisitedList.Add (roomName);
			GameManager.instance.SaveData ();
			return true;
		}	

		Debug.Log ("already existed");
		return false;
	}







}
