using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.scripts;
using UnityEngine.UI;

public class TargetsText : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		for (var i = 0; i < Game.LevelInformation.Targets.Count; i++)
		{
			var text = "" + Game.MonsterCounter[Monster.CharsToObjectTypes[Game.LevelInformation.Targets.ElementAt(i).Key]] + "/";
			text += Game.LevelInformation.Targets.ElementAt(i).Value;
			GetComponentsInChildren<Text>()[i].text = text;
		}
	}
}
