using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Assets.scripts;
using System.Linq;
using Assets.scripts.Enums;
using Random = System.Random;

public class Monster : MonoBehaviour
{
	private Coordinate blackHoleTarget;
	public Coordinate GridPosition { get; set; }
	public Vector3 Destination { get; set; }
	public Coordinate PreviousPosition {get; set;}
	public MonsterType TypeOfObject { get; set;}
	public static Sprite VampireSprite;
	public static Sprite ZombieSprite;
	public static Sprite SpiderSprite;
	public static Sprite BatSprite;
	public static Sprite GhostSprite;
	public static Sprite EmptyCellSprite;
	public static Sprite UnstableVampireSprite;
	public static Sprite UnstableZombieSprite;
	public static Sprite UnstableSpiderSprite;
	public static Sprite UnstableBatSprite;
	public static Sprite UnstableGhostSprite;
	public static Sprite BlackHoleSprite;
	public static Sprite CooconSprite;
	public const float BaseDropSpeed = 10f;
	public bool IsTargetForBlackHole;
	public bool IsFrozen;
	public float DropSpeed;
	public float MoveSpeed;
	public float GrowSpeed;
	public float StartTime = 0;
	public float Delay = 0;
	private bool blackHoleHasJumped;
	private bool isSpritesLoaded;
	public float UpSpeed = 0.01f;
	public const float GravitySpeed = 0.01f;
	public float ToBasketStartTime;
	public bool IsUnstable { get; set; }
	public MonsterState State { get; set; }
	public bool UpdatedField = false;
	public Random rnd = new Random();
	public Vector3 DestroyRotation;
	public int DestroyRotationSpeed;
	public bool IsMonster()
	{
		return AsteroidTypes.Contains(TypeOfObject);
	}

