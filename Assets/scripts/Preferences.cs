using UnityEngine;
using System.Collections;

public class Preferences : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (!PlayerPrefs.HasKey("Lives"))
		{
			PlayerPrefs.SetInt("Sound", 1);
			PlayerPrefs.SetInt("LevelUnlocked", 2);
			PlayerPrefs.SetInt("FromGame",0);
			PlayerPrefs.SetInt("Gold", 50);
			PlayerPrefs.SetInt("Lives", 5);
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
