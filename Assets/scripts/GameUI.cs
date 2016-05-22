using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.scripts;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameUI : MonoBehaviour
{
	public GameObject Hero;
	public GameObject HunterPrefab;
	public GameObject ClericPrefab;
	public GameObject MummyPrefab;
	public GameObject WolverinePrefab;
	public GameObject VampirePrefab;
	public GameObject DeathPrefab;
	public Button SoundButton;
	public static GameUI Instance;
	// Use this for initialization
	void Start ()
	{
		Instance = this;
		Dictionaries.HeroTypesToPrefabs = new Dictionary<HeroType, GameObject>
		{
			{HeroType.Death, GameUI.Instance.DeathPrefab},
			{HeroType.Hunter, GameUI.Instance.HunterPrefab},
			{HeroType.Cleric, GameUI.Instance.ClericPrefab},
			{HeroType.Mummy, GameUI.Instance.MummyPrefab},
			{HeroType.Vampire, GameUI.Instance.VampirePrefab},
			{HeroType.Wolverine, GameUI.Instance.WolverinePrefab},
		};
		Hero =
			(GameObject)
				Instantiate(Dictionaries.HeroTypesToPrefabs[(HeroType) PlayerPrefs.GetInt("CurrentHero")], new Vector3(-3.2f, 5.3f),
					Quaternion.Euler(new Vector3(0, 0)));
		;
		Debug.Log(Hero.ToString());
		SetSoundButton();
		UpdateTurnsLeft();
		UpdateGold();
		SetTargets();
		UpdateLives();
	}

	private void UpdateLives()
	{
		GetComponentsInChildren<Text>()[6].text = PlayerPrefs.GetInt("Lives").ToString();
	}

	private void SetTargets()
	{
		for (var i = 0; i < Game.LevelInformation.Targets.Count; i++)
		{
			GetComponentsInChildren<Image>()[i + 5].sprite = Dictionaries.TypesToSprites[Dictionaries.CharsToObjectTypes[Game.LevelInformation.Targets.ElementAt(i).Key]];
		}
	}

	// Update is called once per frame
	void Update () {
		
		UpdateTurnsLeft();
		UpdateTargets();
	}

	void UpdateTurnsLeft()
	{
		GetComponentsInChildren<Text>()[0].text = Game.TurnsLeft.ToString();
	}

	void UpdateGold()
	{
		GetComponentsInChildren<Text>()[1].text = PlayerPrefs.GetInt("Gold").ToString();
	}

	public void GoToMenu()
	{
		PlayerPrefs.SetInt("FromGame", 1);
		SceneManager.LoadScene("MainMenu");
	}

	public void UpdateTargets()
	{
		for (var i = 0; i < Game.LevelInformation.Targets.Count ; i++)
		{
			var currentMonsters = Dictionaries.MonsterCounter[Dictionaries.CharsToObjectTypes[Game.LevelInformation.Targets.ElementAt(i).Key]];
			var totalMonsters = Game.LevelInformation.Targets.ElementAt(i).Value;
			var monstersLeft = totalMonsters - currentMonsters;
			if (monstersLeft < 0)
				monstersLeft = 0;
			GetComponentsInChildren<Text>()[i +2].text = monstersLeft.ToString();
			if (monstersLeft == 0)
				GetComponentsInChildren<Text>()[i + 2].color = Color.green;
		}

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
		GameObject.Find("GameManager").GetComponentInChildren<AudioSource>().volume = PlayerPrefs.GetInt("Sound") / 2f;
		PlayerPrefs.Save();
	}

	public void SetSoundButton()
	{
		if (PlayerPrefs.GetInt("Sound") == 1)
		{
			SoundButton.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("ButtonsSprites/soundOn");
		}
		else
		{
			SoundButton.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("ButtonsSprites/soundOff");
		}
	}
}
