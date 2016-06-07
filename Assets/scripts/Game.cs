using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Assets.scripts;
using Assets.scripts.Enums;
using UnityEngine.UI;

//using Newtonsoft.Json;
namespace Assets.scripts
{
	public class Game : MonoBehaviour
	{
		// Use this for initialization
		public static GameObject MonsterPrefab;
		public static GameObject BatPrefab;
		public static GameObject VoodooPrefab;
		public static GameObject ZombiePrefab;
		public static GameObject GhostPrefab;
		public static GameObject SpiderPrefab;
		public static GameObject BombFirePrefab;
		public const int MAP_SIZE = 8;
		public static LevelInfo LevelInformation;
		public static int TurnsLeft;
		public static Game Instance;
		public static Vector3 BasketCoordinate = new Vector3(0f, -4f);
		public static Vector3 PotCoordinate = new Vector3(3f, -4f);
		public static Vector3 PortalCoordinate = new Vector3(1.5f, -4.0f);
		public static Texture2D MainCursor;
		public static Texture2D FireCursor;
		public static Texture2D ElectroCursor;
		public static ClickState ClickType;
		public float LastAdviceTime;
		public static int Level;
		public static bool GameIsFinished;
		public static HeroType HeroType;
		private void Awake()
		{
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
				{MonsterType.Pumpkin1, 0},
				{MonsterType.Pumpkin2, 0},
				{MonsterType.Pumpkin3, 0},
				{MonsterType.Skeleton1, 0},
				{MonsterType.Skeleton2, 0},
				{MonsterType.Skeleton3, 0},
				{MonsterType.Skeleton4, 0},

			};
			GameObject.Find("GameManager").GetComponentInChildren<AudioSource>().volume = PlayerPrefs.GetInt("Sound")/2f;
			Level = PlayerPrefs.GetInt("CurrentLevel");
			//LevelInformation = JsonConvert.DeserializeObject<LevelInfo>(File.ReadAllText("Assets/levels/1.json"));
			if (Level == 1)
			{
				LevelInformation = new LevelInfo {Map = "ZSVBBGBB ZHGGBBSS GBZBGSZS ZGGSVHVG GVZBVZBS ZHZVBZGV GBGSZSGS ZBZBBGBZ"};
				LevelInformation.Targets = new Dictionary<char, int>()
				{
					{'Z', 10},
					{'S', 3},
					{'V', 0}
				};
				TurnsLeft = 100;
			}
			if (Level == 2)
			{
				LevelInformation = new LevelInfo { Map = "cSccBcBE EaaBaBSE ekeejVSV EGGSfSVE EVZBmeeo EVZVBZGE EBGyySyE EEEEEEEE",
					Skeleton = true};
				LevelInformation.Targets = new Dictionary<char, int>()
				{
					{'G', 30},
					{'Z', 20},
				};
				TurnsLeft = 100;
			}
			if (Level == 3)
			{
				LevelInformation = new LevelInfo { Map = "ZSVBBGBB ZHGGBBHS GSZEESVS ZGEEEEVG GVEEEEBS ZVZEEZGV GBGSZSGS ZHZBBGBZ" };
				LevelInformation.Targets = new Dictionary<char, int>()
				{
					{'G', 20},
					{'B', 10},
					{'S', 20}
				};
				TurnsLeft = 15;
			}
			if (Level == 4)
			{
				LevelInformation = new LevelInfo { Map = "ZSVBBGBB ZSGGBBVS GBZHHSCS SZGHHVGV GVZBVZBS ZVZVBZGV GBGSZSGS ZBZBBCBZ" };
				LevelInformation.Targets = new Dictionary<char, int>()
				{
					{'V', 40},
					{'Z', 35}
				};
				TurnsLeft = 20;
			}
			if (Level == 5)
			{
				LevelInformation = new LevelInfo { Map = "ECVBBCBE ZBGGVBSS CSZBGSCS ZSGGVBVG GCZHSCBS ZVZVBZGV CBGSZCGS EBZVBGHE" };
				LevelInformation.Targets = new Dictionary<char, int>()
				{
					{'C', 8},
					{'S', 50},
				};
				TurnsLeft = 25;
			}
		}

