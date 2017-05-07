using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;



public class GameData {



	public Dictionary <string,List<string>> itemLookAtMap;
	public Dictionary <string,List<ItemData_CombineInteractions>> itemCombineMap;

	ItemData itemData;


	public GameData()
	{
		
		CreateItemLookAtMap ();

	}


	public void CreateItemLookAtMap()
	{
		
		itemLookAtMap = new Dictionary<string, List<string>> ();
		itemCombineMap = new Dictionary <string,List<ItemData_CombineInteractions>> ();


		//itemData = JsonUtility.FromJson<ItemData> (Resources.LoadAll ("Jsons/Data/itemData"));


		System.Object[] myTextAssets = Resources.LoadAll ("Jsons/Data/");

		foreach (TextAsset txt in myTextAssets) 
		{
			itemData = JsonUtility.FromJson<ItemData> (txt.text);
		}

		/* populate itemLookAtMap - go through itemData, for each itemData_lookAt in the lookAtList
		 insert the itemName and textList as the key and value of itemLookAtMap */

		foreach (ItemData_LookAt lookAt in itemData.lookAtList) 
		{

			itemLookAtMap.Add (lookAt.itemName, lookAt.textList);
			
		}

		foreach (ItemData_Combine combine in itemData.combineList) 
		{

			itemCombineMap.Add (combine.itemName, combine.itemsToCombineList);

		}


	}

}




// ------ DATA CLASSES ------ //


// GENERAL //

[Serializable]
public class ItemData {

	public List<ItemData_LookAt> lookAtList;
	public List<ItemData_Combine> combineList;

}


// LOOK AT //

[Serializable]
public class ItemData_LookAt {

	public string itemName;
	public List<string> textList;
}



// COMBINE //

[Serializable]
public class ItemData_Combine {

	public string itemName;
	public List<ItemData_CombineInteractions> itemsToCombineList;

}

[Serializable]
public class ItemData_CombineInteractions {

	public string targetName;
	public List<SubInteraction> subInteractionList;
	

}

