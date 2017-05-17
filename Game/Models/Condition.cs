using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;




public enum ConditionType
{

	HasItem,
	EventOccured,
	CharacterInRoom,
	LacksItem,
	EventDidntOccur,
	CharacterNotInRoom

}





[Serializable]
public class Condition {


	public ConditionType myType;

	public string hasItem;
	public string eventOccured;
	public string characterInRoom;
	public string lacksItem;
	public string eventDidntOccur;
	public string characterNotInRoom;



	public Condition(ConditionType type, string myString)
	{

		myType = type;

		switch (myType) 
		{
		
			case ConditionType.HasItem:

				hasItem = myString;

				break;


			case ConditionType.EventOccured:

				eventOccured = myString;

				break;


			case ConditionType.CharacterInRoom:

				characterInRoom = myString;

				break;	


			case ConditionType.LacksItem:

				lacksItem = myString;

				break;


			case ConditionType.EventDidntOccur:

				eventDidntOccur = myString;

				break;


			case ConditionType.CharacterNotInRoom:

				characterNotInRoom = myString;

				break;	
		
		}
	}




	public bool EvaluateCondition()
	{

		switch (myType)
		{
			
			case ConditionType.HasItem:
				
				return GameManager.playerData.CheckIfItemExists (hasItem);



			case ConditionType.EventOccured:
				
				return GameManager.playerData.CheckIfEventExists (eventOccured);

			

			case ConditionType.CharacterInRoom:
				
				return GameManager.playerData.CheckIfCharacterExistsInRoom (characterInRoom);

			

			case ConditionType.LacksItem:

				return !GameManager.playerData.CheckIfItemExists (lacksItem);

				break;


			case ConditionType.EventDidntOccur:

				return !GameManager.playerData.CheckIfEventExists (eventDidntOccur);

				break;


			case ConditionType.CharacterNotInRoom:

				return !GameManager.playerData.CheckIfCharacterExistsInRoom (characterNotInRoom);

				break;	


		}

		return false;

	}





}
