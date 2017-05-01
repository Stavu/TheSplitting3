using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]

public class InventoryItem
{

	public string titleName;
	public string fileName;


	public InventoryItem(string fileName, string titleName)
	{

		this.fileName = fileName;
		this.titleName = titleName;


	}



}
