using System;
using AppodealAds.Unity.Common;
using UnityEngine;

namespace AppodealAds.Unity.Android
{
	// Token: 0x02000010 RID: 16
	public class AppodealBannerCallbacks : AndroidJavaProxy
	{
		// Token: 0x060000D8 RID: 216 RVA: 0x00005248 File Offset: 0x00003448
		internal AppodealBannerCallbacks(IBannerAdListener listener) : base("com.appodeal.ads.BannerCallbacks")
		{
			this.listener = listener;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000525C File Offset: 0x0000345C
		private void onBannerLoaded(int height, bool isPrecache)
		{
			this.listener.onBannerLoaded(isPrecache);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000526A File Offset: 0x0000346A
		private void onBannerFailedToLoad()
		{
			this.listener.onBannerFailedToLoad();
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00005277 File Offset: 0x00003477
		private void onBannerShown()
		{
			this.listener.onBannerShown();
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005284 File Offset: 0x00003484
		private void onBannerClicked()
		{
			this.listener.onBannerClicked();
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00005291 File Offset: 0x00003491
		private void onBannerExpired()
		{
			this.listener.onBannerExpired();
		}

		// Token: 0x04000030 RID: 48
		private IBannerAdListener listener;
	}
}
