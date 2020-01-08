using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour {

    //Loads intro scene to move to game, sets to high round
    public void StartGameRoundFive()
    {
        PlayerPrefs.SetInt("StartRound", 5);
        SceneManager.LoadScene("IntroScene");
    }

	//Loads intro scene to move to game scene
    public void StartGame()
    {
        PlayerPrefs.SetInt("StartRound", 0);
        SceneManager.LoadScene("IntroScene");
    }

    //Loads the actual game scene
    public void BeginPlay()
    {
        SceneManager.LoadScene("GameScene");
    }

    //Returns to the main menu
    public void MainMenu()
    {
        SceneManager.LoadScene("OpenScene");
    }
	
	//Quits the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
