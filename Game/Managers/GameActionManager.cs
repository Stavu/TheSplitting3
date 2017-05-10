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





	public void OnSpacebarPressed ()
	{

		// If there's text, close it

		if (InteractionManager.instance.currentTextBox != null) 
		{
			InteractionManager.instance.CloseTextBox ();
			GameManager.instance.inputState = InputState.Character;
			return;
		}


		// if there is no furniture / character selected, return

		if ((ActionBoxManager.instance.currentPhysicalInteractable == null) && (GameManager.instance.inputState != InputState.Inventory))
		{

			return;

		} else {

			//Debug.Log ("currentFurniture " + currentFurniture.myName);

		}


		// If there is already an actionbox, activate interaction

		if ((ActionBoxManager.instance.currentActionBox != null) && (GameManager.instance.inputState == InputState.ActionBox))		
		{			
			ActionBoxManager.instance.ActivateInteraction ();
			return;
		}


		if (GameManager.instance.inputState == InputState.Inventory)
		{

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
		}


		// if there's no action box, create one

		if (GameManager.instance.inputState == InputState.Character) 
		{
			ActionBoxManager.instance.OpenActionBox ();
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
