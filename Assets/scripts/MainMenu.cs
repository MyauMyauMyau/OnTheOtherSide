using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

	public static Camera MainCamera;
	public static Camera AchievementsCamera;
	public static Camera LevelCamera;
	// Use this for initialization
	void Start()
	{
		MainCamera = GameObject.Find("Menu").GetComponentInChildren<Camera>();
		AchievementsCamera = GameObject.Find("AchievementsMenu").GetComponentInChildren<Camera>();
		LevelCamera = GameObject.Find("LevelMenu").GetComponentInChildren<Camera>();
		AchievementsCamera.enabled = false;
		LevelCamera.enabled = false;
		Debug.Log("init");
	}

	void Update()
	{
		if (MainCamera.enabled && Input.GetKeyDown(KeyCode.Escape))
			Quit();
		if ((AchievementsCamera.enabled || LevelCamera.enabled) && Input.GetKeyDown(KeyCode.Escape))
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
		LevelCamera.enabled = false;
		AchievementsCamera.enabled = false;
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
		LevelCamera.enabled = false;
		AchievementsCamera.enabled = false;
	}

	public void GoToLevelMenu()
	{
		LevelCamera.enabled = true;
		MainCamera.enabled = false;
	}
}
