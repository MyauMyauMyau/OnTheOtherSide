using UnityEngine;
using System.Collections;
using Assets.scripts;

public class Preferences : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		if (!PlayerPrefs.HasKey("xxx"))
		{
			PlayerPrefs.SetInt("Sound", 1);
			PlayerPrefs.SetInt("LevelUnlocked", 90);
			PlayerPrefs.SetString("LevelCandles", "~");	
			if (!PlayerPrefs.HasKey("FromGame"))
				PlayerPrefs.SetInt("FromGame",0);
			PlayerPrefs.SetInt("Gold", 5000);
			PlayerPrefs.SetInt("Lives", 5);
			//heroes hunter, cleric, mummy, wolverine, vampire, death
			PlayerPrefs.SetString("Heroes","100000");
			PlayerPrefs.SetInt("CurrentHero", 0);
			
			//upgrades
			PlayerPrefs.SetString("Skills0", "111");
			PlayerPrefs.SetString("Skills1", "000");
			PlayerPrefs.SetString("Skills2", "000");
			PlayerPrefs.SetString("Skills3", "000");
			PlayerPrefs.SetString("Skills4", "000");
			PlayerPrefs.SetString("Skills5", "000");
			for (int i = 1; i < 4; i++)
				PlayerPrefs.SetFloat("Skill" + i, 1f);
			PlayerPrefs.Save();
		}
	}

	void Start()
	{
		
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
