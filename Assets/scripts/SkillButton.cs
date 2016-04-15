using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.scripts;
using Assets.scripts.Enums;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
	public static List<SkillButton> buttons = new List<SkillButton>();
	public float FillAmount;
	public static Sprite ActiveFire;
	public static Sprite Fire;
	public static Sprite ActiveElectro;
	public static Sprite Electro;
	public SkillButtonType Type;
	public static void AddEnergy(MonsterType type)
	{
		foreach (var button in buttons)
		{
			if (button.Type == SkillButtonType.Fire && (type == MonsterType.Vampire || type == MonsterType.Zombie))
				button.FillAmount += 0.02f;
			if (button.Type == SkillButtonType.Electro && type != MonsterType.Vampire && type != MonsterType.Zombie)
				button.FillAmount += 0.02f;
			if (button.FillAmount >= 1)
			{
				button.FillAmount = 1;
				if (button.Type == SkillButtonType.Fire)
					button.GetComponent<Button>().image.overrideSprite = ActiveFire;
				if (button.Type == SkillButtonType.Electro)
					button.GetComponent<Button>().image.overrideSprite = ActiveElectro;
			}
		}
	}


	public static void Deactivate(SkillButtonType type)
	{
		foreach (var skillButton in buttons)
		{
			if (skillButton.Type == type)
			{
				skillButton.FillAmount = 0;
				if (skillButton.Type == SkillButtonType.Fire)
					skillButton.GetComponent<Button>().image.overrideSprite= Fire;
				if (skillButton.Type == SkillButtonType.Electro)
					skillButton.GetComponent<Button>().image.overrideSprite = Electro;
				Cursor.SetCursor(Game.MainCursor, new Vector2(0, 0), CursorMode.Auto);
				Game.ClickType = ClickState.Default;
			}
		}		
	}
	// Use this for initialization
	private void Start()
	{
		FillAmount = 0.95f;
		GetComponent<Button>().interactable = false;
		buttons.Add(this);
		GetComponent<Button>().onClick.AddListener(OnClickEvent);	
	}
	
	// Update is called once per frame
	void Update ()
	{
		GetComponent<Image>().fillAmount = FillAmount;
		if (FillAmount >= 1f)
		{
			GetComponent<Button>().interactable = true;
		}
	}

	void OnClickEvent()
	{
		if (Type == SkillButtonType.Electro)
		{
			Game.ClickType = ClickState.Electro;
			Cursor.SetCursor(Game.ElectroCursor, new Vector2(0, 0), CursorMode.Auto);
		}
		if (Type == SkillButtonType.Fire)
		{
			Game.ClickType = ClickState.Fire;
			Cursor.SetCursor(Game.FireCursor, new Vector2(0, 0), CursorMode.Auto);
		}	
	}
}
