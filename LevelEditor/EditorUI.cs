using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;




public class EditorUI : MonoBehaviour {



	// Singleton //

	public static EditorUI instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

	}

	// Singleton //



	Dropdown backgroundDropdown;
	public GameObject interactableSelectPrefab;
	public GameObject interactableButtonPrefab;

	GameObject interactableSelect;

	InputField roomNameInput;

	Button toGameButton;
	Button furnButton;
	Button characterButton;
	Button tileIntButton;

	Button flipRoomButton;
	Dropdown roomStateDropdown;

	Toggle shadowToggle;


	// Use this for initialization

	public void CreateUI () {


		CreateBgSelect ();



	}



	// Update is called once per frame
	void Update () 
	{

	}




	public void CreateBgSelect()
	{	

		// Lists

		List<string> bgNameList = new List<string> ();
		Sprite[] bgSpriteList = Resources.LoadAll <Sprite> ("Sprites/Rooms/");

		foreach (Sprite spr in bgSpriteList) 
		{

			bgNameList.Add (spr.name);

		}


		// Assign

		toGameButton = transform.Find ("ToGameButton").GetComponent<Button> ();

		roomNameInput = transform.Find ("RoomNameInput").GetComponent<InputField> ();
		backgroundDropdown = gameObject.GetComponentInChildren<Dropdown>();
		furnButton = transform.Find ("FurnitureButton").GetComponent<Button>();
		characterButton = transform.Find ("CharacterButton").GetComponent<Button>();
		tileIntButton = transform.Find ("TileInteractionButton").GetComponent<Button> ();
		flipRoomButton = transform.Find ("FlipRoomButton").GetComponent<Button>();
		roomStateDropdown = transform.Find ("RoomStateDropdown").GetComponent<Dropdown>();
		shadowToggle = transform.Find ("ShadowToggle").GetComponent<Toggle>();


		// Listeners

		toGameButton.onClick.AddListener(() => SceneManager.LoadScene("Main"));

		backgroundDropdown.AddOptions (bgNameList);
		backgroundDropdown.onValueChanged.AddListener (ChangeRoomBg);
	
		furnButton.onClick.AddListener (CreateFurnitureSelect);
		characterButton.onClick.AddListener (CreateCharacterSelect);
		tileIntButton.onClick.AddListener (SetTileInteractionMode);	

		flipRoomButton.onClick.AddListener (FlipRoom);
		roomStateDropdown.onValueChanged.AddListener (SetRoomState);
		roomStateDropdown.value = (int)EditorRoomManager.instance.room.roomState;

		roomNameInput.onEndEdit.AddListener (RoomNameChanged);



		// Room name 

		if (EditorRoomManager.instance.room.myName != null) 
		{
			roomNameInput.text = EditorRoomManager.instance.room.myName;
		}


		// Shadow state 

		if (EditorRoomManager.instance.room.roomState == RoomState.Real) 
		{
			shadowToggle.interactable = false;
		
		} else {

			shadowToggle.interactable = true;
			shadowToggle.isOn = EditorRoomManager.instance.room.myMirrorRoom.inTheShadow;
			roomStateDropdown.interactable = !shadowToggle.isOn;
		}





		shadowToggle.onValueChanged.AddListener (SetShadowState);


	}




	public void ChangeRoomBg(int optionNum)
	{

		Debug.Log ("change room bg");

		string spriteName = backgroundDropdown.options [optionNum].text;

		EditorRoomManager.instance.SetRoomBackground (spriteName);

	}




	// CREATING FURNITURE //



	public void CreateFurnitureSelect()
	{

		interactableSelect = Instantiate (interactableSelectPrefab);

		GameObject content = interactableSelect.GetComponentInChildren<GridLayoutGroup> ().gameObject;
				
		Sprite[] furnitureSpriteList = Resources.LoadAll<Sprite> ("Sprites/Furniture/");

		foreach (Sprite sprite in furnitureSpriteList) 
		{

			GameObject button = Instantiate (interactableButtonPrefab);
			button.transform.SetParent (content.transform);

			button.GetComponent<Image> ().sprite = sprite;
			button.GetComponentInChildren<Text> ().text = sprite.name;

			button.GetComponent<Button> ().onClick.AddListener (() => SetFurnitureBuildMode(sprite.name));


		}


	}



	public void SetFurnitureBuildMode(string furnitureName)
	{

		BuildController.instance.furnitureName = furnitureName;

		Destroy (interactableSelect);


		// set to build Furniture mode

		BuildController.instance.mode = BuildController.Mode.buildFurniture;


	}





	// CREATING CHARACTER //


	public void CreateCharacterSelect()
	{

		interactableSelect = Instantiate (interactableSelectPrefab);

		GameObject content = interactableSelect.GetComponentInChildren<GridLayoutGroup> ().gameObject;

		GameObject[] characterGameObjectList = Resources.LoadAll<GameObject> ("Prefabs/Characters/");

		foreach (GameObject gameObject in characterGameObjectList) 
		{

			GameObject button = Instantiate (interactableButtonPrefab);
			button.transform.SetParent (content.transform);


			button.GetComponent<Image> ().sprite = gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
			button.GetComponentInChildren<Text> ().text = gameObject.name;

			button.GetComponent<Button> ().onClick.AddListener (() => SetCharacterBuildMode(gameObject.name));


		}

	}





	public void SetCharacterBuildMode(string characterName)
	{

		BuildController.instance.characterName = characterName;

		Destroy (interactableSelect);


		// set to build Furniture mode

		BuildController.instance.mode = BuildController.Mode.buildCharacter;


	}







	// CREATE TILE INTERACTION //


	public void SetTileInteractionMode()
	{

		// set to build tileInteraction mode

		BuildController.instance.mode = BuildController.Mode.buildTileInteraction;

	}







	// ROOM NAME

	public void RoomNameChanged(string name)
	{

		EditorRoomManager.instance.room.myName = name;

	}



	// --------------- MIRROR ROOM ---------------- //




	public void FlipRoom ()
	{

		Room newRoom = EditorRoomHelper.CreateFlippedRoom (EditorRoomManager.instance.room);

		string roomString = JsonUtility.ToJson (newRoom);

		// Load room

		EditorRoomManager.roomToLoad = roomString;
		EditorRoomManager.loadRoomFromMemory = true;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);

	}


	public void SetRoomState(int i)
	{

		EditorRoomManager.instance.room.roomState = (RoomState)i;

		if (EditorRoomManager.instance.room.roomState == RoomState.Real) 
		{

			shadowToggle.interactable = false;

		} else {
			
			shadowToggle.interactable = true;

		}
			
	
	}




	public void SetShadowState(bool inShadow)
	{

		EditorRoomManager.instance.room.myMirrorRoom.inTheShadow = inShadow;
		EditorRoomManager.roomToLoad = JsonUtility.ToJson (EditorRoomManager.instance.room);

		Debug.Log (EditorRoomManager.roomToLoad);

		EditorRoomManager.loadRoomFromMemory = true;

		SceneManager.LoadScene(SceneManager.GetActiveScene().name);

	}







	// ----- ALERT ----- //



	public static void DisplayAlert(string textString, UnityEngine.Events.UnityAction action)
	{

		GameObject alertObject = Instantiate(Resources.Load<GameObject>("Prefabs/Editor/Alert"));

		Text alertText = alertObject.transform.Find ("Panel").Find ("Text").GetComponent<Text> ();
		alertText.text = textString;

		Button yesButton = alertObject.transform.Find ("Panel").Find ("YesButton").GetComponent<Button> ();
		Button noButton = alertObject.transform.Find ("Panel").Find ("NoButton").GetComponent<Button> ();

		yesButton.onClick.AddListener (action);
		yesButton.onClick.AddListener (() => Destroy(alertObject));
		noButton.onClick.AddListener (() => Destroy(alertObject));

	}





}
