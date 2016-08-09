using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

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

		public bool IsPossibleTarget(Monster monster, int skillNumber)
		{
			if (skillNumber == 2)
			{
				return (monster.IsMonster() && !monster.IsUpgradable());
			}
			if (skillNumber == 3)
				return (monster.TypeOfMonster == MonsterType.Pumpkin1 || monster.TypeOfMonster == MonsterType.Pumpkin2);
			return false;
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
			AudioHolder.PlayClericExile1();
			SkillsController.Hero.GetComponent<Animator>().SetTrigger("Skill1Cast");
			CastLightnings();
		}

		private void CastLightnings()
		{
			var lightningCount = 0;
			if (Skill1Level == 1)
				lightningCount = 2;
			else if (Skill1Level == 2)
				lightningCount = 5;
			else
				lightningCount = 9;
			var monsterList = new List<Monster>();
			var rnd = new Random();
			for (int i = 0; i < 8; i++)
				for (int j = 0; j < 8; j++)
				{
					if (GameField.Map[i, j] != null && (GameField.Map[i,j].TypeOfMonster == MonsterType.Voodoo 
						|| GameField.Map[i, j].TypeOfMonster == MonsterType.Zombie))
						monsterList.Add(GameField.Map[i, j]);
				}
			var lightningPrefab = Resources.Load("objects/heroes/Cleric/UndeadRemoval/PriestLightning", typeof(GameObject)) as GameObject;

			for (int i = 0; i < lightningCount && i < monsterList.Count; i++)
			{
				var n = rnd.Next(monsterList.Count);
				var monster = monsterList.ElementAt(n);
				var lightning = ((GameObject)Game.Instantiate(lightningPrefab, new Vector3(-2.68f, 5.52f), Quaternion.Euler(new Vector3()))).GetComponent<PriestLightning>();
				lightning.Target = monster.GridPosition;
				monsterList.RemoveAt(n);
			}
		} 
		public void Skill2()
		{

			SkillsController.Hero.GetComponent<Animator>().SetTrigger("Skill2Cast");
			Monster flag = ((GameObject)Object.Instantiate(
						Game.FlagPrefab, new Vector3(-2.68f, 5.52f), 
						Quaternion.Euler(new Vector3())))
						.GetComponent<Monster>();
			flag.Initialise(SkillsController.TargetCoordinates.ElementAt(0).X,
				SkillsController.TargetCoordinates.ElementAt(0).Y, 'F');
			flag.FlagFrequancy = 4 - Skill2Level;
			flag.FlagCounter = 1;
			Debug.Log("em" + flag.FlagFrequancy);

		}

		public void Skill3()
		{
			SkillsController.Hero.GetComponent<Animator>().SetTrigger("Skill3Cast");
			AudioHolder.PlayClericPumpkin();
			CastLight();
		}

		private void CastLight()
		{
			var lightPrefab = Resources.Load("objects/heroes/Cleric/SummonLight/Light", typeof(GameObject)) as GameObject;
			foreach (var Target in SkillsController.TargetCoordinates)
			{
				var light = ((GameObject)Game.Instantiate(lightPrefab, new Vector3(-2.68f, 5.52f)
					, Quaternion.Euler(new Vector3()))).GetComponent<ClericLight>();
				light.Target = Target;										
				if (Skill3Level != 1)
					light.IsPowerful = true;
				else
					light.IsPowerful = false;
			}
		}
	}
}
