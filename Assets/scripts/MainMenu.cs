using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
		if (PlayerPrefs.GetInt("FromGame") == 1)
		{
			PlayerPrefs.SetInt("FromGame", 0);
			GoToLevelMenu();
		}
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
		Application.Quit();
	}

	public void Play()
	{
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

	public void SwitchSound()
	{
		var btn = GameObject.Find("Menu").transform.Find("Canvas").transform.Find("Sound");
		if (PlayerPrefs.GetInt("Sound") == 0)
		{
			PlayerPrefs.SetInt("Sound", 1);
			btn.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("ButtonsSprites/soundOn");
		}
		else
		{
			PlayerPrefs.SetInt("Sound", 0);
			btn.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("ButtonsSprites/soundOff");
		}
		GameObject.Find("Menu").GetComponent<AudioSource>().volume = PlayerPrefs.GetInt("Sound")/2f;
		PlayerPrefs.Save();
	}
}
