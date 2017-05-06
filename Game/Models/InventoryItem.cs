using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]

public class InventoryItem
{

	public string titleName;
	public string fileName;

	[NonSerialized]
	public string lookAtText;

	[NonSerialized]
	public List<Interaction> inventoryItemInteractionList;



	public void Initialize()
	{

		inventoryItemInteractionList = new List<Interaction>();


		Interaction interactionLookAt = new Interaction ();

		interactionLookAt.myVerb = "Look At";
		SubInteraction subInt = new SubInteraction ("showDialogue");
		subInt.textList = new List<string> ();

		if (fileName == null) 
		{
		
			Debug.Log ("fileName is null");

		}


		if (GameManager.inventoryItemData == null) 
		{

			Debug.Log ("inventoryItemData is null");

		}


		if (GameManager.inventoryItemData.itemLookAtMap == null) 
		{

			Debug.Log ("itemLookAtMap is null");

		}


		if (subInt.textList == null) 
		{

			Debug.Log ("subInt.textList is null");

		}

		if (GameManager.inventoryItemData.itemLookAtMap.ContainsKey (fileName))
		{
			subInt.textList.Add (GameManager.inventoryItemData.itemLookAtMap [fileName]);
		}

		interactionLookAt.subInteractionList.Add (subInt);

	
		Interaction interactionCombine = new Interaction ();
		interactionCombine.myVerb = "Combine";

		Interaction interactionOpen = new Interaction ();
		interactionOpen.myVerb = "Open";





		inventoryItemInteractionList.Add (interactionLookAt);
		inventoryItemInteractionList.Add (interactionCombine);
		inventoryItemInteractionList.Add (interactionOpen);

	}



	public InventoryItem(string fileName, string titleName)
	{

		this.fileName = fileName;
		this.titleName = titleName;


	}



}
