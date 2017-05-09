using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class Character : Interactable {


	public string myName;
	public Vector3 myPos {get; set;}

	public float offsetX = 0;
	public float offsetY = 0;

	public List<Interaction> myInteractionList;





	public Character(string myName, int x, int y)
	{

		//Debug.Log ("character constructor");


		// Constructor

		this.myName = myName;

		this.x = x;
		this.y = y;


		myInteractionList = new List<Interaction> ();

	}


}
