using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhysicalInteractableInspector : MonoBehaviour {


	GameObject inspectorObjectPrefab;
	GameObject inspectorObject;

	Transform panel;

	Text nameText;
	InputField identificationText;

	Toggle imageFlippedToggle;
	Toggle persistentToggle;

	// inputs

	InputField sizeXInput;
	InputField sizeYInput;

	InputField posXInput;
	InputField posYInput;

	InputField offsetXInput;
	InputField offsetYInput;

	InputField frameXInput;
	InputField frameYInput;

	InputField frameOffsetXInput;
	InputField frameOffsetYInput;



	// placeholders

	Text sizeXPlaceholder;
	Text sizeYPlaceholder;

	Text posXPlaceholder;
	Text posYPlaceholder;

	Text offsetXPlaceholder;
	Text offsetYPlaceholder;

	Text frameXPlaceholder;
	Text frameYPlaceholder;

	Text frameOffsetXPlaceholder;
	Text frameOffsetYPlaceholder;


	// delete button

	Button deleteButton;







	// Use this for initialization

	void Start () 
	{
		inspectorObjectPrefab = Resources.Load<GameObject> ("Prefabs/Editor/InteractionPanelPrefabs/Inspector");
		
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


		// Assign 

		panel = inspectorObject.transform.FindChild ("Panel");

		nameText = panel.FindChild ("Name").GetComponent<Text> ();
		identificationText = panel.Find ("IdentificationName").GetComponent<InputField> ();

		imageFlippedToggle = panel.Find ("ImageFlippedToggle").GetComponent<Toggle> ();
		persistentToggle = panel.Find ("PersistentToggle").GetComponent<Toggle> ();

		deleteButton = panel.Find ("DeleteButton").GetComponent<Button> ();

		sizeXInput = panel.FindChild ("SizeX").GetComponent<InputField> ();
		sizeYInput = panel.FindChild ("SizeY").GetComponent<InputField> ();

		posXInput = panel.FindChild ("PosX").GetComponent<InputField> ();
		posYInput = panel.FindChild ("PosY").GetComponent<InputField> ();

		offsetXInput = panel.FindChild ("OffsetX").GetComponent<InputField> ();
		offsetYInput = panel.FindChild ("OffsetY").GetComponent<InputField> ();

		frameXInput = panel.FindChild ("FrameX").GetComponent<InputField> ();
		frameYInput = panel.FindChild ("FrameY").GetComponent<InputField> ();

		frameOffsetXInput = panel.FindChild ("FrameOffsetX").GetComponent<InputField> ();
		frameOffsetYInput = panel.FindChild ("FrameOffsetY").GetComponent<InputField> ();


		// placeholders 

		sizeXPlaceholder = panel.FindChild ("SizeX").FindChild ("Placeholder").GetComponent<Text> ();
		sizeYPlaceholder = panel.FindChild ("SizeY").FindChild("Placeholder").GetComponent<Text> ();

		posXPlaceholder = panel.FindChild ("PosX").FindChild ("Placeholder").GetComponent<Text> ();
		posYPlaceholder = panel.FindChild ("PosY").FindChild ("Placeholder").GetComponent<Text> ();

		offsetXPlaceholder = panel.FindChild ("OffsetX").FindChild ("Placeholder").GetComponent<Text> ();
		offsetYPlaceholder = panel.FindChild ("OffsetY").FindChild ("Placeholder").GetComponent<Text> ();

		frameXPlaceholder = panel.FindChild ("FrameX").FindChild ("Placeholder").GetComponent<Text> ();
		frameYPlaceholder = panel.FindChild ("FrameY").FindChild ("Placeholder").GetComponent<Text> ();

		frameOffsetXPlaceholder = panel.FindChild ("FrameOffsetX").FindChild ("Placeholder").GetComponent<Text> ();
		frameOffsetYPlaceholder = panel.FindChild ("FrameOffsetY").FindChild ("Placeholder").GetComponent<Text> ();



		// Text

		nameText.text = currentPhysicalInteractable.myName;
		identificationText.text = currentPhysicalInteractable.identificationName;

		sizeXPlaceholder.text = currentPhysicalInteractable.mySize.x.ToString();
		sizeYPlaceholder.text = currentPhysicalInteractable.mySize.y.ToString();

		posXPlaceholder.text = currentPhysicalInteractable.x.ToString();
		posYPlaceholder.text = currentPhysicalInteractable.y.ToString();
			
		offsetXPlaceholder.text = currentPhysicalInteractable.offsetX.ToString();
		offsetYPlaceholder.text = currentPhysicalInteractable.offsetY.ToString();

		frameXPlaceholder.text = currentPhysicalInteractable.frameExtents.x.ToString();
		frameYPlaceholder.text = currentPhysicalInteractable.frameExtents.y.ToString();

		frameOffsetXPlaceholder.text = currentPhysicalInteractable.frameOffsetX.ToString();
		frameOffsetYPlaceholder.text = currentPhysicalInteractable.frameOffsetY.ToString();



		// Listeners

		identificationText.onEndEdit.AddListener (ChangeIdentificationName);

		sizeXInput.onEndEdit.AddListener (changeWidth);
		sizeYInput.onEndEdit.AddListener (changeHeight);
	
		posXInput.onEndEdit.AddListener (changeX);
		posYInput.onEndEdit.AddListener (changeY);

		offsetXInput.onEndEdit.AddListener (changeOffsetX);
		offsetYInput.onEndEdit.AddListener (changeOffsetY);

		frameXInput.onEndEdit.AddListener (ChangeFrameWidth);
		frameYInput.onEndEdit.AddListener (ChangeFrameHeight);

		frameOffsetXInput.onEndEdit.AddListener (ChangeFrameOffsetX);
		frameOffsetYInput.onEndEdit.AddListener (ChangeFrameOffsetY);


		deleteButton.onClick.AddListener (() => EditorUI.DisplayAlert("Are you sure?", DeletePhysicalInteractable));


		// Toggle 

		if (currentPhysicalInteractable is Furniture) 
		{
			Furniture furn = (Furniture)currentPhysicalInteractable;

			imageFlippedToggle.interactable = true;
			imageFlippedToggle.isOn = furn.imageFlipped;

			imageFlippedToggle.onValueChanged.AddListener (SetImageFlipped);
						
		} else {

			imageFlippedToggle.interactable = false;
		}


		// Persistent - only if furniture

		if (currentPhysicalInteractable is Furniture) 
		{

			Furniture furn = (Furniture)currentPhysicalInteractable;

			if (EditorRoomManager.instance.room.RoomState == RoomState.Real) 
			{
				persistentToggle.interactable = false;

			} else {

				persistentToggle.interactable = true;

				if (EditorRoomManager.instance.room.myMirrorRoom.myFurnitureList_Persistant.Contains (furn)) 
				{
					persistentToggle.isOn = true;

				} else {

					persistentToggle.isOn = false;

				}
			}

			persistentToggle.onValueChanged.AddListener (FurniturePersistantToggleClicked);


		} else {
			
			persistentToggle.interactable = false;
		}




		// create existing interactions

		for (int i = 0; i < 6; i++) {

			Button button = panel.FindChild ("AddInteraction" + i.ToString ()).GetComponent<Button> ();

			if (currentPhysicalInteractable.myInteractionList.Count > i) 
			{

				button.transform.FindChild ("Text").GetComponent<Text> ().text = currentPhysicalInteractable.myInteractionList [i].myVerb;
				Interaction interaction = currentPhysicalInteractable.myInteractionList [i];
				button.onClick.AddListener (() => LoadInteractionAndOpenPanel (interaction));	

			} else {

				button.onClick.AddListener (() => LoadInteractionAndOpenPanel (null));

			}
		}
	}



	public void LoadInteractionAndOpenPanel(Interaction interaction)
	{

		InspectorManager.interactionInspector.loadedInteraction = interaction;

		InspectorManager.interactionInspector.CreateInteractionPanel ();

	}




	public void DestroyInspector()
	{

	//	Debug.Log ("destroy inspector");
		if (inspectorObject != null) 
		{

			Destroy (inspectorObject);
		}

		InspectorManager.interactionInspector.DestroyInteractionPanel ();

	}





	// --------- EDITING --------- // 






	// identification

	public void ChangeIdentificationName (string name)
	{
		if (InspectorManager.instance.chosenFurniture != null) 
		{
			if (name == string.Empty) 
			{
				InspectorManager.instance.chosenFurniture.identificationName = InspectorManager.instance.chosenFurniture.myName;

			} else {			

				InspectorManager.instance.chosenFurniture.identificationName = name;
			}

		} else if (InspectorManager.instance.chosenCharacter != null) 
		{			
			if (name == string.Empty) 
			{
				InspectorManager.instance.chosenCharacter.identificationName = InspectorManager.instance.chosenCharacter.myName;

			} else {			

				InspectorManager.instance.chosenCharacter.identificationName = name;
			}
		}

	}


	// image flipping 


	public void SetImageFlipped(bool isFlipped)
	{

		Furniture furn = InspectorManager.instance.chosenFurniture;

		furn.imageFlipped = isFlipped;
		EditorRoomManager.instance.furnitureGameObjectMap [furn].GetComponent<SpriteRenderer> ().flipX = isFlipped;

		changeOffsetX ((-furn.offsetX).ToString ());

	}



	// persistency



	public void FurniturePersistantToggleClicked(bool isPersistent)
	{

		Furniture furn = InspectorManager.instance.chosenFurniture;

		EditorRoomHelper.SetFurniturePersistency (isPersistent,furn);

	}






	// change size


	public void changeWidth(string x)
	{

		int newX = int.Parse (x);

		if (InspectorManager.instance.chosenFurniture != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableWidth (newX, InspectorManager.instance.chosenFurniture);

		} else if (InspectorManager.instance.chosenCharacter != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableWidth (newX, InspectorManager.instance.chosenCharacter);
		}


	}



	public void changeHeight(string y)
	{
		int newY = int.Parse (y);

		if (InspectorManager.instance.chosenFurniture != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableHeight (newY, InspectorManager.instance.chosenFurniture);

		} else if (InspectorManager.instance.chosenCharacter != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableHeight (newY, InspectorManager.instance.chosenCharacter);
		}

	}



	// change position


	public void changeX(string x)
	{

		int newX = int.Parse (x);

		if (InspectorManager.instance.chosenFurniture != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableTileX (newX, InspectorManager.instance.chosenFurniture);

		} else if (InspectorManager.instance.chosenCharacter != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableTileX (newX, InspectorManager.instance.chosenCharacter);
		}

	}




	public void changeY(string y)
	{
		int newY = int.Parse (y);


		if (InspectorManager.instance.chosenFurniture != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableTileY (newY, InspectorManager.instance.chosenFurniture);

		} else if (InspectorManager.instance.chosenCharacter != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableTileY (newY, InspectorManager.instance.chosenCharacter);
		}
	}




	// change offset


	public void changeOffsetX(string x)
	{

		float newX = float.Parse (x);

		if (InspectorManager.instance.chosenFurniture != null) 
		{
			EditorRoomManager.instance.ChangeInteractableOffsetX (newX, InspectorManager.instance.chosenFurniture);

		} else if (InspectorManager.instance.chosenCharacter != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableOffsetX (newX, InspectorManager.instance.chosenCharacter);
		}

		offsetXInput.text = x;
	}





	public void changeOffsetY(string y)
	{
		float newY = float.Parse (y);

		if (InspectorManager.instance.chosenFurniture != null) 
		{
			EditorRoomManager.instance.ChangeInteractableOffsetY (newY, InspectorManager.instance.chosenFurniture);

		} else if (InspectorManager.instance.chosenCharacter != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableOffsetY (newY, InspectorManager.instance.chosenCharacter);
		}	

		offsetYInput.text = y;
	}


	// frame size

	public void ChangeFrameWidth(string width)
	{

		float newWidth = float.Parse (width);

		if (InspectorManager.instance.chosenFurniture != null) 
		{
			EditorRoomManager.instance.ChangeInteractableFrameWidth (newWidth, InspectorManager.instance.chosenFurniture);

		} else if (InspectorManager.instance.chosenCharacter != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableFrameWidth (newWidth, InspectorManager.instance.chosenCharacter);
		}	

		frameXInput.text = width;

	}




	public void ChangeFrameHeight(string height)
	{

		float newHeight = float.Parse (height);

		if (InspectorManager.instance.chosenFurniture != null) 
		{
			EditorRoomManager.instance.ChangeInteractableFrameHeight (newHeight, InspectorManager.instance.chosenFurniture);

		} else if (InspectorManager.instance.chosenCharacter != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableFrameHeight (newHeight, InspectorManager.instance.chosenCharacter);
		}	

		frameXInput.text = height;


	}



	// change offset


	public void ChangeFrameOffsetX(string x)
	{

		float newX = float.Parse (x);

		if (InspectorManager.instance.chosenFurniture != null) 
		{
			EditorRoomManager.instance.ChangeInteractableFrameOffsetX (newX, InspectorManager.instance.chosenFurniture);

		} else if (InspectorManager.instance.chosenCharacter != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableFrameOffsetX (newX, InspectorManager.instance.chosenCharacter);
		}

		frameOffsetXInput.text = x;
	}





	public void ChangeFrameOffsetY(string y)
	{
		float newY = float.Parse (y);

		if (InspectorManager.instance.chosenFurniture != null) 
		{
			EditorRoomManager.instance.ChangeInteractableFrameOffsetY (newY, InspectorManager.instance.chosenFurniture);

		} else if (InspectorManager.instance.chosenCharacter != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableFrameOffsetY (newY, InspectorManager.instance.chosenCharacter);
		}	

		frameOffsetYInput.text = y;
	}









	// ----- DELETE ----- //

	public void DeletePhysicalInteractable()
	{

		// Furniture

		if (InspectorManager.instance.chosenFurniture != null) 
		{

			Furniture furn = InspectorManager.instance.chosenFurniture;
			GameObject obj = EditorRoomManager.instance.furnitureGameObjectMap[furn];
			Tile tile = EditorRoomManager.instance.room.MyGrid.GetTileAt (furn.x, furn.y);


			Destroy (obj.gameObject);
			EditorRoomManager.instance.furnitureGameObjectMap.Remove (furn);
			EditorRoomManager.instance.room.myFurnitureList.Remove (furn);
			tile.myFurniture = null;

			InspectorManager.instance.chosenFurniture = null;

		}

		// Character 

		if (InspectorManager.instance.chosenCharacter != null) 
		{

			Character character = InspectorManager.instance.chosenCharacter;
			GameObject obj = EditorRoomManager.instance.characterGameObjectMap[character];
			Tile tile = EditorRoomManager.instance.room.MyGrid.GetTileAt (character.x, character.y);


			Destroy (obj.gameObject);
			EditorRoomManager.instance.characterGameObjectMap.Remove (character);
			EditorRoomManager.instance.room.myCharacterList.Remove (character);
			tile.myCharacter = null;

			InspectorManager.instance.chosenCharacter = null;
		}



		EventsHandler.Invoke_cb_tileLayoutChanged ();
		//DestroyInspector ();


	}



}