		private void Start()
		{
			ClickType = ClickState.Default;
			BombFirePrefab = Resources.Load("BombFire", typeof(GameObject)) as GameObject;
			MonsterPrefab = Resources.Load("MonsterObject", typeof (GameObject)) as GameObject;
			BatPrefab = Resources.Load("objects/bat/Bat", typeof(GameObject)) as GameObject;
			VoodooPrefab = Resources.Load("objects/voodoo/Voodoo", typeof(GameObject)) as GameObject;
			ZombiePrefab = Resources.Load("objects/zombie/Zombie", typeof(GameObject)) as GameObject;
			GhostPrefab = Resources.Load("objects/ghost/Ghost", typeof(GameObject)) as GameObject;
			SpiderPrefab = Resources.Load("objects/spider/Spider", typeof(GameObject)) as GameObject;
			Monster.EmptyCellSprite = Resources.Load("objects/graves/grave1", typeof(Sprite)) as Sprite;
			Monster.BombSprite = Resources.Load("Sprites/bomb", typeof(Sprite)) as Sprite;
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
			Cursor.SetCursor(MainCursor, new Vector2(0,0), CursorMode.Auto);
			Dictionaries.TypesToSprites = new Dictionary<MonsterType, Sprite>
			{
				{MonsterType.EmptyCell, Monster.EmptyCellSprite},
				{MonsterType.BlackHole, Monster.BlackHoleSprite},
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
				{MonsterType.Bat, Monster.BatSprite},

			};
			Instance = this;

			if (LevelInformation.Pumpkins)
			{
				Dictionaries.MonsterGenerationList.Add('x');
			}
			if (LevelInformation.Skeleton)
			{
				Dictionaries.MonsterGenerationList.Add('a');
			}
			GenerateMap();

		}

		public static bool PlayerIsBlocked;
		public static bool IsPlayerBlocked()
		{
			if (GameField.IsAnyMoving() || PlayerIsBlocked || GameIsFinished)
				return true;
			return false;
		}
		// Update is called once per frame
		public void Update()
		{
			if (GameIsFinished) return;
			CheckCursorClick();
			if (TurnsLeft == 0)
				PlayerIsBlocked = true;                        
			GameField.CheckUpperBorder();
			if (!GameField.IsAnyMoving())
			{
				if (!GameField.IsAnyCorrectMove())
					GameField.Shuffle();
				GameField.UpdateField();

				if (Time.time > LastAdviceTime + 5f)
				{
					LastAdviceTime += 5f;
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
					MonsterCreate(i, j, LevelInformation.Map.ElementAt(j*(MAP_SIZE + 1) + i));
				}
			}
			WaterField.GenerateRiver();
		}

		public static void MonsterCreate(int x, int y, MonsterType type, float delay = 0)
		{
			char ctype = Dictionaries.CharsToObjectTypes.First(p => p.Value == type).Key;
			MonsterCreate(x,y,ctype,delay);
		}
		public static void MonsterCreate(int x, int y, char type, float delay = 0)
		{
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
				}

				Monster monster = ((GameObject)Instantiate(
					prefab, GameField.GetVectorFromCoord(x, y),
					Quaternion.Euler(new Vector3())))
					.GetComponent<Monster>();
				monster.Initialise(x, y, type, delay); //?
				GameField.Map[x, y] = monster;
				
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
				water.gameObject.GetComponent<CircleCollider2D>().enabled = true;
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
			while (GameField.IsAnyMoving())
			{
				yield return null;
			}
			Instance.LastAdviceTime = Time.time + 5f;
			Instance.Update();
			while (GameField.IsAnyMoving())
			{
				yield return null;
			}
			WaterField.ShiftBridges();
			
		}
	}
}
