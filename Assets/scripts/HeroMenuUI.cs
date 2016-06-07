using UnityEngine;
using System.Collections;
using Assets.scripts;
using UnityEngine.UI;

public class HeroMenuUI : MonoBehaviour
{

	public Button DeathButton;
	public Button HunterButton;
	public Button ClericButton;
	private string heroesInfo;
	// Use this for initialization
	void Start ()
	{
		heroesInfo = PlayerPrefs.GetString("Heroes");
	}
	
	// Update is called once per frame
	void Update () {
		heroesInfo = PlayerPrefs.GetString("Heroes");
		DeathButton.image.color = heroesInfo[5] == '0' ? Color.gray : Color.white;
		HunterButton.image.color = heroesInfo[0] == '0' ? Color.gray : Color.white;
		ClericButton.image.color = heroesInfo[1] == '0' ? Color.gray : Color.white;
	}

	public void OnHeroClick(int herotype)
	{
		if (heroesInfo[herotype] == '0')
			BuyHero((HeroType)herotype);
		else
		{
			Preferences.SetCurrentHero(herotype);
			MainMenu.Play();
		}
		
	}

	void BuyHero(HeroType heroType)
	{
		PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") - Dictionaries.HeroPrices[heroType]);
		PlayerPrefs.SetString("Heroes", heroesInfo.ReplaceAt((int)heroType, 1, "1"));	
	}
}
