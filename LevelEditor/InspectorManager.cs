using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System;



public class InspectorManager : MonoBehaviour {



	// Singleton //

	public static InspectorManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //


	public GameObject inspectorObjectPrefab;
	public GameObject interactionPanelObjectPrefab;
	public GameObject tileInspectorObjectPrefab;

	GameObject inspectorObject;
	//GameObject interactionPanelObject;
	//GameObject tileInspectorObject;


	public static InteractionInspector interactionInspector;
	public static TileInspector tileInspector;


	// public Interaction loadedInteraction;



	// Chosen furniture

	Furniture _chosenFurniture;
	public Furniture chosenFurniture
	{
		get 
		{
			return _chosenFurniture;
		}

		set 
		{
			_chosenFurniture = value;

			if ((_chosenFurniture == null) && (chosenCharacter == null))
			{
				DestroyInspector ();

			} else if (_chosenFurniture != null)
			{
				CreateInspector (_chosenFurniture);
			}
		}
	}



	// Chosen character

	Character _chosenCharacter;
	public Character chosenCharacter
	{
		get 
		{
			return _chosenCharacter;
		}

		set 
		{
			_chosenCharacter = value;

			if ((_chosenCharacter == null) && (chosenFurniture == null))
			{
				DestroyInspector ();

			} else if (_chosenCharacter != null)
			{
				CreateInspector (_chosenCharacter);
			}
		}
	}





	// chosen tile interaction property

	TileInteraction _chosenTileInteraction;
	public TileInteraction chosenTileInteraction
	{
		get 
		{
			return _chosenTileInteraction;
		}

		set 
		{
			_chosenTileInteraction = value;

			if (_chosenTileInteraction == null)  
			{
				//Debug.Log ("destroy tile inspector");
				tileInspector.DestroyTileInspector ();

			} else {

				//Debug.Log ("create tile inspector");
				tileInspector.CreateTileInspector (_chosenTileInteraction);
			}
		}
	}




	// Use this for initialization

	void Start () 
	{	
		
		if (interactionInspector == null) 
		{
			interactionInspector = gameObject.AddComponent<InteractionInspector> ();
		}

		if (tileInspector == null) 
		{
			tileInspector = gameObject.AddComponent<TileInspector> ();
		}

	}


	// Update is called once per frame

	void Update () 
	{
		
	}




	// INSPECTOR //


	public void CreateInspector(PhysicalInteractable currentPhysicalInteractable)
	{


		Debug.Log ("createInspector");


		DestroyInspector ();


		inspectorObject = Instantiate (inspectorObjectPrefab);

		Transform panel = inspectorObject.transform.FindChild ("Panel");

		Debug.Log ("name " + currentPhysicalInteractable.myName);

		panel.FindChild ("Name").GetComponent<Text> ().text = currentPhysicalInteractable.myName;

		panel.FindChild ("SizeX").FindChild("Placeholder").GetComponent<Text> ().text = currentPhysicalInteractable.mySize.x.ToString();
		panel.FindChild ("SizeY").FindChild("Placeholder").GetComponent<Text> ().text = currentPhysicalInteractable.mySize.y.ToString();

		panel.FindChild ("SizeX").GetComponent<InputField> ().onEndEdit.AddListener (changeWidth);
		panel.FindChild ("SizeY").GetComponent<InputField> ().onEndEdit.AddListener (changeHeight);


		panel.FindChild ("PosX").FindChild("Placeholder").GetComponent<Text> ().text = currentPhysicalInteractable.x.ToString();
		panel.FindChild ("PosY").FindChild("Placeholder").GetComponent<Text> ().text = currentPhysicalInteractable.y.ToString();

		panel.FindChild ("PosX").GetComponent<InputField> ().onEndEdit.AddListener (changeX);
		panel.FindChild ("PosY").GetComponent<InputField> ().onEndEdit.AddListener (changeY);


		panel.FindChild ("OffsetX").FindChild("Placeholder").GetComponent<Text> ().text = currentPhysicalInteractable.offsetX.ToString();
		panel.FindChild ("OffsetY").FindChild("Placeholder").GetComponent<Text> ().text = currentPhysicalInteractable.offsetY.ToString();


		panel.FindChild ("OffsetX").GetComponent<InputField> ().onEndEdit.AddListener (changeOffsetX);
		panel.FindChild ("OffsetY").GetComponent<InputField> ().onEndEdit.AddListener (changeOffsetY);


	
		// create existing interactions

		for (int i = 0; i < 3; i++) {
			
			Button button = panel.FindChild ("AddInteraction" + i.ToString ()).GetComponent<Button> ();

			if (currentPhysicalInteractable.myInteractionList.Count > i) 
			{
			
				button.transform.FindChild ("Text").GetComponent<Text> ().text = currentPhysicalInteractable.myInteractionList [i].myVerb;
				Interaction interaction = currentPhysicalInteractable.myInteractionList [i];
				button.onClick.AddListener (() => interactionInspector.OpenInteractionPanel (interaction));	
					

			} else {

			
				button.onClick.AddListener (() => interactionInspector.OpenInteractionPanel (null));

			}
		}
	}




