using System;
using AppodealAds.Unity.Common;
using UnityEngine;

namespace AppodealAds.Unity.Android
{
	// Token: 0x02000013 RID: 19
	public class AppodealNonSkippableVideoCallbacks : AndroidJavaProxy
	{
		// Token: 0x060000EB RID: 235 RVA: 0x00005357 File Offset: 0x00003557
		internal AppodealNonSkippableVideoCallbacks(INonSkippableVideoAdListener listener) : base("com.appodeal.ads.NonSkippableVideoCallbacks")
		{
			this.listener = listener;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000536B File Offset: 0x0000356B
		private void onNonSkippableVideoLoaded(bool isPrecache)
		{
			this.listener.onNonSkippableVideoLoaded(isPrecache);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005379 File Offset: 0x00003579
		private void onNonSkippableVideoFailedToLoad()
		{
			this.listener.onNonSkippableVideoFailedToLoad();
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00005386 File Offset: 0x00003586
		private void onNonSkippableVideoShown()
		{
			this.listener.onNonSkippableVideoShown();
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00005393 File Offset: 0x00003593
		private void onNonSkippableVideoFinished()
		{
			this.listener.onNonSkippableVideoFinished();
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000053A0 File Offset: 0x000035A0
		private void onNonSkippableVideoClosed(bool finished)
		{
			this.listener.onNonSkippableVideoClosed(finished);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000053AE File Offset: 0x000035AE
		private void onNonSkippableVideoExpired()
		{
			this.listener.onNonSkippableVideoExpired();
		}

		// Token: 0x04000033 RID: 51
		private INonSkippableVideoAdListener listener;
	}
}
