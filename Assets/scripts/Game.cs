﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Assets.scripts;
using Assets.scripts.Enums;
using Assets.scripts.Skills;
using SimpleJSON;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//using Newtonsoft.Json;
namespace Assets.scripts
{
	public class Game : MonoBehaviour
	{
		// Use this for initialization
		public static GameObject MonsterPrefab;
		public static GameObject BatPrefab;
		public static GameObject FlagPrefab;
		public static GameObject VoodooPrefab;
		public static GameObject ZombiePrefab;
		public static GameObject PlasmPrefab;
		public static GameObject SnakePrefab;
		public static GameObject GhostPrefab;
		public static GameObject SpiderPrefab;
		public static GameObject BombFirePrefab;
		public static GameObject SkullPrefab;
		public static GameObject SmokePrefab;
		public const int MAP_SIZE = 8;
		public static LevelInfo LevelInformation;
		public static int TurnsLeft;
		public static Game Instance;
		public static Vector3 BasketCoordinate = new Vector3(3f, -4f);
		public static Vector3 PotCoordinate = new Vector3(0f, -4f);
		public static Vector3 PortalCoordinate = new Vector3(1.5f, -4.0f);
		public static Texture2D MainCursor;
		public static Texture2D FireCursor;
		public static Texture2D ElectroCursor;
		public static ClickState ClickType;
		public float LastAdviceTime;
		public static int Level;
		public static bool GameIsFinished;
		public static bool DropIsBlocked = true;
		public static HeroType HeroType;
		public static int CandlesLeft;
		private void Awake()
		{
			CandlesLeft = 3;
			HeroType = (HeroType)PlayerPrefs.GetInt("CurrentHero");
			LastAdviceTime = Time.time;
			//PlayerPrefs.DeleteAll();							   
			GameIsFinished = false;
			PlayerIsBlocked = false;
			WaterField.Bridges = new Dictionary<Coordinate, Direction>();
			WaterField.BridgeObjects = new List<Monster>();
			WaterField.River = new List<Coordinate>();
			Dictionaries.MonsterCounter = new Dictionary<MonsterType, int>()
			{
				{MonsterType.Zombie, 0},
				{MonsterType.Spider, 0},
				{MonsterType.Voodoo, 0},
				{MonsterType.Bat, 0},
				{MonsterType.Ghost, 0},
				{MonsterType.Coocon, 0},
				{MonsterType.Plasm, 0},
				{MonsterType.Snake, 0},
				{MonsterType.Pumpkin1, 0},
				{MonsterType.Pumpkin2, 0},
				{MonsterType.Pumpkin3, 0},
				{MonsterType.Skeleton1, 0},
				{MonsterType.Skeleton2, 0},
				{MonsterType.Skeleton3, 0},
				{MonsterType.Skeleton4, 0},

			};
			Level = PlayerPrefs.GetInt("CurrentLevel");
			var path = "levels/" + Level + "";
			TextAsset file = Resources.Load(path, typeof(TextAsset)) as TextAsset;
			string json = file.ToString();
			LevelInformation = LevelInfo.CreateFromJSON(json);
		}

