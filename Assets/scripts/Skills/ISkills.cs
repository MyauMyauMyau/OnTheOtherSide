using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.scripts.Skills
{
	public interface ISkills
	{
		int TargetsSkill1Lvl1 { get; set; }
		int TargetsSkill1Lvl2 { get; set; }
		int TargetsSkill1Lvl3 { get; set; }
		int TargetsSkill2Lvl1 { get; set; }
		int TargetsSkill2Lvl2 { get; set; }
		int TargetsSkill2Lvl3 { get; set; }
		int TargetsSkill3Lvl1 { get; set; }
		int TargetsSkill3Lvl2 { get; set; }
		int TargetsSkill3Lvl3 { get; set; }
		int GetLevelOfUpgrade(int skillNumber);
		int Skill1Level { get; set; }
		int Skill2Level { get; set; }
		int Skill3Level { get; set; }
		void Skill1();

		void Skill2();

		void Skill3();
	}
}
