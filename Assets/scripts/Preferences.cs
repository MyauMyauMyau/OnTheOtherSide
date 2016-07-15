using UnityEngine;
using System.Collections;
using Assets.scripts;

public class Preferences : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (!PlayerPrefs.HasKey("xxx"))
		{
			PlayerPrefs.SetInt("Sound", 1);
			PlayerPrefs.SetInt("LevelUnlocked", 60);
			if (!PlayerPrefs.HasKey("FromGame"))
				PlayerPrefs.SetInt("FromGame",0);
			PlayerPrefs.SetInt("Gold", 50);
			PlayerPrefs.SetInt("Lives", 100);
			//heroes hunter, cleric, mummy, wolverine, vampire, death
			PlayerPrefs.SetString("Heroes","000000");
			PlayerPrefs.SetInt("CurrentHero", 0);
			
			//upgrades
			PlayerPrefs.SetString("Skills0", "311");
			PlayerPrefs.SetString("Skills1", "331");
			PlayerPrefs.SetString("Skills2", "333");
			PlayerPrefs.SetString("Skills3", "333");
			PlayerPrefs.SetString("Skills4", "222");
			PlayerPrefs.SetString("Skills5", "332");
			for (int i = 1; i < 4; i++)
				PlayerPrefs.SetFloat("Skill" + i, 1f);
			PlayerPrefs.Save();
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void SetCurrentLevel(int level)
	{
		PlayerPrefs.SetInt("CurrentLevel", level);
		PlayerPrefs.Save();
	}

	public static void SetCurrentHero(int herotype)
	{
		PlayerPrefs.SetInt("CurrentHero", herotype);
		PlayerPrefs.Save();
	}
}
