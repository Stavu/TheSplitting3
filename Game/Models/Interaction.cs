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


	public string myVerb;
	public List<SubInteraction> subInteractionList;
	public List<Condition> conditionList;


	//  Constructor

	public Interaction ()
	{
		
		subInteractionList = new List<SubInteraction>();
		conditionList = new List<Condition>();

	}





}
