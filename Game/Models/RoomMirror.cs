using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;



[Serializable]
public class RoomMirror : Room {


	// declerations 

	public bool inTheShadow;

	string bgName_Shadow;

	public List<Furniture> myFurnitureList_Shadow;
	public List<TileInteraction> myTileInteractionList_Shadow;




	// Shallow Room - for editor

	public RoomMirror(int myWidth, int myHeight) : base(myWidth, myHeight)
	{

	}



	// Real Room - for game 

	public RoomMirror(RoomMirror clone): base(clone)
	{

		this.bgName_Shadow = clone.bgName_Shadow;

		this.myFurnitureList_Shadow = clone.myFurnitureList_Shadow;
		this.myTileInteractionList_Shadow = clone.myTileInteractionList_Shadow;


	}





}
