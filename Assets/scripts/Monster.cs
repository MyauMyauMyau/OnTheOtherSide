using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Assets.scripts;
using System.Linq;
using System.Runtime.InteropServices;
using Assets.scripts.Enums;
using Random = System.Random;

public class Monster : MonoBehaviour
{
	private Coordinate blackHoleTarget;
	public Coordinate GridPosition { get; set; }
	public Vector3 Destination { get; set; }
	public Coordinate PreviousPosition {get; set;}
	public MonsterType TypeOfMonster { get; set;}
	public int FlagCounter;
	public static Sprite EmptyCellSprite;
	public static Sprite BlackHoleSprite;
	public static Sprite BatSprite;
	public static Sprite ZombieSprite;
	public static Sprite GhostSprite;
	public static Sprite VoodooSprite;
	public static Sprite SpiderSprite;
	public static Sprite SnakeSprite;
	public static Sprite PlasmSprite;
	public static Sprite CooconSprite;
	public static Sprite BombSprite;
	public static Sprite MagicSkullSprite;
	public static Sprite Pumpkin1Sprite;
	public static Sprite Pumpkin2Sprite;
	public static Sprite Pumpkin3Sprite;
	public static Sprite PumpkinFaceSprite;
	public static Sprite PumpkinBranchSprite;
	public static Sprite SkeletonSprite1;
	public static Sprite SkeletonSprite2;
	public static Sprite SkeletonSprite3;
	public static Sprite SkeletonSprite4;
	public static Sprite SkeletonMudSprite;
	public static Sprite WaterVSprite;
	public static Sprite WaterHSprite;
	public static Sprite WaterUpperRightSprite;
	public static Sprite WaterUpperLeftSprite;
	public static Sprite WaterDownRightSprite;
	public static Sprite WaterDownLeftSprite;
	public static Sprite RaftSprite;
	public static float RemoveStartTime;
	public static float RemoveEndTime;
	public static bool RemoveSoundPlaying;
	public const float BaseDropSpeed = 5f;
	public bool IsTargetForBlackHole;
	public bool IsFrozen;
	public float DropSpeed;
	public float MoveSpeed;
	public float GrowSpeed;
	public float StartTime;
	public float Delay;
	public int BloodVortexLastTimeHandled;
	public int FlagFrequancy;
	private bool blackHoleHasJumped;
	private bool isSpritesLoaded;
	public float UpSpeed = 0.01f;
	public const float GravitySpeed = 0.01f;
	public float ToBasketStartTime;
	public int BloodVortexLivesLeft = 10;
	public MonsterState State { get; set; }
	public static Sprite EmptyCell2Sprite { get; set; }
	public static Sprite CandleCellSprite { get; set; }

	public bool UpdatedField;
	public static Random Rnd = new Random();
	public Vector3 DestroyRotation;
	public int DestroyRotationSpeed;
	public bool IsAnimated;
	public int AnimationStartTime;
	public bool IsMonster()
	{
		return Dictionaries.MonsterTypes.Contains(TypeOfMonster) && TypeOfMonster != MonsterType.Coocon;
	}

