using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;


public class SoundManager : MonoBehaviour {



	// Singleton //

	public static SoundManager instance { get; protected set; }

	void Awake ()
	{		
		if (instance == null) 
		{
			instance = this;

		} else if (instance != this) 
		{
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}
		
	// Singleton //


	public bool soundOn = true;


	public enum SFX_Type
	{
		WalkingOnFloor,
		WalkingOnGrass,
		OpenDoor,
		CloseDoor
	}


	[Serializable]
	public struct NamedSound
	{
		public SFX_Type type;
		public AudioClip[] clips;
	}


	public NamedSound[] soundEffects;
	public Dictionary<SFX_Type, AudioClip[]> soundEffectsMap;





	// Use this for initialization

	public void Initialize () 
	{
		
	}


	void OnDestroy()
	{


	}



	
	// Update is called once per frame

	void Update () 
	{
		
	}





	public void PlaySound(SFX_Type type)
	{

		if (soundOn == false) 
		{
			return;
		}




	}




}
