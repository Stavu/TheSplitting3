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


	public static Action<Direction> cb_noKeyPressed;

	public static void Invoke_cb_noKeyPressed(Direction direction)
	{
		if(cb_noKeyPressed != null)
		{
			cb_noKeyPressed (direction);
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



	public static Action<PhysicalInteractable,Tile> cb_playerHitPhysicalInteractable; 

	public static void Invoke_cb_playerHitPhysicalInteractable(PhysicalInteractable physicalInt, Tile tile)
	{
		if(cb_playerHitPhysicalInteractable != null)
		{
			cb_playerHitPhysicalInteractable (physicalInt, tile);
		}

	}



	public static Action cb_playerLeavePhysicalInteractable; 

	public static void Invoke_cb_playerLeavePhysicalInteractable()
	{
		if(cb_playerLeavePhysicalInteractable != null)
		{
			cb_playerLeavePhysicalInteractable ();
		}

	}


	public static Action<Tile> cb_playerHitTileInteraction;

	public static void Invoke_cb_playerHitTileInteraction(Tile tile)
	{
		if(cb_playerHitTileInteraction != null)
		{
			cb_playerHitTileInteraction (tile);
		}

	}


	public static Action cb_playerLeaveTileInteraction;

	public static void Invoke_cb_playerLeaveTileInteraction()
	{
		if(cb_playerLeaveTileInteraction != null)
		{
			cb_playerLeaveTileInteraction ();
		}

	}




	// Room Manager 



	public static Action<Room> cb_roomCreated; 

	public static void Invoke_cb_roomCreated(Room room)
	{
		if(cb_roomCreated != null)
		{
			cb_roomCreated (room);
		}

	}


	public static Action<Furniture> cb_furnitureChanged; 

	public static void Invoke_cb_furnitureChanged(Furniture furniture)
	{
		if(cb_furnitureChanged != null)
		{
			cb_furnitureChanged (furniture);
		}

	}


	public static Action<Character> cb_characterChanged; 

	public static void Invoke_cb_characterChanged(Character character)
	{
		if(cb_characterChanged != null)
		{
			cb_characterChanged (character);
		}

	}




	public static Action<TileInteraction> cb_tileInteractionChanged; 

	public static void Invoke_cb_tileInteractionChanged(TileInteraction tileInt)
	{
		if(cb_tileInteractionChanged != null)
		{
			cb_tileInteractionChanged (tileInt);
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

	public static Action<InventoryItem> cb_itemAddedToInventory;

	public static void Invoke_cb_itemAddedToInventory(InventoryItem inventoryItem)
	{
		if(cb_itemAddedToInventory != null)
		{
			cb_itemAddedToInventory (inventoryItem);
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



	// Action for tile manager - coloring tiles


	public static Action cb_tileLayoutChanged;

	public static void Invoke_cb_tileLayoutChanged()
	{		

		if (cb_tileLayoutChanged != null) 
		{
			cb_tileLayoutChanged ();
		}

	}


	public static Action<Furniture> cb_editorFurnitureModelChanged;

	public static void Invoke_cb_editorFurnitureModelChanged(Furniture furniture)
	{
		if(cb_editorFurnitureModelChanged != null)
		{
			cb_editorFurnitureModelChanged (furniture);

		}

		if (cb_tileLayoutChanged != null) 
		{
			cb_tileLayoutChanged ();
		}

	}


	public static Action<Character> cb_editorCharacterModelChanged;


	public static void Invoke_cb_editorCharacterModelChanged(Character character)
	{
		if(cb_editorCharacterModelChanged != null)
		{
			cb_editorCharacterModelChanged (character);

		}

		if (cb_tileLayoutChanged != null) 
		{
			cb_tileLayoutChanged ();
		}

	}



	public static Action<TileInteraction> cb_editorTileInteractioneModelChanged;

	public static void Invoke_cb_editorTileInteractioneModelChanged(TileInteraction tileInteraction)
	{
		if(cb_editorTileInteractioneModelChanged != null)
		{
			cb_editorTileInteractioneModelChanged (tileInteraction);
		}

		if (cb_tileLayoutChanged != null) 
		{
			cb_tileLayoutChanged ();
		}

	}


	public static Action<Furniture> cb_editorFurniturePlaced;

	public static void Invoke_cb_editorFurniturePlaced(Furniture furniture)
	{
		if(cb_editorFurniturePlaced != null)
		{
			cb_editorFurniturePlaced (furniture);
		}

		if (cb_tileLayoutChanged != null) 
		{
			cb_tileLayoutChanged ();
		}

	}


	/*

	public static Action<Furniture> cb_editorFurnitureChanged;

	public static void Invoke_cb_editorFurnitureChanged(Furniture furniture)
	{
		if(cb_editorFurnitureChanged != null)
		{
			cb_editorFurnitureChanged (furniture);
		}

		if (cb_tileLayoutChanged != null) 
		{
			cb_tileLayoutChanged ();
		}

	}
	*/



	// Condition Inspector 


	public static Action cb_conditionAdded;

	public static void Invoke_cb_conditionAdded()
	{
		if(cb_conditionAdded != null)
		{
			cb_conditionAdded ();
		}

	}


	// Subinteraction Inspector 

	public static Action cb_subinteractionChanged;

	public static void Invoke_cb_subinteractionChanged()
	{
		if(cb_subinteractionChanged != null)
		{
			Delegate[] delegateList = cb_subinteractionChanged.GetInvocationList();
			Debug.Log (delegateList.Length);

			cb_subinteractionChanged ();
		}

	}




	// Mouse Controller 

	public static Action<Tile> cb_editorTilesSelected;

	public static void Invoke_cb_editorTilesSelected(Tile tile)
	{
		if(cb_editorTilesSelected != null)
		{
			cb_editorTilesSelected (tile);
		}

	}






}
