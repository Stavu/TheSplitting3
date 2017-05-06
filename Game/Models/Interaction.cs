using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;



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
