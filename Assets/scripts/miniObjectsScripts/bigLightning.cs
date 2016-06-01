using UnityEngine;
using System.Collections;
using Assets.scripts;

public class bigLightning : MonoBehaviour
{

	private float startTime;
	public Coordinate Target;
	// Use this for initialization
	void Start () {
		Game.PlayerIsBlocked = true;
		startTime = Time.time;
		GetComponent<Animation>().Play();
	}
	
	// Update is called once per frame
	void Update () {
		if (startTime + 1 < Time.time)
		{
			Game.PlayerIsBlocked = false;
			if (GameField.Map[Target.X, Target.Y].IsMonster() && !GameField.Map[Target.X, Target.Y].IsUpgradable())
				GameField.DestroyAllOf(GameField.Map[Target.X, Target.Y].TypeOfMonster);
			Destroy(gameObject);
			
		}
	}
}
