using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.scripts.miniObjectsScripts;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace Assets.scripts.Skills
{
	class HunterSkills:ISkills
	{

		public HunterSkills()
		{
			TargetsSkill1Lvl1 = 1;
			TargetsSkill1Lvl2 = 2;
			TargetsSkill1Lvl3 = 3;
			TargetsSkill2Lvl1 = 0;
			TargetsSkill2Lvl2 = 0;
			TargetsSkill2Lvl3 = 0;
			TargetsSkill3Lvl1 = 1;
			TargetsSkill3Lvl2 = 2;
			TargetsSkill3Lvl3 = 3;

			Skill1Level = int.Parse(PlayerPrefs.GetString("Skills0").Substring(0, 1));
			Skill2Level = int.Parse(PlayerPrefs.GetString("Skills0").Substring(1, 1));
			Skill3Level = int.Parse(PlayerPrefs.GetString("Skills0").Substring(2, 1));
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
			Game.Instance.StartCoroutine(StartShooting());


		}

		private IEnumerator StartShooting()
		{
			var shotPrefab = Resources.Load("objects/heroes/Hunter/Shooting/shot", typeof(GameObject)) as GameObject;
			foreach (var target in SkillsController.TargetCoordinates)
			{
				var shot = (Object.Instantiate(shotPrefab, new Vector3(-1.7f, 5.30f),
					Quaternion.Euler(new Vector3())) as GameObject).GetComponent<huntersShot>();
				shot.Target = target;
				yield return new WaitForSeconds(0.25f);
			}
		} 
		public void Skill2()
		{
			SkillsController.Hero.GetComponent<Animator>().SetTrigger("Skill2Cast");

			if (Skill2Level == 1)
				Game.Instance.StartCoroutine(BoomBombs(2));
			else if (Skill2Level == 2)
				Game.Instance.StartCoroutine(BoomBombs(4));
			else
				Game.Instance.StartCoroutine(BoomBombs(9));

		}

		private IEnumerator BoomBombs(int targetsCount)
		{
			Game.PlayerIsBlocked = true;
			var boomPrefab = Resources.Load("objects/heroes/Hunter/Mines/boom", typeof(GameObject)) as GameObject;
			var rnd = new Random();
			var monsterList = new List<Monster>();
			var monstersToDestroy = new List<Monster>();
			for (int i = 0; i < 8; i ++)
				for (int j = 0; j < 8; j++)
				{
					if (GameField.Map[i,j] != null && (GameField.Map[i,j].IsMonster() || GameField.Map[i,j].TypeOfMonster == MonsterType.Coocon)
						&& !GameField.Map[i,j].IsUpgradable())
					monsterList.Add(GameField.Map[i,j]);
				}
			var booms = new List<GameObject>();
			for (int i = 0; i < targetsCount; i++)
			{
				var n = rnd.Next(monsterList.Count);
				var monster = monsterList.ElementAt(n);
				var boom = (GameObject)Game.Instantiate(boomPrefab, GameField.GetVectorFromCoord(monster.GridPosition.X, monster.GridPosition.Y), Quaternion.Euler(new Vector3()));
				booms.Add(boom);
				monstersToDestroy.Add(monster);
				monsterList.RemoveAt(n);	
			}
			yield return new WaitForSeconds(1);
			foreach(var boom in booms)
				Game.Destroy(boom);
			foreach (var monster in monstersToDestroy)
				GameField.BurnCell(monster.GridPosition);
			Game.PlayerIsBlocked = true;
		}

		public void Skill3()
		{
			SkillsController.Hero.GetComponent<Animator>().SetTrigger("Skill3Cast");
			Game.Instance.StartCoroutine(DropRafts());
		}

		public IEnumerator DropRafts()
		{
			Game.PlayerIsBlocked = true;
			for (int i = 0; i < SkillsController.TargetCoordinates.Count; i++)
			{
				var coord = SkillsController.TargetCoordinates.ElementAt(i);
				if (!WaterField.IsBridgeOrNull(coord.X, coord.Y))
					DropRaft(coord);
				yield return new WaitForSeconds(0.5f);
			}
			Game.PlayerIsBlocked = false;
		}

		public void DropRaft(Coordinate coord)
		{
			var raftPrefab = Resources.Load("objects/heroes/Hunter/Raft/raft", typeof(GameObject)) as GameObject;
			var raft = (Object.Instantiate(raftPrefab, new Vector3(-1.7f, 5.30f),
					Quaternion.Euler(new Vector3())) as GameObject).GetComponent<Raft>();
			raft.TargetCoordinate = coord;

		}
	}
}
