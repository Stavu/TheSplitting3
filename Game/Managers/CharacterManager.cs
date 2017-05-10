using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {



	// Singleton //

	public static CharacterManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //



	public Dictionary<Character,GameObject> characterGameObjectMap;





	// Use this for initialization

	public void Initialize () 
	{
		EventsHandler.cb_characterChanged += CreateCharacterGameObject;

		characterGameObjectMap = new Dictionary<Character, GameObject> ();

	}



	public void OnDestroy()
	{	

		EventsHandler.cb_characterChanged -= CreateCharacterGameObject;

	}


	bool foo = true;


	// Update is called once per frame

	void Update () 
	{

		if (Input.GetKeyDown (KeyCode.E)) 
		{
			Tile tileA = RoomManager.instance.myRoom.myGrid.GetTileAt (13, 7);
			Tile tileB = RoomManager.instance.myRoom.myGrid.GetTileAt (2, 7);
			Tile tileC = foo ? tileA : tileB;

			Character myCharacter = GetCharacterByName ("llehctiM");

			MoveToTile (myCharacter, tileC);

			foo = !foo;

		}
	}



	public void CreateCharacterGameObject (Character myCharacter)
	{

		GameObject obj = Utilities.CreateCharacterGameObject (myCharacter, this.transform);

		characterGameObjectMap.Add (myCharacter, obj);

	}




	// ---- MOVE CHARACTER ---- //


	public void MoveToTile(Character character, Tile myTargetTile)
	{

		Tile currentTile = RoomManager.instance.myRoom.myGrid.GetTileAt (character.x, character.y);

		if (currentTile.myCharacter == character) 
		{
			currentTile.myCharacter = null;
		}

		character.targetTile = myTargetTile;

		StartCoroutine (MoveCoroutine (character, myTargetTile));

	}



	public IEnumerator MoveCoroutine(Character character, Tile myTargetTile)
	{

		GameObject characterObject = characterGameObjectMap [character];

		Vector3 startPos = characterObject.transform.position;
		Vector3 endPos = Utilities.GetCharacterPosOnTile (character, myTargetTile);

		float distance = Vector3.Distance (startPos, endPos);
		Debug.Log ("distance " + distance);

		float tempSpeed = character.speed / distance;
		Debug.Log ("tempSpeed " + tempSpeed);

		// interpolation
		float inter = 0;


		// Animations

		Animator myAnimator = characterObject.GetComponent<Animator> ();
		Direction lastDirection = Direction.left;


		// ANIMATIONS


		// Walk left

		if (character.x > myTargetTile.x) 
		{
			myAnimator.PlayInFixedTime ("Walk_left");
			lastDirection = Direction.left;

		}


		// Walk right

		if (character.x < myTargetTile.x) 
		{
			myAnimator.PlayInFixedTime ("Walk_right");
			lastDirection = Direction.right;
		}


		// Walk down

		if (character.y > myTargetTile.y) 
		{
			myAnimator.PlayInFixedTime ("Walk_front");
			lastDirection = Direction.down;
		}


		// Walk up

		if (character.y < myTargetTile.y) 
		{
			myAnimator.PlayInFixedTime ("Walk_back");
			lastDirection = Direction.up;
		}





		// while loop - updating the character object position

		while(startPos != endPos)
		{
			inter += tempSpeed * Time.deltaTime;

			if (inter >= 1) 
			{				
				Debug.Log ("I arrived " + character.myName);

				characterGameObjectMap [character].transform.position = startPos = endPos;
				break;

			} else {

				characterGameObjectMap [character].transform.position = Vector3.Lerp (startPos, endPos, inter);

			}

			yield return new WaitForFixedUpdate ();

		}


		// After while loop is done, change the character tile

		character.ChangeTile (myTargetTile);


		switch (lastDirection) 
		{

			case Direction.left:

				myAnimator.PlayInFixedTime ("Idle_left");

				break;


			case Direction.right:

				myAnimator.PlayInFixedTime ("Idle_right");

				break;


			case Direction.down:

				myAnimator.PlayInFixedTime ("Idle_front");

				break;


			case Direction.up:

				myAnimator.PlayInFixedTime ("Idle_back");

				break;

		}




	}



	public Character GetCharacterByName(string characterName)
	{

		foreach (Character character in characterGameObjectMap.Keys) 
		{
			if (character.myName == characterName) 
			{
				return character;
			}			
		}

		return null;
	}



}
