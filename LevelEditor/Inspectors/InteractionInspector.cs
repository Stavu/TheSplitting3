using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionInspector : MonoBehaviour {


	// Declarations

	GameObject interactionPanelObject;
	Interaction loadedInteraction;

	// Prefabs

	GameObject rowObjectPrefab;
	GameObject conditionItemPrefab;


	Transform panel;
	Transform titleRow;
	Transform conditionContainer;

	InputField interactionTitleInput;

	Button interactionConditionButton;

	Button newSubIntButton;

	Button submitButton;
	Button cancelButton;



	// Use this for initialization

	void Start () 
	{
		rowObjectPrefab = Resources.Load<GameObject> ("Prefabs/Editor/Row");
		conditionItemPrefab = Resources.Load<GameObject> ("Prefabs/Editor/ConditionText");

		if ((rowObjectPrefab == null) || (conditionItemPrefab == null)) 
		{
			Debug.LogError ("Something is not quite right");
		}
	}




	// Update is called once per frame

	void Update () 
	{

	}




	// Create interaction panel

	public void CreateInteractionPanel()
	{
		interactionPanelObject = Instantiate (InspectorManager.instance.interactionPanelObjectPrefab);

		panel = interactionPanelObject.transform.Find ("Panel");

		titleRow = panel.Find ("Panel").Find ("Row_Title");
		interactionTitleInput = titleRow.Find ("InteractionTitle").GetComponent<InputField> ();
		conditionContainer = titleRow.Find ("Conditions");


		interactionConditionButton = titleRow.Find("AddConditionButton").GetComponent<Button> ();

		newSubIntButton = panel.Find("AddSubIntPanel").Find("AddButton").GetComponent<Button> ();
		cancelButton = panel.Find("InteractionButtonsPanel").Find("CancelButton").GetComponent<Button> ();
		submitButton = panel.Find("InteractionButtonsPanel").Find("SubmitButton").GetComponent<Button> ();

	}




	// Opening interaction panel after created

	public void OpenInteractionPanel(Interaction interaction = null)
	{

		DestroyInteractionPanel ();
		CreateInteractionPanel ();


		//interactionPanelObject.SetActive (true);
			


		if (interaction != null) 
		{

			loadedInteraction = interaction;

			// interaction title input field

			interactionTitleInput.text = interaction.myVerb;


			// insert interaction conditions 

			for (int i = 0; i < loadedInteraction.conditionList.Count; i++) 
			{

				GameObject obj = Instantiate (conditionItemPrefab);
				obj.transform.SetParent (conditionContainer);

				Condition cond = loadedInteraction.conditionList [i];
				string condString = "";

				switch (cond.myType) 
				{

					case ConditionType.HasItem:

						condString = cond.hasItem;

						break;

					
					case ConditionType.EventOccured:

						condString = cond.eventOccured;

						break;


					case ConditionType.CharacterInRoom:

						condString = cond.characterInRoom;

						break;
											
				}


				obj.GetComponent<Text> ().text = string.Format ("{0} - {1}", cond.myType, condString);


				// Give the button event listener


				obj.Find ("Button").GetComponent<Button> ().onClick.AddListener (() => RemoveCondition(interaction,cond));

			}	


			// Set row height


			if (loadedInteraction.conditionList.Count >= 2) 
			{

				Rect tempRect;
				tempRect = titleRow.GetComponent<RectTransform> ().rect;
				titleRow.GetComponent<RectTransform> ().rect.Set (tempRect.x, tempRect.y, tempRect.width, loadedInteraction.conditionList.Count * 30);

			}


			// New Condition Button 

			interactionConditionButton.onClick.AddListener (OpenConditionPanel);






			// create row for each subinteraction in the interaction's subinterction list





		} else {

			loadedInteraction = null;
		}




	}






	public void OpenConditionPanel ()
	{






	}




	public void AddCondition (Interaction interaction, Condition condition)
	{




	}



	public void RemoveCondition (Interaction interaction, Condition condition)
	{

		interaction.RemoveConditionFromList (condition);
		OpenInteractionPanel ();

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
