using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoinsCounter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetComponents<Text>()[0].text = PlayerPrefs.GetInt("Gold").ToString();
	}
}
