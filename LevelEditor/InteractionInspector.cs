using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionInspector : MonoBehaviour {


	GameObject interactionPanelObject;

	Interaction loadedInteraction;


	// Use this for initialization

	void Start () 
	{
		
	}
	
	// Update is called once per frame

	void Update () 
	{
		
	}

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

		interactionPanelObject = Instantiate (InspectorManager.instance.interactionPanelObjectPrefab);

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

			SubInteraction subInteraction = new SubInteraction ("pickUpItem");
			subInteraction.inventoryItem = new InventoryItem (recieveItemDropdown.options [recieveItemDropdown.value].text, recieveItemTitleInput.text);

			interaction.subInteractionList.Add (subInteraction);

		}


		// Create use item


		//usingItemCheckBox = panel.FindChild("UsingItemCheckBox").GetComponent<Toggle>();


		if (usingItemCheckBox.isOn == true) 
		{

			SubInteraction subInteraction = new SubInteraction ("useItem");
			interaction.subInteractionList.Add (subInteraction);

		}


		PhysicalInteractable currentPhysicalInteractable = null;

		if (InspectorManager.instance.chosenFurniture != null) 
		{

			currentPhysicalInteractable = InspectorManager.instance.chosenFurniture;

		} else if (InspectorManager.instance.chosenCharacter != null) 
		{
			currentPhysicalInteractable = InspectorManager.instance.chosenCharacter;

		}
			


		// If this is a new interaction, add to interaction list, if not, update interaction in list

		if (currentPhysicalInteractable != null)
		{
			if (currentPhysicalInteractable.myInteractionList.Contains (interaction) == false) 
			{
				currentPhysicalInteractable.myInteractionList.Add (interaction);

			} else {

				int i = currentPhysicalInteractable.myInteractionList.IndexOf (interaction);
				currentPhysicalInteractable.myInteractionList [i] = interaction;

			}
		}


		DestroyInteractionPanel ();

		InspectorManager.instance.DestroyInspector ();
		InspectorManager.instance.CreateInspector (currentPhysicalInteractable);


	}




}
