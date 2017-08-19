using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {

	void OnTriggerEnter(Collider col){
		print (col.name+" collided with the ground");
		col.transform.gameObject.SendMessage ("OnTouchGround", SendMessageOptions.DontRequireReceiver);
	}
}
