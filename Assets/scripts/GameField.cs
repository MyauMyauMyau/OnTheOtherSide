using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Assets.scripts;
using Assets.scripts.Enums;
using Random = System.Random;

namespace Assets.scripts
{
	public class GameField : MonoBehaviour
	{
		public static Monster[,] Map;
		public static Coordinate? ClickedObject;
		public static float MoveSpeed = 10.0f;
		public static bool MoveIsFinished = true;
		public static Random Rnd = new Random();
		public static GameField Instance;
		public void Start()
		{
			Instance = this;
		}
		public static void CheckUpperBorder()
		{
			var delay = 0f;
			var step = 3f / Monster.BaseDropSpeed;
			for (int i = 0; i < Map.GetLength(0); i++)
			{
				if (Map[i, 0] == null)
				{
					Game.MonsterCreate(i, 0,
						Dictionaries.MonsterGenerationList.ElementAt(Rnd.Next(Dictionaries.MonsterGenerationList.Count)), delay);
				}
			}
		}
		public static void Shuffle()
		{
			var rnd = new Random();
			var asteroidsCoordinates = new List<Coordinate>();
			for (int i = 0; i < Map.GetLength(0); i++)
			{
				for (int j = 0; j < Map.GetLength(1); j++)
					if (Map[i,j] != null && Map[i, j].IsMonster())
						asteroidsCoordinates.Add(new Coordinate(i, j));
			}
			while (asteroidsCoordinates.Count != 0)
			{
				MoveIsFinished = false;
				var target = rnd.Next(asteroidsCoordinates.Count);
				Swap(asteroidsCoordinates.ElementAt(0), asteroidsCoordinates.ElementAt(target));
				asteroidsCoordinates.RemoveAt(0);
				if (target != 0)
					asteroidsCoordinates.RemoveAt(target -1);
			}
		}
		public static bool IsAnyCorrectMove()
		{
			for (int i = 0; i < Map.GetLength(0)-1; i++)
			{
				for (int j = 0; j < Map.GetLength(1); j++)
				{
					if (Map[i,j] == null || Map[i+1,j] == null)
						continue;
					if (!Map[i,j].IsMonster() || !Map[i+1,j].IsMonster())
						continue;
					if (Map[i, j].IsFrozen || Map[i + 1, j].IsFrozen)
						continue;
					var p1 = new Coordinate(i, j);
					var p2 = new Coordinate(i + 1, j);
					Map.SwapArrayElements(p1, p2);
					if (IsCorrectMove(new List<Coordinate>() {p1, p2}))
					{
						Map.SwapArrayElements(p1, p2);
						return true;
					}
					Map.SwapArrayElements(p1, p2);
				}
			}
			for (int i = 0; i < Map.GetLength(0); i++)
			{
				for (int j = 0; j < Map.GetLength(1) - 1; j++)
				{
					if (Map[i, j] == null || Map[i, j+1] == null)
						continue;
					if (!Map[i, j].IsMonster() || !Map[i, j + 1].IsMonster())
						continue;
					var p1 = new Coordinate(i, j);
					var p2 = new Coordinate(i, j + 1);
					Map.SwapArrayElements(p1, p2);
					if (IsCorrectMove(new List<Coordinate>() { p1, p2 }))
					{
						Map.SwapArrayElements(p1, p2);
						return true;
					}
					Map.SwapArrayElements(p1, p2);
				}
			}
			return false;
		}
		public static bool IsCorrectMove(List<Coordinate> coordinates)
		{
			//correct if any triples
			foreach (var coordinate in coordinates)
			{
				var bottomBoundX = Math.Max(0, coordinate.X - 2);
				var bottomBoundY = Math.Max(0, coordinate.Y - 2);
				var topBoundX = Math.Min(Map.GetLength(0) - 1, coordinate.X + 2);
				var topBoundY = Math.Min(Map.GetLength(1) - 1, coordinate.Y + 2);
				for (int i = bottomBoundX; i <= topBoundX - 2; i++)
				{
					if (Map[i, coordinate.Y] == null || Map[i+1, coordinate.Y] == null || Map[i+2, coordinate.Y] == null) continue;
					if (Map[i, coordinate.Y].TypeOfMonster == Map[i + 1, coordinate.Y].TypeOfMonster
					    && Map[i, coordinate.Y].TypeOfMonster == Map[i + 2, coordinate.Y].TypeOfMonster)
						return true;
				}
				for (int i = bottomBoundY; i <= topBoundY - 2; i++)
				{
					if (Map[coordinate.X, i] == null || Map[coordinate.X, i + 1] == null || Map[coordinate.X ,i + 2] == null) continue;
					if (Map[coordinate.X, i].TypeOfMonster == Map[coordinate.X, i+1].TypeOfMonster
						&& Map[coordinate.X, i].TypeOfMonster == Map[coordinate.X, i+2].TypeOfMonster)
						return true;
				}
			}
			return false;
		}

