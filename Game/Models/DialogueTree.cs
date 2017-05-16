using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


[Serializable]
public class DialogueTree {


	public string myName;
	public List<Conversation> conversationList;

	[NonSerialized]
	public Conversation currentConversation;


	public DialogueTree()
	{
		// test

		myName = "testTree";
		Conversation conversation1 = new Conversation ();
		currentConversation = conversation1;

		conversationList = new List<Conversation> ();
		conversationList.Add (conversation1);

	}



	// Find conversation by name

	public Conversation GetConversationByName (string conversationName)
	{

		foreach (Conversation conversation in conversationList) 
		{
			if (conversation.myName == conversationName) 
			{
				return conversation;
			}			
		}

		Debug.LogError ("Can't find conversation");
		return null;

	}

}



// Conversation - What the conversation is about


[Serializable]
public class Conversation {


	public string myName;
	public List<DialogueOption> optionList;


	// Constructor

	public Conversation()
	{
		// test

		myName = "testConversation";

		optionList = new List<DialogueOption> ();
		DialogueOption option1 = new DialogueOption ("How are you?");
		DialogueOption option2 = new DialogueOption ("My name is Daniel.");
		optionList.Add (option1);
		optionList.Add (option2);

	}

}


// Dialogue option

[Serializable]
public class DialogueOption {


	public string myTitle;

	public List<DialogueSentence> sentenceList;
	public Condition myCondition;



	// Constructor

	public DialogueOption(string title)
	{

		myTitle = title;

		sentenceList = new List<DialogueSentence> ();

		SubInteraction subInt = new SubInteraction ("pickUpItem");
		subInt.inventoryItem = new InventoryItem ("order_details", "Order Details");
		List<SubInteraction> subIntList = new List<SubInteraction> ();
		subIntList.Add (subInt);

		SubInteraction subInt2 = new SubInteraction ("endDialogueTree");
		List<SubInteraction> subIntList2 = new List<SubInteraction> ();
		subIntList2.Add (subInt2);


		DialogueSentence sentence1 = new DialogueSentence ("Daniel", "Hello, how are you?", false);
		DialogueSentence sentence2 = new DialogueSentence ("llehctiM", "fine, thank you.", true, subIntList);
		DialogueSentence sentence3 = new DialogueSentence ("Daniel", "thank you!", false, subIntList2);

		sentenceList.Add (sentence1);
		sentenceList.Add (sentence2);
		sentenceList.Add (sentence3);

	}

}





// Dialogue Sentence

[Serializable]
public class DialogueSentence : ISubinteractable{


	public string speakerName;
	public string myText;
	public bool subinteractImmediately = false;
	public List<SubInteraction> mySubIntList;

	public List<SubInteraction> SubIntList 
	{
		get
		{ 
			return mySubIntList;
		}

		set 
		{
			mySubIntList = value;
		}
	}


	// Constructor

	public DialogueSentence(string speaker, string text, bool subIntIm, List<SubInteraction> subIntList = null)
	{

		speakerName = speaker;
		myText = text;
		mySubIntList = subIntList;
		subinteractImmediately = subIntIm;

	}


}