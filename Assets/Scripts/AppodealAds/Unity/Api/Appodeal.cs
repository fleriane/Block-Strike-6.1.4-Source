using System;
using System.Collections.Generic;
using AppodealAds.Unity.Common;

namespace AppodealAds.Unity.Api
{
	// Token: 0x02000002 RID: 2
	public class Appodeal
	{
		// Token: 0x06000002 RID: 2 RVA: 0x0000473C File Offset: 0x0000293C
		private static IAppodealAdsClient getInstance()
		{
			if (Appodeal.client == null)
			{
				Appodeal.client = AppodealAdsClientFactory.GetAppodealAdsClient();
			}
			return Appodeal.client;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00004757 File Offset: 0x00002957
		public static void initialize(string appKey, int adTypes)
		{
			Appodeal.getInstance().initialize(appKey, adTypes);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00004765 File Offset: 0x00002965
		public static void initialize(string appKey, int adTypes, bool hasConsent)
		{
			Appodeal.getInstance().initialize(appKey, adTypes, hasConsent);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00004774 File Offset: 0x00002974
		public static bool show(int adTypes)
		{
			return Appodeal.getInstance().show(adTypes);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00004781 File Offset: 0x00002981
		public static bool show(int adTypes, string placement)
		{
			return Appodeal.getInstance().show(adTypes, placement);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000478F File Offset: 0x0000298F
		public static bool showBannerView(int YAxis, int XGravity, string placement)
		{
			return Appodeal.getInstance().showBannerView(YAxis, XGravity, placement);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000479E File Offset: 0x0000299E
		public static bool showMrecView(int YAxis, int XGravity, string placement)
		{
			return Appodeal.getInstance().showMrecView(YAxis, XGravity, placement);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000047AD File Offset: 0x000029AD
		public static bool isLoaded(int adTypes)
		{
			return Appodeal.getInstance().isLoaded(adTypes);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000047BA File Offset: 0x000029BA
		public static void cache(int adTypes)
		{
			Appodeal.getInstance().cache(adTypes);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000047C7 File Offset: 0x000029C7
		public static void hide(int adTypes)
		{
			Appodeal.getInstance().hide(adTypes);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000047D4 File Offset: 0x000029D4
		public static void hideBannerView()
		{
			Appodeal.getInstance().hideBannerView();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000047E0 File Offset: 0x000029E0
		public static void hideMrecView()
		{
			Appodeal.getInstance().hideMrecView();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000047EC File Offset: 0x000029EC
		public static void setAutoCache(int adTypes, bool autoCache)
		{
			Appodeal.getInstance().setAutoCache(adTypes, autoCache);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000047FA File Offset: 0x000029FA
		public static bool isPrecache(int adTypes)
		{
			return Appodeal.getInstance().isPrecache(adTypes);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00004807 File Offset: 0x00002A07
		public static void onResume()
		{
			Appodeal.getInstance().onResume();
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00004813 File Offset: 0x00002A13
		public static void setSmartBanners(bool value)
		{
			Appodeal.getInstance().setSmartBanners(value);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00004820 File Offset: 0x00002A20
		public static void setBannerBackground(bool value)
		{
			Appodeal.getInstance().setBannerBackground(value);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000482D File Offset: 0x00002A2D
		public static void setBannerAnimation(bool value)
		{
			Appodeal.getInstance().setBannerAnimation(value);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000483A File Offset: 0x00002A3A
		public static void setTabletBanners(bool value)
		{
			Appodeal.getInstance().setTabletBanners(value);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00004847 File Offset: 0x00002A47
		public static void setTesting(bool test)
		{
			Appodeal.getInstance().setTesting(test);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00004854 File Offset: 0x00002A54
		public static void setLogLevel(Appodeal.LogLevel log)
		{
			Appodeal.getInstance().setLogLevel(log);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00004861 File Offset: 0x00002A61
		public static void setChildDirectedTreatment(bool value)
		{
			Appodeal.getInstance().setChildDirectedTreatment(value);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000486E File Offset: 0x00002A6E
		public static void disableNetwork(string network)
		{
			Appodeal.getInstance().disableNetwork(network);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000487B File Offset: 0x00002A7B
		public static void disableNetwork(string network, int adType)
		{
			Appodeal.getInstance().disableNetwork(network, adType);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00004889 File Offset: 0x00002A89
		public static void disableLocationPermissionCheck()
		{
			Appodeal.getInstance().disableLocationPermissionCheck();
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00004895 File Offset: 0x00002A95
		public static void disableWriteExternalStoragePermissionCheck()
		{
			Appodeal.getInstance().disableWriteExternalStoragePermissionCheck();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000048A1 File Offset: 0x00002AA1
		public static void setTriggerOnLoadedOnPrecache(int adTypes, bool onLoadedTriggerBoth)
		{
			Appodeal.getInstance().setTriggerOnLoadedOnPrecache(adTypes, onLoadedTriggerBoth);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000048AF File Offset: 0x00002AAF
		public static void muteVideosIfCallsMuted(bool value)
		{
			Appodeal.getInstance().muteVideosIfCallsMuted(value);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000048BC File Offset: 0x00002ABC
		public static void showTestScreen()
		{
			Appodeal.getInstance().showTestScreen();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000048C8 File Offset: 0x00002AC8
		public static bool canShow(int adTypes)
		{
			return Appodeal.getInstance().canShow(adTypes);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000048D5 File Offset: 0x00002AD5
		public static bool canShow(int adTypes, string placement)
		{
			return Appodeal.getInstance().canShow(adTypes, placement);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000048E3 File Offset: 0x00002AE3
		public static void setSegmentFilter(string name, bool value)
		{
			Appodeal.getInstance().setSegmentFilter(name, value);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000048F1 File Offset: 0x00002AF1
		public static void setSegmentFilter(string name, int value)
		{
			Appodeal.getInstance().setSegmentFilter(name, value);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000048FF File Offset: 0x00002AFF
		public static void setSegmentFilter(string name, double value)
		{
			Appodeal.getInstance().setSegmentFilter(name, value);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000490D File Offset: 0x00002B0D
		public static void setSegmentFilter(string name, string value)
		{
			Appodeal.getInstance().setSegmentFilter(name, value);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000491B File Offset: 0x00002B1B
		public static void setExtraData(string key, bool value)
		{
			Appodeal.getInstance().setExtraData(key, value);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00004929 File Offset: 0x00002B29
		public static void setExtraData(string key, int value)
		{
			Appodeal.getInstance().setExtraData(key, value);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00004937 File Offset: 0x00002B37
		public static void setExtraData(string key, double value)
		{
			Appodeal.getInstance().setExtraData(key, value);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00004945 File Offset: 0x00002B45
		public static void setExtraData(string key, string value)
		{
			Appodeal.getInstance().setExtraData(key, value);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00004953 File Offset: 0x00002B53
		public static void trackInAppPurchase(double amount, string currency)
		{
			Appodeal.getInstance().trackInAppPurchase(amount, currency);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00004961 File Offset: 0x00002B61
		public static string getNativeSDKVersion()
		{
			return Appodeal.getInstance().getVersion();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000496D File Offset: 0x00002B6D
		public static string getPluginVersion()
		{
			return "2.8.49";
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00004974 File Offset: 0x00002B74
		public static void setInterstitialCallbacks(IInterstitialAdListener listener)
		{
			Appodeal.getInstance().setInterstitialCallbacks(listener);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00004981 File Offset: 0x00002B81
		public static void setNonSkippableVideoCallbacks(INonSkippableVideoAdListener listener)
		{
			Appodeal.getInstance().setNonSkippableVideoCallbacks(listener);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0000498E File Offset: 0x00002B8E
		public static void setRewardedVideoCallbacks(IRewardedVideoAdListener listener)
		{
			Appodeal.getInstance().setRewardedVideoCallbacks(listener);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000499B File Offset: 0x00002B9B
		public static void setBannerCallbacks(IBannerAdListener listener)
		{
			Appodeal.getInstance().setBannerCallbacks(listener);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000049A8 File Offset: 0x00002BA8
		public static void setMrecCallbacks(IMrecAdListener listener)
		{
			Appodeal.getInstance().setMrecCallbacks(listener);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000049B5 File Offset: 0x00002BB5
		public static void requestAndroidMPermissions(IPermissionGrantedListener listener)
		{
			Appodeal.getInstance().requestAndroidMPermissions(listener);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000049C2 File Offset: 0x00002BC2
		public static KeyValuePair<string, double> getRewardParameters()
		{
			return new KeyValuePair<string, double>(Appodeal.getInstance().getRewardCurrency(), Appodeal.getInstance().getRewardAmount());
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000049DD File Offset: 0x00002BDD
		public static KeyValuePair<string, double> getRewardParameters(string placement)
		{
			return new KeyValuePair<string, double>(Appodeal.getInstance().getRewardCurrency(placement), Appodeal.getInstance().getRewardAmount(placement));
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000049FA File Offset: 0x00002BFA
		public static double getPredictedEcpm(int adType)
		{
			return Appodeal.getInstance().getPredictedEcpm(adType);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00004A07 File Offset: 0x00002C07
		public static void destroy(int adTypes)
		{
			Appodeal.getInstance().destroy(adTypes);
		}

		// Token: 0x04000001 RID: 1
		public const int NONE = 0;

		// Token: 0x04000002 RID: 2
		public const int INTERSTITIAL = 3;

		// Token: 0x04000003 RID: 3
		public const int BANNER = 4;

		// Token: 0x04000004 RID: 4
		public const int BANNER_BOTTOM = 8;

		// Token: 0x04000005 RID: 5
		public const int BANNER_TOP = 16;

		// Token: 0x04000006 RID: 6
		public const int BANNER_VIEW = 64;

		// Token: 0x04000007 RID: 7
		public const int MREC = 512;

		// Token: 0x04000008 RID: 8
		public const int REWARDED_VIDEO = 128;

		// Token: 0x04000009 RID: 9
		public const int NON_SKIPPABLE_VIDEO = 128;

		// Token: 0x0400000A RID: 10
		public const int BANNER_HORIZONTAL_SMART = -1;

		// Token: 0x0400000B RID: 11
		public const int BANNER_HORIZONTAL_CENTER = -2;

		// Token: 0x0400000C RID: 12
		public const int BANNER_HORIZONTAL_RIGHT = -3;

		// Token: 0x0400000D RID: 13
		public const int BANNER_HORIZONTAL_LEFT = -4;

		// Token: 0x0400000E RID: 14
		public const string APPODEAL_PLUGIN_VERSION = "2.8.49";

		// Token: 0x0400000F RID: 15
		private static IAppodealAdsClient client;

		// Token: 0x02000003 RID: 3
		public enum LogLevel
		{
			// Token: 0x04000011 RID: 17
			None,
			// Token: 0x04000012 RID: 18
			Debug,
			// Token: 0x04000013 RID: 19
			Verbose
		}
	}
}
