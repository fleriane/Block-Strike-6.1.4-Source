using System;
using AppodealAds.Unity.Api;

namespace AppodealAds.Unity.Common
{
	// Token: 0x02000007 RID: 7
	public interface IAppodealAdsClient
	{
		// Token: 0x0600003D RID: 61
		void initialize(string appKey, int type);

		// Token: 0x0600003E RID: 62
		void initialize(string appKey, int type, bool hasConsent);

		// Token: 0x0600003F RID: 63
		bool isInitialized(int adType);

		// Token: 0x06000040 RID: 64
		bool show(int adTypes);

		// Token: 0x06000041 RID: 65
		bool show(int adTypes, string placement);

		// Token: 0x06000042 RID: 66
		bool isLoaded(int adTypes);

		// Token: 0x06000043 RID: 67
		void cache(int adTypes);

		// Token: 0x06000044 RID: 68
		void hide(int adTypes);

		// Token: 0x06000045 RID: 69
		void setAutoCache(int adTypes, bool autoCache);

		// Token: 0x06000046 RID: 70
		bool isPrecache(int adTypes);

		// Token: 0x06000047 RID: 71
		void onResume();

		// Token: 0x06000048 RID: 72
		bool showBannerView(int YAxis, int XGravity, string Placement);

		// Token: 0x06000049 RID: 73
		bool showMrecView(int YAxis, int XGravity, string Placement);

		// Token: 0x0600004A RID: 74
		void hideBannerView();

		// Token: 0x0600004B RID: 75
		void hideMrecView();

		// Token: 0x0600004C RID: 76
		void setSmartBanners(bool value);

		// Token: 0x0600004D RID: 77
		void setBannerAnimation(bool value);

		// Token: 0x0600004E RID: 78
		void setBannerBackground(bool value);

		// Token: 0x0600004F RID: 79
		void setTabletBanners(bool value);

		// Token: 0x06000050 RID: 80
		void setTesting(bool test);

		// Token: 0x06000051 RID: 81
		void setLogLevel(Appodeal.LogLevel level);

		// Token: 0x06000052 RID: 82
		void setChildDirectedTreatment(bool value);

		// Token: 0x06000053 RID: 83
		void disableNetwork(string network);

		// Token: 0x06000054 RID: 84
		void disableNetwork(string network, int type);

		// Token: 0x06000055 RID: 85
		void disableLocationPermissionCheck();

		// Token: 0x06000056 RID: 86
		void disableWriteExternalStoragePermissionCheck();

		// Token: 0x06000057 RID: 87
		void muteVideosIfCallsMuted(bool value);

		// Token: 0x06000058 RID: 88
		void showTestScreen();

		// Token: 0x06000059 RID: 89
		string getVersion();

		// Token: 0x0600005A RID: 90
		bool canShow(int adTypes);

		// Token: 0x0600005B RID: 91
		bool canShow(int adTypes, string placement);

		// Token: 0x0600005C RID: 92
		void setSegmentFilter(string name, bool value);

		// Token: 0x0600005D RID: 93
		void setSegmentFilter(string name, int value);

		// Token: 0x0600005E RID: 94
		void setSegmentFilter(string name, double value);

		// Token: 0x0600005F RID: 95
		void setSegmentFilter(string name, string value);

		// Token: 0x06000060 RID: 96
		void setExtraData(string key, bool value);

		// Token: 0x06000061 RID: 97
		void setExtraData(string key, int value);

		// Token: 0x06000062 RID: 98
		void setExtraData(string key, double value);

		// Token: 0x06000063 RID: 99
		void setExtraData(string key, string value);

		// Token: 0x06000064 RID: 100
		string getRewardCurrency(string placement);

		// Token: 0x06000065 RID: 101
		double getRewardAmount(string placement);

		// Token: 0x06000066 RID: 102
		string getRewardCurrency();

		// Token: 0x06000067 RID: 103
		double getRewardAmount();

		// Token: 0x06000068 RID: 104
		double getPredictedEcpm(int adTypes);

		// Token: 0x06000069 RID: 105
		void setTriggerOnLoadedOnPrecache(int adTypes, bool onLoadedTriggerBoth);

		// Token: 0x0600006A RID: 106
		void getUserSettings();

		// Token: 0x0600006B RID: 107
		void setAge(int age);

		// Token: 0x0600006C RID: 108
		void setGender(UserSettings.Gender gender);

		// Token: 0x0600006D RID: 109
		void setUserId(string id);

		// Token: 0x0600006E RID: 110
		void trackInAppPurchase(double amount, string currency);

		// Token: 0x0600006F RID: 111
		void setInterstitialCallbacks(IInterstitialAdListener listener);

		// Token: 0x06000070 RID: 112
		void setNonSkippableVideoCallbacks(INonSkippableVideoAdListener listener);

		// Token: 0x06000071 RID: 113
		void setRewardedVideoCallbacks(IRewardedVideoAdListener listener);

		// Token: 0x06000072 RID: 114
		void setBannerCallbacks(IBannerAdListener listener);

		// Token: 0x06000073 RID: 115
		void setMrecCallbacks(IMrecAdListener listener);

		// Token: 0x06000074 RID: 116
		void requestAndroidMPermissions(IPermissionGrantedListener listener);

		// Token: 0x06000075 RID: 117
		void destroy(int adTypes);
	}
}