	public void DestroyInspector()
	{

		Debug.Log ("destroy inspector");
		if (inspectorObject != null) 
		{

			Destroy (inspectorObject);
		}

		interactionInspector.DestroyInteractionPanel ();

	}




	// change size


	public void changeWidth(string x)
	{

		int newX = int.Parse (x);

		if (chosenFurniture != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableWidth (newX, chosenFurniture);

		} else if (chosenCharacter != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableWidth (newX, chosenCharacter);
		}


	}



	public void changeHeight(string y)
	{
		int newY = int.Parse (y);

		if (chosenFurniture != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableHeight (newY, chosenFurniture);

		} else if (chosenCharacter != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableHeight (newY, chosenCharacter);
		}

	}



	// change position


	public void changeX(string x)
	{

		int newX = int.Parse (x);

		if (chosenFurniture != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableTileX (newX, chosenFurniture);
		
		} else if (chosenCharacter != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableTileX (newX, chosenCharacter);
		}
			
	}




	public void changeY(string y)
	{
		int newY = int.Parse (y);


		if (chosenFurniture != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableTileY (newY, chosenFurniture);

		} else if (chosenCharacter != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableTileY (newY, chosenCharacter);
		}
	}




	// change offset


	public void changeOffsetX(string x)
	{

		float newX = float.Parse (x);

		if (chosenFurniture != null) 
		{
			EditorRoomManager.instance.ChangeInteractableOffsetX (newX, chosenFurniture);
		
		} else if (chosenCharacter != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableOffsetX (newX, chosenCharacter);
		}
	}





	public void changeOffsetY(string y)
	{
		float newY = float.Parse (y);

		if (chosenFurniture != null) 
		{
			EditorRoomManager.instance.ChangeInteractableOffsetY (newY, chosenFurniture);

		} else if (chosenCharacter != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableOffsetY (newY, chosenCharacter);
		}	
	}



	// Creating a new interaction panel


