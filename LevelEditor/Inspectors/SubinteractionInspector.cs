using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubinteractionInspector : MonoBehaviour {


	// Declerations


	GameObject subinteractionPanelObject;

	Transform panel;
	Dropdown subIntTypeDropdown;
	InputField textInputBig;
	InputField textInputSmall;

	Transform recieveItem;

	Button cancelButton;
	Button submitButton;

	ISubinteractable subinteractable;
	SubInteraction currentSubint;
	List<string> subIntTypeList;



	// Use this for initialization

	void Start () 
	{
		subIntTypeList = new List<string> ();

		subIntTypeList.Add ("showMonologue");
		subIntTypeList.Add ("showDialogue");
		subIntTypeList.Add ("showDialogueTree");
		subIntTypeList.Add ("moveToRoom");
		subIntTypeList.Add ("pickUpItem");
		subIntTypeList.Add ("useItem");
		
	}


	
	// Update is called once per frame

	void Update () 
	{
		
	}



	// Create Subinteraction Panel


	public void CreateSubinteractionPanel(ISubinteractable iSubinteractable, SubInteraction subInt = null)
	{

		if (subinteractionPanelObject != null) 
		{
			return;
		}

		subinteractable = iSubinteractable;

		subinteractionPanelObject = Instantiate(Resources.Load<GameObject> ("Prefabs/Editor/InteractionPanelPrefabs/SubinteractionPanel"));

		panel = subinteractionPanelObject.transform.Find ("Panel");
		subIntTypeDropdown = panel.Find ("SubIntTypeDropdown").GetComponent<Dropdown> ();
		textInputBig = panel.Find ("TextInput").GetComponent<InputField> ();
		textInputSmall = panel.Find ("TextInputSmall").GetComponent<InputField> ();

		recieveItem = panel.Find ("RecieveItem");

		cancelButton = panel.Find ("CancelButton").GetComponent<Button> ();
		submitButton = panel.Find ("SubmitButton").GetComponent<Button> ();

		currentSubint = subInt;

		OpenSubinteractionPanel (currentSubint);

	}



	// OPEN //


	public void OpenSubinteractionPanel(SubInteraction subInt)
	{		


		// SubInteraction type dropdown

		subIntTypeDropdown.AddOptions (subIntTypeList);

		if (subInt != null) {		
		
			// set dropdown value

			int i = subIntTypeList.IndexOf (subInt.interactionType);
			subIntTypeDropdown.value = i;

			// What's active?

			SetSubinteractionType (i);


			// Fill the active fields


			switch (subInt.interactionType) {

				case "showMonologue":

					textInputBig.text = subInt.rawText;

					break;


				case "showDialogue":

					//textInputSmall.text = subInt.myDialogue;

					break;


				case "showDialogueTree":

					//textInputSmall.text = subInt.myDialogueTree;

					break;


				case "moveToRoom":

					textInputSmall.text = subInt.destinationRoomName;

					break;


				case "pickUpItem":

					recieveItem.Find ("TextInputSmall1").GetComponent<InputField> ().text = subInt.inventoryItem.fileName;
					recieveItem.Find ("TextInputSmall2").GetComponent<InputField> ().text = subInt.inventoryItem.titleName;

					break;


				case "useItem":
					
					break;

			}

		} else {


			// If subint is null, set first value on dropdown

			SetSubinteractionType (0);

		}


		// add listener to dropdown

		subIntTypeDropdown.onValueChanged.AddListener (SetSubinteractionType);


		// Cancel button

		cancelButton.onClick.AddListener(DestroySubinteractionInspector);


		// Submit button

		submitButton.onClick.AddListener (SubmitSubinteraction);



	}



	// Set subinteraction type - hide all fields, then decide what's active according to type

	public void SetSubinteractionType(int type)
	{
		
		textInputBig.gameObject.SetActive (false);
		textInputSmall.gameObject.SetActive (false);
		recieveItem.gameObject.SetActive (false);

		string typeString = subIntTypeList [type];


		switch (typeString) 
		{
			
			case "showMonologue":

				textInputBig.gameObject.SetActive (true);

				break;


			case "showDialogue":

			case "showDialogueTree":

			case "moveToRoom":

				textInputSmall.gameObject.SetActive (true);

				break;


			case "pickUpItem":

				recieveItem.gameObject.SetActive (true);

				break;

			case "useItem":

				break;

		}

	}





	// Cancel //

	public void DestroySubinteractionInspector()
	{
		if (subinteractionPanelObject != null) 
		{
			Destroy (subinteractionPanelObject);

		}
	}


	


	// Submit //

	public void SubmitSubinteraction()
	{


		if (currentSubint == null) 
		{
			Debug.Log ("SubmitSubinteraction: currentSubInt is null");

			int i = subIntTypeDropdown.value;
			string subIntType = subIntTypeList [i];

			currentSubint = new SubInteraction (subIntType);

			subinteractable.SubIntList.Add (currentSubint);

		} 


		// Reseting data fields, then filling them again

		currentSubint.ResetDataFields ();

		switch (currentSubint.interactionType) 
		{

			case "showMonologue":

				currentSubint.rawText = textInputBig.text;
				
				break;


			case "showDialogue":


				break;


			case "showDialogueTree":


				break;


			case "moveToRoom":

				currentSubint.destinationRoomName = textInputSmall.text;

				break;


			case "pickUpItem":

			
				string itemFileName = recieveItem.Find ("TextInputSmall1").GetComponent<InputField> ().text;
				string itemTitleName = recieveItem.Find ("TextInputSmall2").GetComponent<InputField> ().text;

				currentSubint.inventoryItem = new InventoryItem (itemFileName, itemTitleName);

				break;


			case "useItem":

				break;

		}


		EventsHandler.Invoke_cb_subinteractionChanged ();
		Destroy (subinteractionPanelObject);

	}


}
