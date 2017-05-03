using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItemHelper {



	public static void UseItemOnFurniture (InventoryItem item, Furniture furniture)
	{

		Debug.Log ("Use item Helper " + item.fileName + " " + furniture.myName);


		switch (item.fileName) 
		{


			/* -------- ORDER DETAILS -------- */



			case "order_details":


				switch (furniture.myName) 
				{

					case "door_abandoned_main_shadow":
						

						Debug.Log ("used item");

						break;



					default:

						break;

				}

			break;


				/* -------- NEXT ITEM -------- */




		}



	}




}
