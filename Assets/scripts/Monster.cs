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
	public MonsterType TypeOfMonster { get; set;}
	public static Sprite EmptyCellSprite;
	public static Sprite BlackHoleSprite;
	public static Sprite CooconSprite;
	public static Sprite BombSprite;
	public static Sprite WaterVSprite;
	public static Sprite WaterHSprite;
	public static Sprite WaterDSprite;
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
	private bool blackHoleHasJumped;
	private bool isSpritesLoaded;
	public float UpSpeed = 0.01f;
	public const float GravitySpeed = 0.01f;
	public float ToBasketStartTime;
	public MonsterState State { get; set; }
	public bool UpdatedField = false;
	public Random rnd = new Random();
	public Vector3 DestroyRotation;
	public int DestroyRotationSpeed;
	public bool IsMonster()
	{
		return MonsterTypes.Contains(TypeOfMonster);
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
		if (IsFrozen)
		{
			IsFrozen = false;
			gameObject.transform.GetChild(1).gameObject.SetActive(false);
			return;
		}

		if (TypeOfMonster == MonsterType.Bomb)
		{
			AudioHolder.PlayBomb();
			DestroyBomb();
			return;
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
			Game.MonsterCounter[MonsterType.Coocon]++;
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
		Game.MonsterCounter[TypeOfMonster]++;
		SkillButton.AddEnergy(TypeOfMonster);
		State = MonsterState.Destroying;
		AudioHolder.PlayRemove();
		foreach (var ice in iceList)
		{
			GameField.Map[ice.X, ice.Y].DestroyMonster();				
		}
	}

	
	private void DestroyBomb()
	{
		GetComponent<SpriteRenderer>().enabled = false;
		GameObject boom = ((GameObject)Instantiate(
		Game.BombFirePrefab, GameField.GetVectorFromCoord(GridPosition.X, GridPosition.Y),
		Quaternion.Euler(new Vector3())));
		GameField.Map[GridPosition.X, GridPosition.Y] = null;
		State = MonsterState.Destroying;
		var bottomBoundX = Math.Max(0, GridPosition.X - 2);
		var bottomBoundY = Math.Max(0, GridPosition.Y - 2);
		var topBoundX = Math.Min(Game.MAP_SIZE - 1, GridPosition.X + 2);
		var topBoundY = Math.Min(Game.MAP_SIZE - 1, GridPosition.Y + 2);
	

		for (int x = bottomBoundX; x <= topBoundX; x++)
		{
			if (x == GridPosition.X)
				continue;
			if (GameField.Map[x, GridPosition.Y] != null && GameField.Map[x, GridPosition.Y].IsMonster())
				GameField.Map[x, GridPosition.Y].DestroyMonster();
		}
		for (int y = bottomBoundY; y <= topBoundY; y++)
		{
			if (y == GridPosition.Y)
				continue;
			if (GameField.Map[GridPosition.X, y] != null && GameField.Map[GridPosition.X,y].IsMonster())
				GameField.Map[GridPosition.X, y].DestroyMonster();
		}

		
		
	}

	public void Initialise(int x, int y, char type, float delay = 0)
	{
		DestroyRotationSpeed = rnd.Next(500);
		DestroyRotation = (rnd.Next(2) == 0 ? Vector3.back : Vector3.forward);
		DropSpeed = BaseDropSpeed*2;
		

		GrowSpeed = 0.05f;
		GridPosition = new Coordinate(x,y);
		TypeOfMonster = CharsToObjectTypes[type];
		if (!MonsterTypes.Contains(Monster.CharsToObjectTypes[type]))
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = TypesToSprites[TypeOfMonster];
		}

		MoveSpeed = 10f;
		if (TypeOfMonster == MonsterType.BlackHole)
		{
			var light = gameObject.GetComponentInChildren<Light>();
			light.enabled = true;
			light.range = 1.33f;
			light.color = Color.blue;
		}
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
		State = MonsterState.Growing;
	}

	void Update()
	{
		if (State == MonsterState.Destroying)
		{
			if (TypeOfMonster == MonsterType.Voodoo || TypeOfMonster == MonsterType.Zombie)
				SendToBasket();
			if (TypeOfMonster == MonsterType.Spider || TypeOfMonster == MonsterType.Bat)
				SendToPot();
			if (TypeOfMonster == MonsterType.Ghost)
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
						StartCoroutine(Game.NextTurn());
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
			DropMonsters();
		}

	}

	private void DropMonsters()
	{
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
			blackHoleTarget = coords.ElementAt(rnd.Next(coords.Count));
			if (!GameField.Map[blackHoleTarget.X, blackHoleTarget.Y].IsTargetForBlackHole)
				GameField.Map[blackHoleTarget.X, blackHoleTarget.Y].IsTargetForBlackHole = true;
		}
	}

	private IEnumerator ThrowFireball()
	{
		AudioHolder.PlayFireBall();
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
		GameField.DestroySquare3X3(GridPosition);
		AudioHolder.PlayMassRemove();
	}
	private IEnumerator ThrowLightning()
	{
		AudioHolder.PlayElectricity();
		PlayerPrefs.SetInt("Achievement5Unlocked", 1);
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
		GameField.DestroyAllOf(TypeOfMonster);
		AudioHolder.PlayMassRemove();
	}
	void OnMouseDown()
	{
		if (Game.IsPlayerBlocked() || (!IsMonster() && TypeOfMonster != MonsterType.Bomb))
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
				if (TypeOfMonster == MonsterType.Bomb)
				{
					DestroyMonster();
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


	public static readonly Dictionary<char, MonsterType> CharsToObjectTypes = new Dictionary<char, MonsterType>
	{
		{ 'Z', MonsterType.Zombie},
		{ 'S', MonsterType.Spider},
		{ 'V', MonsterType.Voodoo},
		{ 'B', MonsterType.Bat},
		{ 'G', MonsterType.Ghost},
		{ 'H', MonsterType.BlackHole},
		{ 'E', MonsterType.EmptyCell},
		{'1', MonsterType.WaterHorizontal },
		{'2', MonsterType.WaterVertical },
		{'3', MonsterType.WaterDiagonal },
		{'4', MonsterType.WaterHorizontal },
		{'5', MonsterType.WaterVertical },
		{'6', MonsterType.WaterDiagonal },
		{'C', MonsterType.Coocon},
		{'W', MonsterType.Bomb },
		{'R', MonsterType.Raft }
	};

	public static Dictionary<MonsterType, Sprite> TypesToSprites;

	public static readonly List<MonsterType> MonsterTypes = new List<MonsterType>()
	{
		MonsterType.Zombie,
		MonsterType.Spider,
		MonsterType.Ghost,
		MonsterType.Voodoo,
		MonsterType.Bat,
	};

}
