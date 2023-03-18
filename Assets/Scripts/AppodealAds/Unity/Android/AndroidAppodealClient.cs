using System;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using UnityEngine;

namespace AppodealAds.Unity.Android
{
	// Token: 0x0200000F RID: 15
	public class AndroidAppodealClient : IAppodealAdsClient
	{
		// Token: 0x0600009A RID: 154 RVA: 0x00021CB8 File Offset: 0x0001FEB8
		private int nativeAdTypesForType(int adTypes)
		{
			int num = 0;
			if ((adTypes & 3) > 0)
			{
				num |= 3;
			}
			if ((adTypes & 4) > 0)
			{
				num |= 4;
			}
			if ((adTypes & 64) > 0)
			{
				num |= 64;
			}
			if ((adTypes & 16) > 0)
			{
				num |= 16;
			}
			if ((adTypes & 8) > 0)
			{
				num |= 8;
			}
			if ((adTypes & 512) > 0)
			{
				num |= 256;
			}
			if ((adTypes & 128) > 0)
			{
				num |= 128;
			}
			if ((adTypes & 128) > 0)
			{
				num |= 128;
			}
			return num;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004A77 File Offset: 0x00002C77
		public AndroidJavaClass getAppodealClass()
		{
			if (this.appodealClass == null)
			{
				this.appodealClass = new AndroidJavaClass("com.appodeal.ads.Appodeal");
			}
			return this.appodealClass;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004A9A File Offset: 0x00002C9A
		public AndroidJavaClass getAppodealUnityClass()
		{
			if (this.appodealUnityClass == null)
			{
				this.appodealUnityClass = new AndroidJavaClass("com.appodeal.unity.AppodealUnity");
			}
			return this.appodealUnityClass;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004ABD File Offset: 0x00002CBD
		public AndroidJavaObject getAppodealBannerInstance()
		{
			if (this.appodealBannerInstance == null)
			{
				this.appodealBannerClass = new AndroidJavaClass("com.appodeal.unity.AppodealUnityBannerView");
				this.appodealBannerInstance = this.appodealBannerClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
			}
			return this.appodealBannerInstance;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00021D4C File Offset: 0x0001FF4C
		public AndroidJavaObject getActivity()
		{
			if (this.activity == null)
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				this.activity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			}
			return this.activity;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004AFC File Offset: 0x00002CFC
		public void initialize(string appKey, int adTypes)
		{
			this.initialize(appKey, adTypes, true);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00021D88 File Offset: 0x0001FF88
		public void initialize(string appKey, int adTypes, bool hasConsent)
		{
			this.disableNetwork("mobvista");
			this.getAppodealClass().CallStatic("setFramework", new object[]
			{
				"unity",
				Appodeal.getPluginVersion(),
				Application.unityVersion
			});
			if ((adTypes & 64) > 0 || (adTypes & 512) > 0)
			{
				this.getAppodealClass().CallStatic("setFramework", new object[]
				{
					"unity",
					Appodeal.getPluginVersion(),
					Application.unityVersion,
					false,
					false
				});
				this.getAppodealClass().CallStatic("disableNetwork", new object[]
				{
					this.getActivity(),
					"amazon_ads",
					4
				});
			}
			this.getAppodealClass().CallStatic("initialize", new object[]
			{
				this.getActivity(),
				appKey,
				this.nativeAdTypesForType(adTypes),
				hasConsent
			});
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004B07 File Offset: 0x00002D07
		public bool isInitialized(int adType)
		{
			return this.getAppodealClass().CallStatic<bool>("isInitialized", new object[]
			{
				this.nativeAdTypesForType(adType)
			});
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004B2E File Offset: 0x00002D2E
		public bool show(int adTypes)
		{
			return this.getAppodealClass().CallStatic<bool>("show", new object[]
			{
				this.getActivity(),
				this.nativeAdTypesForType(adTypes)
			});
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004B5E File Offset: 0x00002D5E
		public bool show(int adTypes, string placement)
		{
			return this.getAppodealClass().CallStatic<bool>("show", new object[]
			{
				this.getActivity(),
				this.nativeAdTypesForType(adTypes),
				placement
			});
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004B92 File Offset: 0x00002D92
		public bool showBannerView(int YAxis, int XAxis, string Placement)
		{
			return this.getAppodealBannerInstance().Call<bool>("showBannerView", new object[]
			{
				this.getActivity(),
				XAxis,
				YAxis,
				Placement
			});
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004BC9 File Offset: 0x00002DC9
		public bool showMrecView(int YAxis, int XAxis, string Placement)
		{
			return this.getAppodealBannerInstance().Call<bool>("showMrecView", new object[]
			{
				this.getActivity(),
				XAxis,
				YAxis,
				Placement
			});
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004C00 File Offset: 0x00002E00
		public bool isLoaded(int adTypes)
		{
			return this.getAppodealClass().CallStatic<bool>("isLoaded", new object[]
			{
				this.nativeAdTypesForType(adTypes)
			});
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004C27 File Offset: 0x00002E27
		public void cache(int adTypes)
		{
			this.getAppodealClass().CallStatic("cache", new object[]
			{
				this.getActivity(),
				this.nativeAdTypesForType(adTypes)
			});
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00004C57 File Offset: 0x00002E57
		public void hide(int adTypes)
		{
			this.getAppodealClass().CallStatic("hide", new object[]
			{
				this.getActivity(),
				this.nativeAdTypesForType(adTypes)
			});
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004C87 File Offset: 0x00002E87
		public void hideBannerView()
		{
			this.getAppodealBannerInstance().Call("hideBannerView", new object[]
			{
				this.getActivity()
			});
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00004CA8 File Offset: 0x00002EA8
		public void hideMrecView()
		{
			this.getAppodealBannerInstance().Call("hideMrecView", new object[]
			{
				this.getActivity()
			});
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004CC9 File Offset: 0x00002EC9
		public bool isPrecache(int adTypes)
		{
			return this.getAppodealClass().CallStatic<bool>("isPrecache", new object[]
			{
				this.nativeAdTypesForType(adTypes)
			});
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004CF0 File Offset: 0x00002EF0
		public void setAutoCache(int adTypes, bool autoCache)
		{
			this.getAppodealClass().CallStatic("setAutoCache", new object[]
			{
				this.nativeAdTypesForType(adTypes),
				autoCache
			});
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00004D20 File Offset: 0x00002F20
		public void onResume()
		{
			this.getAppodealClass().CallStatic("onResume", new object[]
			{
				this.getActivity(),
				4
			});
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004D4A File Offset: 0x00002F4A
		public void setSmartBanners(bool value)
		{
			this.getAppodealClass().CallStatic("setSmartBanners", new object[]
			{
				value
			});
			this.getAppodealBannerInstance().Call("setSmartBanners", new object[]
			{
				value
			});
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00004D8A File Offset: 0x00002F8A
		public void setBannerAnimation(bool value)
		{
			this.getAppodealClass().CallStatic("setBannerAnimation", new object[]
			{
				value
			});
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00004DAB File Offset: 0x00002FAB
		public void setBannerBackground(bool value)
		{
			Debug.LogWarning("Not Supported by Android SDK");
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004DB7 File Offset: 0x00002FB7
		public void setTabletBanners(bool value)
		{
			this.getAppodealClass().CallStatic("set728x90Banners", new object[]
			{
				value
			});
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00004DD8 File Offset: 0x00002FD8
		public void setTesting(bool test)
		{
			this.getAppodealClass().CallStatic("setTesting", new object[]
			{
				test
			});
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00021E90 File Offset: 0x00020090
		public void setLogLevel(Appodeal.LogLevel logging)
		{
			switch (logging)
			{
			case Appodeal.LogLevel.None:
				this.getAppodealClass().CallStatic("setLogLevel", new object[]
				{
					new AndroidJavaClass("com.appodeal.ads.utils.Log$LogLevel").GetStatic<AndroidJavaObject>("none")
				});
				break;
			case Appodeal.LogLevel.Debug:
				this.getAppodealClass().CallStatic("setLogLevel", new object[]
				{
					new AndroidJavaClass("com.appodeal.ads.utils.Log$LogLevel").GetStatic<AndroidJavaObject>("debug")
				});
				break;
			case Appodeal.LogLevel.Verbose:
				this.getAppodealClass().CallStatic("setLogLevel", new object[]
				{
					new AndroidJavaClass("com.appodeal.ads.utils.Log$LogLevel").GetStatic<AndroidJavaObject>("verbose")
				});
				break;
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00004DF9 File Offset: 0x00002FF9
		public void setChildDirectedTreatment(bool value)
		{
			this.getAppodealClass().CallStatic("setChildDirectedTreatment", new object[]
			{
				value
			});
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00004E1A File Offset: 0x0000301A
		public void disableNetwork(string network)
		{
			this.getAppodealClass().CallStatic("disableNetwork", new object[]
			{
				this.getActivity(),
				network
			});
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004E3F File Offset: 0x0000303F
		public void disableNetwork(string network, int adTypes)
		{
			this.getAppodealClass().CallStatic("disableNetwork", new object[]
			{
				this.getActivity(),
				network,
				this.nativeAdTypesForType(adTypes)
			});
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004E73 File Offset: 0x00003073
		public void disableLocationPermissionCheck()
		{
			this.getAppodealClass().CallStatic("disableLocationPermissionCheck", new object[0]);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004E8B File Offset: 0x0000308B
		public void disableWriteExternalStoragePermissionCheck()
		{
			this.getAppodealClass().CallStatic("disableWriteExternalStoragePermissionCheck", new object[0]);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00004EA3 File Offset: 0x000030A3
		public void setTriggerOnLoadedOnPrecache(int adTypes, bool onLoadedTriggerBoth)
		{
			this.getAppodealClass().CallStatic("setTriggerOnLoadedOnPrecache", new object[]
			{
				this.nativeAdTypesForType(adTypes),
				onLoadedTriggerBoth
			});
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004ED3 File Offset: 0x000030D3
		public void muteVideosIfCallsMuted(bool value)
		{
			this.getAppodealClass().CallStatic("muteVideosIfCallsMuted", new object[]
			{
				value
			});
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004EF4 File Offset: 0x000030F4
		public void showTestScreen()
		{
			this.getAppodealClass().CallStatic("startTestActivity", new object[]
			{
				this.getActivity()
			});
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004F15 File Offset: 0x00003115
		public string getVersion()
		{
			return this.getAppodealClass().CallStatic<string>("getVersion", new object[0]);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00004F2D File Offset: 0x0000312D
		public bool canShow(int adTypes)
		{
			return this.getAppodealClass().CallStatic<bool>("canShow", new object[]
			{
				this.nativeAdTypesForType(adTypes)
			});
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00004F54 File Offset: 0x00003154
		public bool canShow(int adTypes, string placement)
		{
			return this.getAppodealClass().CallStatic<bool>("canShow", new object[]
			{
				this.nativeAdTypesForType(adTypes),
				placement
			});
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00004F7F File Offset: 0x0000317F
		public void setSegmentFilter(string name, bool value)
		{
			this.getAppodealClass().CallStatic("setSegmentFilter", new object[]
			{
				name,
				value
			});
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004FA4 File Offset: 0x000031A4
		public void setSegmentFilter(string name, int value)
		{
			this.getAppodealClass().CallStatic("setSegmentFilter", new object[]
			{
				name,
				value
			});
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004FC9 File Offset: 0x000031C9
		public void setSegmentFilter(string name, double value)
		{
			this.getAppodealClass().CallStatic("setSegmentFilter", new object[]
			{
				name,
				value
			});
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004FEE File Offset: 0x000031EE
		public void setSegmentFilter(string name, string value)
		{
			this.getAppodealClass().CallStatic("setSegmentFilter", new object[]
			{
				name,
				value
			});
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0000500E File Offset: 0x0000320E
		public void setExtraData(string key, bool value)
		{
			this.getAppodealClass().CallStatic("setExtraData", new object[]
			{
				key,
				value
			});
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00005033 File Offset: 0x00003233
		public void setExtraData(string key, int value)
		{
			this.getAppodealClass().CallStatic("setExtraData", new object[]
			{
				key,
				value
			});
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00005058 File Offset: 0x00003258
		public void setExtraData(string key, double value)
		{
			this.getAppodealClass().CallStatic("setExtraData", new object[]
			{
				key,
				value
			});
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000507D File Offset: 0x0000327D
		public void setExtraData(string key, string value)
		{
			this.getAppodealClass().CallStatic("setExtraData", new object[]
			{
				key,
				value
			});
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000509D File Offset: 0x0000329D
		public void trackInAppPurchase(double amount, string currency)
		{
			this.getAppodealClass().CallStatic("trackInAppPurchase", new object[]
			{
				this.getActivity(),
				amount,
				currency
			});
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00021F4C File Offset: 0x0002014C
		public string getRewardCurrency(string placement)
		{
			AndroidJavaObject androidJavaObject = this.getAppodealClass().CallStatic<AndroidJavaObject>("getRewardParameters", new object[]
			{
				placement
			});
			return androidJavaObject.Get<string>("second");
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00021F80 File Offset: 0x00020180
		public double getRewardAmount(string placement)
		{
			AndroidJavaObject androidJavaObject = this.getAppodealClass().CallStatic<AndroidJavaObject>("getRewardParameters", new object[]
			{
				placement
			});
			AndroidJavaObject androidJavaObject2 = androidJavaObject.Get<AndroidJavaObject>("first");
			return androidJavaObject2.Call<double>("doubleValue", new object[0]);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00021FC8 File Offset: 0x000201C8
		public string getRewardCurrency()
		{
			AndroidJavaObject androidJavaObject = this.getAppodealClass().CallStatic<AndroidJavaObject>("getRewardParameters", new object[0]);
			return androidJavaObject.Get<string>("second");
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00021FF8 File Offset: 0x000201F8
		public double getRewardAmount()
		{
			AndroidJavaObject androidJavaObject = this.getAppodealClass().CallStatic<AndroidJavaObject>("getRewardParameters", new object[0]);
			AndroidJavaObject androidJavaObject2 = androidJavaObject.Get<AndroidJavaObject>("first");
			return androidJavaObject2.Call<double>("doubleValue", new object[0]);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000050CB File Offset: 0x000032CB
		public double getPredictedEcpm(int adType)
		{
			return this.getAppodealClass().CallStatic<double>("getPredictedEcpm", new object[]
			{
				adType
			});
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000050EC File Offset: 0x000032EC
		public void destroy(int adTypes)
		{
			this.getAppodealClass().CallStatic("destroy", new object[]
			{
				this.nativeAdTypesForType(adTypes)
			});
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00005113 File Offset: 0x00003313
		public void getUserSettings()
		{
			this.userSettings = this.getAppodealClass().CallStatic<AndroidJavaObject>("getUserSettings", new object[]
			{
				this.getActivity()
			});
		}

		// Token: 0x060000CF RID: 207 RVA: 0x0000513A File Offset: 0x0000333A
		public void setUserId(string id)
		{
			this.userSettings.Call<AndroidJavaObject>("setUserId", new object[]
			{
				id
			});
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00005157 File Offset: 0x00003357
		public void setAge(int age)
		{
			this.userSettings.Call<AndroidJavaObject>("setAge", new object[]
			{
				age
			});
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0002203C File Offset: 0x0002023C
		public void setGender(UserSettings.Gender gender)
		{
			switch (gender)
			{
			case UserSettings.Gender.OTHER:
				this.userSettings.Call<AndroidJavaObject>("setGender", new object[]
				{
					new AndroidJavaClass("com.appodeal.ads.UserSettings$Gender").GetStatic<AndroidJavaObject>("OTHER")
				});
				break;
			case UserSettings.Gender.MALE:
				this.userSettings.Call<AndroidJavaObject>("setGender", new object[]
				{
					new AndroidJavaClass("com.appodeal.ads.UserSettings$Gender").GetStatic<AndroidJavaObject>("MALE")
				});
				break;
			case UserSettings.Gender.FEMALE:
				this.userSettings.Call<AndroidJavaObject>("setGender", new object[]
				{
					new AndroidJavaClass("com.appodeal.ads.UserSettings$Gender").GetStatic<AndroidJavaObject>("FEMALE")
				});
				break;
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00005179 File Offset: 0x00003379
		public void setInterstitialCallbacks(IInterstitialAdListener listener)
		{
			this.getAppodealClass().CallStatic("setInterstitialCallbacks", new object[]
			{
				new AppodealInterstitialCallbacks(listener)
			});
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000519A File Offset: 0x0000339A
		public void setNonSkippableVideoCallbacks(INonSkippableVideoAdListener listener)
		{
			this.getAppodealClass().CallStatic("setNonSkippableVideoCallbacks", new object[]
			{
				new AppodealNonSkippableVideoCallbacks(listener)
			});
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000051BB File Offset: 0x000033BB
		public void setRewardedVideoCallbacks(IRewardedVideoAdListener listener)
		{
			this.getAppodealClass().CallStatic("setRewardedVideoCallbacks", new object[]
			{
				new AppodealRewardedVideoCallbacks(listener)
			});
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000051DC File Offset: 0x000033DC
		public void setBannerCallbacks(IBannerAdListener listener)
		{
			this.getAppodealClass().CallStatic("setBannerCallbacks", new object[]
			{
				new AppodealBannerCallbacks(listener)
			});
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000051FD File Offset: 0x000033FD
		public void setMrecCallbacks(IMrecAdListener listener)
		{
			this.getAppodealClass().CallStatic("setMrecCallbacks", new object[]
			{
				new AppodealMrecCallbacks(listener)
			});
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000521E File Offset: 0x0000341E
		public void requestAndroidMPermissions(IPermissionGrantedListener listener)
		{
			this.getAppodealClass().CallStatic("requestAndroidMPermissions", new object[]
			{
				this.getActivity(),
				new AppodealPermissionCallbacks(listener)
			});
		}

		// Token: 0x0400001A RID: 26
		public const int NONE = 0;

		// Token: 0x0400001B RID: 27
		public const int INTERSTITIAL = 3;

		// Token: 0x0400001C RID: 28
		public const int BANNER = 4;

		// Token: 0x0400001D RID: 29
		public const int BANNER_BOTTOM = 8;

		// Token: 0x0400001E RID: 30
		public const int BANNER_TOP = 16;

		// Token: 0x0400001F RID: 31
		public const int BANNER_VIEW = 64;

		// Token: 0x04000020 RID: 32
		public const int MREC = 256;

		// Token: 0x04000021 RID: 33
		public const int REWARDED_VIDEO = 128;

		// Token: 0x04000022 RID: 34
		private bool isShow;

		// Token: 0x04000023 RID: 35
		private AndroidJavaClass appodealClass;

		// Token: 0x04000024 RID: 36
		private AndroidJavaClass appodealUnityClass;

		// Token: 0x04000025 RID: 37
		private AndroidJavaClass appodealBannerClass;

		// Token: 0x04000026 RID: 38
		private AndroidJavaObject appodealBannerInstance;

		// Token: 0x04000027 RID: 39
		private AndroidJavaObject userSettings;

		// Token: 0x04000028 RID: 40
		private AndroidJavaObject activity;

		// Token: 0x04000029 RID: 41
		private AndroidJavaObject popupWindow;

		// Token: 0x0400002A RID: 42
		private AndroidJavaObject resources;

		// Token: 0x0400002B RID: 43
		private AndroidJavaObject displayMetrics;

		// Token: 0x0400002C RID: 44
		private AndroidJavaObject window;

		// Token: 0x0400002D RID: 45
		private AndroidJavaObject decorView;

		// Token: 0x0400002E RID: 46
		private AndroidJavaObject attributes;

		// Token: 0x0400002F RID: 47
		private AndroidJavaObject rootView;
	}
}
