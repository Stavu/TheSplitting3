using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItemHelper {



	public static void UseItemOnFurniture (InventoryItem item, Furniture furniture)
	{

		Debug.Log ("Use item Helper " + item.fileName + " " + furniture.myName);


		switch (item.fileName) 
		{


			/* -------- COMPASS -------- */



			case "compass":


				switch (furniture.myName) 
				{

					case "door_abandoned_main_shadow":
						

						Debug.Log ("used item");
						InteractionManager.instance.DisplayText (PlayerManager.instance.myPlayer, "This item is no good.");

						InventoryItem picture = new InventoryItem ("missing_picture", "Missing Picture");
						GameManager.playerData.inventory.AddItem (picture);

						break;



					default:

						break;

				}

			break;



				/* -------- MISSING PICTURE -------- */


			case "missing_picture":
				

				switch (furniture.myName) 
				{

					case "door_abandoned_main_shadow":

						Debug.Log ("used item picture");
						InteractionManager.instance.DisplayText (PlayerManager.instance.myPlayer, "Picutre on door.");

					
						break;


					default:

						break;

				}

				break;


		}



	}




}
