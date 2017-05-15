﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ConditionInspector : MonoBehaviour {


	// Declerations


	GameObject conditionPanelObject;

	Transform panel;

	Dropdown conditionTypeDropdown;
	InputField stringInput;

	Button cancelButton;
	Button submitButton;





	// Use this for initialization

	void Start () 
	{
		
	}
	
	// Update is called once per frame

	void Update () 
	{
		
	}



	public void CreateConditionPanel()
	{


		conditionPanelObject = Resources.Load<GameObject> ("Prefabs/InteractionPanelPrefabs/ConditionPanel");


		panel = conditionPanelObject.transform.Find ("Panel");

		conditionTypeDropdown = panel.Find ("ConditionTypeDropdown").GetComponent<Dropdown> ();
		stringInput = panel.Find ("StringInput").GetComponent<InputField> ();

		cancelButton = panel.Find ("CancelButton").GetComponent<Button> ();
		submitButton = panel.Find ("SubmitButton").GetComponent<Button> ();


		// populate values


		// Dropdown

		List<string> conditionTypeList;

		foreach (ConditionType condType in Enum.GetValues(typeof(ConditionType))) 
		{
			conditionTypeList.Add (condType.ToString ());

		}

		conditionTypeDropdown.AddOptions (conditionTypeList);



		// Buttons


		cancelButton.onClick.AddListener (cancelButton);


	
	}



	public void Cancel()
	{

		Destroy (conditionPanelObject);
	}



	public void SubmitCondition()
	{




	}





}