		private void Start()
		{
			ClickType = ClickState.Default;
			SmokePrefab = Resources.Load("objects/smoke/smokePrefab", typeof(GameObject)) as GameObject;
			BombFirePrefab = Resources.Load("BombFire", typeof(GameObject)) as GameObject;
			MonsterPrefab = Resources.Load("MonsterObject", typeof (GameObject)) as GameObject;
			BatPrefab = Resources.Load("objects/bat/Bat", typeof(GameObject)) as GameObject;
			VoodooPrefab = Resources.Load("objects/voodoo/Voodoo", typeof(GameObject)) as GameObject;
			ZombiePrefab = Resources.Load("objects/zombie/Zombie", typeof(GameObject)) as GameObject;
			GhostPrefab = Resources.Load("objects/ghost/Ghost", typeof(GameObject)) as GameObject;
			SpiderPrefab = Resources.Load("objects/spider/Spider", typeof(GameObject)) as GameObject;
			SkullPrefab = Resources.Load("objects/heroes/Mummy/skull/SkullPrefab", typeof(GameObject)) as GameObject;
			FlagPrefab = Resources.Load("objects/heroes/Cleric/HolyFlag/Flag", typeof(GameObject)) as GameObject;
			PlasmPrefab = Resources.Load("objects/plasm/Plasm", typeof(GameObject)) as GameObject;
			SnakePrefab = Resources.Load("objects/snake/Snake", typeof(GameObject)) as GameObject;
			BrokenWebPrefab = Resources.Load("objects/web/BrokenWeb", typeof(GameObject)) as GameObject;
			Monster.EmptyCellSprite = Resources.Load("objects/graves/grave1", typeof(Sprite)) as Sprite;
			Monster.BombSprite = Resources.Load("Sprites/bomb", typeof(Sprite)) as Sprite;
			OffCandleSprite = Resources.Load("GameSprites/candleOff", typeof(Sprite)) as Sprite;
			OnCandleSprite = Resources.Load("GameSprites/candleOn", typeof(Sprite)) as Sprite;
			Monster.BlackHoleSprite = Resources.Load("Sprites/3Black_hole_02", typeof(Sprite)) as Sprite;
			Monster.CooconSprite = Resources.Load("objects/cocoon/cocoon", typeof(Sprite)) as Sprite;
			Monster.WaterHSprite = Resources.Load("objects/river/WaterH", typeof(Sprite)) as Sprite;
			Monster.WaterVSprite = Resources.Load("objects/river/WaterV", typeof(Sprite)) as Sprite;
			Monster.WaterUpperRightSprite = Resources.Load("objects/river/WaterUR", typeof(Sprite)) as Sprite;
			Monster.WaterUpperLeftSprite= Resources.Load("objects/river/WaterUL", typeof(Sprite)) as Sprite;
			Monster.WaterDownRightSprite = Resources.Load("objects/river/WaterDR", typeof(Sprite)) as Sprite;
			Monster.WaterDownLeftSprite = Resources.Load("objects/river/WaterDL", typeof(Sprite)) as Sprite;
			Monster.RaftSprite = Resources.Load("objects/river/Raft", typeof(Sprite)) as Sprite;
			Monster.Pumpkin1Sprite = Resources.Load("objects/pumpkin/pumpkin1", typeof (Sprite)) as Sprite;
			Monster.Pumpkin2Sprite = Resources.Load("objects/pumpkin/pumpkin2", typeof(Sprite)) as Sprite;
			Monster.Pumpkin3Sprite = Resources.Load("objects/pumpkin/pumpkin3", typeof(Sprite)) as Sprite;
			Monster.PumpkinFaceSprite = Resources.Load("objects/pumpkin/pumpkinFace", typeof(Sprite)) as Sprite;
			Monster.PumpkinBranchSprite = Resources.Load("objects/pumpkin/pumpkinTop", typeof(Sprite)) as Sprite;
			Monster.SkeletonSprite1= Resources.Load("objects/skeleton/skeleton1", typeof(Sprite)) as Sprite;
			Monster.SkeletonSprite2= Resources.Load("objects/skeleton/skeleton2", typeof(Sprite)) as Sprite;
			Monster.SkeletonSprite3= Resources.Load("objects/skeleton/skeleton3", typeof(Sprite)) as Sprite;
			Monster.SkeletonSprite4= Resources.Load("objects/skeleton/skeleton4", typeof(Sprite)) as Sprite;
			Monster.SkeletonMudSprite= Resources.Load("objects/skeleton/skeletonMud", typeof(Sprite)) as Sprite;
			Monster.BatSprite = Resources.Load("targetsSprites/bat", typeof(Sprite)) as Sprite;
			Monster.GhostSprite= Resources.Load("targetsSprites/ghost", typeof(Sprite)) as Sprite;
			Monster.ZombieSprite = Resources.Load("targetsSprites/zombie", typeof(Sprite)) as Sprite;
			Monster.VoodooSprite = Resources.Load("targetsSprites/voodoo", typeof(Sprite)) as Sprite;
			Monster.SpiderSprite = Resources.Load("targetsSprites/spider", typeof(Sprite)) as Sprite;
			Monster.PlasmSprite = Resources.Load("targetsSprites/plasm", typeof(Sprite)) as Sprite;
			Monster.SnakeSprite = Resources.Load("targetsSprites/snake", typeof(Sprite)) as Sprite;
			Monster.CandleCellSprite = Resources.Load("objects/graves/candle", typeof(Sprite)) as Sprite;
			Monster.EmptyCell2Sprite = Resources.Load("objects/graves/grave2", typeof(Sprite)) as Sprite;
			Monster.MagicSkullSprite = Resources.Load("objects/heroes/Mummy/skull/scull cell", typeof(Sprite)) as Sprite;
			Cursor.SetCursor(MainCursor, new Vector2(0,0), CursorMode.Auto);
			Dictionaries.HeroTypeToSkills = new Dictionary<HeroType, ISkills>
			{
				{HeroType.Death, new DeathSkills()},
				{HeroType.Hunter, new HunterSkills()},
				{HeroType.Cleric, new ClericSkills()},
				{HeroType.Vampire, new VampireSkills()},
				{HeroType.Mummy, new MummySkills() },
				{HeroType.Wolverine, new WolverineSkills()}
			};
			Dictionaries.TypesToSprites = new Dictionary<MonsterType, Sprite>
			{
				{MonsterType.EmptyCell, Monster.EmptyCellSprite},
				{ MonsterType.EmptyCell2, Monster.EmptyCell2Sprite},
				{ MonsterType.CandleCell, Monster.CandleCellSprite},
				{ MonsterType.BlackHole, Monster.BlackHoleSprite},
				{MonsterType.Coocon, Monster.CooconSprite},
				{MonsterType.Bomb, Monster.BombSprite },
				{MonsterType.WaterHorizontal, Monster.WaterHSprite },
				{MonsterType.WaterVertical, Monster.WaterVSprite },
				{MonsterType.WaterUpperRight, Monster.WaterUpperRightSprite},
				{MonsterType.WaterUpperLeft, Monster.WaterUpperLeftSprite},
				{MonsterType.WaterDownRight, Monster.WaterDownRightSprite},
				{MonsterType.WaterDownLeft, Monster.WaterDownLeftSprite},
				{MonsterType.Raft, Monster.RaftSprite},
				{MonsterType.Pumpkin1, Monster.Pumpkin1Sprite},
				{MonsterType.Pumpkin2, Monster.Pumpkin2Sprite},
				{MonsterType.Pumpkin3, Monster.Pumpkin3Sprite},
				{MonsterType.Skeleton1, Monster.SkeletonSprite1},
				{MonsterType.Skeleton2, Monster.SkeletonSprite2},
				{MonsterType.Skeleton3, Monster.SkeletonSprite3},
				{MonsterType.Skeleton4, Monster.SkeletonSprite4},
				{MonsterType.Zombie, Monster.ZombieSprite},
				{MonsterType.Voodoo, Monster.VoodooSprite},
				{MonsterType.Spider, Monster.SpiderSprite},
				{MonsterType.Ghost, Monster.GhostSprite},
				{MonsterType.Snake, Monster.SnakeSprite},
				{MonsterType.Plasm, Monster.PlasmSprite},
				{MonsterType.Bat, Monster.BatSprite},
				{MonsterType.MagicSkull, Monster.MagicSkullSprite},

			};
			Instance = this;
			TurnsLeft = LevelInformation.Turns;
			Dictionaries.MonsterGenerationList = LevelInformation.Monsters.ToCharArray().ToList();
			GenerateMap();

		}

