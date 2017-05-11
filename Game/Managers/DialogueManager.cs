using System.Collections;
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

	GameObject dialogueTreeObject;

	DialogueTree currentDialogueTree;
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


	}


	public void OnDestroy () 
	{

		EventsHandler.cb_keyPressedDown -= BrowseDialogueOptions;

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

		if (currentDialogueOption == null) 
		{
			return;
		}


		DestroyDialogueTree ();


	}



	// Display Dialogue


	public void DisplayDialogue(List<DialogueSentence> sentenceList)
	{




	}



	// Managing the conversation

	public void Converse()
	{

		DialogueSentence sentence;

		if (nextSentence >= currentDialogueOption.sentenceList.Count) 
		{			
			FinishDialogueOption ();
			return;
		
		} 

		sentence = currentDialogueOption.sentenceList [nextSentence];

		InteractionManager.instance.DisplayText (sentence.speakerName, sentence.myText);


	}




	public void FinishDialogueOption()
	{






	}


	// DESTROY //


	public void DestroyDialogueTree()
	{

		if (dialogueTreeObject != null) 
		{
		
			Destroy (dialogueTreeObject);
			currentDialogueOption = null;
			GameManager.instance.inputState = InputState.Character;
		
		}

	}






}
