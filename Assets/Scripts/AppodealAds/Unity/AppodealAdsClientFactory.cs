using System;
using AppodealAds.Unity.Android;
using AppodealAds.Unity.Common;

namespace AppodealAds.Unity
{
	// Token: 0x02000017 RID: 23
	internal class AppodealAdsClientFactory
	{
		// Token: 0x06000104 RID: 260 RVA: 0x000054B5 File Offset: 0x000036B5
		internal static IAppodealAdsClient GetAppodealAdsClient()
		{
			return new AndroidAppodealClient();
		}
	}
}