		public static GameObject BrokenWebPrefab;

		public static bool	 PlayerIsBlocked;
		public static bool IsPlayerBlocked()
		{
			if (GameField.IsAnyMoving() || PlayerIsBlocked || GameIsFinished)
				return true;
			return false;
		}
		public void GoToMenu()
		{
			PlayerPrefs.SetInt("FromGame", 1);
			PlayerPrefs.Save();
			SceneManager.LoadScene("MainMenu");
		}
		// Update is called once per frame
		public void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				GoToMenu();
			if (GameIsFinished) return;
			CheckCursorClick();
			if (TurnsLeft == 0)
				PlayerIsBlocked = true;                        
			GameField.CheckUpperBorder();
			if (!GameField.IsAnyMoving())
			{
				GameField.UpdateField();
				if (!GameField.IsAnyMovingOrDestroying() && !GameField.IsAnyCorrectMove() && !DropIsBlocked && !PlayerIsBlocked)
					GameField.Shuffle();
				if (Time.time > LastAdviceTime + 5f)
				{
					LastAdviceTime = Time.time + 5f;
					GameField.GetAdvice();
				}
			}
		}

		private void CheckCursorClick()
		{
			
			if (Input.GetMouseButton(0))
			{
				var v3 = Input.mousePosition;
				v3.z = 10.0f;
				v3 = Camera.main.ScreenToWorldPoint(v3);
				var rect = new Rect(new Vector2(-1.8f, -3.4f), new Vector2(7.5f, 7.5f));
				if (ClickType != ClickState.Default && !rect.Contains(v3))
				{
					ClickType = ClickState.Default;
					Cursor.SetCursor(MainCursor, new Vector2(0, 0), CursorMode.Auto);
				}	
			}
		}

