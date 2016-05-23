using UnityEngine;
using System.Collections;
using Assets.scripts;
using UnityEngine.UI;

public class HeroMenuUI : MonoBehaviour
{

	public Button DeathButton;
	private string heroesInfo;
	// Use this for initialization
	void Start ()
	{
		heroesInfo = PlayerPrefs.GetString("Heroes");
	}
	
	// Update is called once per frame
	void Update () {
		heroesInfo = PlayerPrefs.GetString("Heroes");
		if (heroesInfo[5] == '0')
			DeathButton.image.color = Color.gray;
		else
			DeathButton.image.color = Color.white;
	}

	public void OnHeroClick(int herotype)
	{
		if (heroesInfo[herotype] == '0')
			BuyHero(HeroType.Death);
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
