using System;
using AppodealAds.Unity.Common;
using UnityEngine;

namespace AppodealAds.Unity.Android
{
	// Token: 0x02000016 RID: 22
	public class AppodealSkippableVideoCallbacks : AndroidJavaProxy
	{
		// Token: 0x060000FD RID: 253 RVA: 0x00005460 File Offset: 0x00003660
		internal AppodealSkippableVideoCallbacks(ISkippableVideoAdListener listener) : base("com.appodeal.ads.SkippableVideoCallbacks")
		{
			this.listener = listener;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005474 File Offset: 0x00003674
		private void onSkippableVideoLoaded()
		{
			this.listener.onSkippableVideoLoaded();
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005481 File Offset: 0x00003681
		private void onSkippableVideoFailedToLoad()
		{
			this.listener.onSkippableVideoFailedToLoad();
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000548E File Offset: 0x0000368E
		private void onSkippableVideoShown()
		{
			this.listener.onSkippableVideoShown();
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000549B File Offset: 0x0000369B
		private void onSkippableVideoFinished()
		{
			this.listener.onSkippableVideoFinished();
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000054A8 File Offset: 0x000036A8
		private void onSkippableVideoClosed(bool finished)
		{
			this.listener.onSkippableVideoClosed();
		}

		// Token: 0x04000036 RID: 54
		private ISkippableVideoAdListener listener;
	}
}
