using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EditorFurnitureManager : MonoBehaviour 
{

	// Singleton //

	public static EditorFurnitureManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //






	// Use this for initialization

	public void Initialize () 
	{
		EventsHandler.cb_editorNewRoomCreated += FurnitureFactory;
		EventsHandler.cb_editorFurnitureModelChanged += CreateFurnitureObject;
	}


	public void OnDestroy()
	{
		EventsHandler.cb_editorNewRoomCreated -= FurnitureFactory;
		EventsHandler.cb_editorFurnitureModelChanged -= CreateFurnitureObject;
	}



	// Update is called once per frame

	void Update () 
	{
		
	}


	public void FurnitureFactory(Room room)
	{

		//Debug.Log ("FurnitureFactory");

		foreach (Furniture furn in room.myFurnitureList) 
		{			

			EventsHandler.Invoke_cb_editorFurnitureModelChanged (furn);

		}


	}



	public void CreateFurnitureObject(Furniture furn)
	{

		//Debug.Log ("CreateFurnitureObject");

		GameObject obj = Utilities.CreateFurnitureGameObject (furn, this.transform);


		if (furn == null) 
		{
			Debug.Log ("furn = null");
		}

		if (EditorRoomManager.instance.furnitureGameObjectMap == null) 
		{
			EditorRoomManager.instance.furnitureGameObjectMap = new Dictionary<Furniture, GameObject> ();
		}


		EditorRoomManager.instance.furnitureGameObjectMap.Add (furn, obj);

		EventsHandler.Invoke_cb_editorFurniturePlaced (furn);


	}



}
