using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class InteractionManager : MonoBehaviour {



	// Singleton //

	public static InteractionManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //



	public GameObject TextBoxPrefab;
	public GameObject currentTextBox;






	// Use this for initialization

	public void Initialize () {

		EventsHandler.cb_escapePressed += CloseTextBox;
		
	}

	public void OnDestroy()
	{

		EventsHandler.cb_escapePressed -= CloseTextBox;

	}


	// Update is called once per frame

	void Update () 
	{
		
		
	}


	// ------  TEXT ------ //

	public void DisplayText(Player speaker, string text)
	{

		OpenTextBox (speaker);
		InsertText (text);
		GameManager.instance.inputState = InputState.Dialogue;

	}



	// Opening the text box

	public void OpenTextBox(Player speaker)
	{
		
		if (currentTextBox != null) 		
		{			
			return;
		}

		currentTextBox = Instantiate (TextBoxPrefab);
		currentTextBox.GetComponent<RectTransform> ().anchoredPosition = PositionTextBox (speaker);

	}



	public void InsertText(string text)
	{

		currentTextBox.GetComponentInChildren<Text> ().text = text;
	}



	Vector2 PositionTextBox(Player speaker)
	{
		
	
		int offsetX = 0;
		int offsetY = 1;


		Vector2 newPos = new Vector2 (speaker.myPos.x + offsetX, speaker.myPos.y + offsetY);


		//now you can set the position of the ui element

		return newPos;

	}



	public void CloseTextBox ()
	{

		if (currentTextBox != null) {

			Destroy (currentTextBox.gameObject);

			GameManager.textBoxActive = false;
			currentTextBox = null;

			GameManager.instance.inputState = InputState.Character;

		}

	}



	// inventory text


	public void DisplayInventoryText(List<string> stringList)
	{

		// open inventory text box

		GameObject obj = Instantiate (Resources.Load<GameObject>("Prefabs/InventoryTextBox"));
		obj.GetComponent<InventoryTextObject> ().AddTextList (stringList);

	}



	// move to room


	public void MoveToRoom(string roomName, Direction direction)
	{
	
		GameManager.roomToLoad = GameManager.instance.stringRoomMap [roomName];

		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	
	}





	// Pick up item


	public void PickUpItem (InventoryItem inventoryItem)
	{

		//Debug.Log ("PickUpItem");

		GameManager.playerData.inventory.AddItem (inventoryItem);
			

	}



	// Use item (we still don't know what item it is)


	public void UseItem (Furniture furn)
	{

		InventoryUI.instance.OpenInventory (InventoryState.UseItem);

	}





}