		private void GenerateMap()
		{
			GameField.Map = new Monster[MAP_SIZE, MAP_SIZE];
			WaterField.Map = new Monster[MAP_SIZE,MAP_SIZE];
			for (var i = 0; i < MAP_SIZE; i++)
			{
				for (var j = 0; j < MAP_SIZE; j++)
				{
					try
					{
						MonsterCreate(i, j, LevelInformation.Map.ElementAt(j * (MAP_SIZE + 1) + i));
					}
					catch (Exception)
					{
						
						Debug.Log(LevelInformation.Map.ElementAt(j * (MAP_SIZE + 1) + i));
					}
					
				}
			}
			WaterField.GenerateRiver();
			DropIsBlocked = false;
		}

		public static void MonsterCreate(int x, int y, MonsterType type, float delay = 0)
		{
			char ctype = Dictionaries.CharsToObjectTypes.First(p => p.Value == type).Key;
			MonsterCreate(x,y,ctype,delay);
		}
		public static void MonsterCreate(int x, int y, char type, float delay = 0)
		{
			int respY;
			respY = y == 0 ? y - 1 : y;			
			if (Dictionaries.MonsterTypes.Contains(Dictionaries.CharsToObjectTypes[type]))
			{
				GameObject prefab = MonsterPrefab;
				switch (type)
				{
					case 'V':
						prefab = VoodooPrefab;
						break;
					case 'S':
						prefab = SpiderPrefab;
						break;
					case 'B':
						prefab = BatPrefab;
						break;
					case 'G':
						prefab = GhostPrefab;
						break;
					case 'Z':
						prefab = ZombiePrefab;
						break;
					case 'P':
						prefab = PlasmPrefab;
						break;
					case 'N':
						prefab = SnakePrefab;
						break;
				}
				try
				{
					Monster monster = ((GameObject) Instantiate(
						prefab, GameField.GetVectorFromCoord(x, respY),
						Quaternion.Euler(new Vector3())))
						.GetComponent<Monster>();
					monster.Initialise(x, y, type, delay); //?
					GameField.Map[x, y] = monster;
				}
				catch (Exception e)
				{
					Debug.Log(e);
				}

			}
			else if (type >= 'e' && type <= 'p')
			{
				if (type >= 'k')
				{
					Monster bridge = ((GameObject)Instantiate(
						MonsterPrefab, GameField.GetVectorFromCoord(x, y),
						Quaternion.Euler(new Vector3())))
						.GetComponent<Monster>();
					bridge.Initialise(x, y, 'R', delay);
					WaterField.BridgeObjects.Add(bridge);
					WaterField.Bridges.Add(new Coordinate(x,y), Direction.Forward);
					bridge.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
					bridge.gameObject.GetComponent<CircleCollider2D>().enabled = false;
					type -= (char) 6;
				}
				Monster water = ((GameObject)Instantiate(
						MonsterPrefab, GameField.GetVectorFromCoord(x, y),
						Quaternion.Euler(new Vector3())))
						.GetComponent<Monster>();

				water.Initialise(x, y, type, delay); //?
				WaterField.Map[x, y] = water;
				water.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
				water.gameObject.GetComponent<CircleCollider2D>().enabled = false;
			}
			else
			{
				Monster monster = ((GameObject)Instantiate(
						MonsterPrefab, GameField.GetVectorFromCoord(x, y),
						Quaternion.Euler(new Vector3())))
						.GetComponent<Monster>();
				monster.Initialise(x, y, type, delay); //?
				GameField.Map[x, y] = monster;
			}
			
		}

