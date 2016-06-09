using UnityEngine;
using System.Collections;
using Assets.scripts;

public class PriestLightning : MonoBehaviour
{

	public Coordinate Target;
	// Use this for initialization
	void Start () {
		StartCoroutine(SendLightning());	
	}

	private IEnumerator SendLightning()
	{
		Game.PlayerIsBlocked = true;
		var distance = transform.position - GameField.GetVectorFromCoord(Target.X, Target.Y);
		for (int i = 0; i < 25; i++)
		{
			transform.position = transform.position - distance/25;
			yield return new WaitForSeconds(0.02f);
		}

		GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;
		yield return new WaitForSeconds(0.5f);
		GameField.Map[Target.X, Target.Y].DestroyMonster();
		Game.PlayerIsBlocked = false;
		Destroy(gameObject);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
