using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class Furniture : Interactable {

	public string myName;
	//public Vector2 mySize;
	//public int x;
	//public int y;
	public Vector3 myPos {get; set;}
	//public bool walkable = false;

	public float offsetX = 0;
	public float offsetY = 0;

	public List<Interaction> myInteractionList;
	//public FurnitureData myFurnitureData; 


	/*


	// empty constructor


	public Furniture()
	{

		Debug.Log ("empty furniture");

	}

	*/



	public Furniture(string myName, int x, int y)
	{

		//Debug.Log ("furniture constructor");


		// Constructor

		this.myName = myName;

		this.x = x;
		this.y = y;
	

		myInteractionList = new List<Interaction> ();
			
	}




}



