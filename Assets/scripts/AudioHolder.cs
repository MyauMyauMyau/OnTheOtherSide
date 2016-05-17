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
	public static AudioHolder Instance;
	// Use this for initialization
	void Start ()
	{
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void PlayBomb()
	{

		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.Bomb);

	}
	public static void PlayFireBall()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.FireBall);

	}
	public static void PlayElectricity()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.Electricity);

	}
	public static void PlayRemove()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.Remove, 0.25f);

	}
	public static void PlayMassRemove()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.MassRemove, 0.25f);

	}
	public static void PlayLose()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.Lose);

	}
	public static void PlaySwapMonsters()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.SwapMonster);

	}
	public static void PlayWin()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.Win);
	}
	public static void PlayButton()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.Button);
	}
	public static void PlayCocoon()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.Cocoon, 0.25f);
	}
}
