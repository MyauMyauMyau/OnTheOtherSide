using UnityEngine;
using System.Collections;

public class Preferences : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (!PlayerPrefs.HasKey("Sound"))
		{
			PlayerPrefs.SetInt("Sound", 1);
			PlayerPrefs.SetInt("LevelUnlocked", 1);
			PlayerPrefs.SetInt("Achievement1Unlocked", 0);
			PlayerPrefs.SetInt("Achievement2Unlocked", 0);
			PlayerPrefs.SetInt("Achievement3Unlocked", 0);
			PlayerPrefs.SetInt("Achievement4Unlocked", 0);
			PlayerPrefs.SetInt("Achievement5Unlocked", 0);
			PlayerPrefs.SetInt("Achievement6Unlocked", 0);
			PlayerPrefs.SetInt("Achievement7Unlocked", 0);
			PlayerPrefs.SetInt("FromGame",0);
			PlayerPrefs.Save();
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void SetCurrentLevel(int level)
	{
		PlayerPrefs.SetInt("CurrentLevel", level);
	}
}
