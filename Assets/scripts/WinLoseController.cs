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

	private bool IsFinished;
	// Update is called once per frame
	void Update ()
	{
		if (IsFinished)
			return;
		if (CheckWin())
		{
			Game.PlayerIsBlocked = true;
			if (!GameField.IsAnyMoving())
			{
				Game.GameIsFinished = true;
				IsFinished = true;
				AudioHolder.PlayWin();

				if (PlayerPrefs.GetInt("LevelUnlocked") == Game.Level)
				{
					if (PlayerPrefs.GetInt("LevelUnlocked") % 25 == 0)
						FacebookSharer.Instance.ShareYourResult(PlayerPrefs.GetInt("LevelUnlocked"));
					PlayerPrefs.SetInt("LevelUnlocked", Game.Level + 1);
					Debug.Log(Game.Level + " " + PlayerPrefs.GetInt("LevelUnlocked"));
					PlayerPrefs.Save();
				}
				ShowFlag(true);
			}
			Game.PlayerIsBlocked = false;
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
				IsFinished = true;
				AudioHolder.PlayLose();
				if (Game.CandlesLeft == 1)
					Game.BurnCandle(3);
				ShowFlag(false);
			}
		}
	}

	private void ShowFlag(bool isWin)
	{
		var flagName = isWin ? "WinFlag" : "LoseFlag";
		transform.FindChild(flagName).GetComponent<Animation>().Play();
		if (!isWin)
			transform.FindChild(flagName).FindChild("Heart").GetComponent<Animator>().SetTrigger("AnimTrigger");
		else
		{
			var candles = GameObject.Find("WinCandles");
			for (int i = 0; i < 3  - Game.CandlesLeft; i++)
			{
				var candle = candles.transform.GetChild(i);
				candle.GetComponent<Image>().overrideSprite = Game.OffCandleSprite;
				candle.GetComponent<Image>().SetNativeSize();
				candle.transform.position -= new Vector3(0, +0.15f);
				var smoke = ((GameObject)Instantiate(
					Game.SmokePrefab, candle.transform.position + new Vector3(0f, 0.5f),
					Quaternion.Euler(new Vector3())));
				smoke.transform.parent = candle;
				smoke.transform.localScale = new Vector3(1, 1, 1);
			}
			//reward
			var candlesInfo = PlayerPrefs.GetString("LevelCandles");
			int coins = 1;
			if (candlesInfo.Length > Game.Level)
			{
				var candlesLastResult = int.Parse(candlesInfo.ElementAt(Game.Level).ToString());
				coins = (Game.CandlesLeft - candlesLastResult)*2;
				if (coins < 0) coins = 0;
				if (Game.CandlesLeft - candlesLastResult > 0)
					PlayerPrefs.SetString("LevelCandles", candlesInfo.ReplaceAt(Game.Level,1,Game.CandlesLeft.ToString()));
			}
			else
			{
				if (Game.CandlesLeft == 2) coins = 3;
				if (Game.CandlesLeft == 3) coins = 5;
				PlayerPrefs.SetString("LevelCandles", candlesInfo + Game.CandlesLeft);
			}
			PlayerPrefs.Save();
			PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") + coins);
			transform.FindChild(flagName).FindChild("CoinsReward").GetComponent<Text>().text = coins.ToString();


		}
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
