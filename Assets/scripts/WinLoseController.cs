using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.scripts;
using UnityEngine.UI;

public class WinLoseController : MonoBehaviour
{

	private bool win;
	private bool lose;
	private bool canvasIsShown;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) && !canvasIsShown)
			ShowCanvas("Сбегаете?", "Выйти в меню");
		else if (CheckWin() && !canvasIsShown)
		{
			Game.PlayerIsBlocked = true;
			if (!GameField.IsAnyMoving())
			{
				AudioHolder.PlayWin();
				ShowCanvas("Победа!", "Выйти в меню");
				if (PlayerPrefs.GetInt("LevelUnlocked") == Game.Level)
				{
					PlayerPrefs.SetInt("LevelUnlocked", Game.Level + 1);
					PlayerPrefs.Save();
				}
			}
		}
		else if (Game.TurnsLeft == 0 && !canvasIsShown)
		{
			Game.PlayerIsBlocked = true;
			if (!GameField.IsAnyMoving() && !CheckWin())
			{
				AudioHolder.PlayLose();
				ShowCanvas("Поражение!", "Выйти в меню");
			}
		}
		else if (Input.GetKeyDown(KeyCode.Escape))
			HideCanvas();


	}

	private bool CheckWin()
	{
		for (var i = 0; i < Game.LevelInformation.Targets.Count; i++)
		{
			var currentMonsters = Game.MonsterCounter[Monster.CharsToObjectTypes[Game.LevelInformation.Targets.ElementAt(i).Key]];
			var totalMonsters = Game.LevelInformation.Targets.ElementAt(i).Value;
			if (currentMonsters < totalMonsters) return false;
		}
		return true;
	}

	private void HideCanvas()
	{
		canvasIsShown = false;
		var text = transform.FindChild("Text");
		text.gameObject.SetActive(false);
		
		var btn = transform.FindChild("Button");
		btn.gameObject.SetActive(false);
		
	}

	void ShowCanvas(string labelText, string buttonText)
	{
		canvasIsShown = true;
		var text = transform.FindChild("Text");
		text.gameObject.SetActive(true);
		text.GetComponent<Text>().text = labelText;
		var btn = transform.FindChild("Button");
		btn.gameObject.SetActive(true);
		btn.GetComponentInChildren<Text>().text = buttonText;
	}
}
