using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;





public enum RoomState
{

	Real,
	Mirror

}


[Serializable]
public class Room {

	public string myName;

	public int myWidth;
	public int myHeight;

	public Grid myGrid;
	public Grid MyGrid
	{
		get
		{ 
			if (RoomState == RoomState.Mirror) 
			{
				if (myMirrorRoom.inTheShadow == true) 
				{
					return myMirrorRoom.shadowGrid;
				}

				return myGrid;
			}

			return myGrid;
		}

		set 
		{
			if (RoomState == RoomState.Mirror) 
			{
				if (myMirrorRoom.inTheShadow == true) 
				{
					myMirrorRoom.shadowGrid = value;
				}

				myGrid = value;
			}

			myGrid = value;
		}
	}




	public string bgName;
	public bool bgFlipped = false;

	public List <Furniture> myFurnitureList;
	public List <Character> myCharacterList;
	public List <TileInteraction> myTileInteractionList;

	public Dictionary<Furniture,Vector2> furniturePositionMap;

	public RoomState roomState;
	public RoomState RoomState
	{
		get 
		{
			return roomState;

		}

		set 
		{
			roomState = value;

			if ((roomState == RoomState.Mirror) && (myMirrorRoom == null))
			{
				myMirrorRoom = new RoomMirror ();

			}
		}

	}




	public RoomMirror myMirrorRoom;




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

		RoomState = RoomState.Real;

		//this.bgName = bgName;

	
		myGrid = new Grid (myWidth,myHeight);

		this.myFurnitureList = new List<Furniture> ();
		this.myCharacterList = new List<Character> ();
		this.myTileInteractionList = new List<TileInteraction> ();


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

		this.myMirrorRoom = clone.myMirrorRoom;
		this.RoomState = clone.RoomState;

		myGrid = new Grid (myWidth,myHeight);

		if (myMirrorRoom != null) 
		{
			myMirrorRoom.shadowGrid = new Grid (myWidth, myHeight);
		}
	
		CreateRoomInteractables ();

	}


	public void CreateRoomInteractables()
	{


		// Furniture

		foreach (Furniture furn in myFurnitureList) 
		{
			//Debug.Log ("current graphic state:" + furn.currentGraphicState == null);
			List<Tile> FurnitureTiles = GetMyTiles(myGrid,furn.mySize, furn.x, furn.y);
			FurnitureTiles.ForEach (tile => tile.PlaceFurnitureInTile (furn));

		}

		// Character

		foreach (Character character in myCharacterList) 
		{
			List<Tile> CharacterTiles = GetMyTiles(myGrid,character.mySize, character.x, character.y);
			CharacterTiles.ForEach (tile => tile.PlaceCharacterInTile (character));

		}

		// Tile interactions

		foreach (TileInteraction tileInteraction in myTileInteractionList) 
		{
			List<Tile> TileInteractionTiles = GetMyTiles(myGrid,tileInteraction.mySize, tileInteraction.x, tileInteraction.y);
			TileInteractionTiles.ForEach (tile => tile.PlaceTileInteraction (tileInteraction));

		}


		if (RoomState == RoomState.Mirror) 
		{

			if (myMirrorRoom.inTheShadow == true) 
			{

				// -- SHADOW ROOM -- //

				// Furniture

				foreach (Furniture furn in myMirrorRoom.myFurnitureList_Shadow) 
				{
					List<Tile> FurnitureTiles = GetMyTiles(myMirrorRoom.shadowGrid, furn.mySize, furn.x, furn.y);
					FurnitureTiles.ForEach (tile => tile.PlaceFurnitureInTile (furn));
				}

				// Tile interactions

				foreach (TileInteraction tileInteraction in myMirrorRoom.myTileInteractionList_Shadow)
				{
					List<Tile> TileInteractionTiles = GetMyTiles(myMirrorRoom.shadowGrid, tileInteraction.mySize, tileInteraction.x, tileInteraction.y);
					TileInteractionTiles.ForEach (tile => tile.PlaceTileInteraction (tileInteraction));

				}

			} 


			// -- PERSISTENT -- //

			// Furniture

			foreach (Furniture furn in myMirrorRoom.myFurnitureList_Persistant) 
			{
				List<Tile> FurnitureTiles = GetMyTiles(myGrid, furn.mySize, furn.x, furn.y);
				FurnitureTiles.ForEach (tile => tile.PlaceFurnitureInTile (furn));

				List<Tile> FurnitureTiles_Shadow = GetMyTiles(myMirrorRoom.shadowGrid, furn.mySize, furn.x, furn.y);
				FurnitureTiles_Shadow.ForEach (tile => tile.PlaceFurnitureInTile (furn));


			}

			// Tile interactions

			foreach (TileInteraction tileInteraction in myMirrorRoom.myTileInteractionList_Persistant)
			{
				List<Tile> TileInteractionTiles = GetMyTiles(myGrid, tileInteraction.mySize, tileInteraction.x, tileInteraction.y);
				TileInteractionTiles.ForEach (tile => tile.PlaceTileInteraction (tileInteraction));

				List<Tile> TileInteractionTiles_Shadow = GetMyTiles(myMirrorRoom.shadowGrid, tileInteraction.mySize, tileInteraction.x, tileInteraction.y);
				TileInteractionTiles_Shadow.ForEach (tile => tile.PlaceTileInteraction (tileInteraction));

			}

		}


	}



	// Get all the tiles associated with this interactable object

	public List<Tile> GetMyTiles (Grid grid, Vector2 mySize, int x ,int y)
	{

		List<Tile> myTilesList = new List<Tile>();


		for (int i = 0; i < mySize.x; i++) {

			for (int j = 0; j < mySize.y; j++) {


				Tile tempTile = grid.GetTileAt (x + i, y + j);

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



	// Get tile list using a grid and a list of coords

	public List<Tile> GetMyTiles (Grid grid, List<Coords> coordsList)
	{

		List<Tile> myTilesList = new List<Tile>();

		foreach (Coords coords in coordsList) 
		{
			myTilesList.Add (grid.GetTileAt (coords.x, coords.y));
		}

		return myTilesList;

	}


}
