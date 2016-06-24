using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.scripts.Skills
{
	class WolverineSkills : ISkills
	{

		public WolverineSkills()
		{
			TargetsSkill1Lvl1 = 1;
			TargetsSkill1Lvl2 = 2;
			TargetsSkill1Lvl3 = 3;
			TargetsSkill2Lvl1 = 1;
			TargetsSkill2Lvl2 = 3;
			TargetsSkill2Lvl3 = 0;
			TargetsSkill3Lvl1 = 1;
			TargetsSkill3Lvl2 = 2;
			TargetsSkill3Lvl3 = 3;

			Skill1Level = int.Parse(PlayerPrefs.GetString("Skills3").Substring(0, 1));
			Skill2Level = int.Parse(PlayerPrefs.GetString("Skills3").Substring(1, 1));
			Skill3Level = int.Parse(PlayerPrefs.GetString("Skills3").Substring(2, 1));
		}
		public int TargetsSkill1Lvl1 { get; set; }
		public int TargetsSkill1Lvl2 { get; set; }
		public int TargetsSkill1Lvl3 { get; set; }
		public int TargetsSkill2Lvl1 { get; set; }
		public int TargetsSkill2Lvl2 { get; set; }
		public int TargetsSkill2Lvl3 { get; set; }
		public int TargetsSkill3Lvl1 { get; set; }
		public int TargetsSkill3Lvl2 { get; set; }
		public int TargetsSkill3Lvl3 { get; set; }
		public int GetLevelOfUpgrade(int skillNumber)
		{
			if (skillNumber == 1)
				return Skill1Level;
			if (skillNumber == 2)
				return Skill2Level;
			return Skill3Level;
		}

		public int Skill1Level { get; set; }
		public int Skill2Level { get; set; }
		public int Skill3Level { get; set; }
		public void Skill1()
		{
			SkillsController.Hero.GetComponent<Animator>().SetTrigger("Skill1Cast");
			foreach (var lineTarget in SkillsController.LineTargets)
			{
				GameField.ShuffleLine(lineTarget);
			}
		}

		public void Skill2()
		{
			SkillsController.Hero.GetComponent<Animator>().SetTrigger("Skill2Cast");
			var clawsPrefab = Resources.Load("objects/heroes/Wolverine/claw/Claws", typeof(GameObject)) as GameObject;
			var claws = ((GameObject)Object.Instantiate(clawsPrefab, new Vector3(0,0),
				Quaternion.Euler(new Vector3(0, 0, 0)))).GetComponent<Claws>();
			claws.Targets = SkillsController.TargetCoordinates;
		}

		public void Skill3()
		{
			SkillsController.Hero.GetComponent<Animator>().SetTrigger("Skill3Cast");


			foreach (var line in SkillsController.LineTargets)
			{
				var wolfPrefab = Resources.Load("objects/heroes/Wolverine/summon/Wolf", typeof(GameObject)) as GameObject;
				var wolf = ((GameObject)Object.Instantiate(wolfPrefab, new Vector3(-5, GameField.GetVectorFromCoord(0,line).y),
					Quaternion.Euler(new Vector3(0, 0, 0)))).GetComponent<Wolf>();
				wolf.Target = line;
			}
		}
	}
}
