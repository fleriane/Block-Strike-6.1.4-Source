using System;

namespace AppodealAds.Unity.Common
{
	// Token: 0x0200000E RID: 14
	public interface ISkippableVideoAdListener
	{
		// Token: 0x06000094 RID: 148
		void onSkippableVideoLoaded();

		// Token: 0x06000095 RID: 149
		void onSkippableVideoFailedToLoad();

		// Token: 0x06000096 RID: 150
		void onSkippableVideoShown();

		// Token: 0x06000097 RID: 151
		void onSkippableVideoFinished();

		// Token: 0x06000098 RID: 152
		void onSkippableVideoClosed();
	}
}
