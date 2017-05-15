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

	public string conversationName;






	// move to room interaction


	public SubInteraction (string interactionType)
	{

		this.interactionType = interactionType;


	}



	public void SubInteract ()
	{

		switch (interactionType) {


			case "showDialogue":


				Debug.Log ("SubInteract: Show dialogue");

				InteractionManager.instance.DisplayText (Utilities.CreateSentenceList(PlayerManager.instance.myPlayer, textList));

			
				break;


			case "changeConversation":

				DialogueManager.instance.SetConversation (conversationName);

				break;


			case "endDialogueTree":

				DialogueManager.instance.DestroyDialogueTree ();

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


				InteractionManager.instance.OpenInventory_UseItem (ActionBoxManager.instance.currentPhysicalInteractable);

				break;



			case "combine":
				
				InteractionManager.instance.OpenInventory_CombineItem ();

				break;


		}



	}
















}
