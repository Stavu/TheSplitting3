using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene  {


	public string myName;
	public bool isClearToContinue;
	public virtual IEnumerator MyCutscene()
	{
		return null;
	}

	public Cutscene (string myName)
	{
		this.myName = myName;
	}

}




public class DanielScene : Cutscene
{
	public DanielScene (string myName) : base (myName)
	{

		EventsHandler.cb_dialogueEnded += (() => isClearToContinue = true);
		EventsHandler.cb_characterFinishedPath += (() => isClearToContinue = true);

	}


	public override IEnumerator MyCutscene()
	{

		Debug.Log ("my cutscene");

		// Declerations

		Character llehctiM = CharacterManager.instance.GetCharacterByName ("llehctiM");

		// dialogue between daniel and llehctiM

		InteractionManager.instance.DisplayDialogueOption ("daniel_scene_dialogue1");

		/////////// 

		isClearToContinue = false;

		while (isClearToContinue == false) 
		{
			yield return new WaitForFixedUpdate ();
		}

		///////////
		yield return new WaitForSeconds (2);
		///////////

		// llechtiM walks somewhere

		List<Vector2> llehctiMPosList = new List<Vector2> 
		{
			new Vector2 (5, 12),
			new Vector2 (5, 13),
			new Vector2 (5, 10)
		};

		CharacterManager.instance.MoveByPath (llehctiM, llehctiMPosList);

		/////////// 

		isClearToContinue = false;

		while (isClearToContinue == false) 
		{
			yield return new WaitForFixedUpdate ();
		}

		///////////
		yield return new WaitForSeconds (1);
		///////////

		SoundManager.Invoke_cb_playSound ("pottery_break", 1);

		///////////
		yield return new WaitForSeconds (2);
		///////////

		FurnitureManager.instance.SetFurnitureAnimationState ("asylumGate", "Opening");

		///////////
		yield return new WaitForSeconds (2);
		///////////

		// End of Cutscne

		Debug.Log ("end of cutscene");

		CutsceneManager.inCutscene = false;
		EventsHandler.Invoke_cb_inputStateChanged ();

	}


}