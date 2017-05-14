using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTextObject : MonoBehaviour {


	public List<DialogueSentence> sentenceList;
	int currentEntry;
	Text textComponent;


	// Use this for initialization

	public void AddTextList (List<DialogueSentence> list) 
	{

		textComponent = gameObject.transform.FindChild ("Image").FindChild ("Text").GetComponent<Text> ();

		this.sentenceList = list;
	
		PopulateTextBox (sentenceList [0]);

		//currentTextBox.GetComponent<RectTransform> ().anchoredPosition = PositionTextBox (speaker);

	}



	// Update is called once per frame

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Space)) 
		{
			
			if ((sentenceList [currentEntry].mySubIntList != null) && (sentenceList[currentEntry].subinteractImmediately == false))
			{
				foreach (SubInteraction subInt in sentenceList [currentEntry].mySubIntList) 
				{					
					subInt.SubInteract ();
				}

			}

			NextEntry ();
		}	

	}


	// Show next entry, and if you ran out of entries, destroy

	public void NextEntry()
	{
		currentEntry++;

		if (currentEntry < sentenceList.Count) 
		{
			if ((sentenceList [currentEntry].mySubIntList != null) && (sentenceList[currentEntry].subinteractImmediately == true))
			{
				foreach (SubInteraction subInt in sentenceList [currentEntry].mySubIntList) 
				{					
					subInt.SubInteract ();
				}

			}

			PopulateTextBox (sentenceList [currentEntry]);

		} else {

			CloseTextBox ();

		}
	}



	public void CloseTextBox ()
	{			

		GameManager.textBoxActive = false;
		InteractionManager.instance.currentTextBox = null;

		if (DialogueManager.instance.dialogueTreeObject != null) 
		{
			Debug.Log ("close text box");
			DialogueManager.instance.SetDialogueTreeActive (true);
			//GameManager.instance.inputState = InputState.DialogueBox;

		} else {

			GameManager.instance.inputState = InputState.Character;	

		}

		Destroy (gameObject);
	
	}



	public Vector3 PositionTextBox(ISpeaker speaker)
	{		


		if (speaker == null) 
		{
			Debug.Log("speaker is null");

		}

		int offsetX = 0;
		int offsetY = 5;

		Vector3 newPos = new Vector3 (speaker.speakerPos.x + offsetX, speaker.speakerPos.y + offsetY,0);


		//now you can set the position of the ui element

		return newPos;

	}




	public void PopulateTextBox(DialogueSentence sentence)
	{

		textComponent.text = sentence.myText;

		ISpeaker speaker = RoomManager.instance.nameSpeakerMap [sentence.speakerName];
		textComponent.color = speaker.speakerTextColor;
		transform.position = PositionTextBox (speaker);



	}

}
