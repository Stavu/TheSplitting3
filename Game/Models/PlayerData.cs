using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]

public class PlayerData {


	public string currentRoom;
	public Inventory inventory;



	public PlayerData()
	{

		inventory = new Inventory ();

	}


}
