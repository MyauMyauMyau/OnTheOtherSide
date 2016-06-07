using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts.Skills
{
	class ClericSkills : ISkills
	{
		public ClericSkills()
		{
			TargetsSkill1Lvl1 = 0;
			TargetsSkill1Lvl2 = 0;
			TargetsSkill1Lvl3 = 0;
			TargetsSkill2Lvl1 = 1;
			TargetsSkill2Lvl2 = 1;
			TargetsSkill2Lvl3 = 1;
			TargetsSkill3Lvl1 = 1;
			TargetsSkill3Lvl2 = 1;
			TargetsSkill3Lvl3 = 2;

			Skill1Level = int.Parse(PlayerPrefs.GetString("Skills1").Substring(0, 1));
			Skill2Level = int.Parse(PlayerPrefs.GetString("Skills1").Substring(1, 1));
			Skill3Level = int.Parse(PlayerPrefs.GetString("Skills1").Substring(2, 1));
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
			Debug.Log("meow");
			SkillsController.Hero.GetComponent<Animator>().SetTrigger("Skill1Cast");
		}

		public void Skill2()
		{
			SkillsController.Hero.GetComponent<Animator>().SetTrigger("Skill2Cast");
		}

		public void Skill3()
		{
			SkillsController.Hero.GetComponent<Animator>().SetTrigger("Skill3Cast");
		}
	}
}
