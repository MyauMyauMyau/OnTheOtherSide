using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.scripts;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameUI : MonoBehaviour
{

	public Button SoundButton;
	// Use this for initialization
	void Start () {
		SetSoundButton();
		UpdateTurnsLeft();
		UpdateGold();
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
