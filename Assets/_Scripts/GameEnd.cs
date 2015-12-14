using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour {

    public GameObject gameOverScreen;
    public Button restart;
    public Button nextLevel;
    public Button mainMenu;

	// Use this for initialization
	void Awake () {

        if (gameOverScreen == null)
            gameOverScreen = GameObject.Find("GameOverScreen");
        gameOverScreen.SetActive(false);
	}
	
    public void EndGame(bool win) {
        gameOverScreen.SetActive(true);
        Debug.Log("ENDGAME: " + win);
    }
}