		public static void Swap(Coordinate p1, Coordinate p2)
		{
			Map[p1.X, p1.Y].Move(new Coordinate(p2.X, p2.Y));
			Map[p2.X, p2.Y].Move(new Coordinate(p1.X, p1.Y));
			Map.SwapArrayElements(p1,p2);
			ClickedObject = null;
			MoveIsFinished = !MoveIsFinished;
		}
		public static void Jump(Coordinate p1, Coordinate p2)
		{
			Map[p1.X, p1.Y].Move(p2);
			Map[p2.X, p2.Y].Move(p1);
			Map.SwapArrayElements(p1, p2);
			var temp = Map[p1.X, p1.Y].transform.position;
			Map[p1.X, p1.Y].transform.position = Map[p2.X, p2.Y].transform.position;
			Map[p2.X, p2.Y].transform.position = temp;
		}

		public static void Drop(Coordinate p1, Coordinate p2)
		{
			Map[p1.X, p1.Y].Move(p2);
			Map[p2.X, p2.Y] = Map[p1.X, p1.Y];
			Map[p1.X, p1.Y] = null;
		}
		public static void UpdateField()
		{
			CheckBuildingMonsters();
			for (int i = 0; i < Map.GetLength(0); i++)
			{
				for (int j = 0; j < Map.GetLength(1); j++)
				{
					if (Map[i, j] == null || !Map[i, j].IsMonster() || 
						Dictionaries.MonstersUpgradeDictionary.ContainsKey(Map[i,j].TypeOfMonster))
						continue;
					CheckColumn(Map[i, j], i, j);
					if (Map[i, j] == null || !Map[i, j].IsMonster() ||
						Dictionaries.MonstersUpgradeDictionary.ContainsKey(Map[i, j].TypeOfMonster))
						continue;
					CheckRow(Map[i, j], i, j); //checking row here
				}
			}
		}

		public static void CheckBuildingMonsters()
		{
			for (int i = 0; i < Game.MAP_SIZE; i++)
				for (int j = 0; j < Game.MAP_SIZE - 2; j++)
				{
					if (Map[i, j] != null && Map[i, j + 1] != null && Map[i, j + 2] != null && Dictionaries.MonstersUpgradeDictionary.ContainsKey(Map[i, j].TypeOfMonster))
					{
						if (Map[i, j].TypeOfMonster == Map[i, j + 1].TypeOfMonster &&
						    Map[i, j].TypeOfMonster == Map[i, j + 2].TypeOfMonster)
						{
							Instance.StartCoroutine(DestroyUpgradableObjects(new Coordinate(i, j),
								new Coordinate(i, j + 2)));
						}
					}
				}
			for (int i = 0; i < Game.MAP_SIZE - 2; i++)
				for (int j = 0; j < Game.MAP_SIZE; j++)
				{
					if (Map[i, j] != null && Map[i + 1, j] != null && Map[i + 2, j] != null && Dictionaries.MonstersUpgradeDictionary.ContainsKey(Map[i, j].TypeOfMonster))
					{
						if (Map[i, j].TypeOfMonster == Map[i + 1, j].TypeOfMonster &&
							Map[i, j].TypeOfMonster == Map[i + 2, j].TypeOfMonster && Map[i,j].State != MonsterState.Destroying)
						{
							Instance.StartCoroutine(DestroyUpgradableObjects(new Coordinate(i, j),
								new Coordinate(i + 2, j)));
				
						}
					}
				}
		}

