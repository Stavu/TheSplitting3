using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour {


	SpriteRenderer mySpriteRenderer;
	Animator myAnimator;

	AnimatorStateInfo ASI;

	// Use this for initialization
	void Start () 
	{
		mySpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer> ();
		myAnimator = gameObject.GetComponent<Animator> ();

	}

	
	// Update is called once per frame
	void Update () 
	{
		
	}


	public void MoveCharacter(Player myPlayer, Direction myDirection)
	{

		// Updating the player's sorting layer

		Tile currentTile = RoomManager.instance.myRoom.myGrid.GetTileAt(myPlayer.myPos);

		mySpriteRenderer.sortingOrder = -currentTile.y;



		// Change position

		gameObject.transform.position = myPlayer.myPos;




		switch (myDirection) 
		{
		
			case Direction.left:

				PlayAnimation ("Walk_left");

				break;



			case Direction.right:

				PlayAnimation ("Walk_right");

				break;



			case Direction.down:

				PlayAnimation ("Walk_front");	

				break;



			case Direction.up:
			
				PlayAnimation ("Walk_back");	

				break;
						
		
		}


	}




	public void StopCharacter(Direction lastDirection)
	{

		//Debug.Log ("StopCharacter");


		if (GameManager.instance.inputState != InputState.Character)
		{
			return;
		}

			


		switch (lastDirection) 
		{

			case Direction.left:			

				PlayAnimation ("Idle_left");			

				break;


			case Direction.right:
				
				PlayAnimation ("Idle_right");			

				break;


			case Direction.down:

				PlayAnimation ("Idle_front");			

				break;


			case Direction.up:

				PlayAnimation ("Idle_back");

				break;


		}

	}




	void PlayAnimation(string animationName)
	{

		ASI = myAnimator.GetCurrentAnimatorStateInfo (0);

		if (ASI.IsName (animationName) == false) 
		{

			myAnimator.PlayInFixedTime (animationName);
		}
			

	}


}
