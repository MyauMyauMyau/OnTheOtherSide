using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.scripts;

public class Claws : MonoBehaviour
{

	public List<Coordinate> Targets; 
	// Use this for initialization
	void Start ()
	{
		StartCoroutine(ShowClaws());
	}

	private IEnumerator ShowClaws()
	{
		var claw1 = transform.GetChild(0).GetComponent<SpriteRenderer>();
		var claw2 = transform.GetChild(1).GetComponent<SpriteRenderer>();
		claw1.color = new Color(claw1.color.r, claw1.color.g, claw1.color.b, 1);
		for (int i = 0; i < 60; i++)
		{
			if (i == 10)
				claw2.color = new Color(claw1.color.r, claw1.color.g, claw1.color.b, 1);
			if (i>10)
				claw2.color = new Color(claw1.color.r, claw1.color.g, claw1.color.b, 1 - (i - 10)*0.02f);
			if (i<=50)
				claw1.color = new Color(claw1.color.r, claw1.color.g, claw1.color.b, 1 - i * 0.02f);
			if (i == 15)
				DestroyMonsters();
			yield return new WaitForSeconds(0.04f);
		}
					 
	}

	private void DestroyMonsters()
	{
		foreach (var coordinate in Targets)
		{
			if (GameField.Map[coordinate.X, coordinate.Y] != null && GameField.Map[coordinate.X, coordinate.Y].IsFrozen)
				GameField.Map[coordinate.X, coordinate.Y].DestroyMonster();
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
