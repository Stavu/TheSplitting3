using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


/*
public enum InteractionType
{
	
	look_at,
	open,
	close,
	enter,
	walk_in,
	go_out,
	pick_up,
	use_item,
	touch,
	rotate,
	use,
	turn_on,
	turn_off,
	sit,
	smell,
	knock,
	climb,
	flush,
	talk_to,
	listen,
	give_item


}
*/


[Serializable]
public class Interaction
{

	//public InteractionType myInteractionType;
	public string myVerb;
	//public List<string> textList;
	//public List<Condition> myConditionList;
	//public string actionTargetName; 



	// move to room interaction

	public Interaction (InteractionType myInteractionType, string actionTargetName, List<Condition> myConditionList = null)
	{


		this.myInteractionType = myInteractionType;
		this.actionTargetName = actionTargetName;
		this.myConditionList = myConditionList;


		if (myInteractionType == InteractionType.enter) 
		{

			this.myVerb = "Enter";

		}


	}




	public Interaction(InteractionType myInteractionType, List<string> textList, List<Condition> myConditionList = null)
	{

		this.myInteractionType = myInteractionType;
		this.textList = textList;
		this.myConditionList = myConditionList;


		switch (myInteractionType) {


		case InteractionType.look_at:

			this.myVerb = "Look At";
				
			break;					


		case InteractionType.open:

			this.myVerb = "Open";

			break;


		case InteractionType.close:

			this.myVerb = "Close";

			break;


		case InteractionType.enter:

			this.myVerb = "Enter";

			break;


		case InteractionType.walk_in:

			this.myVerb = "Walk In";

			break;


		case InteractionType.go_out:

			this.myVerb = "Go Out";

			break;


		case InteractionType.pick_up:

			this.myVerb = "Pick Up";

			break;


		case InteractionType.use_item:

			this.myVerb = "Use Item";

			break;


		case InteractionType.rotate:

			this.myVerb = "Rotate";

			break;


		case InteractionType.touch:

			this.myVerb = "Touch";

			break;

		case InteractionType.use:

			this.myVerb = "Use";

			break;


		case InteractionType.turn_on:

			this.myVerb = "Turn On";

			break;


		case InteractionType.turn_off:

			this.myVerb = "Turn Off";

			break;


		case InteractionType.sit:

			this.myVerb = "Sit";

			break;


		case InteractionType.smell:

			this.myVerb = "Smell";

			break;


		case InteractionType.knock:

			this.myVerb = "Knock";

			break;


		case InteractionType.climb:

			this.myVerb = "Climb";

			break;


		case InteractionType.flush:

			this.myVerb = "Flush";

			break;



		// character interactions


		case InteractionType.talk_to:

			this.myVerb = "Talk To";

			break;

		
		case InteractionType.listen:

			this.myVerb = "Listen";

			break;


		case InteractionType.give_item:

			this.myVerb = "Give Item";

			break;



		}

	}



	public void Interact ()
	{

		switch (myInteractionType) {

		case InteractionType.look_at:

			if (GameManager.actionBoxActive) 
			{

				Debug.Log ("Interaction Interact look_at");

				InteractionManager.instance.DisplayText (PlayerManager.instance.myPlayer, textList [0]);
			

			}


			break;


		case InteractionType.enter:

			if (GameManager.actionBoxActive) 
			{

				Debug.Log ("Interaction Interact enter");

				InteractionManager.instance.MoveToRoom (actionTargetName, Direction.up);


			}


			break;

		}
	
	}



	public void EnterRoom(string name, Direction direction)
	{


		InteractionManager.instance.MoveToRoom (name, direction);


	}











}
