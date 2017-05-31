﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Utilities {


	public static int myVar;
	public int myVar2;
	int myVar3; 




	public static void AdjustOrthographicCamera(Room room)
	{

		Camera.main.orthographicSize = 12; 

		Camera.main.transform.position = new Vector3 ((room.myWidth / 2), (room.myHeight / 2), -10);


	}


	public static Vector2 GetObjectScreenPosition (Vector3 myPos, RectTransform myTransform, float offsetX, float offsetY)
	{

		Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(myPos);
		Vector2 WorldObject_ScreenPosition = new Vector2
			(
				((ViewportPosition.x * myTransform.sizeDelta.x) - (myTransform.sizeDelta.x*0.5f)) + offsetX,
				((ViewportPosition.y * myTransform.sizeDelta.y) - (myTransform.sizeDelta.y*0.5f)) + offsetY
			);

		return WorldObject_ScreenPosition;

	}


	public static List<string> SeparateText(string rawText)
	{

		if(rawText == string.Empty)
		{
			return null;
		}
	
		List<string> textList = new List<string> ();

		char delimiter = '|';
		string[] result = rawText.Split (delimiter);

		foreach (string str in result)
		{
			textList.Add (str);
		}

		//Debug.Log (textList);


		return textList;



	}



	public static GameObject CreateFurnitureGameObject (Furniture myFurniture, Transform parent)
	{

		//Debug.Log ("Assign Furniture Image");

		GameObject[] animatedObjects = Resources.LoadAll<GameObject> ("Prefabs/Furniture");


		myFurniture.myPos = new Vector3 (myFurniture.x + myFurniture.mySize.x/2, myFurniture.y, 0);

		GameObject obj = null;
		SpriteRenderer sr = null;

		foreach (GameObject gameObj in animatedObjects) 
		{
			if (gameObj.name == myFurniture.myName) 
			{
				obj = GameObject.Instantiate (gameObj);
				obj.transform.SetParent (parent);

				sr = obj.GetComponentInChildren<SpriteRenderer>();

				break;
			}	
		}

		if (obj == null) 
		{
			obj = new GameObject (myFurniture.myName);
			obj.transform.SetParent (parent);

			GameObject childObj = new GameObject ("Image");
			childObj.transform.SetParent (obj.transform);

			sr = childObj.AddComponent<SpriteRenderer>();
			sr.sprite = Resources.Load<Sprite> ("Sprites/Furniture/" + myFurniture.myName); 
			sr.flipX = myFurniture.imageFlipped;
		}



		if (myFurniture.frameExtents == Vector2.zero) 
		{
			Debug.Log ("frameExtents " + myFurniture.frameExtents);
			myFurniture.frameExtents = sr.sprite.bounds.extents;
		}
			

		obj.transform.position = new Vector3 (myFurniture.myPos.x + myFurniture.offsetX, myFurniture.myPos.y + 0.5f + myFurniture.offsetY, myFurniture.myPos.z);
		Debug.Log (obj.transform.position);


		//Debug.Log ("object position" + myFurniture.myName + obj.transform.position + sr.sprite.bounds);


		// sorting order 

		sr.sortingOrder = -myFurniture.y * 10;

		if (myFurniture.walkable == true) 
		{
			sr.sortingOrder = (int) -(myFurniture.y + myFurniture.mySize.y) * 10;

		}

		sr.sortingLayerName = Constants.furniture_character_layer;

		return obj;

	}





	public static void SetPISortingOrder(PhysicalInteractable myPI, GameObject obj)
	{

		// sorting order 

		SpriteRenderer[] sr_list = obj.GetComponentsInChildren<SpriteRenderer> ();
		Debug.Log ("length" + obj.transform.childCount);


		for (int i = 0; i < obj.transform.childCount; i++)
		{
			obj.transform.GetChild (i).GetComponent<SpriteRenderer> ().sortingOrder = i + (-myPI.y * 10);			
		}

	}




	public static GameObject CreateCharacterGameObject (Character myCharacter, Transform parent)
	{


		//Debug.Log ("Assign Furniture Image");

		myCharacter.myPos = new Vector3 (myCharacter.x + myCharacter.mySize.x/2, myCharacter.y, 0);

		GameObject obj = GameObject.Instantiate(Resources.Load<GameObject> ("Prefabs/Characters/" + myCharacter.myName)); 			
		obj.transform.SetParent (parent);


		obj.transform.position = new Vector3 (myCharacter.myPos.x + myCharacter.offsetX, myCharacter.myPos.y + 0.5f + myCharacter.offsetY, myCharacter.myPos.z);

		//Debug.Log ("object position" + myFurniture.myName + obj.transform.position + sr.sprite.bounds);


		// sorting order 

		obj.GetComponentInChildren<SpriteRenderer>().sortingOrder  = -myCharacter.y * 10;
		obj.GetComponentInChildren<SpriteRenderer>().sortingLayerName = Constants.furniture_character_layer;

		return obj;


	}



	public static Vector2 GetCharacterPosOnTile(IWalker character, Vector2 myPos)
	{	
		
		Vector2 pos = new Vector2 (myPos.x + character.speakerSize.x / 2 + character.walkerOffsetX, myPos.y + character.walkerOffsetY + 0.5f);

		return pos;

	}




	// When it's the player	


	public static List<DialogueSentence> CreateSentenceList (ISpeaker speaker, List<string> textlist)
	{

		List<DialogueSentence> sentenceList = new List<DialogueSentence> ();


		foreach (string str in textlist) 
		{
			DialogueSentence tempSentence = new DialogueSentence (speaker.speakerName, str, false);
			sentenceList.Add (tempSentence);
		}

		return sentenceList;

	}



	public static List<DialogueSentence> CreateSentenceList (ISpeaker speaker, string text)
	{

		List<DialogueSentence> sentenceList = new List<DialogueSentence> ();

		DialogueSentence tempSentence = new DialogueSentence (speaker.speakerName, text, false);
		sentenceList.Add (tempSentence);

		return sentenceList;

	}


	public static bool EvaluateConditions(List<Condition> conditionList)
	{

		if (conditionList.Count == 0) 
		{
			return true;
		}


		bool evaluation = true;

		foreach (Condition cond in conditionList) 
		{
			
			evaluation = cond.EvaluateCondition ();

		}

		return evaluation;


	}


	// FADE IN AND OUT - from shadow to mirror and vice versa //


	public static IEnumerator FadeBetweenSprites(List<SpriteRenderer> fadeOutSprites, List<SpriteRenderer> fadeInSprites)
	{
		
		float speed = 1f;
		float inter = 0;

		while(inter < 1)
		{
			foreach (SpriteRenderer sr in fadeOutSprites) 
			{
				sr.color = new Color (1,1,1,1-inter);
			}

			foreach (SpriteRenderer sr in fadeInSprites) 
			{
				sr.color = new Color (1,1,1,inter);
			}

			// speed - how many seconds it will take 

			inter += Time.deltaTime / speed;
		
			yield return new WaitForFixedUpdate ();
		
		}

		SwitchBetweenSprites (fadeOutSprites, fadeInSprites);
	}




	public static void SwitchBetweenSprites(List<SpriteRenderer> outSprites, List<SpriteRenderer> inSprites)
	{

		foreach (SpriteRenderer sr in outSprites) 
		{
			sr.color = Color.clear;
		}

		foreach (SpriteRenderer sr in inSprites) 
		{
			sr.color = Color.white;
		}

	}


	/// <summary>
	/// Gets the passed subinteractions.
	/// Returns a list of the subinteractions that passed the conditions
	/// </summary>
	/// <returns>The passed subinteractions.</returns>
	/// <param name="subIntList">Sub int list.</param>

	public static List<SubInteraction> GetPassedSubinteractions (List<SubInteraction> subIntList)
	{		
		// check if subinteractions passed the conditions

		List<SubInteraction> subinteractionsToDo = new List<SubInteraction> ();

		foreach (SubInteraction subInt in subIntList) 
		{
			bool passedConditions = Utilities.EvaluateConditions (subInt.conditionList);

			if (passedConditions == true) 
			{
				subinteractionsToDo.Add (subInt);
			}
		}

		return subinteractionsToDo;
	}




	public static List<Vector3> GetPhysicalInteractableFrameBounds(PhysicalInteractable myPhysicalInt)
	{
		// declerations 

		Vector2 frameBounds = myPhysicalInt.frameExtents;

		List<Vector3> positions = new List<Vector3> ();

		if (myPhysicalInt is Furniture) 		
		{

			Furniture myFurniture = (Furniture)myPhysicalInt;

			SpriteRenderer sr = FurnitureManager.instance.furnitureGameObjectMap [myFurniture].GetComponentInChildren<SpriteRenderer>();
			Vector3 center = sr.bounds.center + new Vector3 (myFurniture.frameOffsetX, myFurniture.frameOffsetY, 0);

			// center 

			positions.Add(center);

			// positioning frame pieces

			if (frameBounds == Vector2.zero) 
			{
				frameBounds = sr.bounds.extents;
			}			

		}


		if (myPhysicalInt is Character) 		
		{
			Character myCharacter = (Character)myPhysicalInt;

			GameObject myObject = CharacterManager.instance.characterGameObjectMap [myCharacter];
			Vector3 center = myObject.GetComponentInChildren<SpriteRenderer> ().bounds.center  + new Vector3 (myCharacter.frameOffsetX, myCharacter.frameOffsetY, 0);

			// center 
			positions.Add(center);

			// positioning frame pieces

			if (frameBounds == Vector2.zero) 
			{
				frameBounds = myObject.GetComponentInChildren<SpriteRenderer> ().bounds.extents;
			}
		}


		//down left
		positions.Add(new Vector3 (-frameBounds.x, -frameBounds.y,0));

		//down right
		positions.Add(new Vector3 (frameBounds.x, -frameBounds.y,0));

		//up left
		positions.Add(new Vector3 (-frameBounds.x, frameBounds.y,0));

		//up right
		positions.Add(new Vector3 (frameBounds.x, frameBounds.y,0));



		return positions;
	}





	public static List<Vector3> EditorGetPhysicalInteractableFrameBounds(PhysicalInteractable myPhysicalInt)
	{
		// declerations 

		Vector2 frameBounds = myPhysicalInt.frameExtents;

		List<Vector3> positions = new List<Vector3> ();

		if (myPhysicalInt is Furniture) 		
		{

			Furniture myFurniture = (Furniture)myPhysicalInt;


			SpriteRenderer sr = EditorRoomManager.instance.furnitureGameObjectMap[myFurniture].GetComponentInChildren<SpriteRenderer>();

			if (sr == null) 
			{
				Debug.Log ("sr is null");
			}



			Vector3 center = sr.bounds.center;

			// center 

			positions.Add(center);

			// positioning frame pieces

			if (frameBounds == Vector2.zero) 
			{
				frameBounds = sr.bounds.extents;
			}			

		}


		if (myPhysicalInt is Character) 		
		{
			Character myCharacter = (Character)myPhysicalInt;

			GameObject myObject = EditorRoomManager.instance.characterGameObjectMap [myCharacter];
			Vector3 center = myObject.GetComponentInChildren<SpriteRenderer> ().bounds.center;

			// center 
			positions.Add(center);

			// positioning frame pieces

			if (frameBounds == Vector2.zero) 
			{
				frameBounds = myObject.GetComponentInChildren<SpriteRenderer> ().bounds.extents;
			}
		}


		//down left
		positions.Add(new Vector3 (-frameBounds.x, -frameBounds.y,0));

		//down right
		positions.Add(new Vector3 (frameBounds.x, -frameBounds.y,0));

		//up right
		positions.Add(new Vector3 (frameBounds.x, frameBounds.y,0));

		//up left
		positions.Add(new Vector3 (-frameBounds.x, frameBounds.y,0));



		return positions;
	}


}

