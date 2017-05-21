using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EditorFurnitureHandler : MonoBehaviour 
{
	


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



	// Placing Furniture in the editor 

	public void PlaceFurniture(Tile tile, string furnitureName)
	{


		if(furnitureName == null)
		{
			return;
		}


		// If there's already a furniture on this tile, destroy it before creating a new furniture


		if (tile.myFurniture != null)
		{
			EditorRoomManager.instance.room.myFurnitureList.Remove (tile.myFurniture);

			Destroy(EditorRoomManager.instance.furnitureGameObjectMap [tile.myFurniture]);
			EditorRoomManager.instance.furnitureGameObjectMap.Remove (tile.myFurniture);
		}



		// create furniture

		Furniture furn = new Furniture (furnitureName, tile.x, tile.y);


		// set default size

		Sprite furnitureSprite = Resources.Load <Sprite> ("Sprites/Furniture/" + furnitureName);


		furn.mySize = new Vector2 (Mathf.Ceil(furnitureSprite.bounds.size.x), 1f);


		EditorRoomManager.instance.room.myFurnitureList.Add (furn);

		tile.myFurniture = furn;


		EventsHandler.Invoke_cb_editorFurnitureModelChanged (furn);


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

	}

}
