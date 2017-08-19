using UnityEngine;
using System.Collections;

public class TutorialPopup : MonoBehaviour {

	void OnEnable()
	{
		TouchInput.OnContact += HandleContact;
	}

	void OnDisable()
	{
		TouchInput.OnContact -= HandleContact;
	}

	void HandleContact(Vector3 contactPositionInPixels){
		transform.gameObject.SetActive (false);
	}
}
