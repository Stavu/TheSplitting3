using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


[Serializable]
public class Room {

	public string myName;

	public int myWidth;
	public int myHeight;

	public Grid myGrid;

	public string bgName;

	public List <Furniture> myFurnitureList;
	public List <Character> myCharacterList;
	public List <TileInteraction> myTileInteractionList;

	public Dictionary<Furniture,Vector2> furniturePositionMap;




	// empty constructor

	/*
	public Room()
	{

		Debug.Log ("empty room" + myName);

	}
	*/


	// Shallow Room - for editor

	public Room(int myWidth, int myHeight)
	{

		//Debug.Log ("room shallow");

		//this.myName = myName;

		this.myWidth = myWidth;
		this.myHeight = myHeight;

		//this.bgName = bgName;


		myGrid = new Grid (myWidth,myHeight);

	}



	// Real Room - for game 

	public Room(Room clone)
	{

		//Debug.Log ("room clone");


		this.myName = clone.myName;

		this.myWidth = clone.myWidth;
		this.myHeight = clone.myHeight;

		this.bgName = clone.bgName;

		this.myFurnitureList = clone.myFurnitureList;
		this.myCharacterList = clone.myCharacterList;
		this.myTileInteractionList = clone.myTileInteractionList;

		myGrid = new Grid (myWidth,myHeight);

	
		CreateRoomInteractables ();

	}


	public void CreateRoomInteractables()
	{

		//Debug.Log ("Room CreateFurniture");


		// Furniture

		foreach (Furniture furn in myFurnitureList) 
		{
			
			List<Tile> FurnitureTiles = GetMyTiles(furn.mySize, furn.x, furn.y);

			foreach (Tile tile in FurnitureTiles) 
			{
				tile.PlaceFurniture(furn);

			}		

		}


		// Character

		foreach (Character character in myCharacterList) 
		{

			List<Tile> CharacterTiles = GetMyTiles(character.mySize, character.x, character.y);

			foreach (Tile tile in CharacterTiles) 
			{
				tile.PlaceCharacter(character);

			}		

		}



		// Tile interactions

		foreach (TileInteraction tileInteraction in myTileInteractionList) 
		{
			
			List<Tile> tileInteractionTiles = GetMyTiles(tileInteraction.mySize, tileInteraction.x, tileInteraction.y);

			foreach (Tile tile in tileInteractionTiles) 
			{
				tile.PlaceTileInteraction(tileInteraction);

			}		

		}

	}



	// Get all the tiles associated with this interactable object

	public List<Tile> GetMyTiles (Vector2 mySize, int x ,int y)
	{

		List<Tile> myTilesList = new List<Tile>();


		for (int i = 0; i < mySize.x; i++) {

			for (int j = 0; j < mySize.y; j++) {


				Tile tempTile = myGrid.GetTileAt (x + i, y + j);

				if (tempTile == null) 
				{
					Debug.LogError("Room: CreateFurniture temptile is null. something is wrong.");
					continue;
				}

				myTilesList.Add (tempTile);

			}
		}

		return myTilesList;

	}

}
