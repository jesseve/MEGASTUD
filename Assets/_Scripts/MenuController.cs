using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MenuState {
	None = -1,
	MainMenu,
	Options,
	Credits
}

public class MenuController : MonoBehaviour {

	private MenuState currentState = MenuState.None;
	private Stack<MenuState> states;
	public GameObject[] menupanels;

	// Use this for initialization
	void Start () {
		states = new Stack<MenuState> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeState(int index) {
		ChangeState ((MenuState)index);
	}

	public void ChangeState(MenuState newState) {
		if (newState = currentState) {
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
		switch (currentState) 


	}
}
