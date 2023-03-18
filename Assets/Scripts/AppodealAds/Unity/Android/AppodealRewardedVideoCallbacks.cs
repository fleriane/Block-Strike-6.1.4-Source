using System;
using AppodealAds.Unity.Common;
using UnityEngine;

namespace AppodealAds.Unity.Android
{
	// Token: 0x02000015 RID: 21
	public class AppodealRewardedVideoCallbacks : AndroidJavaProxy
	{
		// Token: 0x060000F5 RID: 245 RVA: 0x000053EB File Offset: 0x000035EB
		internal AppodealRewardedVideoCallbacks(IRewardedVideoAdListener listener) : base("com.appodeal.ads.RewardedVideoCallbacks")
		{
			this.listener = listener;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000053FF File Offset: 0x000035FF
		private void onRewardedVideoLoaded(bool precache)
		{
			this.listener.onRewardedVideoLoaded(precache);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000540D File Offset: 0x0000360D
		private void onRewardedVideoFailedToLoad()
		{
			this.listener.onRewardedVideoFailedToLoad();
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000541A File Offset: 0x0000361A
		private void onRewardedVideoShown()
		{
			this.listener.onRewardedVideoShown();
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00005427 File Offset: 0x00003627
		private void onRewardedVideoFinished(double amount, AndroidJavaObject name)
		{
			this.listener.onRewardedVideoFinished(amount, null);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00005436 File Offset: 0x00003636
		private void onRewardedVideoFinished(double amount, string name)
		{
			this.listener.onRewardedVideoFinished(amount, name);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00005445 File Offset: 0x00003645
		private void onRewardedVideoClosed(bool finished)
		{
			this.listener.onRewardedVideoClosed(finished);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00005453 File Offset: 0x00003653
		private void onRewardedVideoExpired()
		{
			this.listener.onRewardedVideoExpired();
		}

		// Token: 0x04000035 RID: 53
		private IRewardedVideoAdListener listener;
	}
}
