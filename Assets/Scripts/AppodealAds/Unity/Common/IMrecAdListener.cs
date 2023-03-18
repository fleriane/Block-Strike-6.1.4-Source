using System;

namespace AppodealAds.Unity.Common
{
	// Token: 0x0200000A RID: 10
	public interface IMrecAdListener
	{
		// Token: 0x06000081 RID: 129
		void onMrecLoaded(bool isPrecache);

		// Token: 0x06000082 RID: 130
		void onMrecFailedToLoad();

		// Token: 0x06000083 RID: 131
		void onMrecShown();

		// Token: 0x06000084 RID: 132
		void onMrecClicked();

		// Token: 0x06000085 RID: 133
		void onMrecExpired();
	}
}
