using UnityEngine;
using System.Collections;
using Assets.scripts;

public class bloodDrop : MonoBehaviour
{

	public GameObject BloodVortexPrefab;
	public Coordinate Target;
	public float StartTime;
	public Vector3 StartPosition { get; set; }
	// Use this for initialization
	void Start ()
	{
		StartTime = Time.time;
		StartPosition = transform.position;
	}

	

	// Update is called once per frame
	void Update () {
		if (Time.time < StartTime)
		{
			return;
		}
		GetComponent<SpriteRenderer>().enabled = true;
		var curPos = Vector3.Lerp(
				StartPosition, GameField.GetVectorFromCoord(Target.X, Target.Y),
				(Time.time - StartTime));
		curPos.y += 4 * Mathf.Sin(Mathf.Clamp01((Time.time - StartTime)) * Mathf.PI);
		transform.position = curPos;
		transform.Rotate(new Vector3(0,0,5f));
		if (transform.position == GameField.GetVectorFromCoord(Target.X, Target.Y))
		{
			var bloodVortex = ((GameObject)Instantiate(BloodVortexPrefab, GameField.GetVectorFromCoord(Target.X, Target.Y), Quaternion.Euler(new Vector3()))
				).GetComponent<Monster>();
			if (GameField.Map[Target.X, Target.Y].IsFrozen)
				GameField.Map[Target.X, Target.Y].DestroyMonster();
			GameField.Map[Target.X, Target.Y].DestroyMonster();
			bloodVortex.Initialise(Target.X, Target.Y, 'X');
			GameField.Map[Target.X, Target.Y] = bloodVortex;
			Destroy(gameObject);
		}
			
	}
}
