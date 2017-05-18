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

	Transform moveToRoom;
	Transform recieveItem;
	Transform playAnimation;

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
		subIntTypeList.Add ("PlayAnimation");
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
		moveToRoom = panel.Find ("MoveToRoom");
		playAnimation = panel.Find ("PlayAnimation");


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

					textInputBig.text = subInt.RawText;

					break;


				case "showDialogue":

					textInputSmall.text = subInt.dialogueOptionTitle;

					break;


				case "showDialogueTree":

					textInputSmall.text = subInt.dialogueTreeName;

					break;

				
				case "PlayAnimation":

					playAnimation.Find("AnimationNameInput").GetComponent<InputField>().text = subInt.animationToPlay;
					playAnimation.Find("FurnitureNameInput").GetComponent<InputField>().text = subInt.targetFurniture;

					break;


				case "moveToRoom":
									
					moveToRoom.Find ("TextInputSmall1").GetComponent<InputField> ().text = subInt.destinationRoomName;
					moveToRoom.Find ("InputX").GetComponent<InputField> ().text = subInt.entrancePoint.x.ToString();
					moveToRoom.Find ("InputY").GetComponent<InputField> ().text = subInt.entrancePoint.y.ToString();

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
		moveToRoom.gameObject.SetActive (false);
		recieveItem.gameObject.SetActive (false);
		playAnimation.gameObject.SetActive (false);


		string typeString = subIntTypeList [type];


		switch (typeString) 
		{
			
			case "showMonologue":

				textInputBig.gameObject.SetActive (true);

				break;


			case "showDialogue":

			case "showDialogueTree":
				
				textInputSmall.gameObject.SetActive (true);

				break;


			case "PlayAnimation":

				playAnimation.gameObject.SetActive (true);

				break;



			case "moveToRoom":

				moveToRoom.gameObject.SetActive (true);

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

				currentSubint.RawText = textInputBig.text;

				break;


			case "showDialogue":

				currentSubint.dialogueOptionTitle = textInputSmall.text;

				break;


			case "showDialogueTree":

				currentSubint.dialogueTreeName = textInputSmall.text;

				break;


			case "PlayAnimation":

				currentSubint.animationToPlay = playAnimation.Find ("AnimationNameInput").GetComponent<InputField> ().text;
				currentSubint.targetFurniture = playAnimation.Find ("FurnitureNameInput").GetComponent<InputField> ().text;
					
				break;


			case "moveToRoom":

				currentSubint.destinationRoomName = moveToRoom.Find ("TextInputSmall1").GetComponent<InputField> ().text;

				Vector2 entrancePoint = new Vector2 (int.Parse (moveToRoom.Find ("InputX").GetComponent<InputField> ().text),
					                      			 int.Parse (moveToRoom.Find ("InputY").GetComponent<InputField> ().text));

				currentSubint.entrancePoint = entrancePoint;

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
