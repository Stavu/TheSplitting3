﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItemHelper {



	public static void UseItemOnPhysicalInteractable (InventoryItem item, PhysicalInteractable physicalInt)
	{

		//Debug.Log ("Use item Helper " + item.fileName + " " + furniture.myName);


		switch (item.fileName) 
		{


			/* -------- COMPASS -------- */



			case "compass":


				switch (physicalInt.myName) 
				{

					case "door_abandoned_main_shadow":
						

						DialogueSentence sentence = new DialogueSentence (PlayerManager.myPlayer.myName, "This item is no good.", false);
						List<DialogueSentence> list = new List<DialogueSentence> ();
						list.Add (sentence);

						InteractionManager.instance.DisplayText (list);

						InventoryItem picture = new InventoryItem ("missing_picture", "Missing Picture");
						GameManager.playerData.inventory.AddItem (picture);

						break;



					default:

						break;

				}

			break;



				/* -------- MISSING PICTURE -------- */


			case "missing_picture":
				

				switch (physicalInt.myName) 
				{

					case "door_abandoned_main_shadow":

						DialogueSentence sentence2 = new DialogueSentence (PlayerManager.myPlayer.myName, "pic on door.", false);
						List<DialogueSentence> list2 = new List<DialogueSentence> ();
						list2.Add (sentence2);

						InteractionManager.instance.DisplayText (list2);

					
						break;


					default:

						break;

				}

				break;


		}



	}




}
