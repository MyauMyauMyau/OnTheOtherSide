using Assets.scripts.Enums;
using UnityEngine;

namespace Assets.scripts.miniObjectsScripts
{
	public class Raft : MonoBehaviour
	{

		public Coordinate TargetCoordinate;
		private float StartTime;
		public Vector3 StartPosition;
		// Use this for initialization
		void Start ()
		{
			StartTime = Time.time;
			StartPosition = transform.position;
		}
	
		// Update is called once per frame
		void Update () {
			var curPos = Vector3.Lerp(
				StartPosition, GameField.GetVectorFromCoord(TargetCoordinate.X, TargetCoordinate.Y),
				(Time.time - StartTime));
			curPos.y += 4 * Mathf.Sin(Mathf.Clamp01((Time.time - StartTime)) * Mathf.PI);
			transform.position = curPos;
			if (transform.position.y - new Vector3(TargetCoordinate.X, TargetCoordinate.Y).y < 0.001f)
			{
				Monster bridge = ((GameObject)Instantiate(
						Game.MonsterPrefab, GameField.GetVectorFromCoord(TargetCoordinate.X, TargetCoordinate.Y),
						Quaternion.Euler(new Vector3())))
						.GetComponent<Monster>();
				bridge.transform.localScale = new Vector3(1,1);
				bridge.Initialise(TargetCoordinate.X, TargetCoordinate.Y, 'R');
				WaterField.BridgeObjects.Add(bridge);
				WaterField.Bridges.Add(new Coordinate(TargetCoordinate.X, TargetCoordinate.Y), Direction.Forward);
				bridge.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
				bridge.gameObject.GetComponent<CircleCollider2D>().enabled = false;
				Destroy(gameObject);
			}
		}
	}
}
