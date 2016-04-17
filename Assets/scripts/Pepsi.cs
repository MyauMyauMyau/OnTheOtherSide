using UnityEngine;
using System.Collections;

public class Pepsi : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetInt("Achievement3Unlocked") == 1)
			Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
	{
		PlayerPrefs.SetInt("Achievement3Unlocked", 1);
		Destroy(gameObject);
	}
}
