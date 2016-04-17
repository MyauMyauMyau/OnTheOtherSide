using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelGrave : MonoBehaviour
{

	private int n;
	// Use this for initialization
	void Start ()
	{
		n = int.Parse(name.Substring(5));
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (PlayerPrefs.GetInt("LevelUnlocked") >= n)
		{
			GetComponentInChildren<Text>().color = Color.white;
			GetComponent<Button>().interactable = true;
		}
		else
		{
			GetComponentInChildren<Text>().color = Color.gray;
			GetComponent<Button>().interactable = false;
		}
	}
}
