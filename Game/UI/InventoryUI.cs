using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {



	// Singleton //

	public static InventoryUI instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //




	public bool inventoryOpen = false;

	GameObject inventoryObject;
	GameObject itemsContainer;

	Dictionary<InventoryItem,GameObject> itemGameObjectMap;

	float spacing = 1.5f;


	// Zoom in window

	GameObject zoomInWindow;


	GameObject itemBigICon;
	GameObject itemTitle;
	GameObject itemInteractionsContainer;


	// When changing current interaction, we find the old interaction object in the dictionary, and deactivating the frame



	Interaction _currentInteraction;

	public Interaction currentInteraction
	{

		get {return _currentInteraction;} 
		set {

			if (_currentInteraction != null)
			{
				
				if (myInteractionObjectMap.ContainsKey (_currentInteraction)) 
				{

					myInteractionObjectMap [_currentInteraction].transform.GetChild (0).gameObject.SetActive (false);
				
				} else {
					
					//Debug.LogError ("The old interaction doesn't appear in the map.");
				}
					
			}


			// set the new interaction 

			_currentInteraction = value;

			Debug.Log ("currentInteraction" + _currentInteraction.myVerb);


			// activating the new frame, for the new interaction

			if (_currentInteraction != null) 
			{
				if (myInteractionObjectMap == null) 
				{
					Debug.LogError ("The map is null");
					return;
				}

				if (myInteractionObjectMap.ContainsKey (_currentInteraction)) 
				{				
					myInteractionObjectMap [_currentInteraction].transform.GetChild (0).gameObject.SetActive (true);
			
				} else {

					Debug.LogError ("The new interaction doesn't appear in the map.");
				}
			}

		} 
	}


	Dictionary<Interaction,GameObject> myInteractionObjectMap;


	// Frames

	GameObject frameGreen;
	GameObject frameOrange;


	// Chosen Item

	InventoryItem _chosenItem;

	public InventoryItem chosenItem 
	{ 
		get
		{
			return _chosenItem;
		}

		set 
		{
			_chosenItem = value;
			MoveGreenFrame ();

		}

	}



	// Use this for initialization

	public void Initialize () 
	{
		
		CreateInventory();
		CloseInventory();

		EventsHandler.cb_key_i_pressed += OnInventoryKeyPressed;
		EventsHandler.cb_keyPressedDown += BrowseInventory;
		EventsHandler.cb_inventoryChanged += UpdateInventory;
		EventsHandler.cb_keyPressedDown += BrowseInteractions;


	}



	void OnDestroy()
	{
		
		EventsHandler.cb_key_i_pressed -= OnInventoryKeyPressed;
		EventsHandler.cb_keyPressedDown -= BrowseInventory;
		EventsHandler.cb_inventoryChanged -= UpdateInventory;
		EventsHandler.cb_keyPressedDown -= BrowseInteractions;

	}



	
	// Update is called once per frame


	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.A))
		{

			Debug.Log ("currentInteraction " + currentInteraction.myVerb);
			Debug.Log ("chosenItem " + chosenItem.fileName);
			Debug.Log ("interactionList" + chosenItem.inventoryItemInteractionList.Count);
			Debug.Log ("interactionObjectMap " + myInteractionObjectMap.Count);


		}
	}


	public void CreateInventory()
	{

		// If there's already an inventory, display error

		if (inventoryObject != null) 
		{
		
			Debug.LogError ("There's already an inventory object");

		}

		// Creating inventory

		Inventory inventory = GameManager.playerData.inventory;
		inventory.myState = InventoryState.Closed; //FIXME
	
		inventoryObject = Instantiate(Resources.Load<GameObject>("Prefabs/InventoryUI"));
		itemsContainer = inventoryObject.transform.FindChild ("Items").gameObject;

					
		if (itemsContainer == null) 
		{
			Debug.LogError ("Can't find items container");
			return;
		}


		itemGameObjectMap = new Dictionary<InventoryItem, GameObject> ();


		// Creating frames

		frameGreen = new GameObject ("frameGreen");
		frameGreen.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/item_frame_green");
		frameGreen.SetActive(false);

		frameOrange = new GameObject ("frameOrange");
		frameOrange.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/item_frame_orange");
		frameOrange.SetActive(false);


		UpdateInventory (inventory);

	}



	public void UpdateInventory(Inventory inventory)
	{

		//Debug.Log ("update inventory");


		// First, destroy all items

		foreach (GameObject obj in itemGameObjectMap.Values) 
		{
			Destroy (obj);
		}

		itemGameObjectMap.Clear ();


		// Creating new items and arranging them


		for (int i = 0; i < inventory.items.Count; i++) 
		{

			InventoryItem item = inventory.items [i];
			GameObject itemObject = new GameObject(item.titleName);
			SpriteRenderer sr = itemObject.AddComponent<SpriteRenderer> ();


			itemObject.transform.SetParent (itemsContainer.transform);
			itemObject.transform.localPosition = new Vector3 (i * spacing,0,0);


			Sprite sprite = Resources.Load<Sprite> ("Sprites/Inventory/Small_items/" + item.fileName);

			if (sprite == null) 
			{

				Debug.LogError ("Can't find sprite in resources");

			}


			sr.sprite = sprite;
			itemGameObjectMap.Add (item, itemObject);

		}


	}



	// When pressing i

	public void OnInventoryKeyPressed()
	{

		if (ActionBoxManager.instance.currentActionBox != null) 
		{
			return;
		}



		if (inventoryOpen == false)
		{
		
			OpenInventory (InventoryState.Browse);
		
		} else {


			CloseInventory ();
			GameManager.instance.inputState = InputState.Character;


		}

	}



	// Opening the inventory


	public void OpenInventory(InventoryState state)
	{

		GameManager.instance.inputState = InputState.Inventory;
		GameManager.playerData.inventory.myState = state;


		foreach (GameObject obj in itemGameObjectMap.Values) 
		{
			obj.GetComponent<SpriteRenderer> ().color = Color.white;
		}

		chosenItem = GameManager.playerData.inventory.items [0];
		

		switch (state) 
		{
			
			case InventoryState.Browse:

				frameGreen.SetActive (true);
				ActivateZoomInWindow ();

				break;


			case InventoryState.UseItem:

				frameGreen.SetActive (true);
			
				break;

		}


		inventoryOpen = true;


	}



	void MoveGreenFrame()
	{
		
		if (chosenItem == null) 
		{
			frameGreen.SetActive (false);
			return;
		
		} else {
			
			frameGreen.SetActive (true);
			frameGreen.transform.position = itemGameObjectMap [chosenItem].transform.position;
		}
			
	}



	// Browsing inventory

	public void BrowseInventory(Direction direction)
	{

		Debug.Log ("browse inventory " + GameManager.instance.inputState);

		if (GameManager.instance.inputState != InputState.Inventory) 
		{
			return;
		}


		// Catch the index og the old chosen item, before changing it

		int i = GameManager.playerData.inventory.items.IndexOf (chosenItem);
		int new_i = i;


		switch (direction)
		{

			case Direction.right:

				new_i++;

				if (new_i >= GameManager.playerData.inventory.items.Count) 
				{
					new_i = 0;			
				}
							
				break;


			case Direction.left:

				new_i--;

				if (new_i < 0) 
				{
					new_i = GameManager.playerData.inventory.items.Count - 1;
				}

				break;
		}


		// Set new chosen item

		if (new_i == i) 
		{
			return;
		}

		chosenItem = GameManager.playerData.inventory.items [new_i];


		// if we're in browsing state, update the zoom in window

		if (GameManager.playerData.inventory.myState == InventoryState.Browse) 
		{			
			UpdateZoomInWindow ();
		}

	}



	// ------- ZOOM IN WiNDOW ------- //


	public void ActivateZoomInWindow()
	{
		
		if (zoomInWindow == null) {
			
			zoomInWindow = Instantiate (Resources.Load<GameObject> ("Prefabs/InventoryZoomInObject"));		

			itemBigICon = zoomInWindow.transform.FindChild ("Background").FindChild ("ItemBigIcon").gameObject;
			itemTitle = zoomInWindow.transform.FindChild ("Background").FindChild ("TextCanvas").FindChild ("ItemTitle").gameObject;
			itemInteractionsContainer = zoomInWindow.transform.FindChild ("Background").FindChild ("TextCanvas").FindChild ("InteractionContainer").gameObject;
					
		} 

		zoomInWindow.SetActive (true);
		UpdateZoomInWindow ();

	}


	// When browsing the inventory, update zoom in window 

	public void UpdateZoomInWindow()
	{
		
		if (itemBigICon == null) 
		{ 
			//Debug.Log ("item big icon = null");
		}

		itemBigICon.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/Inventory/Big_items/" + chosenItem.fileName + "_big");			
		itemTitle.GetComponent<Text>().text = chosenItem.titleName;


		Debug.Log ("count " + chosenItem.inventoryItemInteractionList.Count);
		SetInteractions ();

	}



	// Set the interactions in the zoom in window, according to the item's interactions

	public void SetInteractions()
	{


		// Destroying past objects

		if (myInteractionObjectMap != null) 
		{		
			Debug.LogError ("destroy past objects");

			foreach (GameObject obj in myInteractionObjectMap.Values) 
			{
				Destroy (obj);	
			}	
		}

		
		if (chosenItem.inventoryItemInteractionList.Count == 0) 
		{
			Debug.LogError ("SetInteractions: There are no interactions");
			return;
		}


		if (chosenItem == null) 
		{
			Debug.LogError ("SetInteractions: Chosen item is null");
			return;
		}


		myInteractionObjectMap = new Dictionary<Interaction, GameObject> ();

		GameObject itemInteractionPrefab = (Resources.Load<GameObject>("Prefabs/ItemInteraction"));


		// Create new interaction objects, and put them in the temp dictionary

		for (int i = 0; i < chosenItem.inventoryItemInteractionList.Count; i++) 
		{

			GameObject obj = Instantiate(itemInteractionPrefab, itemInteractionsContainer.transform);

			obj.transform.localPosition = new Vector3 (0, 1 - i, 0);
			obj.GetComponent<Text> ().text = chosenItem.inventoryItemInteractionList[i].myVerb;

			myInteractionObjectMap.Add (chosenItem.inventoryItemInteractionList[i], obj);

		}
	

		// Set the first interaction as the current interaction

		currentInteraction = chosenItem.inventoryItemInteractionList[0];

	}



	public void SetCurrentInteraction(Interaction interaction)
	{
		if (currentInteraction != null) {

			if (myInteractionObjectMap == null) {
				Debug.LogError ("SetCurrentInteraction: the map is null");
		
			} else {




			}
		}

	}





	public void BrowseInteractions(Direction myDirection)
	{

		if((myDirection == Direction.left) || (myDirection == Direction.right))
		{
			//Debug.LogError ("BrowseInteractions: the direction is left or right.");
			return;
		}

		if (GameManager.instance.inputState != InputState.Inventory) 
		{
			Debug.LogError ("BrowseInteractions: input state is not inventory.");

			return;
		}

		if (zoomInWindow == null) 		
		{	
			Debug.LogError ("BrowseInteractions: zoom in window is null.");

			return;
		}

		if (currentInteraction == null) 
		{
			Debug.LogError ("BrowseInteractions: current interaction is null.");
			return;
		}


		int i =	chosenItem.inventoryItemInteractionList.IndexOf (currentInteraction);


		switch (myDirection) 
		{

			case Direction.down:

				if (i < chosenItem.inventoryItemInteractionList.Count - 1) 
				{
					currentInteraction = chosenItem.inventoryItemInteractionList [i + 1];
				
				} else if (i == chosenItem.inventoryItemInteractionList.Count - 1) 
				{
					currentInteraction = chosenItem.inventoryItemInteractionList [0];
				}


				break;



			case Direction.up:

				if (i > 0) 
				{
					currentInteraction = chosenItem.inventoryItemInteractionList [i - 1];
				
				} else if (i == 0) 
				{
					currentInteraction = chosenItem.inventoryItemInteractionList [chosenItem.inventoryItemInteractionList.Count - 1];
				}

				break;

		}

	}






	// ----- CLOSE INVENTORY ----- //


	public void CloseInventory()
	{

		chosenItem = null;


		foreach (GameObject obj in itemGameObjectMap.Values) 
		{
			obj.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0.5f);
		}

		if (zoomInWindow != null) 
		{
			zoomInWindow.SetActive (false);
		}


		inventoryOpen = false;



	}



	// Select item to use - activates when pressing spacebar, and selects the item we are on 


	public void SelectItemToUse()
	{


		if (GameManager.instance.inputState != InputState.Inventory) 
		{
			return;
		}

		if (GameManager.playerData.inventory.myState != InventoryState.UseItem) 
		{	
			Debug.Log ("state is not use item");
			return;
		}

		Debug.Log ("Select Item To Use");

		if (ActionBoxManager.instance.currentFurniture == null) 
		{
			return;
		}


		UseItemHelper.UseItemOnFurniture (chosenItem, ActionBoxManager.instance.currentFurniture);

		CloseInventory ();

	
	}




}
