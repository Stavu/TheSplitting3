﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class DialogueManager : MonoBehaviour {



	// Singleton //

	public static DialogueManager instance {get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //


	public GameObject dialogueTreeBoxPrefab;
	public GameObject dialogueOptionPrefab;

	public GameObject dialogueTreeObject;

	public DialogueTree currentDialogueTree;
	int nextSentence;


	DialogueOption _currentDialogueOption;

	public DialogueOption currentDialogueOption
	{

		get {return _currentDialogueOption;} 
		set {

			if (_currentDialogueOption != null) 
			{
				myOptionObjectDictionary [_currentDialogueOption].transform.GetChild(0).gameObject.SetActive (false);
				myOptionObjectDictionary [_currentDialogueOption].GetComponent<Text> ().color = Color.white;

			}

			_currentDialogueOption = value;

			if (_currentDialogueOption != null) 
			{		
				// Arrow

				myOptionObjectDictionary [_currentDialogueOption].transform.GetChild (0).gameObject.SetActive (true);
				myOptionObjectDictionary [_currentDialogueOption].GetComponent<Text> ().color = new Color (0.1f, 0.8f, 0.8f, 1f);

				//myOptionObjectDictionary [_currentDialogueOption].transform.GetChild (0).GetComponent<Image> ().color = myOptionObjectDictionary [_currentDialogueOption].GetComponent<Text> ().color;

			}

		} 
	}


	Dictionary<DialogueOption,GameObject> myOptionObjectDictionary;




	// Use this for initialization

	public void Initialize () 
	{

		EventsHandler.cb_keyPressedDown += BrowseDialogueOptions;
		//EventsHandler.cb_spacebarPressed += ActivateDialogueOption;


	}


	public void OnDestroy () 
	{

		EventsHandler.cb_keyPressedDown -= BrowseDialogueOptions;
		//EventsHandler.cb_spacebarPressed -= ActivateDialogueOption;

	}


	
	// Update is called once per frame

	void Update () 
	{

		if (Input.GetKeyDown (KeyCode.T)) 
		{

			ActivateDialogueTree(new DialogueTree());

		}

		if (Input.GetKeyDown (KeyCode.Escape)) 
		{

			DestroyDialogueTree ();

		}

	}



	// Activate dialogue tree

	public void ActivateDialogueTree(DialogueTree dialogueTree)
	{
		
		currentDialogueTree = dialogueTree;

		CreateDialogueTreeUI ();

		Debug.Log (JsonUtility.ToJson (currentDialogueTree));


	}



	public void SetConversation(string conversationName)
	{

		currentDialogueTree.currentConversation = currentDialogueTree.GetConversationByName (conversationName);


	}



	// Create Dialogue Tree

	public void CreateDialogueTreeUI()
	{
		
		DestroyDialogueTree ();

		if (myOptionObjectDictionary != null) 
		{
			foreach (GameObject obj in myOptionObjectDictionary.Values) 
			{
				Destroy (obj);
			}
		}

		myOptionObjectDictionary = new Dictionary<DialogueOption, GameObject> ();



		GameManager.instance.inputState = InputState.DialogueBox;

		dialogueTreeObject = Instantiate (dialogueTreeBoxPrefab);

		for (int i = 0; i < currentDialogueTree.currentConversation.optionList.Count; i++) 
		{

			DialogueOption option = currentDialogueTree.currentConversation.optionList [i];
			GameObject optionObj = Instantiate (dialogueOptionPrefab);
			optionObj.transform.SetParent (dialogueTreeObject.transform.FindChild ("Box"));	
			optionObj.transform.localScale = Vector3.one;

			optionObj.GetComponent<Text>().text = option.myTitle;

			myOptionObjectDictionary.Add (option, optionObj);
				
		}


		if (currentDialogueOption == null) 
		{
			currentDialogueOption = currentDialogueTree.currentConversation.optionList[0];
		}


	}





	public void BrowseDialogueOptions(Direction myDirection)
	{

		if (GameManager.instance.inputState != InputState.DialogueBox) 
		{
			return;
		}

		if (dialogueTreeObject == null) 		
		{			
			return;
		}


		int i =	currentDialogueTree.currentConversation.optionList.IndexOf (currentDialogueOption);


		switch (myDirection) 
		{

			case Direction.down:

				if (i < currentDialogueTree.currentConversation.optionList.Count - 1) 
				{
					currentDialogueOption = currentDialogueTree.currentConversation.optionList [i + 1];
				}

				if (i == currentDialogueTree.currentConversation.optionList.Count - 1) 
				{
					currentDialogueOption = currentDialogueTree.currentConversation.optionList [0];
				}


				break;



			case Direction.up:

				if (i > 0) 
				{
					currentDialogueOption = currentDialogueTree.currentConversation.optionList [i - 1];
				}

				if (i == 0) 
				{
					currentDialogueOption = currentDialogueTree.currentConversation.optionList [currentDialogueTree.currentConversation.optionList.Count - 1];
				}

				break;

		}

	}




	// Activate option //


	public void ActivateDialogueOption ()
	{
		
		Debug.Log ("ActivateDialogueOption");

		if (currentDialogueOption == null) 
		{
		
			Debug.Log ("option is null");
		
		}

		Debug.Log ("count" + currentDialogueOption.sentenceList.Count);


		SetDialogueTreeActive (false);
		InteractionManager.instance.DisplayText (currentDialogueOption.sentenceList);

	}





	// HIDE //


	public void SetDialogueTreeActive(bool isActive)
	{
		
		if (dialogueTreeObject != null) 
		{
			dialogueTreeObject.SetActive (isActive);

		}

		if (isActive == true) 
		{
			GameManager.instance.inputState = InputState.DialogueBox;

		} else {
			
			GameManager.instance.inputState = InputState.Dialogue;
		}

	}



	// DESTROY //


	public void DestroyDialogueTree()
	{
		Debug.Log ("DestroyDialogueTree");

		if (dialogueTreeObject != null) 
		{
			Debug.Log ("Destroy");

			Destroy (dialogueTreeObject);
			currentDialogueOption = null;
			dialogueTreeObject = null;

			GameManager.instance.inputState = InputState.Character;
		
		}

	}






}
