using System;

namespace AppodealAds.Unity.Common
{
	// Token: 0x02000008 RID: 8
	public interface IBannerAdListener
	{
		// Token: 0x06000076 RID: 118
		void onBannerLoaded(bool isPrecache);

		// Token: 0x06000077 RID: 119
		void onBannerFailedToLoad();

		// Token: 0x06000078 RID: 120
		void onBannerShown();

		// Token: 0x06000079 RID: 121
		void onBannerClicked();

		// Token: 0x0600007A RID: 122
		void onBannerExpired();
	}
}
