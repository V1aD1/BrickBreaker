using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class RestartButton : MonoBehaviour {
	void OnTouchDown(Vector3 currentPosInPixels){
		print ("restarting scene");
		Application.LoadLevel(Application.loadedLevel);
	}
}
