using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionBoxManager : MonoBehaviour {



	// Singleton //

	public static ActionBoxManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //



	public GameObject FurnitureFramePrefab;
	public GameObject ActionBoxPrefab;
	public GameObject ActionPrefab;

	GameObject currentFurnitureFrame;
	GameObject currentActionBox;

	Furniture currentFurniture;
	Tile currentTile;




	Interaction _currentInteraction;

	public Interaction currentInteraction
	{
		
		get {return _currentInteraction;} 
		set {

				if (_currentInteraction != null) 
				{
					myInteractionObjectDictionary [_currentInteraction].transform.GetChild(0).gameObject.SetActive (false);
				}

				_currentInteraction = value;

				if (_currentInteraction != null) 
				{					
					myInteractionObjectDictionary [_currentInteraction].transform.GetChild (0).gameObject.SetActive (true);
				}

			} 
	}






	Dictionary<Interaction,GameObject> myInteractionObjectDictionary;



	// Use this for initialization

	public void Initialize () {

		EventsHandler.cb_playerHitFurniture += SetFurnitureFrame;
		EventsHandler.cb_playerLeaveFurniture += CloseFurnitureFrame;

		EventsHandler.cb_spacebarPressed += SetActionBox;

		EventsHandler.cb_escapePressed += CloseActionBox;
		EventsHandler.cb_keyPressedDown += BrowseInteractions;
		
	}


	public void OnDestroy()
	{
		
		EventsHandler.cb_playerHitFurniture -= SetFurnitureFrame;
		EventsHandler.cb_playerLeaveFurniture -= CloseFurnitureFrame;

		EventsHandler.cb_spacebarPressed -= SetActionBox;

		EventsHandler.cb_escapePressed -= CloseActionBox;
		EventsHandler.cb_keyPressedDown -= BrowseInteractions;
			
	}


	
	// Update is called once per frame
	void Update () {
		
	}


	/*
	public void SetFurnitureFrame(Furniture myFurniture, Tile furnitureTile)
	{
		
		if (currentFurnitureFrame != null) 		
		{			
			return;
		}

		currentFurniture = myFurniture;
		currentTile = furnitureTile;

		currentFurnitureFrame = Instantiate (FurnitureFramePrefab, new Vector3 (myFurniture.x, myFurniture.y, 0),Quaternion.identity);
			
		currentFurnitureFrame.transform.FindChild ("FramePiece_DL").transform.localPosition = new Vector2 (0, 0);
		currentFurnitureFrame.transform.FindChild ("FramePiece_DR").transform.localPosition = new Vector2 (myFurniture.mySize.x, 0);
		currentFurnitureFrame.transform.FindChild ("FramePiece_UL").transform.localPosition = new Vector2 (0, myFurniture.mySize.y);
		currentFurnitureFrame.transform.FindChild ("FramePiece_UR").transform.localPosition = new Vector2 (myFurniture.mySize.x, myFurniture.mySize.y);

		currentFurnitureFrame.GetComponent<SpriteRenderer> ().sortingLayerName = Constants.ui_layer;


	}
	*/




	public void SetFurnitureFrame(Furniture myFurniture, Tile furnitureTile)
	{

		if (myFurniture.myInteractionList.Count == 0) 
		{		
			return;		
		}


		if (currentFurnitureFrame != null) 		
		{			
			return;
		}

		currentFurniture = myFurniture;

		currentFurnitureFrame = Instantiate (FurnitureFramePrefab);


		// declerations 

		GameObject myFurnitureObject = FurnitureManager.instance.furnitureGameObjectMap [myFurniture];

		Vector3 furnitureCenter = myFurnitureObject.GetComponent<SpriteRenderer> ().bounds.center;
		Debug.Log ("furnitureCenter " + furnitureCenter);

		currentFurnitureFrame.GetComponent<RectTransform> ().anchoredPosition = furnitureCenter;


		// positioning frame pieces

		Vector3 frameBounds = myFurnitureObject.GetComponent<SpriteRenderer> ().bounds.extents;



		currentFurnitureFrame.transform.FindChild ("FramePiece_DL").GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-frameBounds.x, -frameBounds.y);
		currentFurnitureFrame.transform.FindChild ("FramePiece_DR").GetComponent<RectTransform> ().anchoredPosition = new Vector2 (frameBounds.x, -frameBounds.y);
		currentFurnitureFrame.transform.FindChild ("FramePiece_UL").GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-frameBounds.x, frameBounds.y);
		currentFurnitureFrame.transform.FindChild ("FramePiece_UR").GetComponent<RectTransform> ().anchoredPosition = new Vector2 (frameBounds.x, frameBounds.y);


	}




	public void CloseFurnitureFrame (Furniture myFurniture)
	{
		
		if (currentFurnitureFrame != null) {
			
			Destroy (currentFurnitureFrame.gameObject);
			currentFurniture = null;
			currentTile = null;

		}

	}





	public void SetActionBox ()
	{
		
		if (currentFurniture == null) 
		{
			return;
		}

		if (currentActionBox != null) 		
		{			
			ActivateInteraction ();
			return;
		}

		currentActionBox = Instantiate (ActionBoxPrefab, PositionActionBox() ,Quaternion.identity);

		setActionButtons ();

	}



	Vector3 PositionActionBox()
	{

		GameManager.actionBoxActive = true;
		
		Player activeCharacter = PlayerManager.instance.myPlayer;

		Tile characterTile = RoomManager.instance.myRoom.myGrid.GetTileAt (activeCharacter.myPos);
		Tile currentTile = RoomManager.instance.myRoom.myGrid.GetTileAt (currentFurniture.myPos);


		int x = 0;
		int y = 0;


		if (characterTile.y == currentTile.y)
		{
			
			// character is left of object

			if (characterTile.x < currentTile.x)
			{
				x = -3;
				y = 3;
			}

			// character is right of object

			if (characterTile.x > currentTile.x)
			{
				x = 2;
				y = 1;
			}
		}


	
		if (characterTile.x == currentTile.x)
		{

			// character is above object

			if (characterTile.y > currentTile.y) 
			{
				x = -2;
				y = 1;
			}


			// character is below object

			if (characterTile.y < currentTile.y)
			{
				x = -1;
				y = 0;
			}

		}


		return new Vector3 (characterTile.x + x, characterTile.y + y, 0);


	}



	public void setActionButtons()
	{


		if (currentFurniture.myInteractionList.Count == 0) 
		{
			return;
		}


		myInteractionObjectDictionary = new Dictionary<Interaction, GameObject> ();


		for (int i = 0; i < currentFurniture.myInteractionList.Count; i++) 
		{

			GameObject obj = Instantiate (ActionPrefab, currentActionBox.transform);
			obj.transform.localPosition = new Vector3 (0, 1 - i, 0);
			obj.GetComponent<Text> ().text = currentFurniture.myInteractionList[i].myVerb;



			// setting layer

			//obj.GetComponent<SpriteRenderer> ().sortingLayerName = Constants.ui_layer;
			//obj.GetComponent<SpriteRenderer> ().sortingOrder = 1;


			myInteractionObjectDictionary.Add (currentFurniture.myInteractionList[i], obj);


			if (currentInteraction == null) 
			{
				currentInteraction = currentFurniture.myInteractionList[i];
			}

		}


		Debug.Log ("action count" + myInteractionObjectDictionary.Count);
			

	}


	public void BrowseInteractions(Direction myDirection)
	{

		if (currentActionBox == null) 		
		{			
			return;
		}

		if (currentInteraction == null) 
		{
			return;
		}

		int i =	currentFurniture.myInteractionList.IndexOf (currentInteraction);


		switch (myDirection) 
		{

		case Direction.down:

			if (i < currentFurniture.myInteractionList.Count - 1) 
			{
				currentInteraction = currentFurniture.myInteractionList [i + 1];
			}

			if (i == currentFurniture.myInteractionList.Count - 1) 
			{
				currentInteraction = currentFurniture.myInteractionList [0];
			}


			break;



		case Direction.up:

			if (i > 0) 
			{
				currentInteraction = currentFurniture.myInteractionList [i - 1];
			}

			if (i == 0) 
			{
				currentInteraction = currentFurniture.myInteractionList [currentFurniture.myInteractionList.Count - 1];
			}

			break;



		}


	}


	public void CloseActionBox ()
	{

		if (currentActionBox != null) {

			Destroy (currentActionBox.gameObject);

			GameManager.actionBoxActive = false;
			currentInteraction = null;

		}

	}



	public void ActivateInteraction ()
	{

		if (currentInteraction == null) 
		{
			return;
		}

		currentInteraction.Interact ();
	
		CloseActionBox ();


	}





}
