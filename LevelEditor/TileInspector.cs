using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileInspector : MonoBehaviour {


	// Declarations

	GameObject tileInspectorObject;

	Transform panel;

	InputField interactionTextInput;
	Toggle textInputCheckBox;

	InputField destinationRoomInput;
	Toggle enterRoomCheckBox;



	// Use this for initialization

	void Start () 
	{
		
	}
	
	// Update is called once per frame

	void Update () 
	{
		
	}



	// ----- TILE INSPECTOR ----- // 


	public void CreateTileInspector(TileInteraction currentTileInteraction)
	{

		Debug.Log("CreateTileInspector");

		InspectorManager.instance.DestroyInspector ();
		DestroyTileInspector ();

		tileInspectorObject = Instantiate (InspectorManager.instance.tileInspectorObjectPrefab);

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

		textInputCheckBox.onValueChanged.AddListener (CheckTileInspectorToggles);
		enterRoomCheckBox.onValueChanged.AddListener (CheckTileInspectorToggles);

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
			InspectorManager.instance.chosenTileInteraction.mySubInt = subInteraction;

		}


		// create enter room


		InputField destinationRoomInput = panel.FindChild ("DestinationRoomInput").GetComponent<InputField> ();


		if (destinationRoomInput.interactable == true)		
		{

			SubInteraction subInteraction = new SubInteraction ("moveToRoom");
			subInteraction.destinationRoomName = destinationRoomInput.text;

			InspectorManager.instance.chosenTileInteraction.mySubInt = subInteraction;

		}


		DestroyTileInspector ();
		//CreateTileInspector (InspectorManager.instance.chosenTileInteraction);

	}




	// change size


	public void ChangeTileInteractionWidth(string x)
	{

		int newX = int.Parse (x);
		EditorRoomManager.instance.ChangeInteractableWidth (newX, InspectorManager.instance.chosenTileInteraction);

	}



	public void ChangeTileInteractionHeight(string y)
	{
		int newY = int.Parse (y);
		EditorRoomManager.instance.ChangeInteractableHeight (newY, InspectorManager.instance.chosenTileInteraction);

	}




	// change position


	public void ChangeTileInteractionX(string x)
	{

		int newX = int.Parse (x);
		EditorRoomManager.instance.ChangeInteractableTileX (newX, InspectorManager.instance.chosenTileInteraction);

	}



	public void ChangeTileInteractionY(string y)
	{
		int newY = int.Parse (y);
		EditorRoomManager.instance.ChangeInteractableTileY (newY, InspectorManager.instance.chosenTileInteraction);

	}








	// ----- DESTROY ----- //


	public void DestroyTileInspector()
	{
		if (tileInspectorObject != null) 
		{
			Destroy (tileInspectorObject);
		}
	}








}
