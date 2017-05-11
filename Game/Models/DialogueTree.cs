using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DialogueTree {


	string myName;
	public List<Conversation> conversationList;
	public Conversation currentConversation;


	public DialogueTree()
	{
		// test

		myName = "testTree";
		Conversation conversation1 = new Conversation ();
		currentConversation = conversation1;

		conversationList = new List<Conversation> ();

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

public class DialogueOption {


	public string myTitle;

	public List<DialogueSentence> sentenceList;
	public Condition myCondition;


	// Constructor

	public DialogueOption(string title)
	{

		myTitle = title;

		sentenceList = new List<DialogueSentence> ();

		DialogueSentence sentence1 = new DialogueSentence ("Daniel", "Hello, how are you?");
		DialogueSentence sentence2 = new DialogueSentence ("llehctiM", "fine, thank you.");

		sentenceList.Add (sentence1);
		sentenceList.Add (sentence2);


	}

}





// Dialogue Sentence

public class DialogueSentence {


	public string speakerName;
	public string myText;


	// Constructor

	public DialogueSentence(string speaker, string text)
	{

		speakerName = speaker;
		myText = text;


	}




}