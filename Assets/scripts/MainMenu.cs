using UnityEngine;
using System.Collections;
using System.Threading;
using Assets.scripts;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

	public Canvas MainMenuCanvas;
	public Canvas LevelMenuCanvas;
	public Canvas HeroMenuCanvas;
	public Button SoundButton;
	public Button SoundButton2;
	public Button SoundButton3;
	public static MainMenu Instance;
	public bool IsPaused;

	void Awake()
	{
		LevelGrave.Candle = Resources.Load("objects/graveCandle/candle", typeof(GameObject)) as GameObject;
		SkillLevel.Level1Sprite = Resources.Load("objects/skillsLevel/lvl1", typeof(Sprite)) as Sprite;
		SkillLevel.Level2Sprite = Resources.Load("objects/skillsLevel/lvl2", typeof(Sprite)) as Sprite;
		SkillLevel.Level3Sprite = Resources.Load("objects/skillsLevel/lvl3", typeof(Sprite)) as Sprite;
	}
	// Use this for initialization
	void Start()
	{
		Instance = this;
		LevelMenuCanvas.enabled = false;
		HeroMenuCanvas.enabled = true;
		SwitchSound();
		SwitchSound();
		if (PlayerPrefs.GetInt("FromGame") == 1)
		{
			PlayerPrefs.SetInt("FromGame", 0);
			PlayerPrefs.Save();
			GoToLevelMenu();
		}
	}

	void Update()
	{
		if (IsPaused) return;
		if (Input.GetKeyDown(KeyCode.Escape))
			if (MainMenuCanvas.enabled)
				Application.Quit();
			else if (LevelMenuCanvas.enabled)
				GoToHeroMenu();
			else if (HeroMenuCanvas.enabled)
				GoToMainMenu();

	}

	public void GoBack()
	{
		if (MainMenuCanvas.enabled)
			Application.Quit();
		else if (LevelMenuCanvas.enabled)
			GoToHeroMenu();
		else if (HeroMenuCanvas.enabled)
			GoToMainMenu();
	}
	// Update is called once per frame
	public void Quit()
	{
		Application.Quit();
	}

	public void GoToMainMenu()
	{
		AudioHolder.PlayMenuTheme();
		MainMenuCanvas.enabled = true;
		LevelMenuCanvas.enabled = false;
		HeroMenuCanvas.enabled = false;
	}

	public void GoToLevelMenu()
	{
		AudioHolder.PlayMainTheme();
		MainMenuCanvas.enabled = false;
		LevelMenuCanvas.enabled = true;
		HeroMenuCanvas.enabled = false;
	}

	public void GoToHeroMenu()
	{
		AudioHolder.PlayMainTheme();
		MainMenuCanvas.enabled = false;
		LevelMenuCanvas.enabled = false;
		HeroMenuCanvas.enabled = true;
	}
	public void Play()
	{
		var loadingIconPrefab = Resources.Load("objects/loading/Loading", typeof(GameObject)) as GameObject;
		var loading = ((GameObject) Instantiate(
			loadingIconPrefab, new Vector3(Screen.width/2, Screen.height/2), 
			Quaternion.Euler(new Vector3())));
		loading.transform.parent = GameObject.Find("LevelMenu").transform;
		loading.transform.localScale = new Vector3(1,1);
		SceneManager.LoadSceneAsync("game");
	}

	public void SwitchSound()
	{
		
		if (PlayerPrefs.GetInt("Sound") == 0)
		{
			PlayerPrefs.SetInt("Sound", 1);
			SoundButton.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("ButtonsSprites/soundOn");
			SoundButton2.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("ButtonsSprites/soundOn");
			SoundButton3.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("ButtonsSprites/soundOn");
		}
		else
		{
			PlayerPrefs.SetInt("Sound", 0);
			SoundButton.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("ButtonsSprites/soundOff");
			SoundButton2.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("ButtonsSprites/soundOff");
			SoundButton3.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("ButtonsSprites/soundOff");
		}
		GameObject.Find("AudioHolder").GetComponents<AudioSource>()[0].volume = PlayerPrefs.GetInt("Sound")/2f;
		GameObject.Find("AudioHolder").GetComponents<AudioSource>()[1].volume = PlayerPrefs.GetInt("Sound") / 2f;
		PlayerPrefs.Save();
	}
}
