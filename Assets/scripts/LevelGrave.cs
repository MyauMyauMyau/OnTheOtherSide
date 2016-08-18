using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class LevelGrave : MonoBehaviour
{

	private int n;
	private static Vector3 candle1pos = new Vector3(-30.9f, -15.3f);
	private static Vector3 candle2pos = new Vector3(29.7f, -11.3f);
	private static Vector3 candle3pos = new Vector3(15.4f, -25.8f);
	public static GameObject Candle;
	// Use this for initialization
	private void Start()
	{
		var candlesInfo = PlayerPrefs.GetString("LevelCandles");
		n = int.Parse(name.Substring(5));
		if (candlesInfo.Length > n)
		{
			var candles = int.Parse(candlesInfo.ElementAt(n).ToString());
			var candle1 = Instantiate(Candle, new Vector3(), Quaternion.Euler(new Vector3())) as GameObject;
			candle1.transform.SetParent(transform, false);
			candle1.transform.localPosition = candle1pos;
			if (candles > 1)
			{
				var candle2 = Instantiate(Candle, new Vector3(), Quaternion.Euler(new Vector3())) as GameObject;
				candle2.transform.SetParent(transform, false);
				candle2.transform.localPosition = candle2pos;
			}
			if (candles > 2)
			{
				var candle3 = Instantiate(Candle, new Vector3(), Quaternion.Euler(new Vector3())) as GameObject;
				candle3.transform.SetParent(transform, false);
				candle3.transform.localPosition = candle3pos;
			}
		}
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (PlayerPrefs.GetInt("LevelUnlocked") >= n)
		{
			GetComponentInChildren<Text>().color = Color.white;
			GetComponent<Button>().interactable = true;
		}
		else
		{
			GetComponentInChildren<Text>().color = Color.gray;
			GetComponent<Button>().interactable = false;
		}
	}
}
