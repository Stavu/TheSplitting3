using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour {


	// Singleton //

	public static CutsceneManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //



	public Dictionary <string,IEnumerator> stringCutsceneMap;
	public static bool inCutscene = false;


	// Use this for initialization

	void Start () 
	{
		stringCutsceneMap = new Dictionary<string, IEnumerator> ();

		Cutscene cutScene = new DanielScene ("daniel_scene");
		stringCutsceneMap.Add (cutScene.myName, cutScene.MyCutscene());

	}
	
	// Update is called once per frame

	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.G))
		{
			PlayCutscene ("daniel_scene");	
		}
	}



	public void PlayCutscene(string cutsceneName)
	{

		if (stringCutsceneMap.ContainsKey (cutsceneName) == false) 
		{
			Debug.LogError ("no cutscene with this name " + cutsceneName);
			return;
		}

		inCutscene = true;
		EventsHandler.Invoke_cb_inputStateChanged ();

		StartCoroutine (stringCutsceneMap [cutsceneName]);	

	}


}

