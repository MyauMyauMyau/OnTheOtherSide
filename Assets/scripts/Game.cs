using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Assets.scripts;
//using Newtonsoft.Json;
namespace Assets.scripts
{
	public class Game : MonoBehaviour
	{

		// Use this for initialization
		public static GameObject SpaceObjectPrefab;
		public const int MAP_SIZE = 8;
		public static LevelInfo LevelInformation;
		public static int TurnsLeft;
		public static Game instance;
		public static Vector3 BasketCoordinate = new Vector3(-4.2f, -0.8f);
		public static Vector3 PotCoordinate = new Vector3(-5.53f, 0.8f);
		public static Vector3 PortalCoordinate = new Vector3(-3.0f, -2.0f);

		public static Dictionary<MonsterType, int> MonsterCounter = new Dictionary<MonsterType, int>()
		{
			{MonsterType.Zombie, 0},
			{MonsterType.Spider, 0},
			{MonsterType.Vampire, 0},
			{MonsterType.Bat, 0},
			{MonsterType.Ghost, 0},
			{MonsterType.Coocon, 0}
		}; 

		private void Awake()
		{
			//LevelInformation = JsonConvert.DeserializeObject<LevelInfo>(File.ReadAllText("Assets/levels/1.json"));
			LevelInformation =
			new LevelInfo { Map = "ZSVBBGBB ZHGGBBHS GBZBGSCS ZGGEEEVG GVZBVZBS ZVZVBZGV GBGSZSGS ZBZBBGBZ" };
			LevelInformation.Targets = new Dictionary<char, int>()
			{
				{ 'Z', 25},
				{'S', 25},
				{ 'V', 25}
			};
			//new LevelInfo { Map = "GBPY EYGR RYYE GRYR RYGY GPGP RYRB GYGY" };
			
		}
		private void Start()
		{
			SpaceObjectPrefab = Resources.Load("SpaceObjectPrefab", typeof (GameObject)) as GameObject;
			Monster.VampireSprite = Resources.Load("Sprites/2-vampir", typeof(Sprite)) as Sprite;
			Monster.ZombieSprite = Resources.Load("Sprites/2-zombak", typeof(Sprite)) as Sprite;
			Monster.SpiderSprite = Resources.Load("Sprites/2-pauk", typeof(Sprite)) as Sprite;
			Monster.BatSprite = Resources.Load("Sprites/2-letuchka", typeof(Sprite)) as Sprite;
			Monster.GhostSprite = Resources.Load("Sprites/2-prizrak", typeof(Sprite)) as Sprite;
			Monster.EmptyCellSprite = Resources.Load("Sprites/1-pustaiaKLETKA", typeof(Sprite)) as Sprite;
			Monster.UnstableVampireSprite = Resources.Load("Sprites/1-bomba", typeof(Sprite)) as Sprite;
			Monster.UnstableZombieSprite = Resources.Load("Sprites/1-bomba", typeof(Sprite)) as Sprite;
			Monster.UnstableSpiderSprite = Resources.Load("Sprites/1-bomba", typeof(Sprite)) as Sprite;
			Monster.UnstableBatSprite = Resources.Load("Sprites/1-bomba", typeof(Sprite)) as Sprite;
			Monster.UnstableGhostSprite = Resources.Load("Sprites/1-bomba", typeof(Sprite)) as Sprite;
			Monster.BlackHoleSprite = Resources.Load("Sprites/3Black_hole_02", typeof(Sprite)) as Sprite;
			Monster.CooconSprite = Resources.Load("Sprites/1-kokonZAMOROZKA", typeof(Sprite)) as Sprite;

			Monster.TypesToSprites = new Dictionary<MonsterType, Sprite>
			{
				{MonsterType.Zombie, Monster.ZombieSprite},
				{MonsterType.Spider, Monster.SpiderSprite},
				{MonsterType.Vampire, Monster.VampireSprite},
				{MonsterType.Bat, Monster.BatSprite},
				{MonsterType.Ghost, Monster.GhostSprite},
				{MonsterType.EmptyCell, Monster.EmptyCellSprite},
				{MonsterType.BlackHole, Monster.BlackHoleSprite},
				{MonsterType.Coocon, Monster.CooconSprite}
			};
			Monster.StableToUnstableSprites = new Dictionary<MonsterType, Sprite>
			{
				{MonsterType.Zombie, Monster.UnstableZombieSprite},
				{MonsterType.Spider, Monster.UnstableSpiderSprite},
				{MonsterType.Vampire, Monster.UnstableVampireSprite},
				{MonsterType.Bat, Monster.UnstableBatSprite},
				{MonsterType.Ghost, Monster.UnstableGhostSprite},

			};
			instance = this;
			TurnsLeft = 10;
			GenerateMap();

		}

		// Update is called once per frame
		public void Update()
		{
			//if (TurnsLeft == 0)
				//Debug.Log("Game Is Finished");
			GameField.CheckUpperBorder();
			if (!GameField.IsAnyMoving())
			{
				if (!GameField.IsAnyCorrectMove())
					GameField.Shuffle();
				GameField.UpdateField();
			}
		}

		private void GenerateMap()
		{
			GameField.Map = new Monster[MAP_SIZE, MAP_SIZE];
			for (var i = 0; i < MAP_SIZE; i++)
			{
				for (var j = 0; j < MAP_SIZE; j++)
				{
					SpaceObjectCreate(i, j, LevelInformation.Map.ElementAt(j*(MAP_SIZE + 1) + i));
				}
			}
			int trgCnt = 0;
			foreach (var target in LevelInformation.Targets)
			{
				GameObject go = new GameObject();
				SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
				renderer.sprite = Monster.TypesToSprites[Monster.CharsToObjectTypes[target.Key]];
				go.transform.position = new Vector3(-1.1f + trgCnt * 2.2f, -4.2f);
				trgCnt++;
			}
		}

		public static void SpaceObjectCreate(int x, int y, char type, float delay = 0, bool isUnstable = false)
		{
			Monster monster = ((GameObject)Instantiate(
						SpaceObjectPrefab, GameField.GetVectorFromCoord(x,y),
						Quaternion.Euler(new Vector3())))
						.GetComponent<Monster>();
			monster.Initialise(x, y, type, delay, isUnstable); //?
			GameField.Map[x, y] = monster;
		}

	}
}
