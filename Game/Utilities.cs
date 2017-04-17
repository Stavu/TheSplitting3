using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities {


	public static int myVar;
	public int myVar2;
	int myVar3; 


	public static void AdjustOrthographicCamera(Room room)
	{

		Camera.main.orthographicSize = 12; 

		Camera.main.transform.position = new Vector3 ((room.myWidth / 2), (room.myHeight / 2), -10);


	}


	public static Vector2 GetObjectScreenPosition (Vector3 myPos, RectTransform myTransform, float offsetX, float offsetY)
	{

		Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(myPos);
		Vector2 WorldObject_ScreenPosition = new Vector2
			(
				((ViewportPosition.x * myTransform.sizeDelta.x) - (myTransform.sizeDelta.x*0.5f)) + offsetX,
				((ViewportPosition.y * myTransform.sizeDelta.y) - (myTransform.sizeDelta.y*0.5f)) + offsetY
			);

		return WorldObject_ScreenPosition;

	}




}
