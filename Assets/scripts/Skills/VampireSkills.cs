using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.scripts.Skills
{
	class VampireSkills :ISkills
	{

		public VampireSkills()
		{
			TargetsSkill1Lvl1 = 2;
			TargetsSkill1Lvl2 = 4;
			TargetsSkill1Lvl3 = 6;
			TargetsSkill2Lvl1 = 0;
			TargetsSkill2Lvl2 = 0;
			TargetsSkill2Lvl3 = 0;
			TargetsSkill3Lvl1 = 1;
			TargetsSkill3Lvl2 = 2;
			TargetsSkill3Lvl3 = 3;

			Skill1Level = int.Parse(PlayerPrefs.GetString("Skills4").Substring(0, 1));
			Skill2Level = int.Parse(PlayerPrefs.GetString("Skills4").Substring(1, 1));
			Skill3Level = int.Parse(PlayerPrefs.GetString("Skills4").Substring(2, 1));
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

			for (int i = 1; i < SkillsController.TargetCoordinates.Count; i++)
			{
				GameField.MagicSwap(SkillsController.TargetCoordinates.ElementAt(i-1), SkillsController.TargetCoordinates.ElementAt(i));

			}
		}

		public void Skill2()
		{
			SkillsController.Hero.GetComponent<Animator>().SetTrigger("Skill2Cast");
			Game.Instance.StartCoroutine(EatBats());
		}

		private IEnumerator EatBats()
		{
			int count;
			if (Skill2Level == 1)
				count = 3;
			else if (Skill2Level == 2)
				count = 5;
			else
			{
				count = 99;
			}
			var bats = new List<Monster>();
			for (int i = 0; i < Game.MAP_SIZE;i++)
				for (int j = 0; j < Game.MAP_SIZE && count>0; j++)
				{
					if (GameField.Map[i, j] != null && GameField.Map[i, j].TypeOfMonster == MonsterType.Bat)
					{
						count--;
						bats.Add(GameField.Map[i, j]);
						GameField.Map[i, j].State = MonsterState.WaitingForActivation;
					}
				}
			var target = new Vector3(-2.8f, 5.3f);
			for (int i = 0; i < 20; i++)
			{
				foreach (var bat in bats)
				{
					var startCoord = GameField.GetVectorFromCoord(bat.GridPosition.X, bat.GridPosition.Y);
					bat.transform.position = bat.transform.position - (startCoord - target)/20;
				}
				yield return new WaitForSeconds(0.025f);
			}
			Dictionaries.MonsterCounter[MonsterType.Bat] += bats.Count;
			foreach (var bat in bats)
			{
				bat.Destroy();
			}
		}

		public void Skill3()
		{
			SkillsController.Hero.GetComponent<Animator>().SetTrigger("Skill3Cast");
			var bloodPrefab = Resources.Load("objects/heroes/Vampire/Vortex/bloodDrop", typeof(GameObject)) as GameObject;
			foreach (var targetCoordinate in SkillsController.TargetCoordinates)
			{
				var bloodDrop = ((GameObject)Object.Instantiate(bloodPrefab, new Vector3(-2.35f, 4.5f),
					Quaternion.Euler(new Vector3(0,0, 180f)))).GetComponent<bloodDrop>();
				bloodDrop.Target = targetCoordinate;
			}
		}
	}
}