	public void Click()
	{
		OnMouseDown();
	}
	public bool IsFlagPlaced;
	public void PutFlag()
	{
		var startPos = new Vector3(-2.68f, 5.52f);
		var curPos = Vector3.Lerp(
			startPos, GameField.GetVectorFromCoord(GridPosition.X, GridPosition.Y),
			(Time.time - ToBasketStartTime));
		curPos.y += 4 * Mathf.Sin(Mathf.Clamp01((Time.time - ToBasketStartTime)) * Mathf.PI);
		transform.position = curPos;
		if (transform.position == GameField.GetVectorFromCoord(GridPosition.X, GridPosition.Y))
		{
			AudioHolder.PlayClericFlag2();
			IsFlagPlaced = true;
			if (GameField.Map[GridPosition.X, GridPosition.Y].IsFrozen)
				GameField.Map[GridPosition.X, GridPosition.Y].DestroyMonster();
			GameField.Map[GridPosition.X, GridPosition.Y].DestroyMonster();
			GameField.Map[GridPosition.X, GridPosition.Y] = this;
			gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;

		}
	}
	public IEnumerator AnimateAdvice()
	{
		if (State == MonsterState.Destroying) yield break;
		var bufState = State;
		State = MonsterState.Animating;
		for (int i = 0; i < 10; i++)
		{
			
			transform.position = new Vector3(transform.position.x , transform.position.y + 0.03f);
			yield return null;
		}
		for (int i = 0; i < 10; i++)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y - 0.03f);
			yield return null;
		}
		State = bufState;
	}
	public void SendToBasket()
	{
		var curPos = Vector3.Lerp(
			GameField.GetVectorFromCoord(GridPosition.X, GridPosition.Y), Game.BasketCoordinate,
			(Time.time - ToBasketStartTime));
		curPos.y += 4*Mathf.Sin(Mathf.Clamp01((Time.time - ToBasketStartTime))*Mathf.PI);
		transform.position = curPos;
		transform.Rotate(DestroyRotation * Time.deltaTime * DestroyRotationSpeed);
		if (transform.position == Game.BasketCoordinate)
			Destroy(gameObject);
	}
	public void SendToPortal()
	{
		var curPos = Vector3.Lerp(
			GameField.GetVectorFromCoord(GridPosition.X, GridPosition.Y), Game.PortalCoordinate,
			(Time.time - ToBasketStartTime));
		curPos.y += 4 * Mathf.Sin(Mathf.Clamp01((Time.time - ToBasketStartTime)) * Mathf.PI);
		transform.position = curPos;
		transform.Rotate( DestroyRotation * Time.deltaTime*DestroyRotationSpeed);
		if (transform.position == Game.PortalCoordinate)
			Destroy(gameObject);
	}
	public void SendToPot()
	{
		var curPos = Vector3.Lerp(
			GameField.GetVectorFromCoord(GridPosition.X, GridPosition.Y), Game.PotCoordinate,
			(Time.time - ToBasketStartTime));
		curPos.y += 4 * Mathf.Sin(Mathf.Clamp01((Time.time - ToBasketStartTime)) * Mathf.PI);
		transform.position = curPos;
		transform.Rotate(DestroyRotation * Time.deltaTime * DestroyRotationSpeed);
		if (transform.position == Game.PotCoordinate)
			Destroy(gameObject);
	}
	public void DestroyMonster()
	{
		if (!IsMonster() && TypeOfMonster != MonsterType.Coocon) return;
		Game.Instance.LastAdviceTime = Time.time + 5f;
		if (IsFrozen)
		{
			IsFrozen = false;
			Instantiate(Game.BrokenWebPrefab, transform.position, Quaternion.Euler(new Vector3()));
			gameObject.transform.GetChild(1).gameObject.SetActive(false);
			return;
		}

		if (TypeOfMonster == MonsterType.Bomb)
		{
			AudioHolder.PlayBomb();
			DestroyBomb();
			return;
		}

		//make 1st layer
		foreach (var spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
		{
			spriteRenderer.sortingOrder = 3;
		}
		var bottomBoundX = Math.Max(0, GridPosition.X - 1);
		var bottomBoundY = Math.Max(0, GridPosition.Y - 1);
		var topBoundX = Math.Min(Game.MAP_SIZE - 1, GridPosition.X + 1);
		var topBoundY = Math.Min(Game.MAP_SIZE - 1, GridPosition.Y + 1);
		if (TypeOfMonster == MonsterType.Coocon)
		{
			AudioHolder.PlayCocoon();
			for (int i = bottomBoundX; i <= topBoundX; i++)
			{
				for (int j = bottomBoundY; j <= topBoundY; j++)
				{
					if (GameField.Map[i, j] != null && GameField.Map[i, j].IsMonster())
					{
						GameField.Map[i, j].IsFrozen = true;
						GameField.Map[i, j].gameObject.transform.GetChild(1).gameObject.SetActive(true);
					}
				}
			}
			Dictionaries.MonsterCounter[MonsterType.Coocon]++;
			GameField.Map[GridPosition.X, GridPosition.Y] = null;
			ToBasketStartTime = Time.time;
			State = MonsterState.Destroying;
			return;
		}
		var iceList = new List<Coordinate>();
		for (int i = bottomBoundX; i <= topBoundX; i++)
		{
			if (GameField.Map[i, GridPosition.Y] != null &&
				GameField.Map[i, GridPosition.Y].TypeOfMonster == MonsterType.Coocon)
			{
				iceList.Add(new Coordinate(i, GridPosition.Y));
			}
		}
		for (int j = bottomBoundY; j <= topBoundY; j++)
		{
			if (GameField.Map[GridPosition.X, j] != null &&
				GameField.Map[GridPosition.X, j].TypeOfMonster == MonsterType.Coocon)
			{
				iceList.Add(new Coordinate(GridPosition.X,j));
			}
		}
		GameField.Map[GridPosition.X, GridPosition.Y] = null;
		ToBasketStartTime = Time.time;
		Dictionaries.MonsterCounter[TypeOfMonster]++;
		State = MonsterState.Destroying;

		AudioHolder.PlayRemove();
		foreach (var ice in iceList)
		{
			GameField.Map[ice.X, ice.Y].DestroyMonster();				
		}
	}


	public void ActivateFlag()
	{
		Game.PlayerIsBlocked = true;
		var monsters = new List<Coordinate>();
		var bottomBoundX = Math.Max(0, GridPosition.X - 1);
		var bottomBoundY = Math.Max(0, GridPosition.Y - 1);
		var topBoundX = Math.Min(Game.MAP_SIZE - 1, GridPosition.X + 1);
		var topBoundY = Math.Min(Game.MAP_SIZE - 1, GridPosition.Y + 1);
		for (int i = bottomBoundX; i <= topBoundX; i++)
		{
			for (int j = bottomBoundY; j <= topBoundY; j++)
			{
				if (i != j && GameField.Map[i, j] != null && GameField.Map[i, j].IsMonster() && !GameField.Map[i, j].IsFrozen
					&& !GameField.Map[i,j].IsUpgradable())
				{
					monsters.Add(new Coordinate(i,j));		
				}
			}
		}
		if (monsters.Count == 0)
		{
			Game.PlayerIsBlocked = false;
			return;
		}
		var trg = monsters[Rnd.Next(monsters.Count)];
		var SparkPrefab = Resources.Load("objects/heroes/Cleric/HolyFlag/spark", typeof(GameObject)) as GameObject;
		var spark = ((GameObject)Instantiate(
					SparkPrefab, GameField.GetVectorFromCoord(trg.X, trg.Y),
					Quaternion.Euler(new Vector3())))
					.GetComponent<Spark>();
		spark.Target = trg;
	}

	private void DestroyBomb()
	{
		GetComponent<SpriteRenderer>().enabled = false;
		var boom = ((GameObject)Instantiate(
		Game.BombFirePrefab, GameField.GetVectorFromCoord(GridPosition.X, GridPosition.Y),
		Quaternion.Euler(new Vector3()))).GetComponent<BombBoom>();
		GameField.Map[GridPosition.X, GridPosition.Y] = null;
		State = MonsterState.Destroying;
		boom.GridPosition = GridPosition;
	}

	public void Initialise(int x, int y, char type, float delay = 0)
	{
		DestroyRotationSpeed = Rnd.Next(500);
		DestroyRotation = (Rnd.Next(2) == 0 ? Vector3.back : Vector3.forward);
		DropSpeed = BaseDropSpeed*2;
		ToBasketStartTime = Time.time;
		GrowSpeed = 0.05f;
		GridPosition = new Coordinate(x,y);
		TypeOfMonster = Dictionaries.CharsToObjectTypes[type];


		
		if ((IsSceleton() || IsPumpkin()) && TypeOfMonster != MonsterType.Skeleton1)
		{
			transform.localScale = new Vector3(1, 1);
		}
		if (Dictionaries.AnimatedTypes.Contains(TypeOfMonster))
		{
			IsAnimated = true;
			AnimationStartTime = (int)Time.time + Rnd.Next(50);
		}
		if (!Dictionaries.AnimatedTypes.Contains(Dictionaries.CharsToObjectTypes[type]) 
			&& TypeOfMonster != MonsterType.ClericFlag && TypeOfMonster != MonsterType.BloodVortex)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = Dictionaries.TypesToSprites[TypeOfMonster];
		}

		MoveSpeed = 10f;
		//if (TypeOfMonster == MonsterType.BlackHole)
		//{
		//	var light = gameObject.GetComponentInChildren<Light>();
		//	light.enabled = true;
		//	light.range = 1.33f;
		//	light.color = Color.blue;
		//}
		if (TypeOfMonster == MonsterType.Raft)
		{
			gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
		}
		Destination = GameField.GetVectorFromCoord(GridPosition.X, GridPosition.Y);
		if (delay > 0)
		{
			Delay = delay;
			StartTime = Time.time;
			State = MonsterState.WaitingForInitialising;
			return;
		}
		if (TypeOfMonster == MonsterType.Skeleton4 || TypeOfMonster == MonsterType.Pumpkin3)
		{
			DestroyMonster();
		}
		else State = MonsterState.Growing;
	}

	public bool IsUpgradable()
	{
		return (IsSceleton() || IsPumpkin());
	}

	private bool IsPumpkin()
	{
		return (TypeOfMonster == MonsterType.Pumpkin1 || TypeOfMonster == MonsterType.Pumpkin2 || TypeOfMonster == MonsterType.Pumpkin3);
	}

	public bool IsSceleton()
	{
		return (TypeOfMonster == MonsterType.Skeleton1 || TypeOfMonster == MonsterType.Skeleton2 ||
		        TypeOfMonster == MonsterType.Skeleton3 || TypeOfMonster == MonsterType.Skeleton4);
	}
	public void Destroy()
	{
		Destroy(gameObject);
	}
	void Update()
	{
		if (TypeOfMonster == MonsterType.ClericFlag && !IsFlagPlaced)
		{
			PutFlag();
			if ((gameObject.transform.localScale.x < 1)
			&& State != MonsterState.Decreasing)
			{
				Vector3 scale = transform.localScale;

				scale.x += GrowSpeed;
				scale.y += GrowSpeed;
				gameObject.transform.localScale = scale;
			}
			return;
		}
		if (SkillsController.IsActive && State == MonsterState.Clicked )
			{
				State = MonsterState.Default;
				GameField.ClickedObject = null;
			}
		if ((Game.PlayerIsBlocked && State != MonsterState.Destroying) || State == MonsterState.WaitingForActivation || State == MonsterState.Deactivated || State == MonsterState.Animating)
			return;
		if (State == MonsterState.Destroying)
		{
			if (TypeOfMonster == MonsterType.Voodoo || TypeOfMonster == MonsterType.Zombie ||
				TypeOfMonster == MonsterType.Skeleton4)
				SendToBasket();
			if (TypeOfMonster == MonsterType.Spider || TypeOfMonster == MonsterType.Bat ||
				TypeOfMonster == MonsterType.Pumpkin3 || TypeOfMonster == MonsterType.Snake)
				SendToPot();
			if (TypeOfMonster == MonsterType.Ghost || TypeOfMonster == MonsterType.Plasm)
				SendToPortal();
			if (TypeOfMonster == MonsterType.Coocon || TypeOfMonster == MonsterType.Bomb)
				Destroy(gameObject);
			return;
		}
		if (State == MonsterState.WaitingForInitialising)
		{
			if (Time.time - StartTime > Delay)
				State = MonsterState.Dropping;
			else
				return;
		}
		if (Destination != gameObject.transform.position && State != MonsterState.Moving)
		{
			State = MonsterState.Dropping;
		}

		if ((gameObject.transform.localScale.x < 1)
			&& State != MonsterState.Decreasing)
		{
			Vector3 scale = transform.localScale;

			scale.x += GrowSpeed;
			scale.y += GrowSpeed;
			gameObject.transform.localScale = scale;
			if (gameObject.transform.localScale.x < 0.95)
				return;
		}
		else if (State == MonsterState.Growing)
			State = MonsterState.Default;
		if (TypeOfMonster == MonsterType.BlackHole)
		{
			HandleBlackHole();
		}
		if (TypeOfMonster == MonsterType.BloodVortex)
		{
			transform.GetChild(0).transform.Rotate(0,0,Time.deltaTime*-200f);
			transform.GetChild(1).transform.Rotate(0, 0, Time.deltaTime * 200f);
		}
		if (State == MonsterState.Moving || State == MonsterState.Dropping)
		{
			if (transform.position.Equals(Destination))
			{
				if (State == MonsterState.Moving && !GameField.MoveIsFinished)
				{
					if (!GameField.IsCorrectMove(new List<Coordinate>() {GridPosition, PreviousPosition}))
						GameField.Swap(GridPosition, PreviousPosition);
					else
					{
						GameField.MoveIsFinished = true;
						Game.Instance.StartCoroutine(Game.NextTurn());
					}
				}
				State = MonsterState.Default;
			}
			else
			{
				float step;
				
				step = State == MonsterState.Dropping ? this.DropSpeed*Time.deltaTime : this.MoveSpeed*Time.deltaTime;
				transform.position = Vector3.MoveTowards(transform.position, Destination, step);
			}
		}
		if (IsMonster())
			gameObject.transform.FindChild("BackLight").gameObject.GetComponent<SpriteRenderer>().enabled = State == MonsterState.Clicked;
								   
		if (IsMonster() && !IsFrozen && State !=MonsterState.Moving && GridPosition.Y != Game.MAP_SIZE - 1)
		{
			DropMonsters();
		}
		if (IsAnimated && Time.time > AnimationStartTime)
		{
			//IsAnimated = false;
			var anim = GetComponent<Animator>();
			anim.SetTrigger("AnimTrigger");
			AnimationStartTime += 50;
			//anim.ResetTrigger("AnimTrigger");
		}
	}

	private void DropMonsters()
	{
		if (Game.DropIsBlocked) return;
		if (GameField.Map[GridPosition.X, GridPosition.Y + 1] == null && WaterField.IsBridgeOrNull(GridPosition.X, GridPosition.Y + 1))
		{
			GameField.Drop(GridPosition, new Coordinate(GridPosition.X, GridPosition.Y + 1));
		}
		// if downdrop is blocked
		else if (GridPosition.X > 0 &&
				 GameField.Map[GridPosition.X - 1, GridPosition.Y + 1] == null && WaterField.IsBridgeOrNull(GridPosition.X - 1, GridPosition.Y + 1))
		{
			for (int i = GridPosition.Y; i >= 0; i--)
			{
				if (GameField.Map[GridPosition.X - 1, i] != null
					&& (!GameField.Map[GridPosition.X - 1, i].IsMonster() || GameField.Map[GridPosition.X - 1, i].IsFrozen
					) || !WaterField.IsBridgeOrNull(GridPosition.X - 1, i))
				{
					GameField.Drop(GridPosition, new Coordinate(GridPosition.X - 1, GridPosition.Y + 1));
					break;
				}
				if (GameField.Map[GridPosition.X - 1, i] != null && GameField.Map[GridPosition.X - 1, i].IsMonster())
					break;
			}

		}
		else if (GridPosition.X < GameField.Map.GetLength(0) - 1 &&
				 GameField.Map[GridPosition.X + 1, GridPosition.Y + 1] == null && WaterField.IsBridgeOrNull(GridPosition.X + 1, GridPosition.Y + 1))
		{
			for (int i = GridPosition.Y; i >= 0; i--)
			{
				if (GameField.Map[GridPosition.X + 1, i] != null
					&& (!GameField.Map[GridPosition.X + 1, i].IsMonster() || GameField.Map[GridPosition.X + 1, i].IsFrozen 
					) || !WaterField.IsBridgeOrNull(GridPosition.X + 1, i))
				{
					GameField.Drop(GridPosition, new Coordinate(GridPosition.X + 1, GridPosition.Y + 1));
					break;
				}
				if (GameField.Map[GridPosition.X + 1, i] != null && GameField.Map[GridPosition.X + 1, i].IsMonster())
					break;
			}
		}
	}
	private void HandleBlackHole()
	{
		if (State == MonsterState.Decreasing)
		{
			Vector3 scale = transform.localScale;
			scale.x -= GrowSpeed/1.5f;
			scale.y -= GrowSpeed/1.5f;
			if (scale.x <= 0.1)
			{
				State = MonsterState.Growing;
				if (GameField.Map[blackHoleTarget.X, blackHoleTarget.Y] != null) 
					GameField.Map[blackHoleTarget.X, blackHoleTarget.Y].IsTargetForBlackHole = false;
				GameField.Jump(GridPosition, blackHoleTarget);
			}
			transform.localScale = scale;
		}
		transform.Rotate(0f, 0f, -100f*Time.deltaTime);
		if (Game.TurnsLeft%4 == 0) 
		{
			if (!blackHoleHasJumped && !Game.IsPlayerBlocked())
				JumpBlackHole();
		}
		else
			blackHoleHasJumped = false;
	}

	public void HandleMagicSkull()
	{
		if (GridPosition.Y + 1 < Game.MAP_SIZE && ( 
			(GameField.Map[GridPosition.X, GridPosition.Y + 1] == null && WaterField.IsBridgeOrNull(GridPosition.X, GridPosition.Y + 1))
		    || (GameField.Map[GridPosition.X, GridPosition.Y + 1] != null && GameField.Map[GridPosition.X, GridPosition.Y + 1].IsMonster()))
		    && GridPosition.Y - 1 >= 0 && (
			(GameField.Map[GridPosition.X, GridPosition.Y - 1] == null && WaterField.IsBridgeOrNull(GridPosition.X, GridPosition.Y - 1))
			|| (GameField.Map[GridPosition.X, GridPosition.Y - 1] != null && GameField.Map[GridPosition.X, GridPosition.Y - 1].IsMonster())))
		{
			StartCoroutine(ActivateSkull());
			GameField.MagicSwap(new Coordinate(GridPosition.X, GridPosition.Y + 1), new Coordinate(GridPosition.X, GridPosition.Y - 1));
		}
		else if (GridPosition.X + 1 < Game.MAP_SIZE && (
			(GameField.Map[GridPosition.X + 1, GridPosition.Y] == null &&
			 WaterField.IsBridgeOrNull(GridPosition.X + 1, GridPosition.Y))
			||
			(GameField.Map[GridPosition.X + 1, GridPosition.Y] != null &&
			 GameField.Map[GridPosition.X + 1, GridPosition.Y].IsMonster()))
		         && GridPosition.X - 1 >= 0 && (
			         (GameField.Map[GridPosition.X - 1, GridPosition.Y] == null &&
			          WaterField.IsBridgeOrNull(GridPosition.X - 1, GridPosition.Y))
			         ||
			         (GameField.Map[GridPosition.X - 1, GridPosition.Y] != null &&
			          GameField.Map[GridPosition.X - 1, GridPosition.Y].IsMonster())))
		{
			StartCoroutine(ActivateSkull());
			GameField.MagicSwap(new Coordinate(GridPosition.X - 1, GridPosition.Y), new Coordinate(GridPosition.X + 1, GridPosition.Y));
		}
	}

	private IEnumerator ActivateSkull()
	{
		transform.GetChild(0).gameObject.SetActive(true);
		yield return new WaitForSeconds(0.5f);
		transform.GetChild(0).gameObject.SetActive(false);
	}


	public void HandleBloodVortex()
	{
		if (BloodVortexLastTimeHandled == Game.TurnsLeft)
			return;
		BloodVortexLastTimeHandled = Game.TurnsLeft;
		if (BloodVortexLivesLeft < 0) Destroy(gameObject);
		var monsters = new List<Monster>();
		if ( GridPosition.Y + 1 < Game.MAP_SIZE && GameField.Map[GridPosition.X, GridPosition.Y + 1] != null
		    && GameField.Map[GridPosition.X, GridPosition.Y + 1].IsMonster()
		    && !GameField.Map[GridPosition.X, GridPosition.Y + 1].IsUpgradable())
			monsters.Add(GameField.Map[GridPosition.X, GridPosition.Y + 1]);
		if (GridPosition.Y - 1>= 0&& GameField.Map[GridPosition.X, GridPosition.Y - 1] != null
			&& GameField.Map[GridPosition.X, GridPosition.Y - 1].IsMonster()
			&& !GameField.Map[GridPosition.X, GridPosition.Y - 1].IsUpgradable())
			monsters.Add(GameField.Map[GridPosition.X, GridPosition.Y - 1]);
		if (GridPosition.X + 1 < Game.MAP_SIZE && GameField.Map[GridPosition.X + 1, GridPosition.Y] != null
			&& GameField.Map[GridPosition.X+1, GridPosition.Y].IsMonster()
			&& !GameField.Map[GridPosition.X+1, GridPosition.Y].IsUpgradable())
			monsters.Add(GameField.Map[GridPosition.X+1, GridPosition.Y]);
		if (GridPosition.X - 1 >= 0 && GameField.Map[GridPosition.X- 1, GridPosition.Y] != null
			&& GameField.Map[GridPosition.X - 1, GridPosition.Y].IsMonster()
			&& !GameField.Map[GridPosition.X - 1, GridPosition.Y].IsUpgradable())
			monsters.Add(GameField.Map[GridPosition.X - 1, GridPosition.Y]);
		if (monsters.Count == 0) return;
		var trg = monsters[Rnd.Next(monsters.Count)];
		Move(trg.GridPosition);
		if (trg.IsFrozen)
			trg.DestroyMonster();
		trg.DestroyMonster();
		GameField.Map[GridPosition.X, GridPosition.Y] = this;
		GameField.Map[PreviousPosition.X, PreviousPosition.Y] = null;
		
		
	}

	private void JumpBlackHole()
	{
		blackHoleHasJumped = true;
		var map = GameField.Map;
		var unsuitableAsteroids = new List<MonsterType>();
		if (GridPosition.X > 0 && map[GridPosition.X - 1, GridPosition.Y] != null)
			unsuitableAsteroids.Add(map[GridPosition.X - 1, GridPosition.Y].TypeOfMonster);
		if (GridPosition.Y > 0 && map[GridPosition.X, GridPosition.Y - 1] != null)
			unsuitableAsteroids.Add(map[GridPosition.X, GridPosition.Y - 1].TypeOfMonster);
		if (GridPosition.X < map.GetLength(0) - 1 && map[GridPosition.X + 1, GridPosition.Y] != null)
			unsuitableAsteroids.Add(map[GridPosition.X + 1, GridPosition.Y].TypeOfMonster);
		if (GridPosition.Y < map.GetLength(1) - 1 && map[GridPosition.X, GridPosition.Y + 1] != null)
			unsuitableAsteroids.Add(map[GridPosition.X, GridPosition.Y + 1].TypeOfMonster);
		unsuitableAsteroids = unsuitableAsteroids.Distinct().ToList();
		var coords = new List<Coordinate>();
		for (int i = 0; i < map.GetLength(0); i++)
		{
			for (int j = 0; j < map.GetLength(1); j++)
			{
				if (map[i, j] != null && map[i, j].IsMonster() && !unsuitableAsteroids.Contains(map[i, j].TypeOfMonster)
				    &&
				    ((j < map.GetLength(1) - 1 && map[i, j + 1] != null &&
					map[i, j].TypeOfMonster == map[i, j + 1].TypeOfMonster) ||
					(j > 0 && map[i, j - 1] != null &&
					map[i, j].TypeOfMonster == map[i, j - 1].TypeOfMonster) ||
					(i > 0&& map[i - 1,j] != null &&
					map[i, j].TypeOfMonster == map[i - 1, j].TypeOfMonster) ||
					 (i < map.GetLength(0) - 1 && map[i + 1, j] != null &&
					 map[i, j].TypeOfMonster == map[i + 1, j].TypeOfMonster)))
				{
					coords.Add(map[i, j].GridPosition);
				}
			} 
		}
		if (coords.Count > 0)
		{
			State = MonsterState.Decreasing;
			blackHoleTarget = coords.ElementAt(Rnd.Next(coords.Count));
			if (!GameField.Map[blackHoleTarget.X, blackHoleTarget.Y].IsTargetForBlackHole)
				GameField.Map[blackHoleTarget.X, blackHoleTarget.Y].IsTargetForBlackHole = true;
		}
	}

	public bool IsWater()
	{
		return (TypeOfMonster == MonsterType.WaterDownLeft || TypeOfMonster == MonsterType.WaterDownRight
		        || TypeOfMonster == MonsterType.WaterHorizontal || TypeOfMonster == MonsterType.WaterVertical
		        || TypeOfMonster == MonsterType.WaterUpperLeft || TypeOfMonster == MonsterType.WaterUpperRight);
	}
