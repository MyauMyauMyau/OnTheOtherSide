using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

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
			SkillsController.Instance.StartCoroutine
				(SkillsController.Instance.ThrowFireball(Skill1Level));
		}


		

		public void Skill2()
		{
			SkillsController.Hero.GetComponent<Animator>().SetTrigger("Skill2Cast");
			CastLightning();
		}

		private void CastLightning()
		{
			
			var x = SkillsController.TargetCoordinates.ElementAt(0).X;
			var y = SkillsController.TargetCoordinates.ElementAt(0).Y;
			var typeOfMonster =
				GameField.Map[x, y].TypeOfMonster;


			if (Skill2Level == 1 || Skill2Level == 2)
			{
				var mLightningPrefab = Resources.Load("objects/heroes/DeathHero/Lightning/miniLightning", typeof(GameObject)) as GameObject;
				var mLightning = ((GameObject) Object.Instantiate(mLightningPrefab, GameField.GetVectorFromCoord(x, y),
					Quaternion.Euler(new Vector3()))).GetComponent<miniLightning>();
				mLightning.Target = new Coordinate(x, y);
				var targetsLeft = Skill2Level == 1 ? 1 : 4;
				for (int i = 0; i < Game.MAP_SIZE; i++)
					for (int j = 0; j < Game.MAP_SIZE; j++)
					{
						if (i == x && j == y) continue;
						if (targetsLeft == 0) return;
						if (GameField.Map[i, j] != null && GameField.Map[i, j].TypeOfMonster == typeOfMonster)
						{
							mLightning = ((GameObject) Object.Instantiate(mLightningPrefab, GameField.GetVectorFromCoord(i, j),
								Quaternion.Euler(new Vector3()))).GetComponent<miniLightning>();
							mLightning.Target = new Coordinate(i, j);
							targetsLeft--;
						}
					}
			}
			else
			{
				var bLightningPrefab = Resources.Load("objects/heroes/DeathHero/Lightning/bigLightning", typeof(GameObject)) as GameObject;
				var bigLightning = ((GameObject)Object.Instantiate(bLightningPrefab, new Vector3(-2.85f, 0.80f), 
					Quaternion.Euler(new Vector3()))).GetComponent<bigLightning>();
				bigLightning.Target = new Coordinate(x,y);
			}

		}
		public void Skill3()
		{
			SkillsController.Hero.GetComponent<Animator>().SetTrigger("Skill3Cast");
			SkillsController.Instance.StartCoroutine
				(SkillsController.Instance.ThrowIceBall(Skill3Level));
		}
	}
}
