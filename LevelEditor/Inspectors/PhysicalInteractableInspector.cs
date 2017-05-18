using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhysicalInteractableInspector : MonoBehaviour {


	GameObject inspectorObjectPrefab;
	GameObject inspectorObject;



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

		Transform panel = inspectorObject.transform.FindChild ("Panel");

		Debug.Log ("name " + currentPhysicalInteractable.myName);

		panel.FindChild ("Name").GetComponent<Text> ().text = currentPhysicalInteractable.myName;

		panel.Find ("IdentificationName").GetComponent<InputField> ().text = currentPhysicalInteractable.identificationName;
		panel.Find ("IdentificationName").GetComponent<InputField> ().onEndEdit.AddListener (ChangeIdentificationName);

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

		Debug.Log ("destroy inspector");
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
	}







}
