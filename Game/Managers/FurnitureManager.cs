using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FurnitureManager : MonoBehaviour {


	// Singleton //

	public static FurnitureManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //



	public Dictionary<Furniture,GameObject> furnitureGameObjectMap;
	public Dictionary<string,Furniture> nameFurnitureMap;



	// Use this for initialization

	public void Initialize () 
	{		
	
		EventsHandler.cb_furnitureChanged += CreateFurnitureGameObject;

		furnitureGameObjectMap = new Dictionary<Furniture, GameObject> ();
		nameFurnitureMap = new Dictionary<string, Furniture> ();

	}


	public void OnDestroy()
	{	
	
		EventsHandler.cb_furnitureChanged -= CreateFurnitureGameObject;
			
	}




	// Update is called once per frame

	void Update () 
	{
		
	}




	public void CreateFurnitureGameObject (Furniture myFurniture)
	{
		
		GameObject obj = CreateFurnitureGameObject (myFurniture, this.transform);
		furnitureGameObjectMap.Add (myFurniture, obj);

		if (myFurniture.identificationName != null) 
		{			
			nameFurnitureMap.Add (myFurniture.identificationName, myFurniture);
		
		}
	}





	public GameObject CreateFurnitureGameObject (Furniture myFurniture, Transform parent)
	{

		GameObject[] animatedObjects = Resources.LoadAll<GameObject> ("Prefabs/Furniture");

		myFurniture.myPos = new Vector3 (myFurniture.x + myFurniture.mySize.x/2, myFurniture.y, 0);

		GameObject obj = null;
		SpriteRenderer sr = null;

		foreach (GameObject gameObj in animatedObjects) 
		{
			if (gameObj.name == myFurniture.myName) 
			{
				obj = Instantiate (gameObj);
				sr = obj.GetComponentInChildren<SpriteRenderer>();
				break;
			}	
		}

		if (obj == null) 
		{
			obj = new GameObject (myFurniture.myName);
			sr = obj.AddComponent<SpriteRenderer>();
			sr.sprite = Resources.Load<Sprite> ("Sprites/Furniture/" + myFurniture.myName); 
		}


		obj.transform.SetParent (parent);
		obj.transform.position = new Vector3 (myFurniture.myPos.x + myFurniture.offsetX, myFurniture.myPos.y + 0.5f + myFurniture.offsetY, myFurniture.myPos.z);

	
		// sorting order 

		sr.sortingOrder = -myFurniture.y;

		if (myFurniture.walkable == true) 
		{
			sr.sortingOrder = (int) -(myFurniture.y + myFurniture.mySize.y);

		}

		sr.sortingLayerName = Constants.furniture_character_layer;

		return obj;


	}









	// Setting furniture animation state

	public void SetFurnitureAnimationState(string furnitureName, string state)
	{

		if (nameFurnitureMap.ContainsKey (furnitureName)) 
		{
			Furniture furn = nameFurnitureMap [furnitureName];
			GameObject obj = furnitureGameObjectMap [furn];
			Animator animator = obj.GetComponent<Animator> ();

			animator.PlayInFixedTime (state);
	
		} else {

			Debug.LogError ("I don't have this title name");

		}
	}

}
