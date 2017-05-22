using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EditorRoomHelper {





	// Creating a new flipped room in the editor (helper)

	public static Room CreateFlippedRoom (Room room)
	{

		Room newRoom = EditorRoomManager.instance.CreateEmptyRoom (room.myWidth, room.myHeight);
		newRoom.bgName = room.bgName;
		newRoom.bgFlipped = !room.bgFlipped;
		newRoom.roomState = room.roomState;

		newRoom.myGrid = new Grid (newRoom.myWidth, newRoom.myHeight);



		// Interactables

		foreach (Furniture furn in room.myFurnitureList) 
		{			
			Furniture flippedFurn = new Furniture (room, furn);

			newRoom.myFurnitureList.Add (flippedFurn);

			Tile tile = newRoom.MyGrid.GetTileAt (flippedFurn.x, flippedFurn.y);
			tile.myFurniture = flippedFurn;

		}

		newRoom.myFurnitureList.ForEach (furn => Debug.Log (furn.myName));



		foreach (TileInteraction tileInt in room.myTileInteractionList) 
		{		
			TileInteraction flippedTileInteraction = new TileInteraction (room, tileInt);

			newRoom.myTileInteractionList.Add (flippedTileInteraction);

			Tile tile = newRoom.MyGrid.GetTileAt (flippedTileInteraction.x, flippedTileInteraction.y);
			tile.myTileInteraction = flippedTileInteraction;
		}


		// Also flip the interactables of the mirror room 

		if (room.myMirrorRoom != null) 
		{

			newRoom.myMirrorRoom = new RoomMirror ();


			// Persistant Lists

			foreach (Furniture furn in room.myMirrorRoom.myFurnitureList_Persistant) 
			{			
				Furniture flippedFurn = new Furniture (room, furn);

				newRoom.myMirrorRoom.myFurnitureList_Persistant.Add (flippedFurn);

				Tile tile = newRoom.MyGrid.GetTileAt (flippedFurn.x, flippedFurn.y);
				tile.myFurniture = flippedFurn;

				Tile shadowTile = newRoom.myMirrorRoom.shadowGrid.GetTileAt (flippedFurn.x, flippedFurn.y);
				shadowTile.myFurniture = flippedFurn;
			}

			foreach (TileInteraction tileInt in room.myMirrorRoom.myTileInteractionList_Persistant) 
			{			
				TileInteraction flippedTileInteraction = new TileInteraction (room, tileInt);

				newRoom.myMirrorRoom.myTileInteractionList_Persistant.Add (flippedTileInteraction);

				Tile tile = newRoom.MyGrid.GetTileAt (flippedTileInteraction.x, flippedTileInteraction.y);
				tile.myTileInteraction = flippedTileInteraction;

				Tile shadowTile = newRoom.myMirrorRoom.shadowGrid.GetTileAt (flippedTileInteraction.x, flippedTileInteraction.y);
				shadowTile.myTileInteraction = flippedTileInteraction;
			}



			// Shadow Lists


			foreach (Furniture furn in room.myMirrorRoom.myFurnitureList_Shadow) 
			{			
				Furniture flippedFurn = new Furniture (room, furn);

				newRoom.myMirrorRoom.myFurnitureList_Shadow.Add (flippedFurn);

				Tile shadowTile = newRoom.myMirrorRoom.shadowGrid.GetTileAt (flippedFurn.x, flippedFurn.y);
				shadowTile.myFurniture = flippedFurn;
			}


			foreach (TileInteraction tileInt in room.myMirrorRoom.myTileInteractionList_Shadow) 
			{			
				TileInteraction flippedTileInteraction = new TileInteraction (room, tileInt);

				newRoom.myMirrorRoom.myTileInteractionList_Shadow.Add (flippedTileInteraction);

				Tile shadowTile = newRoom.myMirrorRoom.shadowGrid.GetTileAt (flippedTileInteraction.x, flippedTileInteraction.y);
				shadowTile.myTileInteraction = flippedTileInteraction;
			}


		}



		return newRoom;


	}




	public static void SetFurniturePersistency(bool isPersistent, Furniture furn)
	{
		if (isPersistent == true) 
		{				
			EditorRoomManager.instance.room.myMirrorRoom.myFurnitureList_Persistant.Add (furn);

			if (EditorRoomManager.instance.room.myMirrorRoom.inTheShadow == true) 
			{
				EditorRoomManager.instance.room.myMirrorRoom.myFurnitureList_Shadow.Remove (furn);

			} else {

				EditorRoomManager.instance.room.myFurnitureList.Remove (furn);
			}

		} else {

			EditorRoomManager.instance.room.myMirrorRoom.myFurnitureList_Persistant.Remove (furn);

			if (EditorRoomManager.instance.room.myMirrorRoom.inTheShadow == true) 
			{				
				EditorRoomManager.instance.room.myMirrorRoom.myFurnitureList_Shadow.Add (furn);

			} else {

				EditorRoomManager.instance.room.myFurnitureList.Add (furn);
			}

		}

	}



	public static void SetTileInteractionPersistency(bool isPersistent, TileInteraction tileInt)
	{
		if (isPersistent == true) 
		{				
			EditorRoomManager.instance.room.myMirrorRoom.myTileInteractionList_Persistant.Add (tileInt);

			if (EditorRoomManager.instance.room.myMirrorRoom.inTheShadow == true) 
			{
				EditorRoomManager.instance.room.myMirrorRoom.myTileInteractionList_Shadow.Remove (tileInt);

			} else {

				EditorRoomManager.instance.room.myTileInteractionList.Remove (tileInt);
			}

		} else {

			EditorRoomManager.instance.room.myMirrorRoom.myTileInteractionList_Persistant.Remove (tileInt);

			if (EditorRoomManager.instance.room.myMirrorRoom.inTheShadow == true) 
			{				
				EditorRoomManager.instance.room.myMirrorRoom.myTileInteractionList_Shadow.Add (tileInt);

			} else {

				EditorRoomManager.instance.room.myTileInteractionList.Add (tileInt);
			}

		}

	}




}
