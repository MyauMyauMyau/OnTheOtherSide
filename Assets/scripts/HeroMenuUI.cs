using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.scripts;
using UnityEngine.UI;

public class HeroMenuUI : MonoBehaviour
{

	public GameObject[] Heroes;
	public GameObject HeroPanel;
	public GameObject SkillPanel;
	public Sprite[] HeroSprites;
	public Sprite[] SkillSprites;
	private string heroesInfo;
	// Use this for initialization
	void Start ()
	{
		heroesInfo = PlayerPrefs.GetString("Heroes");
		for (int i = 0; i < 6; i++)
		{
			if (heroesInfo[i] == '0')
			{
				Heroes[i].GetComponent<Button>().image.color = Color.gray;
				foreach (var skillButton in Heroes[i].transform.parent.GetComponentsInChildren<Button>().Skip(1))
				{
					skillButton.image.color = Color.gray;
					skillButton.interactable = false;
				}
			}
			else
			{
				Heroes[i].GetComponent<Button>().image.color = Color.white;
				foreach (var skillButton in Heroes[i].transform.parent.GetComponentsInChildren<Button>().Skip(1))
				{
					skillButton.image.color = Color.white;
					skillButton.interactable = true;
				}
			}	
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnHeroClick(int herotype)
	{
		if (heroesInfo[herotype] == '0')
			AdoptHeroShop(herotype);
		else
		{
			Preferences.SetCurrentHero(herotype);
			MainMenu.Instance.GoToLevelMenu();
		}
	}

	private string currentSkillOnList;
	public void OnSkillClick(string skill)
	{
		SkillPanel.GetComponent<BigPlateController>().Activate();
		SkillPanel.transform.GetChild(0).GetComponent<Image>().overrideSprite = 
			SkillSprites[int.Parse(skill.ElementAt(0).ToString())*3 + int.Parse(skill.ElementAt(1).ToString())];
		SkillPanel.transform.GetChild(1).GetComponent<Text>().text = Dictionaries.SkillNames[skill];
		SkillPanel.transform.GetChild(2).GetComponent<Text>().text = Dictionaries.SkillDescriptions[skill];
		var levelOfSkill = int.Parse(PlayerPrefs.GetString
			("Skills" + skill.ElementAt(0)).ElementAt(int.Parse(skill.ElementAt(1).ToString())).ToString());
		if (levelOfSkill == 3)
		{
			SkillPanel.transform.GetChild(3).GetComponent<Text>().text = "Maximum level";
			SkillPanel.transform.GetChild(5).GetComponent<Button>().interactable = false;
		}
		else
		{
			SkillPanel.transform.GetChild(3).GetComponent<Text>().text = "Upgrade for " +
			                                                             Dictionaries.SkilPrices[skill + levelOfSkill] +
			                                                             " coins.";
			if (Dictionaries.SkilPrices[skill + levelOfSkill] > PlayerPrefs.GetInt("Gold"))
				SkillPanel.transform.GetChild(5).GetComponent<Button>().interactable = false;
			else
				SkillPanel.transform.GetChild(5).GetComponent<Button>().interactable = true;
		}
		currentSkillOnList = skill + levelOfSkill;
	}

	private int currentHeroOnList;
	private void AdoptHeroShop(int herotype)
	{
		HeroPanel.GetComponent<BigPlateController>().Activate();
		HeroPanel.transform.GetChild(0).GetComponent<Image>().overrideSprite = HeroSprites[herotype];
		HeroPanel.transform.GetChild(1).GetComponent<Text>().text = Dictionaries.HeroNames[(HeroType) herotype];
		HeroPanel.transform.GetChild(2).GetComponent<Text>().text = Dictionaries.HeroDescriptions[(HeroType)herotype];
		HeroPanel.transform.GetChild(3).GetComponent<Text>().text = "Buy for "  + 
			Dictionaries.HeroPrices[(HeroType)herotype] + " coins";
		if (Dictionaries.HeroPrices[(HeroType) herotype] > PlayerPrefs.GetInt("Gold"))
			HeroPanel.transform.GetChild(4).GetComponent<Button>().interactable = false;
		else
		{
			HeroPanel.transform.GetChild(4).GetComponent<Button>().interactable = true;
		}
		HeroPanel.transform.GetChild(4).GetComponent<Button>();
		currentHeroOnList = herotype;
	}

	public void BuySkill()
	{
		PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") - Dictionaries.SkilPrices[currentSkillOnList]);
		PlayerPrefs.SetString("Skills" + currentSkillOnList.ElementAt(0), 
			PlayerPrefs.GetString("Skills" + currentSkillOnList.ElementAt(0)).ReplaceAt(int.Parse(currentSkillOnList.ElementAt(1).ToString()), 1, (int.Parse(currentSkillOnList.ElementAt(2).ToString()) + 1).ToString()));
		PlayerPrefs.Save();
		foreach (var skillLevel in GameObject.FindObjectsOfType(typeof(SkillLevel)))
		{
			(skillLevel as SkillLevel).Reload();
		}
		Start();
	}

	public void BuyHero()
	{
		PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") - Dictionaries.HeroPrices[(HeroType)currentHeroOnList]);
		PlayerPrefs.SetString("Heroes", heroesInfo.ReplaceAt(currentHeroOnList, 1, "1"));
		PlayerPrefs.SetString("Skills" + currentHeroOnList, "111");
		PlayerPrefs.Save();
		foreach (var skillLevel in GameObject.FindObjectsOfType(typeof(SkillLevel)))
		{
			(skillLevel as SkillLevel).Reload();
		}
		Start();	
	}
}
