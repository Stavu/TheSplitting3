﻿using System.Collections;
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



	//public Dictionary<Character,GameObject> characterGameObjectMap;





	// Use this for initialization

	public void Initialize () 
	{		
		//characterGameObjectMap = new Dictionary<Character, GameObject> ();
	}



	public void OnDestroy()
	{	

	}


	// Update is called once per frame

	void Update () 
	{


	}

	/*

	public void CreateCharacterGameObject (Character myCharacter)
	{

		GameObject obj = Utilities.CreateCharacterGameObject (myCharacter, this.transform);

		PI_Handler.instance.AddPIToMap (myCharacter, obj, myCharacter.identificationName);


	}

	*/



	// ---- MOVE CHARACTER ---- //


	public void MoveByPath (IWalker walker, List<Vector2> posList)
	{
		Queue<Vector2> path = new Queue<Vector2> ();

		foreach (Vector2 pos in posList) 
		{
			path.Enqueue (pos);					
		}

		walker.walkerPath = path;
		walker.walkerTargetPos = walker.walkerPath.Dequeue ();

		MoveToTargetTile (walker, walker.walkerTargetPos);
	}



	public void MoveToTargetTile(IWalker walker, Vector2 myTargetPos)
	{

		Tile currentTile = RoomManager.instance.myRoom.MyGrid.GetTileAt ((int)walker.speakerPos.x, (int)walker.speakerPos.y);

		if (currentTile.myCharacter == walker) 
		{
			currentTile.myCharacter = null;
		}

		walker.walkerTargetPos = myTargetPos;

		StartCoroutine (MoveCoroutine (walker));

	}



	public IEnumerator MoveCoroutine(IWalker walker)
	{
		
		Vector3 startPos = walker.walkerGameObject.transform.position;
		Vector3 endPos = Utilities.GetCharacterPosOnTile (walker, walker.walkerTargetPos);

		float distance = Vector3.Distance (startPos, endPos);
		Debug.Log ("distance " + distance);

		float tempSpeed = walker.walkerSpeed / distance;
		Debug.Log ("tempSpeed " + tempSpeed);

		// interpolation
		float inter = 0;


		// Animations

		Animator myAnimator = walker.walkerGameObject.GetComponent<Animator>();
		Direction lastDirection = Direction.left;


		// ANIMATIONS


		// Walk left

		if (walker.speakerPos.x > walker.walkerTargetPos.x) 
		{
			myAnimator.PlayInFixedTime ("Walk_left");
			lastDirection = Direction.left;

		}


		// Walk right

		if (walker.speakerPos.x < walker.walkerTargetPos.x) 
		{
			myAnimator.PlayInFixedTime ("Walk_right");
			lastDirection = Direction.right;
		}


		// Walk down

		if (walker.speakerPos.y > walker.walkerTargetPos.y) 
		{
			myAnimator.PlayInFixedTime ("Walk_front");
			lastDirection = Direction.down;
		}


		// Walk up

		if (walker.speakerPos.y < walker.walkerTargetPos.y) 
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
				Debug.Log ("I arrived " + walker.speakerName);

				walker.walkerGameObject.transform.position = startPos = endPos;
				break;

			} else {

				walker.walkerGameObject.transform.position = Vector3.Lerp (startPos, endPos, inter);

			}

			yield return new WaitForFixedUpdate ();

		}


		// After while loop is done, change the character tile

		walker.ChangePos (walker.walkerTargetPos);


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



		// Check if I need to continue walking 

		if (walker.walkerPath != null) 
		{
			if (walker.walkerPath.Count > 0) 
			{
				walker.walkerTargetPos = walker.walkerPath.Dequeue ();
				MoveToTargetTile (walker, walker.walkerTargetPos);
			
			} else {

				EventsHandler.Invoke_cb_characterFinishedPath ();
			}
		}


	}




}
