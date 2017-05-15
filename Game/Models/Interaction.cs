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





}
