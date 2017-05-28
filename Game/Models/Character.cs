using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class Character : PhysicalInteractable, ISpeaker, IWalker {


	public float speed = 4f;
	public Tile targetTile;
	public Vector2 targetPos;

	[NonSerialized]
	public Queue<Vector2> path;

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


	// -- Walker -- // 



	public float walkerSpeed
	{
		get
		{ 
			return speed;
		}

		set 
		{
			speed = value;
		}
	}


	public Vector2 walkerTargetPos
	{
		get
		{ 
			return targetPos;
		}

		set 
		{
			targetPos = value;
		}
	}


	public float walkerOffsetX
	{
		get
		{ 
			return offsetX;
		}

		set 
		{
			offsetX = value;
		}
	}


	public float walkerOffsetY
	{
		get
		{ 
			return offsetY;
		}

		set 
		{
			offsetY = value;
		}
	}


	public GameObject walkerGameObject
	{
		get
		{ 
			return CharacterManager.instance.characterGameObjectMap[this];
		}

		set 
		{
			CharacterManager.instance.characterGameObjectMap[this] = value;
		}
	}


	public Queue<Vector2> walkerPath
	{
		get
		{ 
			return path;
		}

		set 
		{
			path = value;
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
	
	string speakerName {get; set;}
	Vector2 speakerSize {get; set;}
	Vector3 speakerPos {get; set;}
	Color speakerTextColor {get; set;}

}




public interface IWalker
{
	string speakerName {get; set;}
	Vector2 speakerSize {get; set;}
	Vector3 speakerPos {get; set;}

	float walkerOffsetX {get; set;}
	float walkerOffsetY {get; set;}

	float walkerSpeed{get; set;}

	Vector2 walkerTargetPos {get; set;}

	GameObject walkerGameObject {get; set;}

	Queue<Vector2> walkerPath {get; set;}


}