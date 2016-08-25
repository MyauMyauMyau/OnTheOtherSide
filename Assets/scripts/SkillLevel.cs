using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class SkillLevel : MonoBehaviour
{

	public int SkillNumber;
	public int HeroNumber;
	public static Sprite Level1Sprite;
	public static Sprite Level2Sprite;
	public static Sprite Level3Sprite;
	// Use this for initialization
	public void Reload()
	{
		Start();
	}
	void Start ()
	{
		var level = int.Parse(PlayerPrefs.GetString("Skills" + HeroNumber).ElementAt(SkillNumber - 1).ToString());
		var image = GetComponent<Image>();
		if (image.enabled == false && level > 0)
			image.enabled = true;
		if (level == 1)
			image.overrideSprite = Level1Sprite;
		if (level == 2)
			image.overrideSprite = Level2Sprite;
		if (level == 3)
			image.overrideSprite = Level3Sprite;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
