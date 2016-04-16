using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

	public static Camera MainCamera;
	public static Camera AchievementsCamera;
	// Use this for initialization
	void Start()
	{
		MainCamera = GameObject.Find("Menu").GetComponentInChildren<Camera>();
		AchievementsCamera = GameObject.Find("AchievementsMenu").GetComponentInChildren<Camera>();
		AchievementsCamera.enabled = false;
		Debug.Log("init");
	}

	void Update()
	{
		if (MainCamera.enabled && Input.GetKeyDown(KeyCode.Escape))
			Quit();
		if (AchievementsCamera.enabled && Input.GetKeyDown(KeyCode.Escape))
		{
			GoToMainMenu();
		}
	}
	// Update is called once per frame
	public void Quit()
	{
		Debug.Log("q");
		Application.Quit();
	}

	public void Play()
	{
		SceneManager.LoadScene("game");
	}

	public void GoToAchievements()
	{
		MainCamera.enabled = false;
		AchievementsCamera.enabled = true;
	}

	public void GoToMainMenu()
	{
		MainCamera.enabled = true;
		AchievementsCamera.enabled = false;
	}
}