	private void SendToBasket()
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
	private void SendToPortal()
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
	private void SendToPot()
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
		var bottomBoundX = Math.Max(0, GridPosition.X - 1);
		var bottomBoundY = Math.Max(0, GridPosition.Y - 1);
		var topBoundX = Math.Min(Game.MAP_SIZE - 1, GridPosition.X + 1);
		var topBoundY = Math.Min(Game.MAP_SIZE - 1, GridPosition.Y + 1);
		if (TypeOfObject == MonsterType.Coocon)
		{
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
			Game.MonsterCounter[MonsterType.Coocon]++;
			GameField.Map[GridPosition.X, GridPosition.Y] = null;
			ToBasketStartTime = Time.time;
			State = MonsterState.Destroying;
			return;
		}
		if (IsFrozen)
		{
			IsFrozen = false;
			gameObject.transform.GetChild(1).gameObject.SetActive(false);
		}
		else
		{
			var iceList = new List<Coordinate>();
			for (int i = bottomBoundX; i <= topBoundX; i++)
			{
				if (GameField.Map[i, GridPosition.Y] != null &&
					GameField.Map[i, GridPosition.Y].TypeOfObject == MonsterType.Coocon)
				{
					iceList.Add(new Coordinate(i, GridPosition.Y));
				}
			}
			for (int j = bottomBoundY; j <= topBoundY; j++)
			{
				if (GameField.Map[GridPosition.X, j] != null &&
					GameField.Map[GridPosition.X, j].TypeOfObject == MonsterType.Coocon)
				{
					iceList.Add(new Coordinate(GridPosition.X,j));
				}
			}
			GameField.Map[GridPosition.X, GridPosition.Y] = null;
			ToBasketStartTime = Time.time;
			Game.MonsterCounter[TypeOfObject]++;
			SkillButton.AddEnergy(TypeOfObject);
			State = MonsterState.Destroying;
			foreach (var ice in iceList)
			{
				GameField.Map[ice.X, ice.Y].DestroyMonster();				
			}
		}
	}
	public void Initialise(int x, int y, char type, float delay = 0, bool isUnstable = false)
	{
		DestroyRotationSpeed = rnd.Next(500);
		DestroyRotation = (rnd.Next(2) == 0 ? Vector3.back : Vector3.forward);
		DropSpeed = BaseDropSpeed*2;
		MoveSpeed = 10f;
		GrowSpeed = 0.05f;
		IsUnstable = isUnstable;
		GridPosition = new Coordinate(x,y);
		TypeOfObject = CharsToObjectTypes[type];
		if (TypeOfObject == MonsterType.BlackHole)
		{
			var light = gameObject.GetComponentInChildren<Light>();
			light.enabled = true;
			light.range = 1.33f;
			light.color = Color.blue;
		}
		if (isUnstable)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = StableToUnstableSprites[TypeOfObject];
		}
		else
			gameObject.GetComponent<SpriteRenderer>().sprite = TypesToSprites[TypeOfObject];

		Destination = GameField.GetVectorFromCoord(GridPosition.X, GridPosition.Y);
		if (delay > 0)
		{
			Delay = delay;
			StartTime = Time.time;
			State = MonsterState.WaitingForInitialising;
			return;
		}
		State = MonsterState.Growing;
	}

	void Update()
	{
		if (State == MonsterState.Destroying)
		{
			if (TypeOfObject == MonsterType.Vampire || TypeOfObject == MonsterType.Zombie)
				SendToBasket();
			if (TypeOfObject == MonsterType.Spider || TypeOfObject == MonsterType.Bat)
				SendToPot();
			if (TypeOfObject == MonsterType.Ghost)
				SendToPortal();
			if (TypeOfObject == MonsterType.Coocon)
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
			if (IsUnstable)
			{
				scale.x = 1f;
				scale.y = 1f;
			}
			else
			{
				scale.x += GrowSpeed;
				scale.y += GrowSpeed;
			}
			gameObject.transform.localScale = scale;
			if (gameObject.transform.localScale.x < 0.95)
				return;
		}
		else if (State == MonsterState.Growing)
			State = MonsterState.Default;
		if (TypeOfObject == MonsterType.BlackHole)
		{
			HandleBlackHole();
		}
		//if (IsUnstable && rnd.Next(2) == 1)
		//{
		//	Vector3 scale = transform.localScale;
		//	scale.x -= 0.01f;
		//	scale.y -= 0.01f;
		//	gameObject.transform.localScale = scale;
		//}
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
						Game.TurnsLeft--;
						Game.instance.Update();
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
			gameObject.GetComponentInChildren<Light>().enabled = State == MonsterState.Clicked;
								   
		if (IsMonster() && !IsFrozen && State !=MonsterState.Moving && GridPosition.Y != Game.MAP_SIZE - 1)
		{
			DropAsteroids();
		}

	}

	private void DropAsteroids()
	{
		if (GameField.Map[GridPosition.X, GridPosition.Y + 1] == null)
		{
			GameField.Drop(GridPosition, new Coordinate(GridPosition.X, GridPosition.Y + 1));
		}
		// if downdrop is blocked
		else if (GridPosition.X > 0 &&
				 GameField.Map[GridPosition.X - 1, GridPosition.Y + 1] == null)
		{
			for (int i = GridPosition.Y; i >= 0; i--)
			{
				if (GameField.Map[GridPosition.X - 1, i] != null
					&& (!GameField.Map[GridPosition.X - 1, i].IsMonster() || GameField.Map[GridPosition.X - 1, i].IsFrozen))
				{
					GameField.Drop(GridPosition, new Coordinate(GridPosition.X - 1, GridPosition.Y + 1));
					break;
				}
				if (GameField.Map[GridPosition.X - 1, i] != null && GameField.Map[GridPosition.X - 1, i].IsMonster())
					break;
			}

		}
		else if (GridPosition.X < GameField.Map.GetLength(0) - 1 &&
				 GameField.Map[GridPosition.X + 1, GridPosition.Y + 1] == null)
		{
			for (int i = GridPosition.Y; i >= 0; i--)
			{
				if (GameField.Map[GridPosition.X + 1, i] != null
					&& (!GameField.Map[GridPosition.X + 1, i].IsMonster() || GameField.Map[GridPosition.X + 1, i].IsFrozen))
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
			if (!blackHoleHasJumped && !GameField.IsAnyMoving())
				JumpBlackHole();
		}
		else
			blackHoleHasJumped = false;
	}

	private void JumpBlackHole()
	{
		blackHoleHasJumped = true;
		var map = GameField.Map;
		var unsuitableAsteroids = new List<MonsterType>();
		if (GridPosition.X > 0 && map[GridPosition.X - 1, GridPosition.Y] != null)
			unsuitableAsteroids.Add(map[GridPosition.X - 1, GridPosition.Y].TypeOfObject);
		if (GridPosition.Y > 0 && map[GridPosition.X, GridPosition.Y - 1] != null)
			unsuitableAsteroids.Add(map[GridPosition.X, GridPosition.Y - 1].TypeOfObject);
		if (GridPosition.X < map.GetLength(0) - 1 && map[GridPosition.X + 1, GridPosition.Y] != null)
			unsuitableAsteroids.Add(map[GridPosition.X + 1, GridPosition.Y].TypeOfObject);
		if (GridPosition.Y < map.GetLength(1) - 1 && map[GridPosition.X, GridPosition.Y + 1] != null)
			unsuitableAsteroids.Add(map[GridPosition.X, GridPosition.Y + 1].TypeOfObject);
		unsuitableAsteroids = unsuitableAsteroids.Distinct().ToList();
		var coords = new List<Coordinate>();
		for (int i = 0; i < map.GetLength(0); i++)
		{
			for (int j = 0; j < map.GetLength(1); j++)
			{
				if (map[i, j] != null && map[i, j].IsMonster() && !unsuitableAsteroids.Contains(map[i, j].TypeOfObject)
				    &&
				    ((j < map.GetLength(1) - 1 && map[i, j + 1] != null &&
					map[i, j].TypeOfObject == map[i, j + 1].TypeOfObject) ||
					(j > 0 && map[i, j - 1] != null &&
					map[i, j].TypeOfObject == map[i, j - 1].TypeOfObject) ||
					(i > 0&& map[i - 1,j] != null &&
					map[i, j].TypeOfObject == map[i - 1, j].TypeOfObject) ||
					 (i < map.GetLength(0) - 1 && map[i + 1, j] != null &&
					 map[i, j].TypeOfObject == map[i + 1, j].TypeOfObject)))
				{
					coords.Add(map[i, j].GridPosition);
					if (map[i, j].IsUnstable)
					{
						State = MonsterState.Decreasing;
						blackHoleTarget = new Coordinate(i,j);
						if (!GameField.Map[blackHoleTarget.X, blackHoleTarget.Y].IsTargetForBlackHole)
						{
							GameField.Map[blackHoleTarget.X, blackHoleTarget.Y].IsTargetForBlackHole = true;
							return;
						}
					}
				}
			} 
		}
		if (coords.Count > 0)
		{
			State = MonsterState.Decreasing;
			blackHoleTarget = coords.ElementAt(rnd.Next(coords.Count));
			if (!GameField.Map[blackHoleTarget.X, blackHoleTarget.Y].IsTargetForBlackHole)
				GameField.Map[blackHoleTarget.X, blackHoleTarget.Y].IsTargetForBlackHole = true;
		}
	}

	private IEnumerator ThrowFireball()
	{
		Game.PlayerIsBlocked = true;
		var fireball = GameObject.Find("hero").transform.FindChild("Fireball").gameObject;
		fireball.SetActive(true);
		var distance = transform.position - fireball.transform.position;
		var posCopy = fireball.transform.position;
		var scaleCopy = fireball.transform.localScale;
		for (int i = 0; i < 50; i++)
		{
			posCopy.x += distance.x/50;
			posCopy.y += distance.y/50;
			scaleCopy.x += 0.01f;
			scaleCopy.y += 0.01f;
			fireball.transform.position = posCopy;
			fireball.transform.localScale = scaleCopy;
			fireball.transform.Rotate(Vector3.back*0.1f);
			yield return new WaitForSeconds(0.005f);
		}
		fireball.transform.position = new Vector3(-3.47f, 1.84f);
		fireball.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
		fireball.SetActive(false);		
		Game.PlayerIsBlocked = false;
		SkillButton.Deactivate(SkillButtonType.Fire);
		GameField.DestroySquare(GridPosition);
	}
	private IEnumerator ThrowLightning()
	{
		Game.PlayerIsBlocked = true;
		var hero = GameObject.Find("hero");
		var l1 = hero.transform.FindChild("Lightning").gameObject;
		var l2 = hero.transform.FindChild("Lightning2").gameObject;
		for (int i = 0; i < 6; i++)
		{
			if (l1.activeSelf)
			{
				l2.SetActive(true);
				l1.SetActive(false);
			}
			else
			{
				l2.SetActive(false);
				l1.SetActive(true);
			}
			yield return new WaitForSeconds(0.1f);
		}
		l2.SetActive(false);
		Game.PlayerIsBlocked = false;
		SkillButton.Deactivate(SkillButtonType.Electro);
		GameField.DestroyAllOf(TypeOfObject);
	}
	void OnMouseDown()
	{										 
		if (!Game.IsPlayerBlocked() || !IsMonster())
			return;

		if (Game.ClickType == ClickState.Electro)
		{
			StartCoroutine(ThrowLightning());
			return;
		}
		if (Game.ClickType == ClickState.Fire)
		{
			StartCoroutine(ThrowFireball());
			return;
		}
		if (IsFrozen)
			return;
		if (State == MonsterState.Default)
		{
			if (GameField.ClickedObject == null)
			{
				GameField.ClickedObject = GridPosition;
				State = MonsterState.Clicked;
			}
			else
			{
				if (GridPosition.IsNeighbourWith(GameField.ClickedObject.Value))
				{
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


	public static readonly Dictionary<char, MonsterType> CharsToObjectTypes = new Dictionary<char, MonsterType>
	{
		{ 'Z', MonsterType.Zombie},
		{ 'S', MonsterType.Spider},
		{ 'V', MonsterType.Vampire},
		{ 'B', MonsterType.Bat},
		{ 'G', MonsterType.Ghost},
		{ 'H', MonsterType.BlackHole},
		{ 'E', MonsterType.EmptyCell},
		{'C', MonsterType.Coocon}
	};

	public static Dictionary<MonsterType, Sprite> TypesToSprites;

	public static Dictionary<MonsterType, Sprite> StableToUnstableSprites;

	private static readonly List<MonsterType> AsteroidTypes = new List<MonsterType>()
	{
		MonsterType.Zombie,
		MonsterType.Spider,
		MonsterType.Ghost,
		MonsterType.Vampire,
		MonsterType.Bat
	};

}
