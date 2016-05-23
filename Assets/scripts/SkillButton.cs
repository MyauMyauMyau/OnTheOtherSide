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
			GameUI.Instance.ActivatePanel(Skill);

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
