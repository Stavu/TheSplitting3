using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


[Serializable]
public class SubInteraction {


	public string interactionType;
	public List<string> textList;
	public List<Condition> myConditionList;
	public string destinationRoomName;
	public string itemToRecieveName;
	public string ItemToUseName;
	public bool ItemToUseRemoveBool;





	// move to room interaction


	public SubInteraction (string interactionType)
	{

		this.interactionType = interactionType;


	}






	public void SubInteract ()
	{

		switch (interactionType) {

		case :

			if (GameManager.actionBoxActive) 
			{

				Debug.Log ("Interaction Interact look_at");

				InteractionManager.instance.DisplayText (PlayerManager.instance.myPlayer, textList [0]);


			}


			break;



	}



	public void EnterRoom(string name, Direction direction)
	{


		InteractionManager.instance.MoveToRoom (name, direction);


	}














}
