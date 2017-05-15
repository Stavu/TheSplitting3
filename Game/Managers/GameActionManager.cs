using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionManager : MonoBehaviour {


	// Singleton //

	public static GameActionManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //


	public TileInteraction currentTileInteraction;


	// Use this for initialization

	public void Initialize () 
	{
		EventsHandler.cb_spacebarPressed += OnSpacebarPressed;
		EventsHandler.cb_escapePressed += OnEscapePressed;

		EventsHandler.cb_playerHitTileInteraction += OnHitTileInteraction;
		EventsHandler.cb_playerLeaveTileInteraction += OnLeaveTileInteraction;

	}


	public void OnDestroy()
	{
		EventsHandler.cb_spacebarPressed -= OnSpacebarPressed;
		EventsHandler.cb_escapePressed -= OnEscapePressed;

		EventsHandler.cb_playerHitTileInteraction -= OnHitTileInteraction;
		EventsHandler.cb_playerLeaveTileInteraction -= OnLeaveTileInteraction;

	}


	
	// Update is called once per frame
	void Update () 
	{
		
	}



	// SPACEBAR PRESSED // 


	public void OnSpacebarPressed ()
	{


		Debug.Log ("input state " + GameManager.instance.inputState);


		switch (GameManager.instance.inputState) 
		{

			case InputState.ActionBox:

				// If there is already an actionbox, activate interaction

				if (ActionBoxManager.instance.currentActionBox != null) 
				{
					ActionBoxManager.instance.ActivateInteraction ();
					return;
				}

				break;



			case InputState.Inventory:

				
				switch (GameManager.playerData.inventory.myState) 
				{

					case InventoryState.UseItem:

						InventoryUI.instance.SelectItemToUse ();

						break;				


					case InventoryState.Browse:

						InventoryUI.instance.ActivateInteraction ();

						break;	


					case InventoryState.Combine:

						InventoryUI.instance.CombineItems ();

						break;
				}	

				return;		




			case InputState.DialogueBox:
				
				
				if (DialogueManager.instance.currentDialogueOption != null)
				{						
					DialogueManager.instance.ActivateDialogueOption ();
					return;
				}

				break;



			case InputState.Character:

				// if there's no action box, create one

				if (ActionBoxManager.instance.currentPhysicalInteractable != null) 
				{
					ActionBoxManager.instance.OpenActionBox ();
				}


				break;

		}

	}





	public void OnEscapePressed()
	{

		if (GameManager.instance.inputState == InputState.Inventory) 
		{
			InventoryUI.instance.CloseInventory();
			return;
		}


		ActionBoxManager.instance.CloseActionBox ();

		/*
		if (GameManager.instance.inputState == InputState.DialogueBox) 
		{


		}
		*/

		// setting the input state back to character when closing the action box

		GameManager.instance.inputState = InputState.Character;

	}





	// Tile Interaction //


	public void OnHitTileInteraction(Tile tile)
	{		

		Debug.Log ("OnHitTileInteraction");
		
		TileInteraction tileInt = tile.myTileInteraction;

		if (currentTileInteraction != tileInt) 
		{
			currentTileInteraction = tileInt;

			if (currentTileInteraction != null)
			{				
				Debug.Log ("Sub Interact");

				tileInt.mySubInt.SubInteract ();
			}

		}

	}



	public void OnLeaveTileInteraction ()
	{
		Debug.Log ("OnLeaveTileInteraction");
		currentTileInteraction = null;

	}


}
