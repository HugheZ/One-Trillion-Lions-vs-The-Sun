using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreFiller : MonoBehaviour {

    public Text scoreText; //text on the UI for score

	// Use this for initialization
	void Start () {
        scoreText.text = string.Format("Final Score: {0:00000000}\nKill Totals:\n" +
            "Bigsteroid => {1:00000000}\n" +
            "Smallsteroid => {2:00000000}\n" +
            "Star => {3:00000000}", 
            PlayerPrefs.GetInt("points"), PlayerPrefs.GetInt("bigPoints"), PlayerPrefs.GetInt("smallPoints"), PlayerPrefs.GetInt("starPoints"));
	}
}
