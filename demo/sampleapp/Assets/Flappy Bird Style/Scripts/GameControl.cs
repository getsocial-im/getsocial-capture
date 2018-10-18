using System;
using System.Linq;
using GetSocialSdk.Capture.Scripts;
using GetSocialSdk.Core;
using GetSocialSdk.Ui;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour 
{
	public static GameControl instance;			//A reference to our game control script so we can access it statically.
	public Text scoreText;						//A reference to the UI text component that displays the player's score.
	public GameObject gameOvertext;				//A reference to the object that displays the text which appears when the player dies.

	private int score = 0;						//The player's score.
	public bool gameOver = false;				//Is the game over?
	public float scrollSpeed = -5.0f;

	public GetSocialCapturePreview capturePreview;
	private GetSocialCapture _capture;


//	private bool startedFlag = false;
	void Awake()
	{
		//If we don't currently have a game control...
		if (instance == null)
			//...set this one to be it...
			instance = this;
		//...otherwise...
		else if(instance != this)
			//...destroy this one because it is a duplicate.
			Destroy (gameObject);
		
		gameOvertext.SetActive(false);

		_capture = GetComponent<GetSocialCapture>();
	}

	private void Start()
	{
		_capture.StartCapture();
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		gameOvertext.SetActive(false);
	}

	public void ShareResult()
	{
		Debug.Log("Starting gif generation");
		Action<byte[]> result = bytes =>
		{
			
		};  
		_capture.GenerateCapture(result);
	}

	public void BirdScored()
	{
		//The bird can't score if the game is over.
		if (gameOver)	
			return;
		//If the game is not over, increase the score...
		score++;
		//...and adjust the score text.
		scoreText.text = "Score: " + score.ToString();
	}

	public void BirdDied()
	{
		//Activate the game over text.
		gameOvertext.SetActive (true);
		//Set the game to be over.
		gameOver = true;

		// stop recording
		_capture.StopCapture();
	 	capturePreview.Play();			
	}
}
