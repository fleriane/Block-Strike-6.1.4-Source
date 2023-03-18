using System;

namespace AppodealAds.Unity.Common
{
	// Token: 0x0200000B RID: 11
	public interface INonSkippableVideoAdListener
	{
		// Token: 0x06000086 RID: 134
		void onNonSkippableVideoLoaded(bool isPrecache);

		// Token: 0x06000087 RID: 135
		void onNonSkippableVideoFailedToLoad();

		// Token: 0x06000088 RID: 136
		void onNonSkippableVideoShown();

		// Token: 0x06000089 RID: 137
		void onNonSkippableVideoFinished();

		// Token: 0x0600008A RID: 138
		void onNonSkippableVideoClosed(bool finished);

		// Token: 0x0600008B RID: 139
		void onNonSkippableVideoExpired();
	}
}