		private static IEnumerator DestroyUpgradableObjects(Coordinate start, Coordinate finish)
		{
			Game.PlayerIsBlocked = true;
			Map[start.X, start.Y].State = MonsterState.Destroying;
			Map[(start.X + finish.X) / 2, (start.Y + finish.Y) / 2].State = MonsterState.Destroying;
			Map[finish.X, finish.Y].State = MonsterState.Destroying;
			var to = GetVectorFromCoord((start.X + finish.X) / 2, (start.Y + finish.Y) / 2);
			var delta = (float)GetDistance(to, Map[start.X, start.Y].transform.position) / 25;
			for (int i = 0; i < 25; i++)
			{
				Map[start.X, start.Y].transform.position =
					Vector3.MoveTowards(Map[start.X, start.Y].transform.position, to, delta);
				Map[finish.X, finish.Y].transform.position =
					Vector3.MoveTowards(Map[finish.X, finish.Y].transform.position, to, delta);
				yield return new WaitForSeconds(0);
			}
			var type = Map[start.X, start.Y].TypeOfMonster;
			if (Map[(start.X + finish.X)/2, (start.Y + finish.Y)/2].IsSceleton())
			{
				Map[start.X, start.Y].GetComponent<SpriteRenderer>().sortingOrder = 2;
				Map[start.X, start.Y].GetComponent<SpriteRenderer>().sprite =
					Monster.SkeletonMudSprite;
				yield return new WaitForSeconds(0.25f);
			}
				
			Map[start.X, start.Y].Destroy();
			Map[(start.X + finish.X) / 2, (start.Y + finish.Y) / 2].Destroy();
			Map[finish.X, finish.Y].Destroy();
			Map[start.X, start.Y] = null;
			Map[(start.X + finish.X) / 2, (start.Y + finish.Y) / 2] = null;
			Map[finish.X, finish.Y] = null;
			if (type == MonsterType.Pumpkin1)
				TransformPumpkin1((start.X + finish.X) / 2, (start.Y + finish.Y) / 2);
			Game.MonsterCreate((start.X + finish.X) / 2, (start.Y + finish.Y) / 2, Dictionaries.MonstersUpgradeDictionary[type]);
			Game.PlayerIsBlocked = false;
		}

		private static void TransformPumpkin1(int x, int y)
		{
			
			Monster branch = ((GameObject)Instantiate(
						Game.MonsterPrefab, GetVectorFromCoord(x, y),
						Quaternion.Euler(new Vector3())))
						.GetComponent<Monster>();

			branch.Initialise(x, y, 'z');
			branch.transform.localScale = new Vector3(1f, 1f);
			branch.GetComponent<SpriteRenderer>().sprite = Monster.PumpkinBranchSprite;
			branch.State = MonsterState.Destroying;
			branch.ToBasketStartTime = Time.time;

			Monster face = ((GameObject)Instantiate(
						Game.MonsterPrefab, GetVectorFromCoord(x, y),
						Quaternion.Euler(new Vector3())))
						.GetComponent<Monster>();
			
			face.Initialise(x, y, 'z');
			face.transform.localScale = new Vector3(1f, 1f);
			face.GetComponent<SpriteRenderer>().sprite = Monster.PumpkinFaceSprite;
			face.State = MonsterState.Destroying;
			face.ToBasketStartTime = Time.time;

		}

