using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Tile {



	public int x {get; protected set;}
	public int y {get; protected set;}

	public Furniture myFurniture; 

	public bool walkable;




	public Tile(int x, int y)
	{

		this.x = x;
		this.y = y;

		//Debug.Log ("Created new tile at" + this.x + this.y); 

	}


	public void PlaceFurniture(Furniture myObject)
	{
		
		if (myObject == null) 
		{
			Debug.LogError("Tile: PlaceRoomObject myObject is null");
		
			return;
		}


		if(myFurniture != null)
		{
			if (myFurniture != myObject) 
			{
				Debug.LogError("Tile: PlaceRoomObject myRoomObject exists");

				return;
			}

		}


		// if everything's okay, set myFurniture

		myFurniture = myObject;



	}


}
