﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FurnitureManager : MonoBehaviour {


	// Singleton //

	public static FurnitureManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //



	public Dictionary<Furniture,GameObject> furnitureGameObjectMap;



	// Use this for initialization

	public void Initialize () 
	{		
	
		EventsHandler.cb_furnitureChanged += CreateFurnitureGameObject;

		furnitureGameObjectMap = new Dictionary<Furniture, GameObject> ();

	}


	public void OnDestroy()
	{	
	
		EventsHandler.cb_furnitureChanged -= CreateFurnitureGameObject;
			
	}




	// Update is called once per frame

	void Update () 
	{
		
	}





	public void CreateFurnitureGameObject (Furniture myFurniture)
	{
		
		GameObject obj = Utilities.CreateFurnitureGameObject (myFurniture, this.transform);

		furnitureGameObjectMap.Add (myFurniture, obj);

	}





}
