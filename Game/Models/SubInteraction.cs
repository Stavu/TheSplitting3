using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


[Serializable]
public class SubInteraction {


	public string interactionType;

	public List<string> textList;
	//public List<Condition> myConditionList;
	public string rawText;
	public Direction direction;

	public string destinationRoomName;
	public string ItemToUseName;
	public bool ItemToUseRemoveBool;
	public List<InventoryItem> inventoryItems;

	public InventoryItem inventoryItem;



	// move to room interaction


	public SubInteraction (string interactionType)
	{

		this.interactionType = interactionType;


	}



	public void SubInteract ()
	{

		switch (interactionType) {


			case "showDialogue":

				if (GameManager.actionBoxActive) 
				{
					
					InteractionManager.instance.DisplayText (PlayerManager.instance.myPlayer, textList [0]);

				}

				break;


			case "showInventoryText":
				
				InteractionManager.instance.DisplayInventoryText (textList);

				break;


			case "changeInventoryItemBigPicture":

				//InteractionManager.instance.ChangeInventoryItemBigPicture (fileName);

				break;


			case "moveToRoom":

				InteractionManager.instance.MoveToRoom (destinationRoomName, direction);

				break;



			case "pickUpItem":

				InteractionManager.instance.PickUpItem (inventoryItem);
				ActionBoxManager.instance.CloseFurnitureFrame ();			

				if (GameManager.instance.inputState == InputState.ActionBox) 
				{
					GameManager.instance.inputState = InputState.Character;
				}


				break;



			case "useItem":


				InteractionManager.instance.OpenInventory_UseItem (ActionBoxManager.instance.currentFurniture);

				break;



			case "combine":
				
				InteractionManager.instance.OpenInventory_CombineItem ();

				break;


		}



	}
















}