		private static double GetDistance(Vector3 p1, Vector3 p2)
		{
			return Math.Sqrt((p1.x - p2.x)*(p1.x - p2.x) + (p1.y - p2.y)*(p1.y - p2.y));
		}
		public static void DestroyAllOf(MonsterType type)
		{
			for (int i = 0; i < Game.MAP_SIZE; i++)
				for (int j = 0; j < Game.MAP_SIZE; j++)
				{
					if (Map[i, j] != null && Map[i, j].TypeOfMonster == type)
					{
						if (!Map[i, j].IsFrozen)
							foreach (var sprite in Map[i, j].GetComponentsInChildren<SpriteRenderer>())
								sprite.color = Color.black;
						Map[i, j].DestroyMonster();
					}
				}
		}

		public static void DestroySquare3X3(Coordinate coordinate)
		{
			var bottomBoundX = Math.Max(0, coordinate.X - 1);
			var bottomBoundY = Math.Max(0, coordinate.Y - 1);
			var topBoundX = Math.Min(Game.MAP_SIZE - 1, coordinate.X + 1);
			var topBoundY = Math.Min(Game.MAP_SIZE - 1, coordinate.Y + 1);
			for (int i = bottomBoundX; i <= topBoundX; i++)
			{
				for (int j = bottomBoundY; j <= topBoundY; j ++)
				{
					if (Map[i, j] != null && Map[i, j].IsMonster())
					{
						if (!Map[i, j].IsFrozen)
						{
							foreach (var sprite in Map[i, j].GetComponentsInChildren<SpriteRenderer>())
								sprite.color = Color.black;
						}
						Map[i, j].DestroyMonster();
					}
				}
			}
		}

