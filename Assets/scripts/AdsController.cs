using System;
using System.Collections.Generic;
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

		void Start()
		{
			text = GameObject.Find("AdButton").GetComponentInChildren<Text>();
		}
		private RevMobBanner banner;
		public void ShowRewardedVideo()
		{
			rewardedVideo = revmob.CreateRewardedVideo();
		}

		public void StopShowingRewardedVideo()
		{
			rewardedVideo.Release();			
		}
		public void SessionIsStarted()
		{			
		}

		public void SessionNotStarted(string message)
		{
			text.text = "session not started" + Time.time;
		}

		public void AdDidReceive(string revMobAdType)
		{
		}

		public void AdDidFail(string revMobAdType)
		{
			text.text = "fail" + Time.time;
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
			throw new NotImplementedException();
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
			if (rewardedVideo != null) rewardedVideo.ShowRewardedVideo();
			text.text = "video loaded full";
		}

		public void RewardedVideoNotCompletelyLoaded()
		{
			text.text = "not full loaded";
			if (rewardedVideo != null) rewardedVideo.ShowRewardedVideo();
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
		}

		public void RewardedPreRollDisplayed()
		{
			text.text = "preroll";
		}
	}
}