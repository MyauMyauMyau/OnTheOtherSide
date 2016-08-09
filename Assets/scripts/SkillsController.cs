using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.scripts.Skills;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts
{
	public class SkillsController : MonoBehaviour
	{
		public static GameObject TargetBrackets;
		public static GameObject TargetBrackets2;
		public static GameObject LineBrackets;
		public static bool IsActive;
		public static List<Coordinate> TargetCoordinates;
		public static List<GameObject> Brackets; 
		public static int MaxTargets;
		public static GameObject Hero;
		public static List<int> LineTargets; 
		public static SkillsController Instance;
		public static Func<Monster, bool> IsMonsterClickable;
		public static int CurrentSkillNumber;
		// Use this for initialization
		void Start ()
		{
			IsActive = false;
			MaxTargets = 0;
			TargetCoordinates = new List<Coordinate>();
			LineTargets = new List<int>();
			Brackets = new List<GameObject>();
			Instance = this;
		}

		public static void Activate(int numOfTargets)
		{
			
			IsActive = true;
			MaxTargets = numOfTargets;
			TargetCoordinates = new List<Coordinate>();
			Brackets = new List<GameObject>();
			LineTargets = new List<int>();
			if (Game.HeroType == HeroType.Hunter && CurrentSkillNumber == 3)
			{
				AudioHolder.PlayHunterBridge1();
				foreach (var coordinate in WaterField.River)
				{
					WaterField.Map[coordinate.X, coordinate.Y].GetComponent<CircleCollider2D>().enabled = true;
				}
			}
			if (Game.HeroType == HeroType.Hunter && CurrentSkillNumber == 1)
				AudioHolder.PlayHunterShoot1();
			if (Game.HeroType == HeroType.Cleric && CurrentSkillNumber == 2)
				AudioHolder.PlayClericFlag1();
			if (Game.HeroType == HeroType.Vampire && CurrentSkillNumber == 1)
				AudioHolder.PlayVampireHypnosis();
			if (Game.HeroType == HeroType.Wolverine && (CurrentSkillNumber == 1 || CurrentSkillNumber == 2))
				AudioHolder.PlayWolfRoar1();
			if (Game.HeroType == HeroType.Mummy)
				AudioHolder.PlayMummyWait();

		}
		public IEnumerator ThrowFireball(int size)
		{
			AudioHolder.PlayDeathFireBall();
			Game.PlayerIsBlocked = true;
			var fireball = GameObject.Find("DeathPrefab(Clone)").transform.FindChild("FireBall").gameObject;
			fireball.SetActive(true);
			fireball.GetComponent<Animation>().Play();
			var distance = GameField.GetVectorFromCoord(
				TargetCoordinates.ElementAt(0).X, TargetCoordinates.ElementAt(0).Y) - GameObject.Find("DeathPrefab(Clone)").transform.position;
			var posCopy = fireball.transform.position;
			var scaleCopy = fireball.transform.localScale;
			for (int i = 0; i < 50; i++)
			{
				posCopy.x += distance.x / 50;
				posCopy.y += distance.y / 50;
				scaleCopy.x += 0.01f/(3f/size);
				scaleCopy.y += 0.01f/(3f/size);
				fireball.transform.position = posCopy;
				fireball.transform.localScale = scaleCopy;
				yield return null;
			}
			fireball.transform.position = new Vector3(-3.47f, 1.84f);
			fireball.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
			fireball.SetActive(false);
			Game.PlayerIsBlocked = false;
			if (size == 3)
				GameField.BurnSquare3X3(TargetCoordinates.ElementAt(0));
			if (size == 2)
				GameField.BurnCross(TargetCoordinates.ElementAt(0));
			if (size == 1)
				GameField.BurnCell(TargetCoordinates.ElementAt(0));
			AudioHolder.PlayMassRemove();
		}
		public static void Deactivate()
		{
			MaxTargets = 0;
			IsActive = false;
			foreach(var bracket in Brackets)
				Destroy(bracket);
			Brackets = new List<GameObject>();
			if (Game.HeroType == HeroType.Hunter && CurrentSkillNumber == 3)
			{
				AudioHolder.StopPlayHunterBridge1();
				foreach (var coordinate in WaterField.River)
				{
					WaterField.Map[coordinate.X, coordinate.Y].GetComponent<CircleCollider2D>().enabled = false;
				}
			}
			if (Game.HeroType == HeroType.Vampire && CurrentSkillNumber == 1)
				AudioHolder.StopPlayVampireHypnosis();
			if (Game.HeroType == HeroType.Wolverine && (CurrentSkillNumber == 1 || CurrentSkillNumber == 2))
				AudioHolder.StopPlayWolfRoar1();
			if (Game.HeroType == HeroType.Mummy)
				AudioHolder.StopPlayMummyWait();

		}
		// Update is called once per frame
		void Update ()
		{
			if (!IsActive) return;
			if (MaxTargets == 0 || TargetCoordinates.Count>0 || LineTargets.Count>0)
				ActivateYes();
			else
				DeactivateYes();
		}

		void DeactivateYes()
		{
			
			var btn = GetComponentInChildren<Button>();
			btn.interactable = false;
			btn.GetComponent<Image>().color = Color.gray;
		}

		void ActivateYes()
		{
			var btn = GetComponentInChildren<Button>();
			btn.interactable = true;
			btn.GetComponent<Image>().color = Color.white;
		}

		public static void BracketMonster(Coordinate gridPosition)
		{
			if (LineBrackets != null)
			{
				var ind = LineTargets.FindIndex(x => x == gridPosition.Y);
				if (ind >= 0)
				{
					LineTargets.RemoveAt(ind);
					Destroy(Brackets.ElementAt(ind));
					Brackets.RemoveAt(ind);
				}
				else
				{
					if (MaxTargets == LineTargets.Count) return;
					var linebracket = (GameObject)Instantiate(LineBrackets,
						GameField.GetVectorFromCoord(3.5f, gridPosition.Y), Quaternion.Euler(new Vector3()));
					Brackets.Add(linebracket);
					LineTargets.Add(gridPosition.Y);
				}
				return;
			}
			if (TargetBrackets2 != null)
			{
				var ind = TargetCoordinates.FindIndex(x => x == gridPosition);
				if (ind >= 0 && ind%2 == 0)
				{
					var ind2 = ind + 1;
					if (TargetCoordinates.Count > ind2)
					{
						var pos = TargetCoordinates.ElementAt(ind2);
						var bracketToRemove = Brackets.First(x => x.transform.position == GameField.GetVectorFromCoord(pos.X, pos.Y));
						Brackets.Remove(bracketToRemove);
						Destroy(bracketToRemove.gameObject);
						TargetCoordinates.RemoveAt(ind2);
					}
				}
				if (ind >= 0)
				{
					TargetCoordinates.Remove(gridPosition);
					var bracketToRemove = Brackets.First(x => x.transform.position == GameField.GetVectorFromCoord(gridPosition.X, gridPosition.Y));
					Brackets.Remove(bracketToRemove);
					Destroy(bracketToRemove.gameObject);
				}
				if (ind < 0)
				{
					if (MaxTargets == TargetCoordinates.Count) return;
					var currentBrackets = TargetCoordinates.Count%2 == 0 ? TargetBrackets : TargetBrackets2;
					var br = (GameObject)Instantiate(currentBrackets,
						GameField.GetVectorFromCoord(gridPosition.X, gridPosition.Y), Quaternion.Euler(new Vector3()));
					Brackets.Add(br);
					TargetCoordinates.Add(gridPosition);
				}

				return;
			}
			if (Brackets.Any(x => x.transform.position == GameField.GetVectorFromCoord(gridPosition.X, gridPosition.Y)))
			{
				TargetCoordinates.Remove(gridPosition);
				var bracketToRemove = Brackets.First(x => x.transform.position == GameField.GetVectorFromCoord(gridPosition.X, gridPosition.Y));
				Brackets.Remove(bracketToRemove);
				Destroy(bracketToRemove.gameObject);

				return;
			}
			if (MaxTargets == TargetCoordinates.Count) return;
			var bracket = (GameObject) Instantiate(TargetBrackets,
				GameField.GetVectorFromCoord(gridPosition.X, gridPosition.Y), Quaternion.Euler(new Vector3()));
			Brackets.Add(bracket);
			TargetCoordinates.Add(gridPosition);
		}

		public IEnumerator ThrowIceBall(int skill3Level)
		{
			Game.PlayerIsBlocked = true;
			var iceBallPrefab = Resources.Load("objects/heroes/DeathHero/Ice/IceBall", typeof(GameObject)) as GameObject;
			var iceBalls = new List<GameObject>();
			for (int i = 0; i < TargetCoordinates.Count; i++)
			{
				
				var iceBall =
					(GameObject)
						Instantiate(iceBallPrefab, GameObject.Find("DeathPrefab(Clone)").transform.position,
							Quaternion.Euler(new Vector3()));
				
				iceBalls.Add(iceBall);
			}
			for (int j = 0; j < 40; j++)
			{
				for (int i = 0; i < iceBalls.Count; i++)
				{
					var iceBall = iceBalls.ElementAt(i);
					var distance = GameField.GetVectorFromCoord(
						TargetCoordinates.ElementAt(i).X, TargetCoordinates.ElementAt(i).Y) -
					               GameObject.Find("DeathPrefab(Clone)").transform.position;
					var posCopy = iceBall.transform.position;
					var scaleCopy = iceBall.transform.localScale;
					posCopy.x += distance.x/40;
					posCopy.y += distance.y/40;
					scaleCopy.x += 0.024f;
					scaleCopy.y += 0.024f;
					iceBall.transform.position = posCopy;
					iceBall.transform.localScale = scaleCopy;
					iceBall.transform.Rotate(new Vector3(0, 0, 10f));
					yield return new WaitForSeconds(0.01f);

				}
			}
			for (int i = 0; i < iceBalls.Count; i++)
			{
				var iceBall = iceBalls.ElementAt(i);
				if (GameField.Map[TargetCoordinates.ElementAt(i).X, TargetCoordinates.ElementAt(i).Y].TypeOfMonster ==
				MonsterType.BlackHole)
				{
					GameField.Map[TargetCoordinates.ElementAt(i).X, TargetCoordinates.ElementAt(i).Y].State
						= MonsterState.Deactivated;
					var iceCellPrefab = Resources.Load("objects/heroes/DeathHero/Ice/IceCell", typeof(GameObject)) as GameObject;
					Instantiate(iceCellPrefab,
						GameField.GetVectorFromCoord(TargetCoordinates.ElementAt(i).X, TargetCoordinates.ElementAt(i).Y),
						Quaternion.Euler(new Vector3()));
				}
				Destroy(iceBall.gameObject);
			}
			AudioHolder.PlayDeathIce();
			Game.PlayerIsBlocked = false;
		}
	}
}
