using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.scripts;
using UnityEngine.UI;

public class ButtonLight : MonoBehaviour {
	float timeOn = 0.1f;
	float timeOff = 0.5f;
	private float changeTime = 0;
	private static Texture activeFireButton;

	void Start()
	{
		Debug.Log("en");
	}
	private void Update()
	{
		Debug.Log("oh"	);
		if (Time.time > changeTime)
		{																								
			GetComponent<Light>().enabled = !GetComponent<Light>().enabled;
			if (GetComponent<Light>().enabled)
			{
				changeTime = Time.time + timeOn;
			}
			else {
				changeTime = Time.time + timeOff;
			}
		}
	}
}
