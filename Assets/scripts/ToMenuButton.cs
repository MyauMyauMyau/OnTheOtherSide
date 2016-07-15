using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ToMenuButton : MonoBehaviour
{

	public static ToMenuButton Instance;
	// Use this for initialization
	void Start ()
	{
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GoToMenu()
	{
		PlayerPrefs.SetInt("FromGame", 1);
		PlayerPrefs.Save();
		SceneManager.LoadScene("MainMenu");
	}
}