//	private IEnumerator ThrowLightning()
//	{
//		AudioHolder.PlayDeathElectricity();
//		Game.PlayerIsBlocked = true;
//		var hero = GameObject.Find("hero");
//		var l1 = hero.transform.FindChild("Lightning").gameObject;
//		var l2 = hero.transform.FindChild("Lightning2").gameObject;
//		for (int i = 0; i < 6; i++)
//		{
//			if (l1.activeSelf)
//			{
//				l2.SetActive(true);
//				l1.SetActive(false);
//			}
//			else
//			{
//				l2.SetActive(false);
//				l1.SetActive(true);
//			}
//			yield return new WaitForSeconds(0.1f);
//		}
//		l2.SetActive(false);
//		Game.PlayerIsBlocked = false;
////		SkillButton.Deactivate(SkillButtonType.Electro);
//		GameField.DestroyAllOf(TypeOfMonster);
//		AudioHolder.PlayMassRemove();
//	}

	void OnMouseDown()
	{
		if (SkillsController.IsActive && SkillsController.IsMonsterClickable(this))
			SkillsController.BracketMonster(GridPosition);
		if (IsFrozen || Game.IsPlayerBlocked() || TypeOfMonster == MonsterType.Coocon ||
		    (!IsMonster() && TypeOfMonster != MonsterType.Bomb))
		{
			if (GameField.ClickedObject != null)
			{
				
				GameField.Map[GameField.ClickedObject.Value.X, GameField.ClickedObject.Value.Y].State = MonsterState.Default;
				GameField.ClickedObject = null;
			}
			return;

		}
		if (State == MonsterState.Default)
		{
			if (GameField.ClickedObject == null)
			{
				if (TypeOfMonster == MonsterType.Bomb)
				{
					DestroyMonster();
					Game.Instance.StartCoroutine(Game.NextTurn());
					return;
				}
				GameField.ClickedObject = GridPosition;
				State = MonsterState.Clicked;
			}
			else
			{
				if (GridPosition.IsNeighbourWith(GameField.ClickedObject.Value))
				{
					AudioHolder.PlaySwapMonsters();
					GameField.Map[GameField.ClickedObject.Value.X, GameField.ClickedObject.Value.Y].State = MonsterState.Default;
					State = MonsterState.Default;
					GameField.Swap(GridPosition, GameField.ClickedObject.Value);
				}
				else
				{
					GameField.Map[GameField.ClickedObject.Value.X, GameField.ClickedObject.Value.Y].State = MonsterState.Default;
					GameField.ClickedObject = null;
					State = MonsterState.Default;
				}
			}
		}
		else if (State == MonsterState.Clicked)
		{
			State = MonsterState.Default;
			GameField.ClickedObject = null;
		}
	}

	public void Move(Coordinate newPosition)
	{
		
		State = MonsterState.Moving;
		PreviousPosition = GridPosition;
		GridPosition = newPosition;
		Destination = GameField.GetVectorFromCoord(newPosition.X, newPosition.Y);
	}

	public void DominantMove(Coordinate newPosition)
	{
		State = MonsterState.Moving;
		PreviousPosition = newPosition;
		GridPosition = newPosition;
		Destination = GameField.GetVectorFromCoord(newPosition.X, newPosition.Y);
	}
}
