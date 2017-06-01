using System.Collections;
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

		PI_gameObjectMap = new Dictionary<PhysicalInteractable, GameObject> ();
		PI_nameMap = new Dictionary<string, PhysicalInteractable> ();
	}


	void OnDestroy()
	{
		EventsHandler.cb_newAnimationState -= ChangeCurrentGraphicState;

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
