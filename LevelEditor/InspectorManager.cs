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
			
				button.transform.FindChild ("Text").GetComponent<Text>().text = chosenFurniture.myInteractionList [i].myInteractionType.ToString ();
				Interaction interaction = chosenFurniture.myInteractionList [i];
				button.onClick.AddListener (() => OpenEditInteractionPanel(interaction));	
					

			} else {

				button.onClick.AddListener (CreateInteractionPanel);



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





	public void CreateInteractionPanel()
	{

		DestroyInteractionPanel ();

		interactionPanelObject = Instantiate (interactionPanelObjectPrefab);
		Dropdown interactionDropdown = interactionPanelObject.GetComponentInChildren<Dropdown>();

		System.Array optionStringArray = Enum.GetValues (typeof(InteractionType));
		List<string> optionStringList = new List<string>();


		// FIXME

		foreach (var item in optionStringArray) 
		{

			optionStringList.Add (item.ToString());
			
		}


		interactionDropdown.AddOptions (optionStringList);


		// submit button


		interactionPanelObject.transform.FindChild ("Panel").FindChild("SubmitButton").GetComponent<Button> ().onClick.AddListener  (() => SubmitInteraction());


		// enter room dropdown

		Dropdown enterRoomDropdown = interactionPanelObject.transform.FindChild("EnterRoomDropdown").GetComponentInChildren<Dropdown>();



		// recieve item dropdown

		Dropdown recieveItemDropdown = interactionPanelObject.transform.FindChild("RecieveItemDropdown").GetComponentInChildren<Dropdown>();


	}




	public void OpenEditInteractionPanel(Interaction interaction)
	{

		DestroyInteractionPanel ();

		interactionPanelObject = Instantiate (interactionPanelObjectPrefab);

		// interaction dropdown

		Dropdown interactionDropdown = interactionPanelObject.GetComponentInChildren<Dropdown>();
		interactionDropdown.value = (int) interaction.myInteractionType;

		List<string> interactionList = new List<string> ();
		interactionList.Add(interaction.myInteractionType.ToString());

		interactionDropdown.AddOptions(interactionList);
		interactionDropdown.interactable = false;


		// set texts


		InputField[] textComp = interactionPanelObject.transform.FindChild("Interactions").GetComponentsInChildren<InputField> ();

		for (int i = 0; i < interaction.textList.Count; i++) 
		{
			
			textComp [i].text = interaction.textList [i];
		}



		// submit button

		interactionPanelObject.transform.FindChild ("Panel").FindChild("SubmitButton").GetComponent<Button> ().onClick.AddListener (() => SubmitInteraction(interaction));


		// enter room dropdown

		Dropdown enterRoomDropdown = interactionPanelObject.transform.FindChild("EnterRoomDropdown").GetComponentInChildren<Dropdown>();



		// recieve item dropdown

		Dropdown recieveItemDropdown = interactionPanelObject.transform.FindChild("RecieveItemDropdown").GetComponentInChildren<Dropdown>();



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





	public void DestroyInteractionPanel()
	{

		if (interactionPanelObject != null) 
		{

			Destroy (interactionPanelObject);

		}

	}


	public void SubmitInteraction(Interaction interaction = null)
	{

		Dropdown dropdown = interactionPanelObject.GetComponentInChildren<Dropdown>();

		InteractionType currentType = (InteractionType)dropdown.value;
		Debug.Log ("currentType" + currentType);


	
		switch(currentType)
		{

		case InteractionType.look_at:
				
						
			List<string> interactionTextList = new List<string> ();
			string inputText = interactionPanelObject.transform.FindChild ("Panel").FindChild ("InteractionText").GetComponent<InputField> ().text;


			char delimiter = '|';
			string[] result = inputText.Split (delimiter);

			foreach (string str in result)
			{
				interactionTextList.Add (str);
			}

			Debug.Log (interactionTextList);

			if (interaction == null) 
			{
				interaction = new Interaction (currentType, interactionTextList);	

			} else {

				interaction.textList = interactionTextList;
			}

			// Hello! How are you?| My name is Daniel. | What's yours?


			break;



			/*
		case InteractionType.enter:


			break;
			*/

		}


		if (chosenFurniture.myInteractionList.Contains (interaction) == false) 
		{
			chosenFurniture.myInteractionList.Add (interaction);
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
