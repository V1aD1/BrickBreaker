using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {

	public int score = 100;
	public int ballCollisionScoreDecrease = 1;
	public int ballShootScoreDecrease = 5;

	//when the ball goes underneath the paddle, it dies
	public int ballDeathScoreDecrease = 5;

	//when the ball dies, it's automatically reset, so points are lost here also
	public int ballResetScoreDecrease = 5;
	public Text scoreText;

	public GameObject paddle;



	public List<GameObject> allBricksInLevel;

	public GameObject EndMessage;

	public string nextLevelName;



	public static EventManager instance = null;

	// Use this for initialization
	void Start () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		scoreText.text = score.ToString();

		bool completedLevel = true;

		foreach(GameObject brick in allBricksInLevel){
			if (brick.activeSelf) {
				completedLevel = false;
			}
		}

		if (completedLevel) {
			GoToNextLevel ();
		}
	}

	//this function decreases score every time the ball collides with something 
	public void BallCollision(){
		score -= ballCollisionScoreDecrease;
	}

	//this function decreases score every time the ball is shot
	public void BallShoot(){
		score -= ballShootScoreDecrease;
	}

	//this function decreases score every time the ball dies,
	//meaning that the score decreases by ballDeathScoreDecrease and
	//by ballResetScoreDecrease;
	public void BallDeath(){
		score -= ballDeathScoreDecrease;
		paddle.GetComponent<Paddle> ().ResetBall ();
		score -= ballResetScoreDecrease;
	}

	void GoToNextLevel(){

		if (nextLevelName == "Main Menu") {
			ShowThankYouMessage ();
		}

		if (nextLevelName != null) {
			Application.LoadLevel (nextLevelName);
		} 

	}

	public void RestartLevel(){
		print ("restarting scene");
		Application.LoadLevel(Application.loadedLevel);
	}

	//this function will display a thank you before returning to the main menu
	void ShowThankYouMessage(){


	}

}
