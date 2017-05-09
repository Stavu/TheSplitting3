﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


public class PlayerManager : MonoBehaviour {


	// Singleton //

	public static PlayerManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //


	public GameObject CharacterPrefab;
	public Player myPlayer;

	public Dictionary<Player,GameObject> playerGameObjectMap;




	// Use this for initialization

	public void Initialize () 
	{

		EventsHandler.cb_roomCreated += CreatePlayer;
		EventsHandler.cb_playerCreated += CreatePlayerObject;
		EventsHandler.cb_keyPressed += MoveCharacter;
		//EventsHandler.cb_characterMove += UpdatePlayerObjectPosition;
		EventsHandler.cb_characterMove += FindPlayerTile;
		EventsHandler.cb_noKeyPressed += StopPlayer;

		playerGameObjectMap = new Dictionary<Player, GameObject> ();

	}


	public void OnDestroy()
	{	
		EventsHandler.cb_roomCreated -= CreatePlayer;
		EventsHandler.cb_playerCreated -= CreatePlayerObject;
		EventsHandler.cb_keyPressed -= MoveCharacter;
		//EventsHandler.cb_characterMove -= UpdatePlayerObjectPosition;
		EventsHandler.cb_characterMove -= FindPlayerTile;
		EventsHandler.cb_noKeyPressed -= StopPlayer;
	}

	
	// Update is called once per frame

	void Update () 
	{
		
	}


	// Create Character 


	public void CreatePlayer(Room myRoom)	
	{
			
		//Debug.Log ("created character");	

		myPlayer = new Player("Daniel", new Vector2(1,1), new Vector3(3.5f,3.5f,0));

		//EventsHandler.Invoke_cb_playerCreated (myPlayer);

		CreatePlayerObject (myPlayer);

	}



	public void CreatePlayerObject(Player myPlayer)
	{

		//Debug.Log ("created character object");	

		GameObject obj = Instantiate (Resources.Load<GameObject>("Prefabs/Characters/" + myPlayer.myName));

		obj.name = myPlayer.myName;
		obj.transform.position = myPlayer.myPos;

		//obj.GetComponent<SpriteRenderer> ().sortingLayerName = Constants.furniture_character_layer;

	
		playerGameObjectMap.Add (myPlayer,obj);


	}



	public void MoveCharacter(Direction myDirection)
	{


		if (GameManager.instance.inputState != InputState.Character) 
		{
			return;	
		
		}


		float playerSpeed = 4f * Time.deltaTime;
		float offsetX = 0;
		float offsetY = 0;

		Vector3 newPos = new Vector3 (-1000, -1000, -1000); 




		// check the new position according to the diretion 

		switch (myDirection) 
		{

		case Direction.left:

			newPos = new Vector3 ((myPlayer.myPos.x - playerSpeed), myPlayer.myPos.y, myPlayer.myPos.z);
			offsetX = -0.5f;
			playerGameObjectMap [myPlayer].transform.localScale = new Vector3(1,1,1);

			break;


		case Direction.right:

			newPos = new Vector3 ((myPlayer.myPos.x + playerSpeed), myPlayer.myPos.y, myPlayer.myPos.z);
			offsetX = 0.5f;
			playerGameObjectMap [myPlayer].transform.localScale = new Vector3(-1,1,1);

			break;


		case Direction.up:

			newPos = new Vector3 (myPlayer.myPos.x, (myPlayer.myPos.y + playerSpeed), myPlayer.myPos.z);
			offsetY = 0.5f;

			break;


		case Direction.down:

			newPos = new Vector3 (myPlayer.myPos.x, (myPlayer.myPos.y - playerSpeed), myPlayer.myPos.z);

			break;


		}


		Tile tile = RoomManager.instance.myRoom.myGrid.GetTileAt(new Vector3 (newPos.x + offsetX, newPos.y + offsetY, newPos.z));

		if (tile == null) 
		{			
			StopPlayer (InputManager.instance.lastDirection);
			return;
		}



		if (tile != null)
		{
			// FURNITURE - if there a furinture at this tile

			if (tile.myFurniture != null) 
			{
				if (tile.myFurniture.walkable == false) 
				{		
					EventsHandler.Invoke_cb_playerHitFurniture (tile.myFurniture, tile);
					StopPlayer (InputManager.instance.lastDirection);

					return;
				}

			}				


			// if there's no furniture at this tile

			if (ActionBoxManager.instance.currentFurniture != null) 
			{
				EventsHandler.Invoke_cb_playerLeaveFurniture ();
			}	



			// TILE INTERACTION - If the next tile is interactable

			if (tile.myTileInteraction != null) 
			{
				if (tile.myTileInteraction.walkable == false) 
				{
					StopPlayer (InputManager.instance.lastDirection);
				}
			
				EventsHandler.Invoke_cb_playerHitTileInteraction (tile);

				if (tile.myTileInteraction.walkable == false) 
				{							
					return;
				}

			}


			if (GameActionManager.instance.currentTileInteraction != null) 
			{
				EventsHandler.Invoke_cb_playerLeaveTileInteraction ();

			}
										

			// Walk to new pos

			myPlayer.ChangePosition (newPos);
			UpdatePlayerObjectPosition (myPlayer, myDirection);


		}

	}


	// When character has stopped 

	public void StopPlayer(Direction lastDirection)
	{
		
		GameObject obj = playerGameObjectMap [myPlayer];

		obj.GetComponent<PlayerObject> ().StopCharacter (lastDirection);

	}


	// Updating the character object position


	public void UpdatePlayerObjectPosition(Player myPlayer, Direction myDirection)
	{

		GameObject obj = playerGameObjectMap [myPlayer];
		obj.GetComponent<PlayerObject> ().MoveCharacter (myPlayer, myDirection);

	}




	public void FindPlayerTile(Player myPlayer)
	{

		Tile currentTile = RoomManager.instance.myRoom.myGrid.GetTileAt(myPlayer.myPos);


		// light the tile

		TileManager.instance.tileGameObjectMap [currentTile].GetComponent<SpriteRenderer> ().color = new Color (0.1f,0.1f,0.1f,0.2f);

	}




	public void UpdatePlayerSortingLayer(Player myPlayer)
	{
		
		Tile currentTile = RoomManager.instance.myRoom.myGrid.GetTileAt(myPlayer.myPos);

		playerGameObjectMap[myPlayer].GetComponent<SpriteRenderer> ().sortingOrder = -currentTile.y;


	}

			

}
