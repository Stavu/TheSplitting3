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

		myGrid = new Grid (myWidth,myHeight);

	
		CreateFurniture ();

	}


	public void CreateFurniture()
	{

		//Debug.Log ("Room CreateFurniture");



		foreach (Furniture furn in myFurnitureList) 
		{


			List<Tile> FurnitureTiles = GetFurnitureTiles(furn);

			foreach (Tile tile in FurnitureTiles) {

				tile.PlaceFurniture(furn);


			}		

		}

	}




	public List<Tile> GetFurnitureTiles (Furniture furn)
	{

		List<Tile> myFurnitureTilesList = new List<Tile>();


		for (int i = 0; i < furn.mySize.x; i++) {

			for (int j = 0; j < furn.mySize.y; j++) {


				Tile tempTile = myGrid.GetTileAt (furn.x + i, furn.y + j);


				if (tempTile == null) 
				{
					Debug.LogError("Room: CreateFurniture temptile is null. something is wrong.");
					continue;
				}



				myFurnitureTilesList.Add (tempTile);


			}
		}


		return myFurnitureTilesList;

	}

}
