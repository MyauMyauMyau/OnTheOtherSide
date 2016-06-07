using UnityEngine;
using System.Collections;
using Assets.scripts;

public class huntersShot : MonoBehaviour
{

	public Coordinate Target;
	// Use this for initialization
	void Start ()
	{
		StartCoroutine(Shoot());
	}

	private IEnumerator Shoot()
	{
		Game.PlayerIsBlocked = true;
		for (int i = 0; i < 20; i++)
		{
			transform.position = transform.position + new Vector3(1.5f/20, 0);		
			yield return new WaitForSeconds(0.015f);
		}
		transform.position = new Vector3(-4.5f, GameField.GetVectorFromCoord(Target.X, Target.Y).y);
		var distance = transform.position.x - GameField.GetVectorFromCoord(Target.X, Target.Y).x;
		for (int i = 0; i < Target.Y*5; i++)
		{
			transform.position = transform.position - new Vector3(distance/(Target.Y*5), 0);
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