		public static IEnumerator NextTurn()
		{
			TurnsLeft--;		
			if (TurnsLeft == 0)
			{				
				yield break;
			}
			Instance.LastAdviceTime = Time.time + 5f;
			while (GameField.IsAnyMovingOrDestroying())
			{
				yield return null;
			}
			if (GameIsFinished) yield break;
			if (CandlesLeft == 3 && (LevelInformation.Turns - TurnsLeft) >= LevelInformation.Candles1)
				BurnCandle(1);
			if (CandlesLeft == 2 && (LevelInformation.Turns - TurnsLeft) >= LevelInformation.Candles2)
				BurnCandle(2);
			WaterField.ShiftBridges();
			while (GameField.IsAnyMovingOrDestroying())
			{
				yield return null;
			}
			ActivateFlagsAndVortexes();
			GameField.MoveIsFinished = true;
			
		}

		public static Sprite OffCandleSprite;
		public static Sprite OnCandleSprite;
		
		public static void BurnCandle(int candleToBurn)
		{
			CandlesLeft--;
			var candle = GameObject.Find("CandlesCounter").transform.GetChild(candleToBurn - 1);
			candle.GetComponent<Image>().overrideSprite = OffCandleSprite;
			candle.GetComponent<Image>().SetNativeSize();
			candle.transform.position-=new Vector3(0,+0.15f);
			var smoke = ((GameObject) Instantiate(
				SmokePrefab, candle.transform.position + new Vector3(0f,0.5f),
				Quaternion.Euler(new Vector3())));
			//smoke.transform.parent = candle;
			//smoke.transform.localScale = new Vector3(1,1,1);
		}

		private static void ActivateFlagsAndVortexes()
		{
			for (int i = 0; i < MAP_SIZE; i++)
				for (int j = 0; j < MAP_SIZE; j++)
				{
					if (GameField.Map[i, j] != null && GameField.Map[i, j].TypeOfMonster == MonsterType.ClericFlag)
					{
						GameField.Map[i, j].FlagCounter--;
						if (GameField.Map[i, j].FlagCounter <= 0)
						{
							GameField.Map[i, j].ActivateFlag();
							GameField.Map[i, j].FlagCounter = GameField.Map[i, j].FlagFrequancy;
						}

					}
					if (GameField.Map[i, j] != null && GameField.Map[i, j].TypeOfMonster == MonsterType.BloodVortex)
					{
						GameField.Map[i, j].BloodVortexLivesLeft--;
						GameField.Map[i, j].HandleBloodVortex();

					}
					if (GameField.Map[i, j] != null && GameField.Map[i, j].TypeOfMonster == MonsterType.MagicSkull)
					{
						GameField.Map[i, j].HandleMagicSkull();
					}
				}


		}
	}
}
