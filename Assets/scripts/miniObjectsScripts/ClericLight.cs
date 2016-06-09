using UnityEngine;
using System.Collections;
using Assets.scripts;

public class ClericLight : MonoBehaviour {

	public Coordinate Target;

	public bool IsPowerful;
	// Use this for initialization
	void Start()
	{
		StartCoroutine(SendLight());
	}

	private IEnumerator SendLight()
	{
		Game.PlayerIsBlocked = true;
		var distance = transform.position - GameField.GetVectorFromCoord(Target.X, Target.Y);
		for (int i = 0; i < 25; i++)
		{
			transform.localScale = transform.localScale + new Vector3(0.04f, 0.04f);
			transform.position = transform.position - distance / 25;
			yield return new WaitForSeconds(0.02f);
		}
		var type = GameField.Map[Target.X, Target.Y].TypeOfMonster;
		GameField.Map[Target.X, Target.Y].Destroy();
		GameField.TransformPumpkin1(Target.X, Target.Y);
		if (type == MonsterType.Pumpkin1 && !IsPowerful)
			Game.MonsterCreate(Target.X, Target.Y, Dictionaries.MonstersUpgradeDictionary[type]);
		else
			Game.MonsterCreate(Target.X, Target.Y, Dictionaries.MonstersUpgradeDictionary[MonsterType.Pumpkin2]);
		Game.PlayerIsBlocked = false;
		Destroy(gameObject);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
