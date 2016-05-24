using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts
{
	public class SkillsController : MonoBehaviour
	{
		public static GameObject TargetBrackets;
		public static bool IsActive;
		public static List<Coordinate> TargetCoordinates;
		public static List<GameObject> Brackets; 
		public static int MaxTargets;
		public static GameObject Hero;
		// Use this for initialization
		void Start ()
		{
			IsActive = false;
			MaxTargets = 0;
			TargetCoordinates = new List<Coordinate>();
			Brackets = new List<GameObject>();
		}

		public static void Activate(int numOfTargets)
		{
			IsActive = true;
			MaxTargets = numOfTargets;
			TargetCoordinates = new List<Coordinate>();

		}

		public static void Deactivate()
		{
			MaxTargets = 0;
			IsActive = false;
			foreach(var bracket in Brackets)
				Destroy(bracket);
			Brackets = new List<GameObject>();
		}
		// Update is called once per frame
		void Update ()
		{
			if (!IsActive) return;
			if (MaxTargets == 0 || TargetCoordinates.Count>0)
				ActivateYes();
			else
				DeactivateYes();
		}

		void DeactivateYes()
		{
			
			var btn = GetComponentInChildren<Button>();
			btn.interactable = false;
			btn.GetComponent<Image>().color = Color.gray;
		}

		void ActivateYes()
		{
			var btn = GetComponentInChildren<Button>();
			btn.interactable = true;
			btn.GetComponent<Image>().color = Color.white;
		}

		public static void BracketMonster(Coordinate gridPosition)
		{
			if (MaxTargets == TargetCoordinates.Count) return;
			var bracket = (GameObject) Instantiate(TargetBrackets,
				GameField.GetVectorFromCoord(gridPosition.X, gridPosition.Y), Quaternion.Euler(new Vector3()));
			Brackets.Add(bracket);
			TargetCoordinates.Add(gridPosition);
		}
	}
}
