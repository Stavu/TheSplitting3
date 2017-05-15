using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorTileInteractionHandler : MonoBehaviour {

	// Use this for initialization

	public void Initialize ()
	{

	}

	
	// Update is called once per frame
	void Update () 
	{
		
	}




	// ----- Tile Interaciton ----- //



	public void PlaceTileInteraction(Tile tile)
	{


		// If there's already a tileInteraction on this tile, destroy it before creating a new tileInteraction


		if (tile.myTileInteraction != null)
		{
			EditorRoomManager.instance.room.myTileInteractionList.Remove (tile.myTileInteraction);

		}

		// create tileInteraction

		TileInteraction tileInteraction = new TileInteraction (tile.x, tile.y);


		// set default size

		tileInteraction.mySize = Vector2.one;



		// Adding to room tileInteraction List

		EditorRoomManager.instance.room.myTileInteractionList.Add (tileInteraction);

		tile.myTileInteraction = tileInteraction;


		EventsHandler.Invoke_cb_editorTileInteractioneModelChanged (tileInteraction);



	}




}
