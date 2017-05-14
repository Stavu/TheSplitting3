using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class Character : PhysicalInteractable, ISpeaker{


	public float speed = 4f;
	public Tile targetTile;

	public Color myTextColor;



	public string speakerName 
	{
		get
		{ 
			return myName;
		}

		set 
		{
			myName = value;
		}
	}



	public Vector2 speakerSize 
	{
		get
		{ 
			return mySize;
		}

		set 
		{
			mySize = value;
		}
	}


	public Vector3 speakerPos
	{
		get
		{ 
			return myPos;
		}

		set 
		{
			myPos = value;
		}
	}


	public Color speakerTextColor
	{
		get
		{ 
			return GameManager.speakerColorMap [speakerName];
		}

		set 
		{
			myTextColor = value;
		}
	}





	public Character(string myName, int x, int y)
	{
		
		// Constructor

		this.myName = myName;

		this.x = x;
		this.y = y;


		myInteractionList = new List<Interaction> ();


		myTextColor = Color.cyan;


	}




	public void ChangeTile(Tile newTile)
	{


		this.x = newTile.x;
		this.y = newTile.y;

		this.myPos = new Vector3 (newTile.x, newTile.y, 0);

		newTile.myCharacter = this;

	}



}



// Interface

public interface ISpeaker
{	
	
	string speakerName { get; set; }
	Vector2 speakerSize {get; set;}
	Vector3 speakerPos {get; set;}
	Color speakerTextColor {get; set;}



}
