using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.scripts;
using UnityEngine.UI;

public class 
	sText : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		for (var i = 0; i < Game.LevelInformation.Targets.Count; i++)
		{
			var currentMonsters = Dictionaries.MonsterCounter[Dictionaries.CharsToObjectTypes[Game.LevelInformation.Targets.ElementAt(i).Key]];
			var totalMonsters = Game.LevelInformation.Targets.ElementAt(i).Value;
			var text = "" + currentMonsters  + "/";
			text += totalMonsters;
			GetComponentsInChildren<Text>()[i].text = text;
			if (currentMonsters >= totalMonsters)
				GetComponentsInChildren<Text>()[i].color = Color.green;
		}
	}
}
