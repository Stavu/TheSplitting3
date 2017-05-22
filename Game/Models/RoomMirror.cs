using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;



[Serializable]
public class RoomMirror : Room {


	// declerations 

	public bool inTheShadow;

	string bgName_Shadow;

	public List<Furniture> myFurnitureList_Persistant;
	public List<TileInteraction> myTileInteractionList_Persistant;

	public List<Furniture> myFurnitureList_Shadow;
	public List<TileInteraction> myTileInteractionList_Shadow;




	// Shallow Room - for editor

	public RoomMirror(Room room) : base(room.myWidth, room.myHeight)
	{

		this.myFurnitureList_Persistant = new List<Furniture> ();
		this.myTileInteractionList_Persistant = new List<TileInteraction> ();

		this.myFurnitureList_Shadow = new List<Furniture> ();
		this.myTileInteractionList_Shadow = new List<TileInteraction> ();





	}



	// Real Room - for game 

	public RoomMirror(RoomMirror clone): base(clone)
	{

		this.bgName_Shadow = clone.bgName_Shadow;

		this.myFurnitureList_Shadow = clone.myFurnitureList_Shadow;
		this.myTileInteractionList_Shadow = clone.myTileInteractionList_Shadow;


	}








}
