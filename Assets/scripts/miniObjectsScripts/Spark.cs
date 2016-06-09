using UnityEngine;
using System.Collections;
using Assets.scripts;

public class Spark : MonoBehaviour {

	// Use this for initialization
	public Coordinate Target;
	void Start ()
	{
		StartCoroutine(MakeSpark());
	}

	private IEnumerator MakeSpark()
	{
		Game.PlayerIsBlocked = true;
		GameField.Map[Target.X, Target.Y].State = MonsterState.WaitingForActivation;
		for (int i = 0; i < 25; i++)
		{
			transform.localScale = transform.localScale + new Vector3(0.04f, 0.04f);
			yield return new WaitForSeconds(0.02f);
		}
		GameField.Map[Target.X, Target.Y].DestroyMonster();
		Game.PlayerIsBlocked = false;
		Destroy(gameObject);
	}	
	
	// Update is called once per frame
	void Update () {
	
	}
}
