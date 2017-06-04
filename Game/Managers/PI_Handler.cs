﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PI_Handler : MonoBehaviour {



	// Singleton //

	public static PI_Handler instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //



	public Dictionary<PhysicalInteractable,GameObject> PI_gameObjectMap;
	public Dictionary<string,PhysicalInteractable> PI_nameMap;



	// Use this for initialization
	public void Initialize () 
	{

		EventsHandler.cb_newAnimationState += ChangeCurrentGraphicState;

		EventsHandler.cb_furnitureChanged += CreatePIGameObject;
		EventsHandler.cb_characterChanged += CreatePIGameObject;


		PI_gameObjectMap = new Dictionary<PhysicalInteractable, GameObject> ();
		PI_nameMap = new Dictionary<string, PhysicalInteractable> ();
	}


	void OnDestroy()
	{
		EventsHandler.cb_newAnimationState -= ChangeCurrentGraphicState;

		EventsHandler.cb_furnitureChanged -= CreatePIGameObject;
		EventsHandler.cb_characterChanged -= CreatePIGameObject;


		PI_gameObjectMap.Clear ();
		PI_nameMap.Clear ();
	}


	// Update is called once per frame
	void Update () 
	{
		
	}



	public void AddPIToMap(PhysicalInteractable physicalInteractable, GameObject obj, string name)
	{

		PI_gameObjectMap.Add (physicalInteractable, obj);
		PI_nameMap.Add (name, physicalInteractable);

	}


	public void CreatePIGameObject (PhysicalInteractable myPhysicalInteractable)
	{
		// if the furniture has an identification name, use it as the name. If it doesn't, use the file name.

		bool useIdentifiactionName = ((myPhysicalInteractable.identificationName != null) && (myPhysicalInteractable.identificationName != string.Empty));
		string PI_name = useIdentifiactionName ? myPhysicalInteractable.identificationName : myPhysicalInteractable.fileName;			

		myPhysicalInteractable.myPos = new Vector3 (myPhysicalInteractable.x + myPhysicalInteractable.mySize.x/2, myPhysicalInteractable.y, 0);

		GameObject obj = null;
		SpriteRenderer sr = null;


		// Animated Object

		if (GameManager.stringPrefabMap.ContainsKey (myPhysicalInteractable.fileName)) {

			obj = Instantiate (GameManager.stringPrefabMap [myPhysicalInteractable.fileName]);
			sr = obj.GetComponentInChildren<SpriteRenderer> ();
			string state = GameManager.playerData.GetAnimationState (myPhysicalInteractable.identificationName);

			PI_Handler.instance.AddPIToMap (myPhysicalInteractable, obj, PI_name);

			if (state != string.Empty) {
				PI_Handler.instance.SetPIAnimationState (myPhysicalInteractable.identificationName, state, obj);
			} 

		} else {
			
			// if not animated object

			obj = new GameObject (myPhysicalInteractable.fileName);
			GameObject childObj = new GameObject ("Image");
			childObj.transform.SetParent (obj.transform);

			PI_Handler.instance.AddPIToMap (myPhysicalInteractable, obj, PI_name);

			sr = childObj.AddComponent<SpriteRenderer>();
			sr.sprite = Resources.Load<Sprite> ("Sprites/Furniture/" + myPhysicalInteractable.fileName); 

		}


		obj.transform.SetParent (this.transform);
		obj.transform.position = new Vector3 (myPhysicalInteractable.myPos.x + myPhysicalInteractable.offsetX, myPhysicalInteractable.myPos.y + 0.5f + myPhysicalInteractable.offsetY, myPhysicalInteractable.myPos.z);


		// sorting order 
		Utilities.SetPISortingOrder (myPhysicalInteractable, obj);


		if (myPhysicalInteractable.walkable == true) 
		{
			sr.sortingOrder = (int) -(myPhysicalInteractable.y + myPhysicalInteractable.mySize.y) * 10;

		}

		sr.sortingLayerName = Constants.furniture_character_layer;

	}



	public void ChangeCurrentGraphicState(PhysicalInteractable physicalInteractable, string state)
	{
		foreach (GraphicState graphicState in physicalInteractable.graphicStates) 
		{
			if (graphicState.graphicStateName == state) 
			{
				physicalInteractable.currentGraphicState = graphicState;

				//GameManager.playerData.AddAnimationState (PI_name, state);

			}
		}
	}





	// Setting furniture animation state

	public void SetPIAnimationState(string PI_name, string state, GameObject obj = null)
	{

		if (PI_nameMap.ContainsKey (PI_name)) 
		{
			PhysicalInteractable physicalInteractable = PI_nameMap [PI_name];

			if(obj == null)
			{				
				obj = PI_gameObjectMap [physicalInteractable];
			}

			Animator animator = obj.GetComponent<Animator> ();

			ChangeCurrentGraphicState (physicalInteractable, state);

			animator.PlayInFixedTime (state);
			Utilities.SetPISortingOrder (physicalInteractable, obj);

		} else {

			Debug.LogError ("I don't have this title name " + PI_name);

		}
	}













}
