using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemData {



	public Dictionary <string,string> itemLookAtMap;


	public InventoryItemData()
	{

		itemLookAtMap = new Dictionary<string, string> ();

		itemLookAtMap.Add ("compass", "It's the compass I brought from the car's trunk.");
		itemLookAtMap.Add ("order_details", "I don't know what it is, but it looks expensive.");
		itemLookAtMap.Add ("flashlight", "It's my flashlight.");



	}






}
