using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum MenuState {
	None = -1,
	MainMenu,
	Options,
	Credits
}

public class MenuController : MonoBehaviour {

	public string levelName = "_DebugJukka";

	private MenuState currentState = MenuState.None;
	private Stack<MenuState> states;
	public GameObject[] menupanels;
	public GameObject quitPopUp;

	// Use this for initialization
	void Start () {
		states = new Stack<MenuState> ();
		ChangeState(MenuState.MainMenu);
		HandleQuitPopUp(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeState(int index) {
		ChangeState ((MenuState)index);
	}

	public void ChangeState(MenuState newState) {
		if (newState == currentState) {
			return;
		}
		states.Push (currentState);
		currentState = newState;
		HandlePanels ();
	}

	public void Back() {
		if (states.Count > 1) {
			currentState = states.Pop ();
			HandlePanels ();
		}
	}

	private void HandlePanels() {
		switch (currentState) {
		//Add the functions you want to be called when the state is activated here
		}

		for (int i = 0; i < menupanels.Length; i++) {
			menupanels[i].SetActive(i == (int)currentState);
		}


	}

	public void HandleQuitPopUp(bool active) {
		quitPopUp.SetActive(active);
	}

	public void QuitGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.QuitGame();
#endif
	}

	public void StartGame() {
		SceneManager.LoadScene(levelName);
	}
}
