using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

	public Canvas MainMenuCanvas;
	public Canvas LevelMenuCanvas;
	public Canvas HeroMenuCanvas;
	public Button SoundButton;
	// Use this for initialization
	void Start()
	{
		LevelMenuCanvas.enabled = false;
		HeroMenuCanvas.enabled = true;
		if (PlayerPrefs.GetInt("FromGame") == 1)
		{
			PlayerPrefs.SetInt("FromGame", 0);
			GoToLevelMenu();
		}
	}

	void Update()
	{
	}
	// Update is called once per frame
	public void Quit()
	{
		Application.Quit();
	}

	public void GoToMainMenu()
	{
		MainMenuCanvas.enabled = true;
		LevelMenuCanvas.enabled = false;
		HeroMenuCanvas.enabled = false;
	}

	public void GoToLevelMenu()
	{
		MainMenuCanvas.enabled = false;
		LevelMenuCanvas.enabled = true;
		HeroMenuCanvas.enabled = false;
	}

	public void GoToHeroMenu()
	{
		MainMenuCanvas.enabled = false;
		LevelMenuCanvas.enabled = false;
		HeroMenuCanvas.enabled = true;
	}
	public static void Play()
	{
		SceneManager.LoadScene("game");
	}

	public void SwitchSound()
	{
		
		if (PlayerPrefs.GetInt("Sound") == 0)
		{
			PlayerPrefs.SetInt("Sound", 1);
			SoundButton.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("ButtonsSprites/soundOn");
		}
		else
		{
			PlayerPrefs.SetInt("Sound", 0);
			SoundButton.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("ButtonsSprites/soundOff");
		}
		GameObject.Find("Menu").GetComponent<AudioSource>().volume = PlayerPrefs.GetInt("Sound")/2f;
		PlayerPrefs.Save();
	}
}
