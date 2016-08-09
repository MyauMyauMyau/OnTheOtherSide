using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LivesCounter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetComponentsInChildren<Text>()[0].text = PlayerPrefs.GetInt("Lives").ToString();
	}
}
