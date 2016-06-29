using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJSON;
using UnityEngine;

namespace Assets.scripts
{
	public class LevelInfo
	{
		public string Map;
		public string Monsters;
		public int Turns;
		public Dictionary<char, int> Targets;
		public static LevelInfo CreateFromJSON(string jsonString)
		{
			var parsed = JSON.Parse(jsonString);
			var levelInfo = new LevelInfo();
			levelInfo.Map = parsed["Map"];
			Debug.Log(levelInfo.Map);
			levelInfo.Monsters = parsed["Monsters"];
			levelInfo.Turns = parsed["Turns"].AsInt;
			var unparsedDict = parsed["Targets"];
			levelInfo.Targets = new Dictionary<char, int>();
			foreach (KeyValuePair<string, JSONNode> target in unparsedDict.AsObject)
			{ 	
				if (target.Key.ElementAt(0) != '\'')
					levelInfo.Targets.Add(target.Key.ElementAt(0), target.Value.AsInt);
				else
					levelInfo.Targets.Add(target.Key.ElementAt(1), target.Value.AsInt);
			}
			return levelInfo;
		}
	}
}
