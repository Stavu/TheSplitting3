﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubinteractionInspector : MonoBehaviour {


	// Declerations


	GameObject subinteractionPanelObject;

	Transform panel;
	Dropdown subIntTypeDropdown;
	InputField textInputBig;
	InputField textInputSmall;

	Transform moveToRoom;
	Transform recieveItem;
	Transform playAnimation;
	Transform playSound;

	Button cancelButton;
	Button submitButton;

	ISubinteractable subinteractable;
	SubInteraction currentSubint;
	List<string> subIntTypeList;



	// Use this for initialization

	void Start () 
	{
		subIntTypeList = new List<string> 
		{
			"showMonologue",
			"showDialogue",
			"showDialogueTree",
			"PlayAnimation",
			"PlaySound",
			"moveToRoom",
			"intoShadows",
			"outOfShadows",
			"pickUpItem",
			"useItem"
		};
	}


	
	// Update is called once per frame

	void Update () 
	{
		
	}



	// Create Subinteraction Panel


	public void CreateSubinteractionPanel(ISubinteractable iSubinteractable, SubInteraction subInt = null)
	{

		if (subinteractionPanelObject != null) 
		{
			return;
		}

		subinteractable = iSubinteractable;

		subinteractionPanelObject = Instantiate(Resources.Load<GameObject> ("Prefabs/Editor/InteractionPanelPrefabs/SubinteractionPanel"));

		panel = subinteractionPanelObject.transform.Find ("Panel");
		subIntTypeDropdown = panel.Find ("SubIntTypeDropdown").GetComponent<Dropdown> ();
		textInputBig = panel.Find ("TextInput").GetComponent<InputField> ();
		textInputSmall = panel.Find ("TextInputSmall").GetComponent<InputField> ();

		recieveItem = panel.Find ("RecieveItem");
		moveToRoom = panel.Find ("MoveToRoom");
		playAnimation = panel.Find ("PlayAnimation");
		playSound = panel.Find ("PlaySound");

		cancelButton = panel.Find ("CancelButton").GetComponent<Button> ();
		submitButton = panel.Find ("SubmitButton").GetComponent<Button> ();

		currentSubint = subInt;

		OpenSubinteractionPanel (currentSubint);

	}



	// OPEN //


	public void OpenSubinteractionPanel(SubInteraction subInt)
	{		

		Debug.Log ("open sub interaction panel");
		// SubInteraction type dropdown

		subIntTypeDropdown.AddOptions (subIntTypeList);

		if (subInt != null) {		
		
			// set dropdown value

			int i = subIntTypeList.IndexOf (subInt.interactionType);
			subIntTypeDropdown.value = i;

			// What's active?

			SetSubinteractionType (i);


			// Fill the active fields


			switch (subInt.interactionType) {

				case "showMonologue":

					textInputBig.text = subInt.RawText;

					break;


				case "showDialogue":

					textInputSmall.text = subInt.dialogueOptionTitle;

					break;


				case "showDialogueTree":

					textInputSmall.text = subInt.dialogueTreeName;

					break;


				case "PlaySound":

					playSound.Find("SoundNameInput").GetComponent<InputField>().text = subInt.soundToPlay;
					playSound.Find("NumberOfPlaysInput").GetComponent<InputField>().text = subInt.numberOfPlays.ToString();

					break;

				
				case "PlayAnimation":

					Debug.Log("play animation");
					// populating the animation dropdown according to the object's animations 

					List<string> animationList = new List<string> ();
					Furniture furn = InspectorManager.instance.chosenFurniture;

					GameObject prefab = Resources.Load<GameObject> ("Prefabs/Furniture/" + furn.myName);
					Debug.Log ("create prefab");

					if (prefab != null) 
					{		
						Debug.Log ("prefab isn't null");
						Animator animator = prefab.GetComponent<Animator> ();

						if (animator != null) 
						{	
							Debug.Log ("animator isn't null");
							foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips) 
							{
								animationList.Add (clip.name);
								Debug.Log ("animation list " + animationList.Count);
							}
						}
					}	

					playAnimation.Find ("AnimationDropdown").GetComponent<Dropdown> ().AddOptions (animationList);

					if ((currentSubint.animationToPlay != string.Empty) && (animationList.Contains(currentSubint.animationToPlay)))
					{
						playAnimation.Find ("AnimationDropdown").GetComponent<Dropdown> ().value = animationList.IndexOf (currentSubint.animationToPlay);
					}

					playAnimation.Find("FurnitureNameInput").GetComponent<InputField>().text = subInt.targetFurniture;

					break;


				case "moveToRoom":
									
					moveToRoom.Find ("TextInputSmall1").GetComponent<InputField> ().text = subInt.destinationRoomName;
					moveToRoom.Find ("InputX").GetComponent<InputField> ().text = subInt.entrancePoint.x.ToString();
					moveToRoom.Find ("InputY").GetComponent<InputField> ().text = subInt.entrancePoint.y.ToString();

					break;


				case "intoShadows":		


				case "outOfShadows":

					break;


				case "pickUpItem":

					recieveItem.Find ("TextInputSmall1").GetComponent<InputField> ().text = subInt.inventoryItem.fileName;
					recieveItem.Find ("TextInputSmall2").GetComponent<InputField> ().text = subInt.inventoryItem.titleName;

					break;


				case "useItem":
					
					break;

			}

		} else {


			// If subint is null, set first value on dropdown

			SetSubinteractionType (0);

		}


		// add listener to dropdown

		subIntTypeDropdown.onValueChanged.AddListener (SetSubinteractionType);


		// Cancel button

		cancelButton.onClick.AddListener(DestroySubinteractionInspector);


		// Submit button

		submitButton.onClick.AddListener (SubmitSubinteraction);



	}



	// Set subinteraction type - hide all fields, then decide what's active according to type

	public void SetSubinteractionType(int type)
	{
		
		textInputBig.gameObject.SetActive (false);
		textInputSmall.gameObject.SetActive (false);
		moveToRoom.gameObject.SetActive (false);
		recieveItem.gameObject.SetActive (false);
		playAnimation.gameObject.SetActive (false);
		playSound.gameObject.SetActive (false);

		string typeString = subIntTypeList [type];


		switch (typeString) 
		{
			
			case "showMonologue":

				textInputBig.gameObject.SetActive (true);

				break;


			case "showDialogue":

			case "showDialogueTree":

				textInputSmall.gameObject.SetActive (true);

				break;


			case "PlaySound":
				
				playSound.gameObject.SetActive (true);
			
				break;


			case "PlayAnimation":

				playAnimation.gameObject.SetActive (true);

				Debug.Log("play animation");
				// populating the animation dropdown according to the object's animations 

				List<string> animationList = new List<string> ();
				Furniture furn = InspectorManager.instance.chosenFurniture;

				GameObject prefab = Resources.Load<GameObject> ("Prefabs/Furniture/" + furn.myName);
				Debug.Log ("create prefab");

				if (prefab != null) 
				{		
					Debug.Log ("prefab isn't null");
					Animator animator = prefab.GetComponent<Animator> ();

					if (animator != null) 
					{	
						Debug.Log ("animator isn't null");
						foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips) 
						{
							animationList.Add (clip.name);
							Debug.Log ("animation list " + animationList.Count);
						}
					}
				}	

				playAnimation.Find ("AnimationDropdown").GetComponent<Dropdown> ().AddOptions (animationList);

				break;			


			case "moveToRoom":

				moveToRoom.gameObject.SetActive (true);

				break;


			case "intoShadows":

			case "outOfShadows":

				break;


			case "pickUpItem":

				recieveItem.gameObject.SetActive (true);

				break;

			case "useItem":

				break;

		}

	}




	// Cancel //

	public void DestroySubinteractionInspector()
	{
		if (subinteractionPanelObject != null) 
		{
			Destroy (subinteractionPanelObject);

		}
	}
		


	// Submit //

	public void SubmitSubinteraction()
	{


		if (currentSubint == null) 
		{
			Debug.Log ("SubmitSubinteraction: currentSubInt is null");

			int i = subIntTypeDropdown.value;
			string subIntType = subIntTypeList [i];

			currentSubint = new SubInteraction (subIntType);

			subinteractable.SubIntList.Add (currentSubint);

		} 


		// Reseting data fields, then filling them again

		currentSubint.ResetDataFields ();

		switch (currentSubint.interactionType) 
		{

			case "showMonologue":

				currentSubint.RawText = textInputBig.text;

				break;


			case "showDialogue":

				currentSubint.dialogueOptionTitle = textInputSmall.text;

				break;


			case "showDialogueTree":

				currentSubint.dialogueTreeName = textInputSmall.text;

				break;


			case "PlaySound":
				
				currentSubint.soundToPlay = playSound.Find ("SoundNameInput").GetComponent<InputField> ().text;

				string numberOfPlaysString = playSound.Find ("NumberOfPlaysInput").GetComponent<InputField> ().text;

				if ((numberOfPlaysString == null) || (numberOfPlaysString == string.Empty))
				{
					currentSubint.numberOfPlays = 1;
					break;
				}

				currentSubint.numberOfPlays = int.Parse(numberOfPlaysString);

				break;


			case "PlayAnimation":

				int i = playAnimation.Find ("AnimationDropdown").GetComponent<Dropdown> ().value;
				currentSubint.animationToPlay = playAnimation.Find ("AnimationDropdown").GetComponent<Dropdown> ().options [i].text;
				currentSubint.targetFurniture = playAnimation.Find ("FurnitureNameInput").GetComponent<InputField> ().text;
					
				break;


			case "moveToRoom":

				currentSubint.destinationRoomName = moveToRoom.Find ("TextInputSmall1").GetComponent<InputField> ().text;

				Vector2 entrancePoint = new Vector2 (int.Parse (moveToRoom.Find ("InputX").GetComponent<InputField> ().text),
					                      			 int.Parse (moveToRoom.Find ("InputY").GetComponent<InputField> ().text));

				currentSubint.entrancePoint = entrancePoint;

				break;


			case "intoShadows":

			case "outOfShadows":

				break;


			case "pickUpItem":
							
				string itemFileName = recieveItem.Find ("TextInputSmall1").GetComponent<InputField> ().text;
				string itemTitleName = recieveItem.Find ("TextInputSmall2").GetComponent<InputField> ().text;

				currentSubint.inventoryItem = new InventoryItem (itemFileName, itemTitleName);

				break;


			case "useItem":

				break;

		}


		EventsHandler.Invoke_cb_subinteractionChanged ();
		Destroy (subinteractionPanelObject);

	}


}