	/*

	// Declarations

	Transform panel;
	InputField interactionTitleInput;

	InputField interactionTextInput;
	Toggle textInputCheckBox;

	InputField destinationRoomInput;
	Toggle enterRoomCheckBox;

	Toggle recieveItemCheckBox;
	Dropdown recieveItemDropdown;
	InputField recieveItemTitleInput;

	Toggle usingItemCheckBox;



	public void CreateInteractionPanel()
	{
		
		interactionPanelObject = Instantiate (interactionPanelObjectPrefab);

		// Panel
		panel = interactionPanelObject.transform.FindChild ("Panel");

		// Interaction title
		interactionTitleInput = panel.FindChild ("InteractionTitle").GetComponent<InputField> ();

		// Dialogue
		interactionTextInput = panel.FindChild("InteractionTextInput").GetComponent<InputField> ();
		textInputCheckBox = panel.FindChild ("TextInputCheckBox").GetComponent<Toggle> ();

		// Destination room
		destinationRoomInput = panel.FindChild("DestinationRoomInput").GetComponent<InputField>();
		enterRoomCheckBox = panel.FindChild ("EnterRoomCheckBox").GetComponent<Toggle> ();

		// recieve item dropdown
		recieveItemCheckBox = panel.FindChild ("RecieveItemCheckBox").GetComponent<Toggle>();
		recieveItemDropdown = panel.FindChild("RecieveItemDropdown").GetComponentInChildren<Dropdown>();
		recieveItemTitleInput = panel.FindChild("RecieveItemTitleInput").GetComponent<InputField>();

		//Using item
		usingItemCheckBox = panel.FindChild("UsingItemCheckBox").GetComponent<Toggle>();




		// Submit button
		panel.FindChild("SubmitButton").GetComponent<Button> ().onClick.AddListener  (() => SubmitInteraction());



	}






	// Opening interaction panel after created

	public void OpenInteractionPanel(Interaction interaction = null)
	{
		
		DestroyInteractionPanel ();
		CreateInteractionPanel ();


		//interactionPanelObject.SetActive (true);

		// set dialogue texts
		// set destination room text


		// recieve item dropdown

		recieveItemDropdown.ClearOptions ();
		recieveItemDropdown.AddOptions (LoadInventoryItems());


		// Populating values to subinteractions


		if (interaction != null) 
		{
			
			loadedInteraction = interaction;

			// interaction title input field

			interactionTitleInput.text = interaction.myVerb;


			foreach (SubInteraction subInt in interaction.subInteractionList) 
			{

				switch (subInt.interactionType) 
				{

					case "showDialogue":

						interactionTextInput.interactable = true; 
						interactionTextInput.text = subInt.rawText;
						textInputCheckBox.isOn = true;

						break;


					case "moveToRoom":

						destinationRoomInput.interactable = true; 
						destinationRoomInput.text = subInt.destinationRoomName;
						enterRoomCheckBox.isOn = true;

						break;


					case "pickUpItem":

						List<string> itemStringList = LoadInventoryItems ();

						//Debug.Log (recieveItemDropdown.options.Count);

						recieveItemCheckBox.isOn = true;
						recieveItemDropdown.interactable = true; 
						int index;

						if (itemStringList.Contains (subInt.inventoryItem.fileName) == false) 
						{
							Debug.LogError ("Can't find item to recieve file name");

						} else {

							index = itemStringList.IndexOf (subInt.inventoryItem.fileName);
							recieveItemDropdown.value = index;
						}

						recieveItemTitleInput.text = subInt.inventoryItem.titleName;

						break;


					case "useItem":
						
						usingItemCheckBox.isOn = true;

						break;


				}

			}

		} else {

			loadedInteraction = null;
		}
	}



	public List<string> LoadInventoryItems()
	{

		Sprite[] itemSprites = Resources.LoadAll<Sprite> ("Sprites/Inventory/Small_items");
		List<string> itemStringList = new List<string> ();

		//List<Dropdown.OptionData> itemDataList = new List<Dropdown.OptionData> ();

		foreach (Sprite spr in itemSprites) 
		{
			//Dropdown.OptionData data = new Dropdown.OptionData (spr.name, spr);		
			itemStringList.Add (spr.name);
		}

		return itemStringList;
	}




	public void DestroyInteractionPanel()
	{

		if (interactionPanelObject != null) 
		{

			Destroy(interactionPanelObject);

		}

	}





	public void SubmitInteraction()
	{

	
		// Check if there are subinteractions

		Transform panel = interactionPanelObject.transform.FindChild ("Panel");


		// create interaction 

		Interaction interaction;

		if (loadedInteraction != null) 
		{
			interaction = loadedInteraction;

			// clearing subinteraction list

			interaction.subInteractionList.Clear ();


		
		} else {

			interaction = new Interaction ();

		}


		// interaction verb 

		InputField interactionTitleInput = panel.FindChild ("InteractionTitle").GetComponent<InputField> ();

		interaction.myVerb = interactionTitleInput.text;
		//Debug.Log ("currentType" + interaction.myVerb);



			
		// create subInteraction list



		// create show dialogue


		InputField interactionTextInput = panel.FindChild ("InteractionTextInput").GetComponent<InputField> ();


		if (interactionTextInput.interactable == true) 		
		{

			SubInteraction subInteraction = new SubInteraction ("showDialogue");
			subInteraction.rawText = interactionTextInput.text;

			interaction.subInteractionList.Add (subInteraction);

			subInteraction.textList = Utilities.SeparateText (subInteraction.rawText);

		}


		// create enter room


		InputField destinationRoomInput = panel.FindChild ("DestinationRoomInput").GetComponent<InputField> ();
	

		if (destinationRoomInput.interactable == true)		
		{
			
			SubInteraction subInteraction = new SubInteraction ("moveToRoom");
			subInteraction.destinationRoomName = destinationRoomInput.text;
		

			interaction.subInteractionList.Add (subInteraction);

		}


		// create recieve item


		Dropdown recieveItemDropdown = panel.FindChild("RecieveItemDropdown").GetComponent<Dropdown>();
		InputField recieveItemTitleInput = panel.FindChild ("RecieveItemTitleInput").GetComponent<InputField> ();
			
			
		if ((recieveItemDropdown.interactable == true) && (recieveItemTitleInput.interactable == true)) 
		{
			//Debug.Log ("creating subinteraction pick up item");

			SubInteraction subInteraction = new SubInteraction ("pickUpItem");
			subInteraction.inventoryItem = new InventoryItem (recieveItemDropdown.options [recieveItemDropdown.value].text, recieveItemTitleInput.text);

			//Debug.Log ("name" + subInteraction.inventoryItem.fileName);

			interaction.subInteractionList.Add (subInteraction);


		}


		// Create use item

	
		//usingItemCheckBox = panel.FindChild("UsingItemCheckBox").GetComponent<Toggle>();


		if (usingItemCheckBox.isOn == true) 
		{
		
			SubInteraction subInteraction = new SubInteraction ("useItem");
			interaction.subInteractionList.Add (subInteraction);

		}



		// If this is a new interaction, add to interaction list, if not, update interaction in list


		if (chosenFurniture.myInteractionList.Contains (interaction) == false) 
		{
			chosenFurniture.myInteractionList.Add (interaction);

		} else {

			int i = chosenFurniture.myInteractionList.IndexOf (interaction);
			chosenFurniture.myInteractionList [i] = interaction;

		}


		DestroyInteractionPanel ();

		DestroyInspector ();
		CreateInspector (chosenFurniture);


	}
	*/

