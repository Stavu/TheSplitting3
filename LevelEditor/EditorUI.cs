using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;




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




	public GameObject bgSelectPrefab; 
	Dropdown dropDownMenu;
	public GameObject furnitureSelectPrefab;
	public GameObject furnitureButtonPrefab;

	GameObject furnitureSelect;


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

		bgSelectPrefab = Instantiate (bgSelectPrefab);
		dropDownMenu = bgSelectPrefab.GetComponentInChildren<Dropdown>();
		List<string> bgNameList = new List<string> ();
		Sprite[] bgSpriteList = Resources.LoadAll <Sprite> ("Sprites/Rooms/");


		foreach (Sprite spr in bgSpriteList) 
		{

			bgNameList.Add (spr.name);

		}

		dropDownMenu.AddOptions (bgNameList);

		dropDownMenu.onValueChanged.AddListener (ChangeRoomBg);



		// new furniture button

		Button furnButton = bgSelectPrefab.transform.FindChild ("FurnitureButton").GetComponent<Button>();
		furnButton.onClick.AddListener (CreateFurnitureSelect);


		// tile interaction button

		Button tileIntButton = bgSelectPrefab.transform.FindChild ("TileInteractionButton").GetComponent<Button>();
		tileIntButton.onClick.AddListener (SetTileInteractionMode);



		// room name button

		InputField roomNameInput = bgSelectPrefab.transform.FindChild ("RoomNameInput").GetComponent<InputField> ();

		roomNameInput.onEndEdit.AddListener (RoomNameChanged);

		if (EditorRoomManager.instance.room.myName != null) 
		{

			roomNameInput.text = EditorRoomManager.instance.room.myName;
		}


	}




	public void ChangeRoomBg(int optionNum)
	{

		string spriteName = dropDownMenu.options [optionNum].text;

		EditorRoomManager.instance.SetRoomBackground (spriteName);

	}




	// Adding Furniture



	public void CreateFurnitureSelect()
	{

		furnitureSelect = Instantiate (furnitureSelectPrefab);

		GameObject content = furnitureSelect.GetComponentInChildren<GridLayoutGroup> ().gameObject;
				
		Sprite[] furnitureSpriteList = Resources.LoadAll<Sprite> ("Sprites/Furniture/");

		foreach (Sprite sprite in furnitureSpriteList) 
		{

			GameObject button = Instantiate (furnitureButtonPrefab);
			button.transform.SetParent (content.transform);

			button.GetComponent<Image> ().sprite = sprite;
			button.GetComponentInChildren<Text> ().text = sprite.name;

			button.GetComponent<Button> ().onClick.AddListener (() => FurnitureImageSelected(sprite.name));


		}


	}



	public void FurnitureImageSelected(string furnitureName)
	{

		BuildController.instance.furnitureName = furnitureName;

		Destroy (furnitureSelect);


		// set to build Furniture mode

		BuildController.instance.mode = BuildController.Mode.buildFurniture;


	}



	public void SetTileInteractionMode()
	{

		// set to build tileInteraction mode

		BuildController.instance.mode = BuildController.Mode.buildTileInteraction;


	}




	public void RoomNameChanged(string name)
	{

		EditorRoomManager.instance.room.myName = name;



	}


}
