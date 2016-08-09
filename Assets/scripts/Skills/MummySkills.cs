using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.scripts.miniObjectsScripts;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.scripts.Skills
{
	class MummySkills : ISkills
	{
		public bool IsPossibleTarget(Monster monster, int skillNumber)
		{
			if (skillNumber == 1)
			{
				return (monster.IsMonster() && !monster.IsUpgradable() );
			}
			if (skillNumber == 3)
			{
				return (!monster.IsUpgradable() && monster.TypeOfMonster != MonsterType.BlackHole && monster.TypeOfMonster != MonsterType.Bomb);
			}
			if (skillNumber == 2)
				return (monster.TypeOfMonster == MonsterType.Coocon);
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

		public MummySkills()
		{
			TargetsSkill1Lvl1 = 1;
			TargetsSkill1Lvl2 = 1;
			TargetsSkill1Lvl3 = 1;
			TargetsSkill2Lvl1 = 1;
			TargetsSkill2Lvl2 = 2;
			TargetsSkill2Lvl3 = 3;
			TargetsSkill3Lvl1 = 1;
			TargetsSkill3Lvl2 = 2;
			TargetsSkill3Lvl3 = 3;

			Skill1Level = int.Parse(PlayerPrefs.GetString("Skills2").Substring(0, 1));
			Skill2Level = int.Parse(PlayerPrefs.GetString("Skills2").Substring(1, 1));
			Skill3Level = int.Parse(PlayerPrefs.GetString("Skills2").Substring(2, 1));
		}
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
			Game.Instance.StartCoroutine(CastRay());
		}

		private IEnumerator CastRay()
		{
			var rayPrefab = Resources.Load("objects/heroes/Mummy/beam/RayPrefab", typeof(GameObject)) as GameObject;
			var monster = SkillsController.TargetCoordinates.First();
			var start = GameField.GetVectorFromCoord(monster.X, monster.Y - 10);
			var finish = GameField.GetVectorFromCoord(monster.X, monster.Y);
			var dist = start - finish;
			var ray = (GameObject)Game.Instantiate(rayPrefab, start, Quaternion.Euler(new Vector3()));
			for (int i = 0; i < 40; i++)
			{
				ray.transform.position = ray.transform.position - dist/40;
				yield return new WaitForSeconds(0.005f);
			}
			AudioHolder.PlayMummyBeamDrop();
			var boomPrefab = Resources.Load("objects/heroes/Mummy/beam/boomPrefab", typeof(GameObject)) as GameObject;
			var boomList = new List<GameObject>();
			if (Skill1Level >= 1)
				if (GameField.Map[monster.X, monster.Y] != null)
				{
					var boom = (GameObject)Game.Instantiate(boomPrefab, GameField.GetVectorFromCoord(monster.X, monster.Y), Quaternion.Euler(new Vector3()));
					boomList.Add(boom);
					GameField.Map[monster.X, monster.Y].DestroyMonster();
				}
			if (Skill1Level >= 2)
			{
				if (monster.X - 1 >= 0 && monster.Y + 1 < Game.MAP_SIZE && GameField.Map[monster.X - 1, monster.Y + 1] != null &&
				    !GameField.Map[monster.X - 1, monster.Y + 1].IsUpgradable())
				{
					var boom = (GameObject)Game.Instantiate(boomPrefab, GameField.GetVectorFromCoord(monster.X-1, monster.Y + 1), Quaternion.Euler(new Vector3()));
					boomList.Add(boom);
					GameField.Map[monster.X-1, monster.Y + 1].DestroyMonster();
				}
				if (monster.Y + 1 < Game.MAP_SIZE && GameField.Map[monster.X, monster.Y + 1] != null &&
				    !GameField.Map[monster.X, monster.Y + 1].IsUpgradable())
				{
					var boom = (GameObject)Game.Instantiate(boomPrefab, GameField.GetVectorFromCoord(monster.X, monster.Y + 1), Quaternion.Euler(new Vector3()));
					boomList.Add(boom);
					GameField.Map[monster.X, monster.Y + 1].DestroyMonster();
				}
				if (monster.X + 1 < Game.MAP_SIZE && monster.Y + 1 < Game.MAP_SIZE &&
				    GameField.Map[monster.X + 1, monster.Y + 1] != null &&
				    !GameField.Map[monster.X + 1, monster.Y + 1].IsUpgradable())
				{
					var boom = (GameObject)Game.Instantiate(boomPrefab, GameField.GetVectorFromCoord(monster.X + 1, monster.Y + 1), Quaternion.Euler(new Vector3()));
					boomList.Add(boom);
					GameField.Map[monster.X + 1, monster.Y + 1].DestroyMonster();
				}
			}
			if (Skill1Level == 3)
			{
				if (monster.X - 1 >= 0 && monster.Y + 2 < Game.MAP_SIZE && GameField.Map[monster.X - 1, monster.Y + 2] != null &&
				    !GameField.Map[monster.X - 1, monster.Y + 2].IsUpgradable())
				{
					var boom = (GameObject)Game.Instantiate(boomPrefab, GameField.GetVectorFromCoord(monster.X - 1, monster.Y + 2), Quaternion.Euler(new Vector3()));
					boomList.Add(boom);
					GameField.Map[monster.X - 1, monster.Y + 2].DestroyMonster();
				}
				if (monster.X - 2 >= 0 && monster.Y + 2 < Game.MAP_SIZE && GameField.Map[monster.X - 2, monster.Y + 2] != null &&
				    !GameField.Map[monster.X - 2, monster.Y + 2].IsUpgradable())
				{
					var boom = (GameObject)Game.Instantiate(boomPrefab, GameField.GetVectorFromCoord(monster.X - 2, monster.Y + 2), Quaternion.Euler(new Vector3()));
					boomList.Add(boom);
					GameField.Map[monster.X - 2, monster.Y + 2].DestroyMonster();
				}
				if (monster.Y + 2 < Game.MAP_SIZE && GameField.Map[monster.X, monster.Y + 2] != null &&
				    !GameField.Map[monster.X, monster.Y + 2].IsUpgradable())
				{
					var boom = (GameObject)Game.Instantiate(boomPrefab, GameField.GetVectorFromCoord(monster.X, monster.Y +2), Quaternion.Euler(new Vector3()));
					boomList.Add(boom);
					GameField.Map[monster.X, monster.Y + 2].DestroyMonster();
				}
				if (monster.X + 1 < Game.MAP_SIZE && monster.Y + 2 < Game.MAP_SIZE &&
				    GameField.Map[monster.X + 1, monster.Y + 2] != null &&
				    !GameField.Map[monster.X + 1, monster.Y + 2].IsUpgradable())
				{
					var boom = (GameObject)Game.Instantiate(boomPrefab, GameField.GetVectorFromCoord(monster.X + 1, monster.Y + 2), Quaternion.Euler(new Vector3()));
					boomList.Add(boom);
					GameField.Map[monster.X +1, monster.Y + 2].DestroyMonster();
				}
				if (monster.X + 2 < Game.MAP_SIZE && monster.Y + 2 < Game.MAP_SIZE &&
				    GameField.Map[monster.X + 2, monster.Y + 2] != null &&
				    !GameField.Map[monster.X + 2, monster.Y + 2].IsUpgradable())
				{
					var boom = (GameObject)Game.Instantiate(boomPrefab, GameField.GetVectorFromCoord(monster.X + 2, monster.Y + 2), Quaternion.Euler(new Vector3()));
					boomList.Add(boom);
					GameField.Map[monster.X +2, monster.Y + 2].DestroyMonster();
				}
			}
			yield return new WaitForSeconds(0.2f);
			foreach (var boom in boomList)
			{
				Object.Destroy(boom.gameObject);	
			}
			Object.Destroy(ray.gameObject);
		}


		public void Skill2()
		{
			SkillsController.Hero.GetComponent<Animator>().SetTrigger("Skill2Cast");
			Game.Instance.StartCoroutine(SendScarabs());
		}

		private IEnumerator SendScarabs()
		{
			var scarabPrefab = Resources.Load("objects/heroes/Mummy/bugs/Scarab", typeof(GameObject)) as GameObject;
			for (int i = 0; i < Skill2Level && i < SkillsController.TargetCoordinates.Count; i++)
			{
				var startPos = i == 1 ? new Vector3(-2.57f, 5.17f):new Vector3(-3.25f, 5.17f);
				var scarab = ((GameObject)Object.Instantiate(scarabPrefab, startPos,
					Quaternion.Euler(new Vector3()))).GetComponent<Scarab>();
				scarab.Target = SkillsController.TargetCoordinates.ElementAt(i);
				yield return new WaitForSeconds(0.25f);
			}
		}

		public void Skill3()
		{
			SkillsController.Hero.GetComponent<Animator>().SetTrigger("Skill3Cast");
			var skullPrefab = Resources.Load("objects/heroes/Mummy/skull/SkullToDrop", typeof(GameObject)) as GameObject;
			for (int i = 0; i < Skill3Level && i < SkillsController.TargetCoordinates.Count; i++)
			{
				var skullToDrop = ((GameObject)Object.Instantiate(skullPrefab, new Vector3(-2.68f, 5.52f),
					Quaternion.Euler(new Vector3()))).GetComponent<Skull>();
				skullToDrop.TargetCoordinate = SkillsController.TargetCoordinates.ElementAt(i);
			}
			 

		}
	}
}
