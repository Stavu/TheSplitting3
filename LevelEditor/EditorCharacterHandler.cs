using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCharacterHandler : MonoBehaviour {


	// Use this for initialization

	public void Initialize () 
	{
		EventsHandler.cb_editorNewRoomCreated += CharacterFactory;
		EventsHandler.cb_editorCharacterModelChanged += CreateCharacterObject;
	}


	public void OnDestroy()
	{
		EventsHandler.cb_editorNewRoomCreated -= CharacterFactory;
		EventsHandler.cb_editorCharacterModelChanged -= CreateCharacterObject;
	}



	// Update is called once per frame

	void Update () 
	{

	}



	// Placing Furniture in the editor 

	public void PlaceCharacter(Tile tile, string characterName)
	{


		if(characterName == null)
		{
			return;
		}


		// If there's already a character on this tile, destroy it before creating a new character


		if (tile.myCharacter != null)
		{
			EditorRoomManager.instance.room.myCharacterList.Remove (tile.myCharacter);

			Destroy(EditorRoomManager.instance.characterGameObjectMap [tile.myCharacter]);
			EditorRoomManager.instance.characterGameObjectMap.Remove (tile.myCharacter);
		}



		// create furniture

		Character character = new Character (characterName, tile.x, tile.y);


		// set size

		character.mySize = Vector2.one;


		EditorRoomManager.instance.room.myCharacterList.Add (character);

		tile.myCharacter = character;


		EventsHandler.Invoke_cb_editorCharacterModelChanged (character);


	}



	public void CharacterFactory(Room room)
	{

		//Debug.Log ("FurnitureFactory");

		foreach (Character character in room.myCharacterList) 
		{			

			EventsHandler.Invoke_cb_editorCharacterModelChanged (character);

		}

	}



	public void CreateCharacterObject(Character character)
	{			

		GameObject obj = Utilities.CreateCharacterGameObject (character, this.transform);


		if (character == null) 
		{
			Debug.Log ("character = null");
		}

		if (EditorRoomManager.instance.characterGameObjectMap == null) 
		{
			EditorRoomManager.instance.characterGameObjectMap = new Dictionary<Character, GameObject> ();
		}


		EditorRoomManager.instance.characterGameObjectMap.Add (character, obj);	

	}
}
