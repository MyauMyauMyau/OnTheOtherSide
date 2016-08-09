using UnityEngine;
using System.Collections;

public class AudioHolder : MonoBehaviour
{

	public AudioClip Bomb;
	public AudioClip Button;
	public AudioClip Cocoon;
	public AudioClip DeathElectricity;
	public AudioClip DeathFireBall;
	public AudioClip DeathWind;
	public AudioClip DeathIce;
	public AudioClip Lose;
	public AudioClip MainTheme;
	public AudioClip MassRemove;
	public AudioClip Remove;
	public AudioClip SwapMonster;
	public AudioClip Win;
	public AudioClip MenuTheme;			
	public AudioClip HunterShoot1;
	public AudioClip HunterShoot2;
	public AudioClip HunterMine;
	public AudioClip HunterBridge1;
	public AudioClip HunterBridge2;
	public AudioClip ClericExile1;
	public AudioClip ClericExile2;
	public AudioClip ClericFlag1;
	public AudioClip ClericFlag2;
	public AudioClip ClericPumpkin;
	public AudioClip VampireHypnosis;
	public AudioClip VampireHunger;
	public AudioClip VampireStick;
	public AudioClip VampireBloodDrop;
	public AudioClip WolfRoar1;
	public AudioClip WolfRoar2;
	public AudioClip WolfClutches;
	public AudioClip WolfHowl;
	public AudioClip MummyWait;
	public AudioClip MummyBeamDrop;
	public AudioClip MummyScarabs1;
	public AudioClip MummyScarabs2;
	public AudioClip MummySkullDrop;
	public static AudioHolder Instance;
	// Use this for initialization

	void Awake()
	{
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
			return;
		} else {
			Instance = this;
		}
		DontDestroyOnLoad(gameObject);
	}
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public static void PlayMenuTheme()
	{
		Instance.GetComponent<AudioSource>().clip = Instance.MenuTheme;
		Instance.GetComponent<AudioSource>().Play();

	}
	public static void PlayMainTheme()
	{
		if (Instance.GetComponent<AudioSource>().clip != Instance.MainTheme)
		{
			Instance.GetComponent<AudioSource>().clip = Instance.MainTheme;
			Instance.GetComponent<AudioSource>().Play();
		}
	}
	public static void PlayBomb()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.Bomb);
	}
	public static void PlayVampireHypnosis()
	{
		Instance.GetComponents<AudioSource>()[1].clip = Instance.VampireHypnosis;
		Instance.GetComponents<AudioSource>()[1].Play();
	}
	public static void PlayMummyWait()
	{
		Instance.GetComponents<AudioSource>()[1].clip = Instance.MummyWait;
		Instance.GetComponents<AudioSource>()[1].Play();
	}
	public static void StopPlayMummyWait()
	{
		Instance.GetComponents<AudioSource>()[1].Stop();
	}

	public static void PlayMummyScarabs1()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.MummyScarabs1);
	}
	public static void PlayMummyScarabs2()
	{
		Instance.GetComponents<AudioSource>()[1].PlayOneShot(Instance.MummyScarabs2);
	}
	public static void PlayMummyBeamDrop()
	{
		Instance.GetComponents<AudioSource>()[1].PlayOneShot(Instance.MummyBeamDrop);
	}
	public static void PlayMummySkullDrop()
	{
		Instance.GetComponents<AudioSource>()[1].PlayOneShot(Instance.MummySkullDrop);
	}
	public static void PlayWolfRoar1()
	{
		Instance.GetComponents<AudioSource>()[1].clip = Instance.WolfRoar1;
		Instance.GetComponents<AudioSource>()[1].Play();
	}

	public static void StopPlayWolfRoar1()
	{
		Instance.GetComponents<AudioSource>()[1].Stop();
	}
	public static void PlayWolfRoar2()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.WolfRoar2);
	}
	public static void PlayWolfClutches()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.WolfClutches);
	}
	public static void PlayWolfHowl()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.WolfHowl);
	}
	public static void StopPlayVampireHypnosis()
	{				  
		Instance.GetComponents<AudioSource>()[1].Stop();
	}
	public static void PlayClericExile1()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.ClericExile1);
	}
	public static void PlayVampireHunger()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.VampireHunger);
	}
	public static void PlayVampireStick()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.VampireStick);
	}
	public static void PlayVampireBloodDrop()
	{
		Instance.GetComponents<AudioSource>()[1].PlayOneShot(Instance.VampireBloodDrop);
	}
	public static void PlayClericExile2()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.ClericExile2);
	}

	public static void PlayClericFlag1()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.ClericFlag1);
	}
	public static void PlayClericFlag2()
	{
		Instance.GetComponents<AudioSource>()[1].PlayOneShot(Instance.ClericFlag2);
	}
	public static void PlayClericPumpkin()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.ClericPumpkin);
	}
	public static void PlayHunterShoot1()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.HunterShoot1);
	}
	public static void PlayHunterShoot2()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.HunterShoot2);

	}
	public static void PlayHunterMine()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.HunterMine);
	}
	public static void PlayHunterBridge1()
	{
		Instance.GetComponents<AudioSource>()[1].clip = Instance.HunterBridge1;
		Instance.GetComponents<AudioSource>()[1].Play();
	}
	public static void StopPlayHunterBridge1()
	{
		Instance.GetComponents<AudioSource>()[1].Stop();
	}
	public static void PlayHunterBridge2()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.HunterBridge2);
	}
	public static void PlayDeathFireBall()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.DeathFireBall);

	}
	public static void PlayDeathElectricity()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.DeathElectricity);

	}
	public static void PlayDeathWind()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.DeathWind);

	}
	public static void PlayDeathIce()
	{
		Instance.GetComponent<AudioSource>().PlayOneShot(Instance.DeathIce);

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
