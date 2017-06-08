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
	public static Player myPlayer;
	PlayerObject playerObject;

	public Dictionary<Player,GameObject> playerGameObjectMap;
	public static Vector2 entrancePoint = new Vector2(15,6);

	public static List<Player> playerList;


	// Use this for initialization

	public void Initialize () 
	{		
		EventsHandler.cb_roomCreated 	+= CreatePlayer;
		EventsHandler.cb_playerCreated 	+= CreatePlayerObject;
		EventsHandler.cb_keyPressed 	+= MovePlayer;
		EventsHandler.cb_noKeyPressed 	+= StopPlayer;
		EventsHandler.cb_playerMove		+= SavePlayerPosition;

		playerGameObjectMap = new Dictionary<Player, GameObject> ();
	}


	public void OnDestroy()
	{	
		EventsHandler.cb_roomCreated 	-= CreatePlayer;
		EventsHandler.cb_playerCreated 	-= CreatePlayerObject;
		EventsHandler.cb_keyPressed 	-= MovePlayer;
		EventsHandler.cb_noKeyPressed 	-= StopPlayer;
		EventsHandler.cb_playerMove 	-= SavePlayerPosition;
	}

	
	// Update is called once per frame

	void Update () 
	{
		
	}


	// Create Players


	public void CreatePlayers()
	{
		// When first loading the game - create one and assign player

		if (playerList == null) 
		{
			playerList = new List<Player> ();

			Player player_daniel = new Player ("Daniel", new Vector2 (1, 1), new Vector3(3.5f,3.5f,0));
			player_daniel.currentRoom = "test_mom";

			Player player_llehctiM = new Player ("llehctiM", new Vector2 (1, 1), new Vector3 (entrancePoint.x, entrancePoint.y, 0));
			player_llehctiM.currentRoom = "doorTest";

			playerList.Add (player_daniel);
			playerList.Add (player_llehctiM);

			myPlayer = player_llehctiM;
			myPlayer.isActive = true;
		}
	}


	// Create Character 


	public void CreatePlayer(Room myRoom)	
	{		
		//myPlayer = new Player("Daniel", new Vector2(1,1), new Vector3(entrancePoint.x,entrancePoint.y,0));

		myPlayer.myPos = entrancePoint;
		CreatePlayerObject (myPlayer);
	}


	public void CreatePlayerObject(Player myPlayer)
	{
		//Debug.Log ("created character object");

		playerObject = (Instantiate (Resources.Load<GameObject>("Prefabs/Characters/" + myPlayer.fileName))).AddComponent<PlayerObject>();

		Debug.Log (myPlayer.fileName);

		playerObject.gameObject.name = myPlayer.fileName;
		playerObject.transform.position = myPlayer.myPos;

		//obj.GetComponent<SpriteRenderer> ().sortingLayerName = Constants.furniture_character_layer;
	
		playerGameObjectMap.Add (myPlayer,playerObject.gameObject);
	}


	// -------- MOVE PLAYER --------- // 

	public void MovePlayer(Direction myDirection)
	{
		if (GameManager.instance.inputState != InputState.Character) 
		{
			return;			
		}

		// 4 tiles in one second

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


		Tile tile = RoomManager.instance.myRoom.MyGrid.GetTileAt(new Vector3 (newPos.x + offsetX, newPos.y + offsetY, newPos.z));

		if (tile == null) 
		{			
			StopPlayer (InputManager.instance.lastDirection);
			return;
		}

		if (tile != null)
		{
			// FURNITURE - if there a furniture at this tile

			if (tile.myFurniture != null) 
			{
				if (tile.myFurniture.walkable == false) 
				{		
					EventsHandler.Invoke_cb_playerHitPhysicalInteractable (tile.myFurniture, tile);
					StopPlayer (InputManager.instance.lastDirection);

					return;
				}
			}				

			// CHARACTER - if there a character at this tile

			if (tile.myCharacter != null) 
			{
				if (tile.myCharacter.walkable == false) 
				{		
					EventsHandler.Invoke_cb_playerHitPhysicalInteractable (tile.myCharacter, tile);
					StopPlayer (InputManager.instance.lastDirection);

					return;
				}
			}		

			// if there's no character at this tile

			if (ActionBoxManager.instance.currentPhysicalInteractable != null) 
			{
				EventsHandler.Invoke_cb_playerLeavePhysicalInteractable ();
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
			myPlayer.myDirection = myDirection;
			UpdatePlayerObjectPosition (myPlayer, myDirection);
		}
	}





	// When character has stopped 

	public void StopPlayer(Direction lastDirection)
	{		
		playerObject.StopCharacter (lastDirection);
	}


	// Updating the character object position

	public void UpdatePlayerObjectPosition(Player myPlayer, Direction myDirection)
	{		
		playerObject.MovePlayerObject (myPlayer, myDirection);
	}


	// Updating the character sorting layer

	public void UpdatePlayerSortingLayer(Player myPlayer)
	{		
		Tile currentTile = RoomManager.instance.myRoom.MyGrid.GetTileAt(myPlayer.myPos);
		playerGameObjectMap[myPlayer].GetComponent<SpriteRenderer> ().sortingOrder = -currentTile.y * 10;
	}
			


	// --- SWITCH PLAYER --- //


	public void ChangeCharacterToPlayer()
	{
		
	}


	public void SwitchPlayer(string newPlayer)
	{

		if (myPlayer.identificationName == newPlayer) 
		{
			Debug.LogError ("switched to the same player");
			return;
		}

		foreach (Player player in playerList) 
		{
			if (player.identificationName == newPlayer) 
			{	

				Debug.Log ("switch player");
				myPlayer = player;
				Debug.Log (myPlayer.identificationName);

				// check if player is already in the room

				if (GameManager.userData.CheckIfCharacterExistsInRoom (player.identificationName)) 
				{
					Debug.Log ("character exists in room");
					// catch character game object and transfer it to the player

					Character character = (Character)PI_Handler.instance.name_PI_map [player.identificationName];
					GameObject characterObject = PI_Handler.instance.PI_gameObjectMap [character];

					if (characterObject.GetComponent<PlayerObject> () == null) 
					{
						characterObject.AddComponent<PlayerObject> ();
					}

					myPlayer.myPos = characterObject.transform.position;

					playerObject = characterObject.GetComponent<PlayerObject>();

					playerGameObjectMap.Clear ();
					playerGameObjectMap.Add (player,characterObject);
				}			


				EventsHandler.Invoke_cb_playerSwitched (player);
				return;	
			}
		}

		Debug.LogError ("couldn't find player");
	}



	// Get player by name

	public Player GetPlayerByName(string name)
	{
		foreach (Player player in playerList) 
		{
			if (player.identificationName == name) 
			{
				return player;
			}
		}

		return null;
	}



	// Save player position to player data when player has moved

	public void SavePlayerPosition(Player player)
	{
		foreach (PlayerData playerData in GameManager.userData.playerDataList) 
		{
			if (playerData.playerName == player.identificationName) 
			{
				playerData.currentPos = player.myPos;
			}			
		}
	}



}
