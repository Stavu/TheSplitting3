using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class Player {



	public string myName;
	public Vector2 mySize;

	public Vector3 myPos;

	/*
	public Vector3 myPos
	{ 
		get {return _myPos;} 
		set {

			_myPos = value;

			invokePlayerMove ();
		
		} 
	}
	*/



	public Color myTextColor;


	public Player(string myName, Vector2 mySize, Vector3 myPos)
	{
		
		// Constructor

		this.myName = myName;
		this.mySize = mySize;	
		this.myPos = myPos;	


		if (myName == "Daniel") 
		{
			Debug.Log ("color");
			myTextColor = GameManager.instance.danielColor;
		}



	}

	public void ChangePosition(Vector3 myPos)
	{

		this.myPos = myPos;
		EventsHandler.Invoke_cb_characterMove (this);

	}




}
