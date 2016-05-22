using UnityEngine;
using System.Collections;
using Assets.scripts;

public class Preferences : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (!PlayerPrefs.HasKey("CurrentHero"))
		{
			PlayerPrefs.SetInt("Sound", 1);
			PlayerPrefs.SetInt("LevelUnlocked", 2);
			PlayerPrefs.SetInt("FromGame",0);
			PlayerPrefs.SetInt("Gold", 50);
			PlayerPrefs.SetInt("Lives", 5);
			//heroes hunter, cleric, mummy, wolverine, vampire, death
			PlayerPrefs.SetString("Heroes","000000");
			PlayerPrefs.SetInt("CurrentHero", 0);
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
