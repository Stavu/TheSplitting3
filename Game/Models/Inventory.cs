using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;



public enum InventoryState
{

	Browse,
	UseItem,
	Combine,
	Closed

}



[Serializable]

public class Inventory {


	public List<InventoryItem> items;

	[NonSerialized]
	public InventoryState myState = InventoryState.Closed;



	public Inventory ()
	{

		// fresh inventory

		items = new List<InventoryItem> ();

		/*
		Sprite[] spriteList = Resources.LoadAll<Sprite> ("Sprites/Inventory/Small_items");

		foreach (Sprite spr in spriteList) 
		{

			InventoryItem item = new InventoryItem (spr.name,(spr.name + "title"));
		
			items.Add (item);
			
		}
		*/

		//FIXME

	}




	// Adding Item

	public void AddItem(InventoryItem item)
	{

		if (items.Contains (item)) 
		{
			Debug.LogError ("There's already an item with the same name");
			return;
		}

		items.Add (item);
		Debug.Log ("item name" + item.fileName);

		EventsHandler.Invoke_cb_inventoryChanged (this);

		GameManager.instance.SaveData ();
	}



	// Removing Item

	public void RemoveItem(InventoryItem item)
	{

		if (items.Contains (item)) 
		{
			items.Remove (item);
		}

		EventsHandler.Invoke_cb_inventoryChanged (this);

		GameManager.instance.SaveData ();

		
	}



}
