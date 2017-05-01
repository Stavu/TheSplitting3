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

	GameObject inspectorObject;
	GameObject interactionPanelObject;

	Interaction loadedInteraction;

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

			if (_chosenFurniture == null) 
			{
				DestroyInspector ();

			} else {

				CreateInspector (_chosenFurniture);
			}
		}
	}






	// Use this for initialization
	void Start () 
	{		

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}


	void CreateInspector(Furniture currentFurniture)
	{

		DestroyInspector ();


		inspectorObject = Instantiate (inspectorObjectPrefab);

		Transform panel = inspectorObject.transform.FindChild ("Panel");


		panel.FindChild ("Name").GetComponent<Text> ().text = currentFurniture.myName;

		panel.FindChild ("SizeX").FindChild("Placeholder").GetComponent<Text> ().text = currentFurniture.mySize.x.ToString();
		panel.FindChild ("SizeY").FindChild("Placeholder").GetComponent<Text> ().text = currentFurniture.mySize.y.ToString();

		panel.FindChild ("SizeX").GetComponent<InputField> ().onEndEdit.AddListener (changeWidth);
		panel.FindChild ("SizeY").GetComponent<InputField> ().onEndEdit.AddListener (changeHeight);


		panel.FindChild ("PosX").FindChild("Placeholder").GetComponent<Text> ().text = currentFurniture.x.ToString();
		panel.FindChild ("PosY").FindChild("Placeholder").GetComponent<Text> ().text = currentFurniture.y.ToString();

		panel.FindChild ("PosX").GetComponent<InputField> ().onEndEdit.AddListener (changeX);
		panel.FindChild ("PosY").GetComponent<InputField> ().onEndEdit.AddListener (changeY);


		panel.FindChild ("OffsetX").FindChild("Placeholder").GetComponent<Text> ().text = currentFurniture.offsetX.ToString();
		panel.FindChild ("OffsetY").FindChild("Placeholder").GetComponent<Text> ().text = currentFurniture.offsetY.ToString();


		panel.FindChild ("OffsetX").GetComponent<InputField> ().onEndEdit.AddListener (changeOffsetX);
		panel.FindChild ("OffsetY").GetComponent<InputField> ().onEndEdit.AddListener (changeOffsetY);


	
		// create existing interactions

		for (int i = 0; i < 3; i++) 		
		{
			
			Button button = panel.FindChild ("AddInteraction" + i.ToString ()).GetComponent<Button> ();

			if (chosenFurniture.myInteractionList.Count > i) {
			
				button.transform.FindChild ("Text").GetComponent<Text>().text = chosenFurniture.myInteractionList [i].myVerb;
				Interaction interaction = chosenFurniture.myInteractionList [i];
				button.onClick.AddListener (() => OpenInteractionPanel(interaction));	
					

			} else {

			
				button.onClick.AddListener (() => OpenInteractionPanel(null));



			}


		}

	}




	void DestroyInspector()
	{

		if (inspectorObject != null) 
		{

			Destroy (inspectorObject);
		}

		DestroyInteractionPanel ();

	}




	// change size


	public void changeWidth(string x)
	{

		int newX = int.Parse (x);
		EditorRoomManager.instance.ChangeFurnitureWidth (newX, chosenFurniture);

	}



	public void changeHeight(string y)
	{
		int newY = int.Parse (y);
		EditorRoomManager.instance.ChangeFurnitureHeight (newY, chosenFurniture);

	}






	// change position


	public void changeX(string x)
	{

		int newX = int.Parse (x);
		EditorRoomManager.instance.ChangeFurnitureTileX (newX, chosenFurniture);
	
	}



	public void changeY(string y)
	{
		int newY = int.Parse (y);
		EditorRoomManager.instance.ChangeFurnitureTileY (newY, chosenFurniture);

	}




	// change offset


	public void changeOffsetX(string x)
	{

		float newX = float.Parse (x);
		EditorRoomManager.instance.ChangeFurnitureOffsetX (newX, chosenFurniture);

	}



	public void changeOffsetY(string y)
	{
		float newY = float.Parse (y);
		EditorRoomManager.instance.ChangeFurnitureOffsetY (newY, chosenFurniture);

	}



	// Creating a new interaction panel


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

				}

			}

		} else {

			loadedInteraction = null;
		}




		/*
		if (enterRoomDropdown.value != null) 
		{
			enterRoomDropdown.value = (int) interaction.myInteractionType;
		}


		List<string> roomList = new List<string> ();
		roomList.Add(interaction.myInteractionType.ToString());

		enterRoomDropdown.AddOptions(interactionList);
		*/

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


	/*
	public void DeactivateInteractionPanel()
	{

		if (interactionPanelObject != null) 
		{
			
			interactionPanelObject.SetActive (false);

		}

	}
	*/



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
			Debug.Log ("creating subinteraction pick up item");


			SubInteraction subInteraction = new SubInteraction ("pickUpItem");

			subInteraction.inventoryItem = new InventoryItem (recieveItemDropdown.options [recieveItemDropdown.value].text, recieveItemTitleInput.text);


			Debug.Log ("name" + subInteraction.inventoryItem.fileName);

			interaction.subInteractionList.Add (subInteraction);


		}



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



	/*

	public Interaction BuildIneraction()
	{




	}


	*/


}
