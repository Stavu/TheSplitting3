using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public static class EventsHandler
{


	// Input Manager 


	public static Action<Direction> cb_keyPressed; 

	public static void Invoke_cb_keyPressed(Direction direction)
	{
		if(cb_keyPressed != null)
		{
			cb_keyPressed (direction);
		}

	}


	public static Action<Direction> cb_keyPressedDown;

	public static void Invoke_cb_keyPressedDown(Direction direction)
	{
		if(cb_keyPressedDown != null)
		{
			cb_keyPressedDown (direction);
		}

	}


	public static Action cb_spacebarPressed;

	public static void Invoke_cb_spacebarPressed()
	{
		if(cb_spacebarPressed != null)
		{
			cb_spacebarPressed ();
		}

	}


	public static Action cb_escapePressed;

	public static void Invoke_cb_escapePressed()
	{
		if(cb_escapePressed != null)
		{
			cb_escapePressed ();
		}

	}


	public static Action cb_key_i_pressed;

	public static void Invoke_cb_key_i_pressed()
	{
		if(cb_key_i_pressed != null)
		{
			cb_key_i_pressed ();
		}

	}




	// Player Manager 


	public static Action<Player> cb_playerCreated; 

	public static void Invoke_cb_playerCreated(Player player)
	{
		if(cb_playerCreated != null)
		{
			cb_playerCreated (player);
		}

	}



	public static Action<Furniture,Tile> cb_playerHitFurniture; 

	public static void Invoke_cb_playerHitFurniture(Furniture furniture, Tile tile)
	{
		if(cb_playerHitFurniture != null)
		{
			cb_playerHitFurniture (furniture, tile);
		}

	}



	public static Action<Furniture> cb_playerLeaveFurniture; 

	public static void Invoke_cb_playerLeaveFurniture(Furniture furniture)
	{
		if(cb_playerLeaveFurniture != null)
		{
			cb_playerLeaveFurniture (furniture);
		}

	}




	// Room Manager 


	public static Action<Furniture> cb_furnitureChanged; 

	public static void Invoke_cb_furnitureChanged(Furniture furniture)
	{
		if(cb_furnitureChanged != null)
		{
			cb_furnitureChanged (furniture);
		}

	}


	public static Action<Room> cb_roomCreated; 

	public static void Invoke_cb_roomCreated(Room room)
	{
		if(cb_roomCreated != null)
		{
			cb_roomCreated (room);
		}

	}



	// Player 


	public static Action<Player> cb_characterMove; 

	public static void Invoke_cb_characterMove(Player player)
	{
		if(cb_characterMove != null)
		{
			cb_characterMove (player);
		}

	}



	// Inventory


	public static Action<Inventory> cb_inventoryChanged;

	public static void Invoke_cb_inventoryChanged(Inventory inventory)
	{
		if(cb_inventoryChanged != null)
		{
			cb_inventoryChanged (inventory);
		}

	}





	/* ------- EDITOR -------- */



	// Editor Room Manager 


	public static Action<Room> cb_editorNewRoomCreated;

	public static void Invoke_cb_editorNewRoomCreated(Room room)
	{
		if(cb_editorNewRoomCreated != null)
		{
			cb_editorNewRoomCreated (room);
		}

	}


	/*
	public static Action<string> cb_editorBgNameChanged;

	public static void Invoke_cb_editorBgNameChanged(string bgName)
	{
		if(cb_editorBgNameChanged != null)
		{
			cb_editorBgNameChanged (bgName);
		}

	}
	*/


	public static Action<Furniture> cb_editorFurnitureModelChanged;

	public static void Invoke_cb_editorFurnitureModelChanged(Furniture furniture)
	{
		if(cb_editorFurnitureModelChanged != null)
		{
			cb_editorFurnitureModelChanged (furniture);
		}

	}


	public static Action<Furniture> cb_editorFurniturePlaced;

	public static void Invoke_cb_editorFurniturePlaced(Furniture furniture)
	{
		if(cb_editorFurniturePlaced != null)
		{
			cb_editorFurniturePlaced (furniture);
		}

	}


	public static Action<Furniture> cb_editorFurnitureChanged;

	public static void Invoke_cb_editorFurnitureChanged(Furniture furniture)
	{
		if(cb_editorFurnitureChanged != null)
		{
			cb_editorFurnitureChanged (furniture);
		}

	}



	// Mouse Controller 

	public static Action<List<Tile>> cb_editorTilesSelected;

	public static void Invoke_cb_editorTilesSelected(List<Tile> tileList)
	{
		if(cb_editorTilesSelected != null)
		{
			cb_editorTilesSelected (tileList);
		}

	}






}
