using UnityEngine;
using System.Collections;


public class Ball : MonoBehaviour {



	void OnTouchGround(){
		print ("The ball has hit the ground and must be reset");
		EventManager.instance.BallDeath ();
	}

	void OnCollisionEnter(){
		EventManager.instance.BallCollision ();
	}
}
