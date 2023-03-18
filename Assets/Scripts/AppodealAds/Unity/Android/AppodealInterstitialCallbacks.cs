using System;
using AppodealAds.Unity.Common;
using UnityEngine;

namespace AppodealAds.Unity.Android
{
	// Token: 0x02000011 RID: 17
	public class AppodealInterstitialCallbacks : AndroidJavaProxy
	{
		// Token: 0x060000DE RID: 222 RVA: 0x0000529E File Offset: 0x0000349E
		internal AppodealInterstitialCallbacks(IInterstitialAdListener listener) : base("com.appodeal.ads.InterstitialCallbacks")
		{
			this.listener = listener;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000052B2 File Offset: 0x000034B2
		private void onInterstitialLoaded(bool isPrecache)
		{
			this.listener.onInterstitialLoaded(isPrecache);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000052C0 File Offset: 0x000034C0
		private void onInterstitialFailedToLoad()
		{
			this.listener.onInterstitialFailedToLoad();
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000052CD File Offset: 0x000034CD
		private void onInterstitialShown()
		{
			this.listener.onInterstitialShown();
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000052DA File Offset: 0x000034DA
		private void onInterstitialClicked()
		{
			this.listener.onInterstitialClicked();
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000052E7 File Offset: 0x000034E7
		private void onInterstitialClosed()
		{
			this.listener.onInterstitialClosed();
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000052F4 File Offset: 0x000034F4
		private void onInterstitialExpired()
		{
			this.listener.onInterstitialExpired();
		}

		// Token: 0x04000031 RID: 49
		private IInterstitialAdListener listener;
	}
}
