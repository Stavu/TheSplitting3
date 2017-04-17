using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


public enum Direction
{
	left,right,up,down
}



public class InputManager : MonoBehaviour {



	// Singleton //

	public static InputManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //





	// Use this for initialization
	public void Initialize () {
		
	}
	
	// Update is called once per frame

	void Update () {


		CheckKeys ();
		CheckKeysDown ();

	}



	void CheckKeys()
	{



		if (Input.GetKey (KeyCode.LeftArrow)) 		
		{

			EventsHandler.Invoke_cb_keyPressed (Direction.left);

		}

		else if (Input.GetKey (KeyCode.RightArrow))			
		{
			EventsHandler.Invoke_cb_keyPressed (Direction.right);

		}


		else if (Input.GetKey (KeyCode.DownArrow))			
		{
			EventsHandler.Invoke_cb_keyPressed (Direction.down);

		}


		else if (Input.GetKey (KeyCode.UpArrow))			
		{
			EventsHandler.Invoke_cb_keyPressed (Direction.up);

		}


		if (Input.GetKeyDown (KeyCode.Space))
		{			
			EventsHandler.Invoke_cb_spacebarPressed ();

		}

		if (Input.GetKeyDown (KeyCode.Escape))
		{			
			EventsHandler.Invoke_cb_escapePressed ();
		}


	}


	void CheckKeysDown()
	{
		
		if (Input.GetKeyDown (KeyCode.UpArrow)) 
		{			
			
			EventsHandler.Invoke_cb_keyPressedDown (Direction.up);

		} 

		else if (Input.GetKeyDown (KeyCode.DownArrow)) 
		
		{
			
			EventsHandler.Invoke_cb_keyPressedDown (Direction.down);
		
		}
	}






}
