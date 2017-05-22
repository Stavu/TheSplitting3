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


	// Constructor for flipped furniture

	public Furniture(Room room, Furniture furn)
	{		
	
		this.myName = furn.myName;
		this.x = room.MyGrid.myWidth - 1 - furn.x - ((int)furn.mySize.x - 1);
		this.y = furn.y;

		this.imageFlipped = !furn.imageFlipped;

		this.offsetX = -furn.offsetX;
		this.offsetY = furn.offsetY;
		this.mySize = furn.mySize;
		this.walkable = furn.walkable;

	}




}



