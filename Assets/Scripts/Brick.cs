using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour {

	public GameObject particleEmitter;
	public float delayBeforeDestruction = 0.1f;

	void OnCollisionEnter(Collision col){
		print (transform.name + " has been collided with by " + col.gameObject.name);

		StartCoroutine(Die ());
	}

	void OnParticleCollision(GameObject other){
		if(gameObject.active == true)
			StartCoroutine(Die ());
	}

	IEnumerator Die(){
		yield return new WaitForSeconds (delayBeforeDestruction);

		if(particleEmitter != null){ 
			particleEmitter.transform.SetParent (null);
			particleEmitter.transform.localScale = new Vector3 (1f,1f,1f);
			particleEmitter.SetActive (true);

			print ("particle emitter: " + particleEmitter.name + " has been enabled");
		}
		print (transform.name + " has been disabled");
		transform.gameObject.SetActive (false);
	}
}
