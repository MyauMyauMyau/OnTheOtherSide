using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.scripts;
using Facebook.Unity;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FacebookSharer : MonoBehaviour
{

	public static FacebookSharer Instance;
	// Use this for initialization
	void Start ()
	{
		
	}

	// Awake function from Unity's MonoBehavior
	void Awake()
	{
		Instance = this;
		if (!FB.IsInitialized)
		{
			// Initialize the Facebook SDK
			FB.Init(InitCallback, OnHideUnity);
		}
		else {
			// Already initialized, signal an app activation App Event
			FB.ActivateApp();
		}
		DontDestroyOnLoad(this);
	}

	private void InitCallback()
	{
		if (FB.IsInitialized)
		{
			// Signal an app activation App Event
			FB.ActivateApp();
			// Continue with Facebook SDK
			// ...
		}
		else {
			Debug.Log("Failed to Initialize the Facebook SDK");
		}
	}

	private void OnHideUnity(bool isGameShown)
	{
		if (!isGameShown)
		{
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		}
		else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
	private void AuthCallback(ILoginResult result)
	{
		if (FB.IsLoggedIn)
		{
			// AccessToken class will have session details
			var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
			// Print current access token's User ID
			Debug.Log(aToken.UserId);
			// Print current access token's granted permissions
			foreach (string perm in aToken.Permissions)
			{
				Debug.Log(perm);
			}
		}
		else {
			Debug.Log("User cancelled login");
		}
	}

	public void ShareYourResult(int levels)
	{
		if (!FB.IsLoggedIn)
		{
			var perms = new List<string>() { "public_profile", "email", "user_friends" };
			FB.LogInWithReadPermissions(perms, AuthCallback);
		}
		else
		{
			Debug.Log("onShareClicked");
			FB.FeedShare(
					linkCaption: "+3 Gold incoming^^",
					picture: new Uri("http://cs626117.vk.me/v626117734/17006/J4QSS-OcPJ8.jpg"),
					linkName: "Play Monster Saga On Android!",
					link: new Uri("http://apps.facebook.com/"),
					linkDescription: "Yeah! I've completed " + levels + " levels!",
					mediaSource: "Meow!",
					callback: FeedCallBack
					);
		}
	}

	public GameObject RewardText;
	private void FeedCallBack(IShareResult result)
	{
		if (!result.Cancelled && result.Error == null)
		{
			PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") + 3);
			var flyingText = ((GameObject)Instantiate(
						RewardText, new Vector3(Screen.width / 2, Screen.height / 2),
						Quaternion.Euler(new Vector3())))
						.GetComponent<FlyingText>();
			
			if (SceneManager.GetActiveScene().name == "game")
				flyingText.transform.parent = GameObject.Find("UICanvas").transform;
			else
			{
				flyingText.transform.parent = GameObject.Find("MainMenu").transform;
			}
			flyingText.GetComponent<Text>().text = "+3 Gold!";
			PlayerPrefs.Save();
		}	
	}																											   
	public void PostFacebook()
	{
		if (!FB.IsLoggedIn)
		{
			var perms = new List<string>() { "public_profile", "email", "user_friends" };
			FB.LogInWithReadPermissions(perms, AuthCallback);
		}
		else
		{
			Debug.Log("onShareClicked");
			FB.FeedShare(
					linkCaption: "Play new game!",
					picture: new Uri("http://cs626117.vk.me/v626117734/17006/J4QSS-OcPJ8.jpg"),
					linkName: "Play Monster Saga On Android!",
					link: new Uri("http://apps.facebook.com/"),
					linkDescription: "There are a lot of larch trees around here, aren't there?",
					mediaSource: "Meow!",
					callback: FeedCallBack
					);
		}
	}

}
