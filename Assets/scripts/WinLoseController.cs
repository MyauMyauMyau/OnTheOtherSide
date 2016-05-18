using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.scripts;
using UnityEngine.UI;

public class WinLoseController : MonoBehaviour
{
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Game.GameIsFinished) return;
		if (CheckWin())
		{
			Game.PlayerIsBlocked = true;
			if (!GameField.IsAnyMoving())
			{
				Game.GameIsFinished = true;
				AudioHolder.PlayWin();
				ShowFlag(true);
				if (PlayerPrefs.GetInt("LevelUnlocked") == Game.Level)
				{
					PlayerPrefs.SetInt("LevelUnlocked", Game.Level + 1);
					PlayerPrefs.Save();
				}
			}
		}
		else if (Game.TurnsLeft == 0)
		{
			if (!GameField.IsAnyMoving())
				GameField.UpdateField();
			Game.PlayerIsBlocked = true;
			if (!GameField.IsAnyMoving() && !CheckWin())
			{
				PlayerPrefs.SetInt("Lives", PlayerPrefs.GetInt("Lives") - 1);
				Game.GameIsFinished = true;
				AudioHolder.PlayLose();
				ShowFlag(false);
			}
		}
	}

	private void ShowFlag(bool isWin)
	{
		var flagName = isWin ? "WinFlag" : "LoseFlag";
		transform.FindChild(flagName).GetComponent<Animation>().Play();
	}

	

	private bool CheckWin()
	{
		for (var i = 0; i < Game.LevelInformation.Targets.Count; i++)
		{
			var currentMonsters = Dictionaries.MonsterCounter[Dictionaries.CharsToObjectTypes[Game.LevelInformation.Targets.ElementAt(i).Key]];
			var totalMonsters = Game.LevelInformation.Targets.ElementAt(i).Value;
			if (currentMonsters < totalMonsters) return false;
		}
		return true;
	}
}
