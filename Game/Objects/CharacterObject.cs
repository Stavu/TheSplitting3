using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : MonoBehaviour {


	SpriteRenderer mySpriteRenderer;


	// Use this for initialization
	void Start () 
	{
		mySpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer> ();
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



		// Animations

		Animator myAnimator = gameObject.GetComponent<Animator> ();


		switch (myDirection) 
		{
		
			case Direction.left:

				Debug.Log ("animation left");

				myAnimator.PlayInFixedTime ("Walk_left");

				break;



			case Direction.right:

				break;



			case Direction.down:

				break;



			case Direction.up:

				break;
						
		
		}


	}



}
