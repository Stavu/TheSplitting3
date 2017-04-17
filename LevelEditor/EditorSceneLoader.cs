using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorSceneLoader : MonoBehaviour {


	public GameObject managers;



	// Use this for initialization
	void Start () {

		CreateManagers ();

	}




	// Update is called once per frame
	void Update () {

	}



	void CreateManagers()
	{

		Instantiate (managers);	

		EditorTileManager.instance.Initialize ();
		BuildController.instance.Initialize ();
	
		EditorRoomManager.instance.SetRoomBackground ("abandoned_lobby_bg");
		EditorUI.instance.CreateUI ();
	}



}
