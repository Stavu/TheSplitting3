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

				//	Debug.Log ("show dialogue");

					InteractionManager.instance.DisplayText (PlayerManager.instance.myPlayer, textList [0]);

				}


				break;



			case "moveToRoom":

				InteractionManager.instance.MoveToRoom (destinationRoomName, direction);

				break;



			case "pickUpItem":

				//Debug.Log ("subinteraction" + "pickUp Item");
				InteractionManager.instance.PickUpItem (inventoryItem);
				ActionBoxManager.instance.CloseFurnitureFrame ();			
				GameManager.instance.inputState = InputState.Character;


				break;



			case "useItem":

				Debug.Log ("subinteraction " + "use Item");
			
				InteractionManager.instance.UseItem (ActionBoxManager.instance.currentFurniture);

				break;





		}



	}
















}
