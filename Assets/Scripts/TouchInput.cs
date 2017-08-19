using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchInput : MonoBehaviour
{

	public GameObject paddle;
	public LayerMask touchInputMask;

	public delegate void TouchAction (Vector3 contactPositionInPixels);

	public static event TouchAction OnContact;
	public static event TouchAction StopContact;

	private List<GameObject> touchList = new List<GameObject> ();
	private GameObject[] touchesOld;
	private RaycastHit hit;
	private Camera camera;

	void Awake ()
	{
		camera = GetComponent<Camera> ();
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
#if UNITY_EDITOR
			

		if (Input.GetMouseButtonDown (0) || Input.GetMouseButton(0)) {
			Ray ray = camera.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit, touchInputMask)) {

				GameObject recipient = hit.transform.gameObject;

				if (Input.GetMouseButtonDown (0)) {
					recipient.SendMessage ("OnTouchDown", Input.mousePosition, SendMessageOptions.DontRequireReceiver);
				}

				else{
					recipient.SendMessage ("OnTouchSlide", Input.mousePosition, SendMessageOptions.DontRequireReceiver);
				}
			}
			OnContact (Input.mousePosition);
		}

		else if (Input.GetMouseButtonUp(0)){
			StopContact(Input.mousePosition);
		}
		
#endif

		if (Input.touchCount > 0) {
			print ("screen is being touched");

			touchesOld = new GameObject[touchList.Count];
			touchList.CopyTo (touchesOld);
			touchList.Clear ();

			//this game only takes into account one touch at a time
			Touch touch = Input.touches [0];
			print (touch.position);

			Ray ray = camera.ScreenPointToRay (touch.position);


			if (Physics.Raycast (ray, out hit, touchInputMask)) {

				GameObject recipient = hit.transform.gameObject;

				if (touch.phase == TouchPhase.Began) {
					print ("OnTouchDown");
					recipient.SendMessage ("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
				} else if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
					print ("OnTouchStay");
					recipient.SendMessage ("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
				} 

			}

			if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
				StopContact (Input.mousePosition);
			} else {
				OnContact (touch.position);
			}

			//logic for user's finger sliding off the screen
			foreach (GameObject g in touchesOld) {
				if (!touchList.Contains (g)) {
					print ("Touch slid off");
					StopContact(Input.mousePosition);
				}
			}

		}	
	}
}
