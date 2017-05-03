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
		EventsHandler.cb_spacebarPressed += SelectItemToUse;

	}



	void OnDestroy()
	{
		
		EventsHandler.cb_key_i_pressed -= OnInventoryKeyPressed;
		EventsHandler.cb_keyPressedDown -= BrowseInventory;
		EventsHandler.cb_inventoryChanged -= UpdateInventory;
		EventsHandler.cb_spacebarPressed -= SelectItemToUse;
	}



	
	// Update is called once per frame


	void Update () 
	{
		
	}


	public void CreateInventory()
	{

		// If there's already an inventory, display error

		if (inventoryObject != null) {
		
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


	public void OnInventoryKeyPressed()
	{

		if (inventoryOpen == false) {
		
			OpenInventory (InventoryState.Browse);
		
		} else {


			CloseInventory ();


		}

	}


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


	public void BrowseInventory(Direction direction)
	{

		if (GameManager.instance.inputState != InputState.Inventory) 
		{
			return;
		}


		int i = GameManager.playerData.inventory.items.IndexOf (chosenItem);
	

		switch (direction)
		{

			case Direction.right:

				i++;

				if (i >= GameManager.playerData.inventory.items.Count) 
				{
					i = 0;			
				}
							
				break;


			case Direction.left:

				i--;

				if (i < 0) 
				{
					i = GameManager.playerData.inventory.items.Count - 1;
				}

				break;
		}


		chosenItem = GameManager.playerData.inventory.items [i];


		// if we're in browsing state, update the zoom in window

		if (GameManager.playerData.inventory.myState == InventoryState.Browse) 
		{			
			UpdateZoomInWindow ();
		}

	}



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


	public void UpdateZoomInWindow()
	{
		if (itemBigICon == null) { 
			//Debug.Log ("item big icon = null");
		}

		itemBigICon.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/Inventory/Big_items/" + chosenItem.fileName + "_big");			
		itemTitle.GetComponent<Text>().text = chosenItem.titleName;

	}


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

		GameManager.instance.inputState = InputState.Character;

	}




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

	
	}




}
