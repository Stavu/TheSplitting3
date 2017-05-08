using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Tile {



	public int x {get; protected set;}
	public int y {get; protected set;}

	public Furniture myFurniture; 
	public TileInteraction myTileInteraction;

	public bool walkable;




	public Tile(int x, int y)
	{

		this.x = x;
		this.y = y;

		//Debug.Log ("Created new tile at" + this.x + this.y); 

	}


	public void PlaceFurniture(Furniture furniture)
	{
		
		if (furniture == null) 
		{
			Debug.LogError("Tile: PlaceRoomObject myObject is null");
		
			return;
		}


		if(myFurniture != null)
		{
			if (myFurniture != furniture) 
			{
				Debug.LogError("Tile: PlaceRoomObject myRoomObject exists");

				return;
			}

		}


		// if everything's okay, set myFurniture

		myFurniture = furniture;

	}



	public void PlaceTileInteraction(TileInteraction tileInteraction)
	{


		if (tileInteraction == null) 
		{
			Debug.LogError("Tile: PlaceRoomObject tileInteraction is null");

			return;
		}


		if(myTileInteraction != null)
		{
			if (myTileInteraction != tileInteraction) 
			{
				Debug.LogError("Tile: PlaceRoomObject tileInteraction exists");

				return;
			}

		}


		// if everything's okay, set myFurniture

		myTileInteraction = tileInteraction;


	}




}
