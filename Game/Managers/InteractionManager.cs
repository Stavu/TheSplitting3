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

	GameObject currentTextBox;




	// Use this for initialization

	public void Initialize () {

		EventsHandler.cb_escapePressed += CloseTextBox;
		
	}

	public void OnDestroy()
	{

		EventsHandler.cb_escapePressed -= CloseTextBox;

	}


	// Update is called once per frame
	void Update () {
		
	}


	// text

	public void DisplayText(Player speaker, string text)
	{

		OpenTextBox (speaker);
		InsertText (text);

	}


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
		
		//Vector2 textBoxPos = Camera.main.WorldToViewportPoint (speaker.myPos);
	
		int offsetX = 0;
		int offsetY = 1;


		Vector2 newPos = new Vector2 (speaker.myPos.x + offsetX, speaker.myPos.y + offsetY);


		/*
		//this is the ui element

		//RectTransform UI_Element = currentTextBox.transform.FindChild ("Image");


		//first you need the RectTransform component of your canvas

		RectTransform CanvasRect = currentTextBox.GetComponent<RectTransform>();


		//then you calculate the position of the UI element
		//0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

		Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(speaker.myPos);
		Vector2 WorldObject_ScreenPosition = new Vector2
			(
			((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x*0.5f)) + xOffset,
			((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y*0.5f)) + yOffset
			);
		*/

		//now you can set the position of the ui element

		return newPos;

	}



	public void CloseTextBox ()
	{

		if (currentTextBox != null) {

			Destroy (currentTextBox.gameObject);

			GameManager.textBoxActive = false;
			currentTextBox = null;

		}

	}





	// move to room


	public void MoveToRoom(string roomName, Direction direction)
	{
	
		GameManager.roomToLoad = GameManager.instance.stringRoomMap [roomName];

		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	
	}




}
