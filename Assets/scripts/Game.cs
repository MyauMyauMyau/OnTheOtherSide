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
		public static GameObject SpaceObjectPrefab;
		public static GameObject BombFirePrefab;
		public const int MAP_SIZE = 8;
		public static LevelInfo LevelInformation;
		public static int TurnsLeft;
		public static Game Instance;
		public static Vector3 BasketCoordinate = new Vector3(-4.2f, -0.8f);
		public static Vector3 PotCoordinate = new Vector3(-5.53f, 0.8f);
		public static Vector3 PortalCoordinate = new Vector3(-3.0f, -2.0f);
		public static Texture2D MainCursor;
		public static Texture2D FireCursor;
		public static Texture2D ElectroCursor;
		public static ClickState ClickType;
		public static Dictionary<MonsterType, int> MonsterCounter;

		public static int Level;
		private void Awake()
		{
			PlayerIsBlocked = false;
			SkillButton.buttons = new List<SkillButton>();
			WaterField.Bridges = new Dictionary<Coordinate, Direction>();
			WaterField.BridgeObjects = new List<Monster>();
			WaterField.River = new List<Coordinate>();
			MonsterCounter = new Dictionary<MonsterType, int>()
			{
				{MonsterType.Zombie, 0},
				{MonsterType.Spider, 0},
				{MonsterType.Vampire, 0},
				{MonsterType.Bat, 0},
				{MonsterType.Ghost, 0},
				{MonsterType.Coocon, 0}
			};
			GameObject.Find("GameManager").GetComponentInChildren<AudioSource>().volume = PlayerPrefs.GetInt("Sound")/2f;
			Level = PlayerPrefs.GetInt("CurrentLevel");
			//LevelInformation = JsonConvert.DeserializeObject<LevelInfo>(File.ReadAllText("Assets/levels/1.json"));
			if (Level == 1)
			{
				LevelInformation = new LevelInfo {Map = "ZSVBBGBB ZHGGBBSS GBZBGSZS ZGGSVGVG GVZBVZBS ZVZVBZGV GBGSZSGS ZBZBBGBZ"};
				LevelInformation.Targets = new Dictionary<char, int>()
				{
					{'Z', 5},
					{'S', 5},
					{'V', 5}
				};
				TurnsLeft = 5;
			}
			if (Level == 2)
			{
				LevelInformation = new LevelInfo { Map = "ESVVBGBE EGGBVBSE 14113VSV EGGS2SVE EVZB6114 EVZVBZGE EBGSZSGE EEEEEEEE" };
				LevelInformation.Targets = new Dictionary<char, int>()
				{
					{'G', 30},
					{'Z', 20},
				};
				TurnsLeft = 15;
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
			SpaceObjectPrefab = Resources.Load("SpaceObjectPrefab", typeof (GameObject)) as GameObject;
			SkillButton.ActiveFire = Resources.Load<Sprite>("ButtonsSprites/fireActiveButton");
			SkillButton.Fire = Resources.Load<Sprite>("ButtonsSprites/fireLoad");
			SkillButton.ActiveElectro = Resources.Load<Sprite>("ButtonsSprites/electroActiveButton");
			SkillButton.Electro = Resources.Load<Sprite>("ButtonsSprites/electroLoad");
			Monster.VampireSprite = Resources.Load("Sprites/2-vampir", typeof(Sprite)) as Sprite;
			Monster.ZombieSprite = Resources.Load("Sprites/2-zombak", typeof(Sprite)) as Sprite;
			Monster.SpiderSprite = Resources.Load("Sprites/2-pauk", typeof(Sprite)) as Sprite;
			Monster.BatSprite = Resources.Load("Sprites/2-letuchka", typeof(Sprite)) as Sprite;
			Monster.GhostSprite = Resources.Load("Sprites/2-prizrak", typeof(Sprite)) as Sprite;
			Monster.EmptyCellSprite = Resources.Load("Sprites/1-pustaiaKLETKA", typeof(Sprite)) as Sprite;
			Monster.BombSprite = Resources.Load("Sprites/1-bomba", typeof(Sprite)) as Sprite;
			Monster.BlackHoleSprite = Resources.Load("Sprites/3Black_hole_02", typeof(Sprite)) as Sprite;
			Monster.CooconSprite = Resources.Load("Sprites/1-kokonZAMOROZKA", typeof(Sprite)) as Sprite;
			Monster.WaterHSprite = Resources.Load("Sprites/WaterH", typeof(Sprite)) as Sprite;
			Monster.WaterVSprite = Resources.Load("Sprites/WaterV", typeof(Sprite)) as Sprite;
			Monster.WaterDSprite = Resources.Load("Sprites/WaterD", typeof(Sprite)) as Sprite;
			Monster.RaftSprite = Resources.Load("Sprites/Raft", typeof(Sprite)) as Sprite;
			MainCursor = Resources.Load("Cursors/MainCursor") as Texture2D;
			FireCursor = Resources.Load("Cursors/FireCursor") as Texture2D;
			ElectroCursor = Resources.Load("Cursors/ElectricityCursor") as Texture2D;
			Cursor.SetCursor(MainCursor, new Vector2(0,0), CursorMode.Auto);
			Monster.TypesToSprites = new Dictionary<MonsterType, Sprite>
			{
				{MonsterType.Zombie, Monster.ZombieSprite},
				{MonsterType.Spider, Monster.SpiderSprite},
				{MonsterType.Vampire, Monster.VampireSprite},
				{MonsterType.Bat, Monster.BatSprite},
				{MonsterType.Ghost, Monster.GhostSprite},
				{MonsterType.EmptyCell, Monster.EmptyCellSprite},
				{MonsterType.BlackHole, Monster.BlackHoleSprite},
				{MonsterType.Coocon, Monster.CooconSprite},
				{MonsterType.Bomb, Monster.BombSprite },
				{MonsterType.WaterHorizontal, Monster.WaterHSprite },
				{MonsterType.WaterVertical, Monster.WaterVSprite },
				{MonsterType.WaterDiagonal, Monster.WaterDSprite },
				{MonsterType.Raft, Monster.RaftSprite},
			};
			Instance = this;
			

			GenerateMap();

		}

		public static bool PlayerIsBlocked;
		public static bool IsPlayerBlocked()
		{
			if (GameField.IsAnyMoving() || PlayerIsBlocked)
				return true;
			return false;
		}
		// Update is called once per frame
		public void Update()
		{
			CheckCursorClick();
			if (TurnsLeft == 0)
				PlayerIsBlocked = true;                        
			GameField.CheckUpperBorder();
			if (!GameField.IsAnyMoving())
			{
				if (!GameField.IsAnyCorrectMove())
					GameField.Shuffle();
				GameField.UpdateField();
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
			int trgCnt = 0;
			foreach (var target in LevelInformation.Targets)
			{
				GameObject go = new GameObject();
				SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
				renderer.sprite = Monster.TypesToSprites[Monster.CharsToObjectTypes[target.Key]];
				go.transform.position = new Vector3(-1.1f + trgCnt * 2.2f, -4.2f);
				trgCnt++;
			}
			WaterField.GenerateRiver();
		}

		public static void MonsterCreate(int x, int y, char type, float delay = 0)
		{
			if (type >= '1' && type <= '6')
			{
				if (type >= '4')
				{
					Monster bridge = ((GameObject)Instantiate(
						SpaceObjectPrefab, GameField.GetVectorFromCoord(x, y),
						Quaternion.Euler(new Vector3())))
						.GetComponent<Monster>();
					bridge.Initialise(x, y, 'R', delay);
					WaterField.BridgeObjects.Add(bridge);
					WaterField.Bridges.Add(new Coordinate(x,y), Direction.Forward);
					bridge.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
					bridge.gameObject.GetComponent<CircleCollider2D>().enabled = false;
					type -= (char) 3;
				}
				Monster water = ((GameObject)Instantiate(
						SpaceObjectPrefab, GameField.GetVectorFromCoord(x, y),
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
						SpaceObjectPrefab, GameField.GetVectorFromCoord(x, y),
						Quaternion.Euler(new Vector3())))
						.GetComponent<Monster>();
				monster.Initialise(x, y, type, delay); //?
				GameField.Map[x, y] = monster;
			}
			
		}

		public static void NextTurn()
		{
			TurnsLeft--;
			Instance.Update();
			WaterField.ShiftBridges();
			
		}
	}
}
