using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;




public enum ConditionType
{

	HasItem,
	EventOccured,
	CharacterInRoom

}





[Serializable]
public class Condition {


	public ConditionType myType;

	public string hasItem;
	public string eventOccured;
	public string characterInRoom;



	public Condition(ConditionType type, string myString)
	{

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


		}

		return false;

	}





}
