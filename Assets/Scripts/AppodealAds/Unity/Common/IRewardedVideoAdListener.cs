using System;

namespace AppodealAds.Unity.Common
{
	// Token: 0x0200000D RID: 13
	public interface IRewardedVideoAdListener
	{
		// Token: 0x0600008E RID: 142
		void onRewardedVideoLoaded(bool precache);

		// Token: 0x0600008F RID: 143
		void onRewardedVideoFailedToLoad();

		// Token: 0x06000090 RID: 144
		void onRewardedVideoShown();

		// Token: 0x06000091 RID: 145
		void onRewardedVideoFinished(double amount, string name);

		// Token: 0x06000092 RID: 146
		void onRewardedVideoClosed(bool finished);

		// Token: 0x06000093 RID: 147
		void onRewardedVideoExpired();
	}
}
