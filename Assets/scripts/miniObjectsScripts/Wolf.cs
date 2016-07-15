using System;
using UnityEngine;
using System.Collections;
using Assets.scripts;

public class Wolf : MonoBehaviour
{

	public int Target;
	// Use this for initialization
	void Start ()
	{
		StartCoroutine(RunLine());
	}

	private IEnumerator RunLine()
	{
		Game.PlayerIsBlocked = true;
		Game.DropIsBlocked = true;
		var dist = transform.position - new Vector3(transform.position.x + 10, transform.position.y);
		var step = dist/100;
		for (int i = 0; i < 100; i++)
		{
			transform.position -= step;
			var k = i - 15;
			//Debug.Log(GameField.Map[7, Target].TypeOfMonster);
			if (k>=0 && k < 64 && k%8 == 0)
			{
				if (GameField.Map[k/8, Target] != null && GameField.Map[k/8, Target].IsMonster()
				    && !GameField.Map[k/8, Target].IsUpgradable())
				{
					GameField.Map[k / 8, Target].DestroyMonster();
					
				}
					
			}
			yield return new WaitForSeconds(0.02f);
		}
		Game.DropIsBlocked = false;
		Game.PlayerIsBlocked = false;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
