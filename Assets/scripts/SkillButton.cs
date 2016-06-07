using System;
using System.Reflection;
using Assets.scripts.Skills;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts
{
	public class SkillButton : MonoBehaviour
	{

		public Sprite InactiveImage;
		public Sprite ActiveImage;
		public float FillAmount;
		public int ButtonNumber;
		public Action Skill;
		public int NumberOfTargets;
		public GameObject TargetBrackets;
		// Use this for initialization
		void Start ()
		{
			FillAmount = 1f;

			var skills = Dictionaries.HeroTypeToSkills[Game.HeroType];
			Skill = () =>
			{
				skills.GetType().GetMethod("Skill" + ButtonNumber).Invoke(skills, null);
				Deactivate();
			};
			var propName = "TargetsSkill" + ButtonNumber + "Lvl" + skills.GetLevelOfUpgrade(ButtonNumber);
			NumberOfTargets =
				(int)skills.GetType()
					.GetProperty(propName)
					.GetGetMethod()
					.Invoke(skills, null);
			Debug.Log(NumberOfTargets);
		}
	
		// Update is called once per frame
		void Update () {

			if (FillAmount >= 1)
			{
				Activate();	
			}
			else
			{
				GetComponent<Image>().fillAmount = FillAmount;
			}
		}

		public void OnClick()
		{
			if (Game.IsPlayerBlocked()) return;
			SkillsController.Hero.GetComponent<Animator>().SetTrigger("Skill" + ButtonNumber + "Ready");
			GameUI.Instance.ActivatePanel(Skill, NumberOfTargets);
			SkillsController.TargetBrackets = TargetBrackets;
		}

		public void Activate()
		{
			GetComponent<Button>().image.overrideSprite = ActiveImage;
			FillAmount = 1;
			GetComponent<Button>().interactable = true;
			GetComponent<Image>().fillAmount = FillAmount;
		}

		public void Deactivate()
		{
			FillAmount = 0;
			GetComponent<Button>().image.overrideSprite = InactiveImage;
			GetComponent<Button>().interactable = false;
		}
	}
}
