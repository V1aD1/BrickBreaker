using UnityEngine;
using System.Collections;

public class RotatableObject : MonoBehaviour {

	public float rotationSpeed = 360f;
	private bool currentlySelected = false;

	private Vector3 positionInPixels;

	private Vector3 startRot;

	void Awake(){
		startRot = transform.localRotation.eulerAngles;
	}

	void OnEnable()
	{
		TouchInput.OnContact += HandleContact;
		TouchInput.StopContact += Deselect;
	}

	void OnDisable()
	{
		TouchInput.OnContact -= HandleContact;
		TouchInput.StopContact -= Deselect;
	}

	void OnTouchDown(Vector3 currentPosInPixels){
		print ("brick was touched");
		startRot = transform.localRotation.eulerAngles;
		positionInPixels = currentPosInPixels;
		currentlySelected = true;
	}

	void HandleContact(Vector3 contactPositionInPixels){
		if (currentlySelected) {
			Vector3 currRot = transform.localRotation.eulerAngles;

			float rotRatio =  Mathf.Abs(Vector3.Distance(contactPositionInPixels, positionInPixels)) / Screen.width;
			rotRatio *= rotationSpeed;

			transform.localRotation = Quaternion.Euler (new Vector3 (currRot.x, currRot.y, startRot.z + rotRatio));
		}
	}

	void Deselect(Vector3 contactPositionInPixels ){
		print ("deselecting the brick");
		currentlySelected = false;
	}

}
