using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {

	public GameObject paddleBody;
	public GameObject ball;
	public float ballSpeed = 10f;
	public float paddleSpeed = 0.1f;
	public Camera camera;
	public GameObject trajectorySprite;

	private bool holdingBall = true;

	private Rigidbody ballRb;

	private bool currentlySelected = false;

	private Vector3 positionInPixels;

	private Vector3 localBallPos = new Vector3 (0f, 3.07f, 0f);

	private float maxRotation = 90f;

	private float trajectoryRatio = 0f;

	// Use this for initialization
	void Awake () {
		ballRb = ball.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		positionInPixels = camera.WorldToScreenPoint (transform.position);
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

	void HandleContact(Vector3 contactPositionInPixels){

		//set trajectory for the ball
		if (currentlySelected && holdingBall) {
			print ("user is holding on to paddle");

			trajectorySprite.SetActive (true);

			//this function assumes the trajectory sprite should be rotated about the Z axis
			//in order to map trajectory, the function followed is:
			//(x position of touch in percentage of screen width) * 180 - 90,
			//in this way: 
			//if user is touching the middle of the screen, the sprite will be rotated 0 degrees,
			//if user is touching leftmost point of the screen, the sprite will be rotated -90 degrees,
			//if user is touching rightmost point of the screen, the sprite will be rotated 90 degrees 
			trajectoryRatio = (contactPositionInPixels.x/ Screen.width)*180f - 90f;

			Vector3 currRot = trajectorySprite.transform.localRotation.eulerAngles;

			trajectorySprite.transform.localRotation = Quaternion.Euler(new Vector3(currRot.x, currRot.y, trajectoryRatio));
		} 

		//only move the paddle if the user is not directly touching it
		else {
			MovePaddle (contactPositionInPixels);
		}
	}

	//determines if paddle should be moved left or right
	void MovePaddle(Vector3 contactPositionInPixels){
		if (contactPositionInPixels.x < positionInPixels.x)
			MoveLeft (contactPositionInPixels);
		else if (contactPositionInPixels.x >= positionInPixels.x)
			MoveRight (contactPositionInPixels);
	}

	//moves paddle to the left
	void MoveLeft(Vector3 contactPositionInPixels){
		float xPos = transform.position.x - paddleSpeed;
		transform.position = new Vector3 (Mathf.Max(xPos, camera.ScreenToWorldPoint(contactPositionInPixels).x), 
											transform.position.y, 
											transform.position.z);
	}

	//moves paddle to the right
	void MoveRight(Vector3 contactPositionInPixels){
		float xPos = transform.position.x + paddleSpeed;
		transform.position = new Vector3(Mathf.Min(xPos, camera.ScreenToWorldPoint(contactPositionInPixels).x), 
											transform.position.y, 
											transform.position.z);
	}


	void OnTouchDown(){
		print ("paddle was touched");
		currentlySelected = true;
	}

	void OnTouchSlide(){

	}

	//this function handles deselection (if the paddle was selected in the first place)
	void Deselect(Vector3 contactPositionInPixels ){
		print ("deselecting the paddle");

		//this means that the user has selected the paddle and let go of the screen,
		//meaning the ball is ready to be launched in the desired location
		if (currentlySelected) {
			if (holdingBall) {
				ball.transform.parent = null;
				holdingBall = false;

				ballRb.isKinematic = false;

				float multiplier = 1f;
				if (trajectoryRatio > 0)
					multiplier = multiplier * -1f;

				float xForce = Mathf.Cos (((90f - Mathf.Abs(trajectoryRatio))* Mathf.PI)/180f) * ballSpeed;
				float yForce = Mathf.Sin (((90f - Mathf.Abs(trajectoryRatio))* Mathf.PI)/180f) * ballSpeed;

				ballRb.AddForce (new Vector3 (multiplier*xForce, Mathf.Abs(yForce), 0));
				EventManager.instance.BallShoot();
			}
		}

		currentlySelected = false;
		trajectorySprite.SetActive (false);
	}

	public void ResetBall(){
		print ("resetting the ball");
		ball.GetComponent<Rigidbody> ().isKinematic = true;
		ball.transform.SetParent (paddleBody.transform);
		ball.transform.localPosition = localBallPos;
		holdingBall = true;
	}
}
