using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class Furniture : PhysicalInteractable {


	public bool imageFlipped = false;


	public Furniture(string myName, int x, int y)
	{

		//Debug.Log ("furniture constructor");


		// Constructor

		this.myName = myName;
		this.identificationName = myName;

		this.x = x;
		this.y = y;
	

		myInteractionList = new List<Interaction> ();
			
	}

}



