using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class Character : PhysicalInteractable {


	public float speed = 4f;
	public Tile targetTile;


	public Character(string myName, int x, int y)
	{
		
		// Constructor

		this.myName = myName;

		this.x = x;
		this.y = y;


		myInteractionList = new List<Interaction> ();

	}




	public void ChangeTile(Tile newTile)
	{


		this.x = newTile.x;
		this.y = newTile.y;

		this.myPos = new Vector3 (newTile.x, newTile.y, 0);

		newTile.myCharacter = this;

	}


}
