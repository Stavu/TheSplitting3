using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class PhysicalInteractable : Interactable {


	public string identificationName;
	public string myName;

	public Vector3 myPos {get; set;}

	//public Vector2 frameExtents;

	//public float frameOffsetX;
	//public float frameOffsetY;

	public float offsetX = 0;
	public float offsetY = 0;

	public List<Interaction> myInteractionList;

	public List<GraphicState> graphicStates;

	[NonSerialized]
	public GraphicState currentGraphicState;

	public GraphicState CurrentGraphicState()
	{
		if (currentGraphicState == null) 
		{
			currentGraphicState = graphicStates [0];
		} 

		return currentGraphicState;
	}





}



[Serializable]
public class GraphicState {

	public string graphicStateName;

	public Vector2 frameExtents;

	public float frameOffsetX;
	public float frameOffsetY;

	public List<Coords> coordsList;




}



[Serializable]
public struct Coords {

	public int x;
	public int y;

	// Constructor
	public Coords(int x, int y)
	{
		this.x = x;
		this.y = y;
	}
}