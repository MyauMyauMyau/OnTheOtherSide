using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts
{
	public class AdsController : MonoBehaviour, IRevMobListener
	{
		private Text text;
		private static readonly Dictionary<String, String> REVMOB_APP_IDS = new Dictionary<String, String>() {
			{ "Android", "57a48cf71897503f0bf50c0a"},
			{ "IOS", "57a4a85f952150510b857f93" }
		};
		private RevMob revmob;
		private RevMobFullscreen rewardedVideo;
		void Awake()
		{
			revmob = RevMob.Start(REVMOB_APP_IDS, "AdsHolder");
		}

		private float LoadingStartTime;
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape) && Loading != null)
				DeactivateLoading(true);
		}

		public GameObject StopLoadingText;
		private void DeactivateLoading(bool IsExplicit)
		{
			if (!IsExplicit)
			{
				var flyingText = ((GameObject)Instantiate(
						StopLoadingText, new Vector3(Screen.width / 2, Screen.height / 2),
						Quaternion.Euler(new Vector3())))
						.GetComponent<FlyingText>();
				flyingText.transform.parent = GameObject.Find("MainMenu").transform;
				flyingText.GetComponent<Text>().text = "No ads available!";
			}
			StopShowingRewardedVideo();
			Destroy(Loading.gameObject);
			Loading = null;
		}

		void Start()
		{
			text = GameObject.Find("AdButton").GetComponentInChildren<Text>();
		}
		public void ShowRewardedVideo()
		{
			rewardedVideo = revmob.CreateRewardedVideo();
			ShowLoading();
		}

		private GameObject Loading;
		private void ShowLoading()
		{
			var loadingIconPrefab = Resources.Load("objects/loading/Loading", typeof(GameObject)) as GameObject;
			Loading = ((GameObject)Instantiate(
				loadingIconPrefab, new Vector3(Screen.width / 2, Screen.height / 2),
				Quaternion.Euler(new Vector3())));
			Loading.transform.parent = GameObject.Find("MainMenu").transform;
			Loading.transform.localScale = new Vector3(1, 1);
		}

		public void StopShowingRewardedVideo()
		{
			if (rewardedVideo != null) rewardedVideo.Release();			
		}
		public void SessionIsStarted()
		{			
		}

		public void SessionNotStarted(string message)
		{
		}

		public void AdDidReceive(string revMobAdType)
		{
		}

		public void AdDidFail(string revMobAdType)
		{
			DeactivateLoading(false);
		}

		public void AdDisplayed(string revMobAdType)
		{
		}

		public void UserClickedInTheAd(string revMobAdType)
		{
		}

		public void UserClosedTheAd(string revMobAdType)
		{
		}

		public void VideoLoaded()
		{
			throw new NotImplementedException();
		}

		public void VideoNotCompletelyLoaded()
		{
			DeactivateLoading(false);
		}

		public void VideoStarted()
		{
			throw new NotImplementedException();
		}

		public void VideoFinished()
		{
			throw new NotImplementedException();
		}

		public void RewardedVideoLoaded()
		{
			if (rewardedVideo != null)
			{
				DeactivateLoading(true);
				rewardedVideo.ShowRewardedVideo();
			}
		}

		public void RewardedVideoNotCompletelyLoaded()
		{
			DeactivateLoading(false);
		}

		public void RewardedVideoStarted()
		{
		}

		public void RewardedVideoFinished()
		{
		}

		public void RewardedVideoCompleted()
		{
			PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") + 1);
			var flyingText = ((GameObject)Instantiate(
						StopLoadingText, new Vector3(Screen.width / 2, Screen.height / 2),
						Quaternion.Euler(new Vector3())))
						.GetComponent<FlyingText>();
			flyingText.transform.parent = GameObject.Find("MainMenu").transform;
			flyingText.GetComponent<Text>().text = "+1 Gold!";
		}

		public void RewardedPreRollDisplayed()
		{
			text.text = "preroll";
		}
	}
}