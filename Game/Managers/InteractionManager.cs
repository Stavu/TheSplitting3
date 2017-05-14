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


		
	}

	public void OnDestroy()
	{

	

	}


	// Update is called once per frame

	void Update () 
	{
				
	}


	// ------  TEXT ------ //


	public void DisplayText(List<DialogueSentence> sentenceList)
	{

		if (currentTextBox != null) 		
		{			
			return;
		}

		currentTextBox = Instantiate (TextBoxPrefab);
		DialogueTextObject textObject = currentTextBox.AddComponent<DialogueTextObject> ();
		textObject.AddTextList (sentenceList);

		GameManager.instance.inputState = InputState.Dialogue;

	
	}

	/*
	// Change conversation


	public void ChangeConversation(string conversationName)
	{
		
	
	}
	*/

	// inventory text


	public void DisplayInventoryText(List<string> stringList)
	{
		
		// checking if one already exists

		InventoryTextObject tempObj = GameObject.FindObjectOfType<InventoryTextObject> ();
	
		if (tempObj != null) 
		{
			return;
		}


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


	public void OpenInventory_UseItem (PhysicalInteractable physicalInt)
	{

		InventoryUI.instance.OpenInventory (InventoryState.UseItem);

	}


	// Combine item (we have a current item, but need to chose an item to combine it with


	public void OpenInventory_CombineItem ()
	{

		InventoryUI.instance.OpenInventory (InventoryState.Combine);

	}


}
