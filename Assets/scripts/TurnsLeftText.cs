using UnityEngine;
using System.Collections;
using Assets.scripts;
using UnityEngine.UI;

public class TurnsLeftText : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		GetComponent<Text>().text = "Ходов осталось: " + Game.TurnsLeft;
	}
}
