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
	public static ConditionInspector conditionInspector;
	public static SubinteractionInspector subinteractionInspector;
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

		if (conditionInspector == null) 
		{
			conditionInspector = gameObject.AddComponent<ConditionInspector> ();
		}

		if (subinteractionInspector == null) 
		{
			subinteractionInspector = gameObject.AddComponent<SubinteractionInspector> ();
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





	// --------- EDITING --------- // 


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





}
