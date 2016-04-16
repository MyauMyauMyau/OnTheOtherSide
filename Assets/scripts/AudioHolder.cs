using UnityEngine;
using System.Collections;

public class AudioHolder : MonoBehaviour
{

	public AudioClip Bomb;
	public AudioClip Button;
	public AudioClip Cocoon;
	public AudioClip Electricity;
	public AudioClip FireBall;
	public AudioClip Lose;
	public AudioClip MainTheme;
	public AudioClip MassRemove;
	public AudioClip Remove;
	public AudioClip SwapMonster;
	public AudioClip Win;
	public static AudioHolder instance;
	// Use this for initialization
	void Start ()
	{
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void PlayBomb()
	{

		instance.GetComponent<AudioSource>().PlayOneShot(instance.Bomb);

	}
	public static void PlayFireBall()
	{
		instance.GetComponent<AudioSource>().PlayOneShot(instance.FireBall);

	}
	public static void PlayElectricity()
	{
		instance.GetComponent<AudioSource>().PlayOneShot(instance.Electricity);

	}
	public static void PlayRemove()
	{
		instance.GetComponent<AudioSource>().PlayOneShot(instance.Remove, 0.25f);

	}
	public static void PlayMassRemove()
	{
		instance.GetComponent<AudioSource>().PlayOneShot(instance.MassRemove, 0.25f);

	}
	public static void PlayLose()
	{
		instance.GetComponent<AudioSource>().PlayOneShot(instance.Lose);

	}
	public static void PlaySwapMonsters()
	{
		instance.GetComponent<AudioSource>().PlayOneShot(instance.SwapMonster);

	}
	public static void PlayWin()
	{
		instance.GetComponent<AudioSource>().PlayOneShot(instance.Win);
	}
	public static void PlayButton()
	{
		instance.GetComponent<AudioSource>().PlayOneShot(instance.Button);
	}
	public static void PlayCocoon()
	{
		instance.GetComponent<AudioSource>().PlayOneShot(instance.Cocoon, 0.25f);
	}
}
