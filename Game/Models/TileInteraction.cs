using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class TileInteraction {


	public Vector2 mySize;
	public int x;
	public int y;
	public bool walkable = false;


	public SubInteraction mySubInt;


	public TileInteraction(int x, int y)
	{

		this.x = x;
		this.y = y;

	}



}
