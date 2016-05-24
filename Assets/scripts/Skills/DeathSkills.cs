using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.Skills
{
	class DeathSkills : ISkills
	{
		public int TargetsSkill1Lvl1 { get; set; }
		public int TargetsSkill1Lvl2 { get; set; }
		public int TargetsSkill1Lvl3 { get; set; }
		public int TargetsSkill2Lvl1 { get; set; }
		public int TargetsSkill2Lvl2 { get; set; }
		public int TargetsSkill2Lvl3 { get; set; }
		public int TargetsSkill3Lvl1 { get; set; }
		public int TargetsSkill3Lvl2 { get; set; }
		public int TargetsSkill3Lvl3 { get; set; }
		public int Skill1Level { get; set; }
		public int Skill2Level { get; set; }
		public int Skill3Level { get; set; }

		public DeathSkills()
		{
			TargetsSkill1Lvl1 = 1;
			TargetsSkill1Lvl2 = 1;
			TargetsSkill1Lvl3 = 1;
			TargetsSkill2Lvl1 = 1;
			TargetsSkill2Lvl2 = 1;
			TargetsSkill2Lvl3 = 1;
			TargetsSkill3Lvl1 = 1;
			TargetsSkill3Lvl2 = 2;
			TargetsSkill3Lvl3 = 3;

			Skill1Level = int.Parse(PlayerPrefs.GetString("Skills5").Substring(0, 1));
			Skill2Level = int.Parse(PlayerPrefs.GetString("Skills5").Substring(1, 1));
			Skill3Level = int.Parse(PlayerPrefs.GetString("Skills5").Substring(2, 1));
		}

		public int GetLevelOfUpgrade(int skillNumber)
		{
			if (skillNumber == 1)
				return Skill1Level;
			if (skillNumber == 2)
				return Skill2Level;
			return Skill3Level;
		}
		public void Skill1()
		{
			SkillsController.Hero.GetComponent<Animator>().SetTrigger("Skill1Cast");
			UnityEngine.Debug.Log("meow1");
		}

		public void Skill2()
		{
			SkillsController.Hero.GetComponent<Animator>().SetTrigger("Skill2Cast");
			UnityEngine.Debug.Log("meow2");
		}

		public void Skill3()
		{
			SkillsController.Hero.GetComponent<Animator>().SetTrigger("Skill3Cast");
			UnityEngine.Debug.Log("meow3");
		}
	}
}
