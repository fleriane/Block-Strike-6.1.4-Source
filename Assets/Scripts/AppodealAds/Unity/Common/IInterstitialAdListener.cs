using System;

namespace AppodealAds.Unity.Common
{
	// Token: 0x02000009 RID: 9
	public interface IInterstitialAdListener
	{
		// Token: 0x0600007B RID: 123
		void onInterstitialLoaded(bool isPrecache);

		// Token: 0x0600007C RID: 124
		void onInterstitialFailedToLoad();

		// Token: 0x0600007D RID: 125
		void onInterstitialShown();

		// Token: 0x0600007E RID: 126
		void onInterstitialClosed();

		// Token: 0x0600007F RID: 127
		void onInterstitialClicked();

		// Token: 0x06000080 RID: 128
		void onInterstitialExpired();
	}
}
