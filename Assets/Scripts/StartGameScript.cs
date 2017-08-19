using UnityEngine;
using System.Collections;

public class StartGameScript : MonoBehaviour {
	public void StartGame(){
		print ("restarting scene");
		Application.LoadLevel("Level One");
	}
}
