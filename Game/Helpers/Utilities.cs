using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

		myFurniture.myPos = new Vector3 (myFurniture.x + myFurniture.mySize.x/2, myFurniture.y, 0);

		GameObject obj = new GameObject (myFurniture.myName);
		obj.transform.SetParent (parent);


		SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();

		sr.sprite = Resources.Load<Sprite> ("Sprites/Furniture/" + myFurniture.myName); 

		obj.transform.position = new Vector3 (myFurniture.myPos.x + myFurniture.offsetX, myFurniture.myPos.y + 0.5f + myFurniture.offsetY, myFurniture.myPos.z);

		//Debug.Log ("object position" + myFurniture.myName + obj.transform.position + sr.sprite.bounds);


		// sorting order 

		sr.sortingOrder = -myFurniture.y;

		if (myFurniture.walkable == true) 
		{
			sr.sortingOrder = (int) -(myFurniture.y + myFurniture.mySize.y);

		}

		sr.sortingLayerName = Constants.furniture_character_layer;

		return obj;


	}



	public static GameObject CreateCharacterGameObject (Character myCharacter, Transform parent)
	{

		//Debug.Log ("Assign Furniture Image");

		myCharacter.myPos = new Vector3 (myCharacter.x + myCharacter.mySize.x/2, myCharacter.y, 0);

		GameObject obj = GameObject.Instantiate(Resources.Load<GameObject> ("Sprites/Characters/" + myCharacter.myName)); 			
		obj.transform.SetParent (parent);


		obj.transform.position = new Vector3 (myCharacter.myPos.x + myCharacter.offsetX, myCharacter.myPos.y + 0.5f + myCharacter.offsetY, myCharacter.myPos.z);

		//Debug.Log ("object position" + myFurniture.myName + obj.transform.position + sr.sprite.bounds);


		// sorting order 

		obj.GetComponentInParent<SpriteRenderer>().sortingOrder  = -myCharacter.y;
		obj.GetComponentInParent<SpriteRenderer>().sortingLayerName = Constants.furniture_character_layer;

		return obj;


	}




}