		private static void CheckRow(Monster cell, int i, int j)
		{
			var unstableList = new List<Coordinate>();
			var asteroidsColumnList = new List<Coordinate>();
			asteroidsColumnList.Add(cell.GridPosition);
			var unstableIsAdded = false;
			while (i < Map.GetLength(0) - 1 && Map[i + 1, j] != null
				&& (Map[i + 1, j].TypeOfMonster == cell.TypeOfMonster))
			{
				i++;
				asteroidsColumnList.Add(Map[i, j].GridPosition);
			}

			if (asteroidsColumnList.Count < 3)
				return;
			var listToDestroy = new List<Coordinate>();
			foreach (var coordinate in asteroidsColumnList)
			{
				listToDestroy.Add(coordinate);
			}
			if (asteroidsColumnList.Count >= 5)
			{
				var coord = asteroidsColumnList.ElementAt(asteroidsColumnList.Count/2);
				Map[coord.X, coord.Y].DestroyMonster();
				listToDestroy.Remove(coord);
				unstableList.Add(coord);
				unstableIsAdded = true;
			}	
			var bufRowList = new List<Coordinate>();
			foreach (var asteroid in asteroidsColumnList)
			{
				j = asteroid.Y;
				i = asteroid.X;
				while (j < Map.GetLength(0) - 1
					   && Map[i, j + 1] != null && (Map[i, j + 1].TypeOfMonster == cell.TypeOfMonster))
				{
					j++;
					bufRowList.Add(Map[i, j].GridPosition);
				}
				j = asteroid.Y;
				while (j > 0
					   && Map[i, j - 1] != null && (Map[i, j - 1].TypeOfMonster == cell.TypeOfMonster))
				{
					j--;
					bufRowList.Add(Map[i, j].GridPosition);
				}
				if (bufRowList.Count >= 5)
				{
					var coord = bufRowList.ElementAt(asteroidsColumnList.Count / 2);
					Map[coord.X, coord.Y].DestroyMonster();
					listToDestroy.Remove(coord);
					unstableList.Add(coord);
					unstableIsAdded = true;
				}
				if (bufRowList.Count >= 2)
				{
					foreach (var coordinate in bufRowList)
					{
						listToDestroy.Add(coordinate);
					}
					if (!unstableIsAdded)
					{
						Map[asteroid.X, asteroid.Y].DestroyMonster();
						listToDestroy.Remove(asteroid);
						unstableList.Add(asteroid);
						unstableIsAdded = true;
					} 
				}
				bufRowList.Clear();
			}
			foreach (var coordinate in listToDestroy
				.Where(x => Map[x.X, x.Y] != null && Map[x.X, x.Y].IsMonster()))
			{
				Map[coordinate.X, coordinate.Y].DestroyMonster();
			}
			foreach (var coord in unstableList)
			{
				Game.MonsterCreate(coord.X, coord.Y,'W', 0);
			}
		}
		private static void CheckColumn(Monster cell, int i, int j)
		{
			
			var unstableList = new List<Coordinate>();
			var asteroidsColumnList = new List<Coordinate> {cell.GridPosition};
			var unstableIsAdded = false;
			while (j < Game.MAP_SIZE - 1 && Map[i, j + 1] != null && 
				(Map[i, j + 1].TypeOfMonster == cell.TypeOfMonster))
			{
				j++;
				asteroidsColumnList.Add(Map[i, j].GridPosition);
			}
			if (asteroidsColumnList.Count < 3)
				return;
			var listToDestroy = new List<Coordinate>();
			foreach (var coordinate in asteroidsColumnList)
			{
				listToDestroy.Add(coordinate);
			}
			if (asteroidsColumnList.Count >= 5)
			{
				var coord = asteroidsColumnList.ElementAt(asteroidsColumnList.Count / 2);
				Map[coord.X, coord.Y].DestroyMonster();
				listToDestroy.Remove(coord);
				unstableList.Add(coord);
				unstableIsAdded = true;
			}

			var bufRowList = new List<Coordinate>();
			foreach (var asteroid in asteroidsColumnList)
			{
				j = asteroid.Y;
				i = asteroid.X;
				while (i < Map.GetLength(0) - 1
				       && Map[i + 1, j] != null && (Map[i + 1, j].TypeOfMonster == cell.TypeOfMonster))
				{
					i++;
					bufRowList.Add(Map[i, j].GridPosition);
				}
				i = asteroid.X;
				while (i > 0
				       && Map[i - 1, j] != null && (Map[i - 1, j].TypeOfMonster == cell.TypeOfMonster))
				{
					i--;
					bufRowList.Add(Map[i, j].GridPosition);
				}
				if (bufRowList.Count >= 5)
				{
					var coord = bufRowList.ElementAt(asteroidsColumnList.Count / 2);
					Map[coord.X, coord.Y].DestroyMonster();
					listToDestroy.Remove(coord);
					unstableList.Add(coord);
					unstableIsAdded = true;
				}
				if (bufRowList.Count >= 2)
				{
					foreach (var coordinate in bufRowList)
					{
						listToDestroy.Add(coordinate);
					}
					if (!unstableIsAdded)
					{
						Map[asteroid.X, asteroid.Y].DestroyMonster();
						listToDestroy.Remove(asteroid);
						unstableList.Add(asteroid);
						unstableIsAdded = true;
					}
				}
				bufRowList.Clear();
			}
			foreach (var coordinate in listToDestroy
				.Where(x=>Map[x.X, x.Y] != null && Map[x.X, x.Y].IsMonster()))
			{
				Map[coordinate.X, coordinate.Y].DestroyMonster();
			}
			foreach (var coord in unstableList)
			{
				Game.MonsterCreate(coord.X, coord.Y, 
					'W');
			}
		}
		public static Vector3 GetVectorFromCoord(int i, int j)
		{
			return new Vector3(i*1.07f - Game.MAP_SIZE/2 + 0.25f ,
							Game.MAP_SIZE/2 - j*1.048f + 0.5f, 0);
		}

		public static bool IsAnyMoving()
		{
			for (int i = 0; i < Game.MAP_SIZE; i++)
				for (int j = 0; j < Game.MAP_SIZE; j++)
					if (Map[i,j] != null && 
						(Map[i, j].State == MonsterState.Dropping 
						|| Map[i, j].State == MonsterState.Moving || Map[i, j].State == MonsterState.Growing
						|| Map[i, j].State == MonsterState.Decreasing
						|| Map[i,j].State == MonsterState.Destroying))
							return true;
			return false;
		}
		
	}
}
