using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.scripts
{
	public class LevelInfo
	{
		public string Map;
		public string Monsters;
		public int Turns;
		public Dictionary<char, int> Targets;
	}
}
