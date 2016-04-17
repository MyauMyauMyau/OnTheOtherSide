using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ToMenuButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GoToMenu()
	{
		PlayerPrefs.SetInt("FromGame", 1);
		SceneManager.LoadScene("MainMenu");
	}
}