	/*
	// ----- TILE INSPECTOR ----- // 


	void CreateTileInspector(TileInteraction currentTileInteraction)
	{
		
		DestroyInspector ();
		DestroyTileInspector ();

		tileInspectorObject = Instantiate (tileInspectorObjectPrefab);

		Transform panel = tileInspectorObject.transform.FindChild ("Panel");


		// SIZE AND POSITION //

		panel.FindChild ("SizeX").FindChild("Placeholder").GetComponent<Text> ().text = currentTileInteraction.mySize.x.ToString();
		panel.FindChild ("SizeY").FindChild("Placeholder").GetComponent<Text> ().text = currentTileInteraction.mySize.y.ToString();

		panel.FindChild ("SizeX").GetComponent<InputField> ().onEndEdit.AddListener (ChangeTileInteractionWidth);
		panel.FindChild ("SizeY").GetComponent<InputField> ().onEndEdit.AddListener (ChangeTileInteractionHeight);

		panel.FindChild ("PosX").FindChild("Placeholder").GetComponent<Text> ().text = currentTileInteraction.x.ToString();
		panel.FindChild ("PosY").FindChild("Placeholder").GetComponent<Text> ().text = currentTileInteraction.y.ToString();

		panel.FindChild ("PosX").GetComponent<InputField> ().onEndEdit.AddListener (ChangeTileInteractionX);
		panel.FindChild ("PosY").GetComponent<InputField> ().onEndEdit.AddListener (ChangeTileInteractionY);


		// INTERACTIONS //

		// Dialogue

		interactionTextInput = panel.FindChild("InteractionTextInput").GetComponent<InputField> ();
		textInputCheckBox = panel.FindChild ("TextInputCheckBox").GetComponent<Toggle> ();
		textInputCheckBox.onValueChanged.AddListener (CheckTileInspectorToggles);


		if (currentTileInteraction.mySubInt != null) 
		{			
			if (currentTileInteraction.mySubInt.rawText != null) 
			{

				Debug.Log ("CreateTileInspector: insert raw text" + currentTileInteraction.mySubInt.rawText);

				textInputCheckBox.isOn = true;
				interactionTextInput.interactable = true;

				interactionTextInput.text = currentTileInteraction.mySubInt.rawText;

			} else {

				Debug.Log ("rawText = null");
			}
		}


		// Destination room

		destinationRoomInput = panel.FindChild("DestinationRoomInput").GetComponent<InputField>();
		enterRoomCheckBox = panel.FindChild ("EnterRoomCheckBox").GetComponent<Toggle> ();
		enterRoomCheckBox.onValueChanged.AddListener (CheckTileInspectorToggles);


		if (currentTileInteraction.mySubInt != null) 
		{			
			if (currentTileInteraction.mySubInt.destinationRoomName != null) 
			{
				enterRoomCheckBox.isOn = true;
				destinationRoomInput.interactable = true;

				destinationRoomInput.text = currentTileInteraction.mySubInt.destinationRoomName;

			}
		}


		// Inspector toggles 

		CheckTileInspectorToggles (true);


		// Submit button

		panel.FindChild("SubmitButton").GetComponent<Button> ().onClick.AddListener  (() => SubmitTileInteraction());

	}


	public void CheckTileInspectorToggles(bool boolean)
	{

		if (textInputCheckBox.isOn == true) 
		{
			enterRoomCheckBox.isOn = false;
			enterRoomCheckBox.interactable = false;
			destinationRoomInput.interactable = false;

		} else {

			enterRoomCheckBox.interactable = true;
		}


		if (enterRoomCheckBox.isOn == true) 
		{

			textInputCheckBox.isOn = false;
			textInputCheckBox.interactable = false;
			interactionTextInput.interactable = false;


		} else {

			textInputCheckBox.interactable = true;
		}
	}





	public void SubmitTileInteraction()
	{

	
		// Check if there are subinteractions

		Transform panel = tileInspectorObject.transform.FindChild ("Panel");


	
		// create show dialogue


		InputField interactionTextInput = panel.FindChild ("InteractionTextInput").GetComponent<InputField> ();


		if (interactionTextInput.interactable == true) 		
		{

			SubInteraction subInteraction = new SubInteraction ("showDialogue");
			subInteraction.rawText = interactionTextInput.text;
			Debug.Log ("raw " + subInteraction.rawText);

			subInteraction.textList = Utilities.SeparateText (subInteraction.rawText);
			chosenTileInteraction.mySubInt = subInteraction;

		}


		// create enter room


		InputField destinationRoomInput = panel.FindChild ("DestinationRoomInput").GetComponent<InputField> ();


		if (destinationRoomInput.interactable == true)		
		{

			SubInteraction subInteraction = new SubInteraction ("moveToRoom");
			subInteraction.destinationRoomName = destinationRoomInput.text;

			chosenTileInteraction.mySubInt = subInteraction;

		}


		DestroyTileInspector ();
		CreateTileInspector (chosenTileInteraction);

	}




	// change size


	public void ChangeTileInteractionWidth(string x)
	{

		int newX = int.Parse (x);
		EditorRoomManager.instance.ChangeInteractableWidth (newX, chosenTileInteraction);

	}



	public void ChangeTileInteractionHeight(string y)
	{
		int newY = int.Parse (y);
		EditorRoomManager.instance.ChangeInteractableHeight (newY, chosenTileInteraction);

	}




	// change position


	public void ChangeTileInteractionX(string x)
	{

		int newX = int.Parse (x);
		EditorRoomManager.instance.ChangeInteractableTileX (newX, chosenTileInteraction);

	}



	public void ChangeTileInteractionY(string y)
	{
		int newY = int.Parse (y);
		EditorRoomManager.instance.ChangeInteractableTileY (newY, chosenTileInteraction);

	}








	// ----- DESTROY ----- //


	void DestroyTileInspector()
	{
		if (tileInspectorObject != null) 
		{
			Destroy (tileInspectorObject);
		}
	}



	*/




}
