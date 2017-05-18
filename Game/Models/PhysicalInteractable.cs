using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class PhysicalInteractable : Interactable {


	public string identificationName;
	public string myName;

	public Vector3 myPos {get; set;}

	public float offsetX = 0;
	public float offsetY = 0;

	public List<Interaction> myInteractionList;



}
