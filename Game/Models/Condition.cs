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
	CharacterNotInRoom,
	InTheShadow,
	NotInTheShadow

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


			case ConditionType.InTheShadow:

				break;	


			case ConditionType.NotInTheShadow:

				break;	

		
		}
	}




	public bool EvaluateCondition()
	{

		switch (myType)
		{

			// Positive Conditions
			
			case ConditionType.HasItem:
				
				return GameManager.playerData.CheckIfItemExists (hasItem);


			case ConditionType.EventOccured:
				
				return GameManager.playerData.CheckIfEventExists (eventOccured);

			
			case ConditionType.CharacterInRoom:
				
				return GameManager.playerData.CheckIfCharacterExistsInRoom (characterInRoom);
							

			case ConditionType.InTheShadow:

				if (RoomManager.instance.myRoom.myMirrorRoom == null) 
				{
					Debug.LogError ("mirror room doesn't exist");
					return false;
				}

				return RoomManager.instance.myRoom.myMirrorRoom.inTheShadow;					



			// Negative Conditions

			case ConditionType.LacksItem:

				return !GameManager.playerData.CheckIfItemExists (lacksItem);


			case ConditionType.EventDidntOccur:

				return !GameManager.playerData.CheckIfEventExists (eventDidntOccur);


			case ConditionType.CharacterNotInRoom:

				return !GameManager.playerData.CheckIfCharacterExistsInRoom (characterNotInRoom);


			case ConditionType.NotInTheShadow:

				if (RoomManager.instance.myRoom.myMirrorRoom == null) 
				{
					Debug.LogError ("mirror room doesn't exist");
					return false;
				}

				return !RoomManager.instance.myRoom.myMirrorRoom.inTheShadow;
					
		}

		return false;

	}





}
