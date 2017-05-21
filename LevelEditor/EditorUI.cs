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



	Dropdown dropDownMenu;
	public GameObject interactableSelectPrefab;
	public GameObject interactableButtonPrefab;

	GameObject interactableSelect;





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

		//bgSelectPrefab = Instantiate (bgSelectPrefab);
		dropDownMenu = gameObject.GetComponentInChildren<Dropdown>();
		List<string> bgNameList = new List<string> ();
		Sprite[] bgSpriteList = Resources.LoadAll <Sprite> ("Sprites/Rooms/");


		foreach (Sprite spr in bgSpriteList) 
		{

			bgNameList.Add (spr.name);

		}

		dropDownMenu.AddOptions (bgNameList);
		dropDownMenu.onValueChanged.AddListener (ChangeRoomBg);


		// Go to game button

		Button toGameButton = transform.Find ("ToGameButton").GetComponent<Button> ();
		toGameButton.onClick.AddListener(() => SceneManager.LoadScene("Main"));


		// new furniture button

		Button furnButton = transform.Find ("FurnitureButton").GetComponent<Button>();
		furnButton.onClick.AddListener (CreateFurnitureSelect);


		// new character button

		Button characterButton = transform.Find ("CharacterButton").GetComponent<Button>();
		characterButton.onClick.AddListener (CreateCharacterSelect);


		// tile interaction button

		Button tileIntButton = transform.Find ("TileInteractionButton").GetComponent<Button>();
		tileIntButton.onClick.AddListener (SetTileInteractionMode);


		// flip room button

		Button flipRoomButton = transform.Find ("FlipRoomButton").GetComponent<Button>();
		flipRoomButton.onClick.AddListener (FlipRoom);


		// room state dropdown

		Dropdown roomStateDropdown = transform.Find ("RoomStateDropdown").GetComponent<Dropdown>();
		roomStateDropdown.onValueChanged.AddListener (SetRoomState);

		roomStateDropdown.value = (int)EditorRoomManager.instance.room.roomState;



		// room name button

		InputField roomNameInput = transform.Find ("RoomNameInput").GetComponent<InputField> ();

		roomNameInput.onEndEdit.AddListener (RoomNameChanged);

		if (EditorRoomManager.instance.room.myName != null) 
		{

			roomNameInput.text = EditorRoomManager.instance.room.myName;
		}


	}




	public void ChangeRoomBg(int optionNum)
	{

		Debug.Log ("change room bg");

		string spriteName = dropDownMenu.options [optionNum].text;

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





	public void FlipRoom ()
	{


		Room newRoom = EditorRoomManager.instance.CreateFlippedRoom (EditorRoomManager.instance.room);

		string roomString = JsonUtility.ToJson (newRoom);

		// Load room

		EditorRoomManager.roomToLoad = roomString;
		EditorRoomManager.loadRoomFromMemory = true;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);


	}


	public void SetRoomState(int i)
	{

		EditorRoomManager.instance.room.roomState = (RoomState)i;




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
