using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AchievementsList : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		if (PlayerPrefs.GetInt("Achievement1Unlocked") == 1)
			GetComponentsInChildren<Text>()[0].enabled = true;
		if (PlayerPrefs.GetInt("Achievement2Unlocked") == 1)
			GetComponentsInChildren<Text>()[1].enabled = true;
		if (PlayerPrefs.GetInt("Achievement3Unlocked") == 1)
			GetComponentsInChildren<Text>()[2].enabled = true;
		if(PlayerPrefs.GetInt("Achievement4Unlocked") == 1)
			GetComponentsInChildren<Text>()[3].enabled = true;
		if(PlayerPrefs.GetInt("Achievement5Unlocked") == 1)
			GetComponentsInChildren<Text>()[4].enabled = true;
	}
	
	// Update is called once per frame
	
}
