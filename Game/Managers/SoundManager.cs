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
		LoadAllSounds ();
		RegisterSounds ();
	}
		
	// Singleton //


	public static bool soundIsOff = false;

	Dictionary<string,AudioClip> stringAudioClipMap;
	AudioSource[] audioSources = new AudioSource[10];
	int nextAudioSource = 0;

	AudioSource[] audioSources_loop = new AudioSource[5];
	int nextAudioSource_loop = 0;


	/*
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

	*/




	void OnDestroy()
	{
		cb_playSound -= PlaySound;
		cb_leftRoom_start -= StopLoopSounds;
	}

	
	// Update is called once per frame

	void Update () 
	{
		
	}



	public void RegisterSounds()
	{		
		cb_playSound += PlaySound;
		cb_leftRoom_start += StopLoopSounds;

	}



	public void LoadAllSounds()
	{

		for (int i = 0; i < audioSources.Length; i++) 
		{
			audioSources[i] = gameObject.AddComponent<AudioSource> ();			
		}	

		for (int i = 0; i < audioSources_loop.Length; i++) 
		{
			audioSources_loop[i] = gameObject.AddComponent<AudioSource> ();			
		}


		AudioClip[] clipList = Resources.LoadAll<AudioClip> ("Audio/SoundEffects");

		stringAudioClipMap = new Dictionary<string, AudioClip> ();


		// Populate clipList

		foreach (AudioClip clip in clipList) 
		{
			stringAudioClipMap.Add (clip.name, clip);
		}

	}




	public void PlaySound(string soundName, int numberOfPlays)
	{

		Debug.Log ("play sound " + soundName);

		if (numberOfPlays == 0) 
		{
			audioSources_loop[nextAudioSource_loop].clip = stringAudioClipMap [soundName];
			StartCoroutine(PlaySoundList (audioSources_loop [nextAudioSource_loop], numberOfPlays));

			nextAudioSource_loop ++;

		} else {

			audioSources[nextAudioSource].clip = stringAudioClipMap [soundName];
			StartCoroutine(PlaySoundList (audioSources [nextAudioSource], numberOfPlays));

			nextAudioSource ++;
		}
	}



	IEnumerator PlaySoundList(AudioSource audioSource, int n)
	{

		if (n == 0) 
		{				
			Debug.Log ("n = 0");

			audioSource.loop = true;
			audioSource.Play ();

			yield return null;
		
		} else {

			Debug.Log ("n != 0");

			int i = 0;

			while (i < n) 
			{
				audioSource.Play ();
				i++;

				yield return new WaitForSeconds (audioSource.clip.length);
			}
		}
	}




	public void SetSound(bool soundOff)
	{
		Debug.Log ("SetSound");

		foreach (AudioSource audioSource in audioSources) 
		{
			audioSource.mute = soundOff;

		}
	
		foreach (AudioSource audioSource in audioSources_loop) 
		{
			audioSource.mute = soundOff;

		}

		soundIsOff = soundOff;

	}



	// When leaving a room, stopping loop sounds


	public void StopLoopSounds(float fadeSpeed)
	{
		
		StartCoroutine(FadeOutLoopSounds(fadeSpeed));

	}


	IEnumerator FadeOutLoopSounds(float fadeSpeed)
	{
		float i = 1;

		while (i > 0) 
		{	
			foreach (AudioSource audioSource in audioSources_loop) 
			{
				audioSource.volume = i;				
			}

			i -= Time.deltaTime / fadeSpeed;

			yield return new WaitForFixedUpdate ();
		}

		foreach (AudioSource audioSource in audioSources_loop) 
		{
			audioSource.Stop();
		}
	}








	// ------------- STATIC EVENTS ------------- //



	public static Action<string,int> cb_playSound; 

	public static void Invoke_cb_playSound(string soundName, int numberOfPlays)
	{
		if(cb_playSound != null)
		{
			cb_playSound (soundName,numberOfPlays);
		}

	}



	public static Action<float> cb_leftRoom_start;

	public static void Invoke_cb_leftRoom_start(float speed)
	{
		if(cb_leftRoom_start != null)
		{
			cb_leftRoom_start (speed);
		}

	}





}






