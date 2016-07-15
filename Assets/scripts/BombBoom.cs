using System;
using UnityEngine;
using System.Collections;
using Assets.scripts;
public class BombBoom : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(Boom());
	}

	public Coordinate GridPosition;
	// Update is called once per frame
	void Update () {
	
	}

	private IEnumerator Boom()
	{
		Game.PlayerIsBlocked = true;
		Game.DropIsBlocked = true;
		var scale = gameObject.transform.localScale;
		for (int i = 0; i < 20; i++)
		{
			scale.x += 0.05f;
			scale.y += 0.05f;
			transform.localScale = scale;
			yield return null;
		}


		var bottomBoundX = Math.Max(0, GridPosition.X - 2);
		var bottomBoundY = Math.Max(0, GridPosition.Y - 2);
		var topBoundX = Math.Min(Game.MAP_SIZE - 1, GridPosition.X + 2);
		var topBoundY = Math.Min(Game.MAP_SIZE - 1, GridPosition.Y + 2);


		for (int x = bottomBoundX; x <= topBoundX; x++)
		{
			if (x == GridPosition.X)
				continue;
			if (GameField.Map[x, GridPosition.Y] != null && GameField.Map[x, GridPosition.Y].IsMonster()
				&& !GameField.Map[x, GridPosition.Y].IsUpgradable())
				GameField.Map[x, GridPosition.Y].DestroyMonster();
		}
		for (int y = bottomBoundY; y <= topBoundY; y++)
		{
			if (y == GridPosition.Y)
				continue;
			if (GameField.Map[GridPosition.X, y] != null && GameField.Map[GridPosition.X, y].IsMonster()
				&& !GameField.Map[GridPosition.X, y].IsUpgradable())
				GameField.Map[GridPosition.X, y].DestroyMonster();
		}
		Game.PlayerIsBlocked = false;
		Game.DropIsBlocked = false;
		DestroyObject(gameObject);		
	}
}
