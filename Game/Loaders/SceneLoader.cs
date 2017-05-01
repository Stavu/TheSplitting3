﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour {


	public GameObject managers;




	// Use this for initialization
	void Start () {

		CreateManagers ();

	}



	
	// Update is called once per frame
	void Update () {
		
	}



	void CreateManagers()
	{

		Instantiate (managers);
			

		GameManager.instance.Initialize ();
		RoomManager.instance.Initialize ();
		FurnitureManager.instance.Initialize ();
		InputManager.instance.Initialize ();
		TileManager.instance.Initialize ();
		PlayerManager.instance.Initialize ();
		ActionBoxManager.instance.Initialize ();
		InteractionManager.instance.Initialize ();	
	
		GameManager.instance.CreatePlayerData ();

		RoomManager.instance.BuildRoom ();


		FindObjectOfType<DebugHelper> ().Initialize ();

		InventoryUI.instance.Initialize ();


	}



}