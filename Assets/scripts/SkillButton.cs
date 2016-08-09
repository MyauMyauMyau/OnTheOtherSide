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
		public GameObject TargetBrackets2;
		public GameObject LineBrackets;
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
			SkillsController.CurrentSkillNumber = ButtonNumber;
			SkillsController.Hero.GetComponent<Animator>().SetTrigger("Skill" + ButtonNumber + "Ready");
			
			SkillsController.TargetBrackets = TargetBrackets;
			SkillsController.TargetBrackets2 = TargetBrackets2;
			SkillsController.LineBrackets = LineBrackets;
			GameUI.Instance.ActivatePanel(Skill, NumberOfTargets);

			var skills = Dictionaries.HeroTypeToSkills[Game.HeroType];
			SkillsController.IsMonsterClickable =
				(m => (bool) skills.GetType().GetMethod("IsPossibleTarget")
				.Invoke(skills, new object[] {m, ButtonNumber}));
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
