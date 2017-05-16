using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;



[Serializable]
public class SubInteraction : IConditionable {


	public string interactionType;

	public List<string> textList;
	public List<Condition> conditionList;
	public string rawText;
	public Direction direction;

	public string destinationRoomName;
	public string ItemToUseName;
	public bool ItemToUseRemoveBool;
	//public List<InventoryItem> inventoryItems;

	public InventoryItem inventoryItem;

	public string conversationName;



	public List<Condition> ConditionList 
	{
		get
		{ 
			return conditionList;
		}

		set 
		{
			conditionList = value;
		}
	}






	// move to room interaction


	public SubInteraction (string interactionType)
	{

		this.interactionType = interactionType;
		conditionList = new List<Condition>();

	}



	public void RemoveConditionFromList(Condition condition)
	{

		if (condition == null) 
		{

			Debug.LogError ("condition is null");
			return;
		}


		if (conditionList.Contains (condition) == false) 
		{
			Debug.LogError ("condition is not in list");
			return;
		}

		conditionList.Remove (condition);

	}




	// ----- SUBINTERACT ----- //



	public void SubInteract ()
	{

		switch (interactionType) {



			case "showMonologue":


				Debug.Log ("SubInteract: Show monologue");

				InteractionManager.instance.DisplayText (Utilities.CreateSentenceList(PlayerManager.instance.myPlayer, textList));


				break;



			case "showDialogue":


				Debug.Log ("SubInteract: Show dialogue");

				InteractionManager.instance.DisplayText (Utilities.CreateSentenceList(PlayerManager.instance.myPlayer, textList));
							
				break;


			case "showDialogueTree":

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


		


			case "combine":
				
				InteractionManager.instance.OpenInventory_CombineItem ();

				break;


		}

	}



	public void ResetDataFields()
	{

		this.rawText = string.Empty;
		this.destinationRoomName = string.Empty;

		this.inventoryItem = null;

	}






}







// Interface

public interface ISubinteractable
{	

	List<SubInteraction> SubIntList { get; set; }


}
