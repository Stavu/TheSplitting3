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

	GameObject currentPhysicalInteractableFrame;
	public GameObject currentActionBox;

	public PhysicalInteractable currentPhysicalInteractable;



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
					// Interaction frame
					myInteractionObjectDictionary [_currentInteraction].transform.GetChild (0).gameObject.SetActive (true);
					myInteractionObjectDictionary [_currentInteraction].transform.GetChild (0).GetComponent<Image> ().color = myInteractionObjectDictionary [_currentInteraction].GetComponent<Text> ().color;

				}

			} 
	}


	List<Interaction> activeInteractionList;
	Dictionary<Interaction,GameObject> myInteractionObjectDictionary;



	// Use this for initialization

	public void Initialize () 
	{

		EventsHandler.cb_playerHitPhysicalInteractable += SetPhysicalInteractableFrame;
		EventsHandler.cb_playerLeavePhysicalInteractable += CloseFurnitureFrame;	

		EventsHandler.cb_keyPressedDown += BrowseInteractions;

		currentPhysicalInteractable = null;
		
	}


	public void OnDestroy()
	{
		
		EventsHandler.cb_playerHitPhysicalInteractable -= SetPhysicalInteractableFrame;
		EventsHandler.cb_playerLeavePhysicalInteractable -= CloseFurnitureFrame;

		EventsHandler.cb_keyPressedDown -= BrowseInteractions;
			
	}


	
	// Update is called once per frame

	void Update () 
	{
		
	}



	public void SetPhysicalInteractableFrame(PhysicalInteractable myPhysicalInt, Tile tile)
	{

		if (myPhysicalInt.myInteractionList.Count == 0) 
		{		
			return;		
		}


		if (currentPhysicalInteractableFrame != null) 		
		{			
			return;
		}

		currentPhysicalInteractable = myPhysicalInt;

		currentPhysicalInteractableFrame = Instantiate (FurnitureFramePrefab);


		// declerations 

		Vector3 frameBounds = Vector3.zero;


		if (myPhysicalInt is Furniture) 		
		{

			Furniture myFurniture = (Furniture)myPhysicalInt;

			SpriteRenderer sr = FurnitureManager.instance.furnitureGameObjectMap [myFurniture].GetComponent<SpriteRenderer>();
		
			if (sr == null) 
			{
				sr = FurnitureManager.instance.furnitureGameObjectMap [myFurniture].GetComponentInChildren<SpriteRenderer>();
			}

			Vector3 center = sr.bounds.center;

			currentPhysicalInteractableFrame.GetComponent<RectTransform> ().anchoredPosition = center;

			// positioning frame pieces

			frameBounds = sr.bounds.extents;

		}


		if (myPhysicalInt is Character) 		
		{

			Character myCharacter = (Character)myPhysicalInt;

			GameObject myObject = CharacterManager.instance.characterGameObjectMap [myCharacter];
			Vector3 center = myObject.GetComponentInChildren<SpriteRenderer> ().bounds.center;

			currentPhysicalInteractableFrame.GetComponent<RectTransform> ().anchoredPosition = center;

			// positioning frame pieces

			frameBounds = myObject.GetComponentInChildren<SpriteRenderer> ().bounds.extents;

		}


		currentPhysicalInteractableFrame.transform.FindChild ("FramePiece_DL").GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-frameBounds.x, -frameBounds.y);
		currentPhysicalInteractableFrame.transform.FindChild ("FramePiece_DR").GetComponent<RectTransform> ().anchoredPosition = new Vector2 (frameBounds.x, -frameBounds.y);
		currentPhysicalInteractableFrame.transform.FindChild ("FramePiece_UL").GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-frameBounds.x, frameBounds.y);
		currentPhysicalInteractableFrame.transform.FindChild ("FramePiece_UR").GetComponent<RectTransform> ().anchoredPosition = new Vector2 (frameBounds.x, frameBounds.y);





	}




	public void CloseFurnitureFrame ()
	{
		
		if (currentPhysicalInteractableFrame != null) 
		{
			Destroy (currentPhysicalInteractableFrame.gameObject);
			currentPhysicalInteractable = null;

		}

	}



	// Create actionbox

	public void OpenActionBox()
	{

		currentActionBox = Instantiate (ActionBoxPrefab, PositionActionBox() ,Quaternion.identity);

		setInteractionButtons ();

		GameManager.actionBoxActive = true;
		GameManager.instance.inputState = InputState.ActionBox;

	}


	Vector3 PositionActionBox()
	{
				
		Player activePlayer = PlayerManager.myPlayer;

		Tile playerTile = RoomManager.instance.myRoom.MyGrid.GetTileAt (activePlayer.myPos);
		Tile currentTile = RoomManager.instance.myRoom.MyGrid.GetTileAt (currentPhysicalInteractable.myPos);


		int x = 0;
		int y = 0;


		if (playerTile.y == currentTile.y)
		{
			
			// character is left of object

			if (playerTile.x < currentTile.x)
			{
				x = -3;
				y = 3;
			}

			// character is right of object

			if (playerTile.x > currentTile.x)
			{
				x = 2;
				y = 1;
			}
		}


	
		if (playerTile.x == currentTile.x)
		{

			// character is above object

			if (playerTile.y > currentTile.y) 
			{
				x = -2;
				y = 1;
			}


			// character is below object

			if (playerTile.y < currentTile.y)
			{
				x = -1;
				y = 0;
			}

		}


		return new Vector3 (playerTile.x + x, playerTile.y + y, 0);


	}



	public void setInteractionButtons()
	{


		if (currentPhysicalInteractable.myInteractionList.Count == 0) 
		{
			return;
		}

		activeInteractionList = new List<Interaction> ();
		myInteractionObjectDictionary = new Dictionary<Interaction, GameObject> ();


		for (int i = 0; i < currentPhysicalInteractable.myInteractionList.Count; i++) 
		{

			// Check if passed the conditions, if not, continute to next interaction 
					
			bool passedConditions = Utilities.EvaluateConditions (currentPhysicalInteractable.myInteractionList [i].conditionList);

			if (passedConditions == false) 
			{
				continue;
			}


			GameObject obj = Instantiate (ActionPrefab, currentActionBox.transform);
			obj.transform.localPosition = new Vector3 (0, 1 - i, 0);
			obj.GetComponent<Text> ().text = currentPhysicalInteractable.myInteractionList[i].myVerb;

			activeInteractionList.Add(currentPhysicalInteractable.myInteractionList[i]);
			myInteractionObjectDictionary.Add (currentPhysicalInteractable.myInteractionList[i], obj);


			// if the interaction is use item, and there are no items in the inventory, it will be in 0.5 alpha

			if ((currentPhysicalInteractable.myInteractionList[i].myVerb == "Use Item") && (GameManager.playerData.inventory.items.Count == 0)) 
			{
				obj.GetComponent<Text> ().color = new Color (1f, 1f, 1f, 0.5f); 
			}


			if (currentInteraction == null) 
			{
				currentInteraction = currentPhysicalInteractable.myInteractionList[i];
			}

		}

	}



	public void BrowseInteractions(Direction myDirection)
	{

		if (GameManager.instance.inputState != InputState.ActionBox) 
		{
			return;
		}

		if (currentActionBox == null) 		
		{			
			return;
		}

		if (currentInteraction == null) 
		{
			return;
		}

		int i =	activeInteractionList.IndexOf (currentInteraction);


		switch (myDirection) 
		{

		case Direction.down:

				if (i < activeInteractionList.Count - 1) 
			{
				currentInteraction = activeInteractionList [i + 1];
			}

				if (i == activeInteractionList.Count - 1) 
			{
				currentInteraction = activeInteractionList [0];
			}


			break;



		case Direction.up:

			if (i > 0) 
			{
				currentInteraction = activeInteractionList [i - 1];
			}

			if (i == 0) 
			{
				currentInteraction = activeInteractionList [activeInteractionList.Count - 1];
			}

			break;

		}

	}




	public void CloseActionBox ()
	{


		if (currentActionBox != null) 
		{

			Destroy (currentActionBox.gameObject);

			GameManager.actionBoxActive = false;
			currentInteraction = null;

		}

	}




	// Activate interaction



	public void ActivateInteraction ()
	{

		if (currentInteraction == null) 
		{
			return;
		}

			
		if (currentInteraction.subInteractionList.Count == 0) 
		{
			Debug.LogError ("SubInteract: There are no subinteractions.");
			GameManager.instance.inputState = InputState.Character;
		
			CloseFurnitureFrame ();
			CloseActionBox ();

			return;
		}



		foreach (SubInteraction subInt in currentInteraction.subInteractionList) 
		{

			subInt.SubInteract ();
			
		}

	
		CloseActionBox ();


	}





}
