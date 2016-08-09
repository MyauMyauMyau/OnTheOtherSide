using UnityEngine;
using System.Collections;
using Assets.scripts;

public class Skull : MonoBehaviour
{

	public Coordinate TargetCoordinate;
	private float StartTime;
	public Vector3 StartPosition;
	// Use this for initialization
	private void Start()
	{
		StartTime = Time.time;
		StartPosition = transform.position;
	}

	// Update is called once per frame
	private void Update()
	{
		var curPos = Vector3.Lerp(
			StartPosition, GameField.GetVectorFromCoord(TargetCoordinate.X, TargetCoordinate.Y),
			(Time.time - StartTime));
		curPos.y += 4*Mathf.Sin(Mathf.Clamp01((Time.time - StartTime))*Mathf.PI);
		transform.position = curPos;
		if (transform.position == GameField.GetVectorFromCoord(TargetCoordinate.X, TargetCoordinate.Y))
		{
			Monster monster = ((GameObject)Instantiate(
						Game.SkullPrefab, GameField.GetVectorFromCoord(TargetCoordinate.X, TargetCoordinate.Y),
						Quaternion.Euler(new Vector3())))
						.GetComponent<Monster>();
			monster.Initialise(TargetCoordinate.X, TargetCoordinate.Y, 'M');
			if (GameField.Map[TargetCoordinate.X, TargetCoordinate.Y].IsMonster()  && !GameField.Map[TargetCoordinate.X, TargetCoordinate.Y].IsUpgradable())
			{
				if (GameField.Map[TargetCoordinate.X, TargetCoordinate.Y].IsFrozen)
					GameField.Map[TargetCoordinate.X, TargetCoordinate.Y].DestroyMonster();
				GameField.Map[TargetCoordinate.X, TargetCoordinate.Y].DestroyMonster();
				AudioHolder.PlayMummySkullDrop();
			}
			else
				GameField.Map[TargetCoordinate.X, TargetCoordinate.Y].Destroy();
			GameField.Map[TargetCoordinate.X, TargetCoordinate.Y] = monster;
			Destroy(gameObject);
		}
	}
}