using System;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using UnityEngine;

namespace AppodealAds.Unity.Dummy
{
	// Token: 0x02000018 RID: 24
	public class DummyClient : IAppodealAdsClient
	{
		// Token: 0x06000106 RID: 262 RVA: 0x000054BC File Offset: 0x000036BC
		public void initialize(string appKey, int adTypes)
		{
			Debug.Log("Call to Appodeal.initialize on not supported platform");
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000054BC File Offset: 0x000036BC
		public void initialize(string appKey, int adTypes, bool hasConsent)
		{
			Debug.Log("Call to Appodeal.initialize on not supported platform");
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000054C8 File Offset: 0x000036C8
		public bool isInitialized(int adType)
		{
			Debug.Log("Call Appodeal.isInitialized on not supported platform");
			return false;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000054D5 File Offset: 0x000036D5
		public bool show(int adTypes)
		{
			Debug.Log("Call to Appodeal.show on not supported platform");
			return false;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000054D5 File Offset: 0x000036D5
		public bool show(int adTypes, string placement)
		{
			Debug.Log("Call to Appodeal.show on not supported platform");
			return false;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000054E2 File Offset: 0x000036E2
		public bool showBannerView(int YAxis, int XAxis, string Placement)
		{
			Debug.Log("Call to Appodeal.showBannerView on not supported platform");
			return false;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000054EF File Offset: 0x000036EF
		public bool showMrecView(int YAxis, int XGravity, string Placement)
		{
			Debug.Log("Call to Appodeal.showMrecView on not supported platform");
			return false;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000054E2 File Offset: 0x000036E2
		public bool isLoaded(int adTypes)
		{
			Debug.Log("Call to Appodeal.showBannerView on not supported platform");
			return false;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000054FC File Offset: 0x000036FC
		public void cache(int adTypes)
		{
			Debug.Log("Call to Appodeal.cache on not supported platform");
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005508 File Offset: 0x00003708
		public void hide(int adTypes)
		{
			Debug.Log("Call to Appodeal.hide on not supported platform");
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00005514 File Offset: 0x00003714
		public void hideBannerView()
		{
			Debug.Log("Call to Appodeal.hideBannerView on not supported platform");
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00005520 File Offset: 0x00003720
		public void hideMrecView()
		{
			Debug.Log("Call to Appodeal.hideMrecView on not supported platform");
		}

		// Token: 0x06000112 RID: 274 RVA: 0x0000552C File Offset: 0x0000372C
		public bool isPrecache(int adTypes)
		{
			Debug.Log("Call to Appodeal.isPrecache on not supported platform");
			return false;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00005539 File Offset: 0x00003739
		public void setAutoCache(int adTypes, bool autoCache)
		{
			Debug.Log("Call to Appodeal.setAutoCache on not supported platform");
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00005545 File Offset: 0x00003745
		public void onResume()
		{
			Debug.Log("Call to Appodeal.onResume on not supported platform");
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00005551 File Offset: 0x00003751
		public void setSmartBanners(bool value)
		{
			Debug.Log("Call to Appodeal.setSmartBanners on not supported platform");
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000555D File Offset: 0x0000375D
		public void setBannerAnimation(bool value)
		{
			Debug.Log("Call to Appodeal.setBannerAnimation on not supported platform");
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005569 File Offset: 0x00003769
		public void setBannerBackground(bool value)
		{
			Debug.Log("Call to Appodeal.setBannerBackground on not supported platform");
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00005575 File Offset: 0x00003775
		public void setTabletBanners(bool value)
		{
			Debug.Log("Call to Appodeal.setTabletBanners on not supported platform");
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00005581 File Offset: 0x00003781
		public void setTesting(bool test)
		{
			Debug.Log("Call to Appodeal.setTesting on not supported platform");
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000558D File Offset: 0x0000378D
		public void setLogLevel(Appodeal.LogLevel logging)
		{
			Debug.Log("Call to Appodeal.setLogLevel on not supported platform");
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005599 File Offset: 0x00003799
		public void setChildDirectedTreatment(bool value)
		{
			Debug.Log("Call to Appodeal.setChildDirectedTreatment on not supported platform");
		}

		// Token: 0x0600011C RID: 284 RVA: 0x000055A5 File Offset: 0x000037A5
		public void disableNetwork(string network)
		{
			Debug.Log("Call to Appodeal.disableNetwork on not supported platform");
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000055A5 File Offset: 0x000037A5
		public void disableNetwork(string network, int adTypes)
		{
			Debug.Log("Call to Appodeal.disableNetwork on not supported platform");
		}

		// Token: 0x0600011E RID: 286 RVA: 0x000055B1 File Offset: 0x000037B1
		public void disableLocationPermissionCheck()
		{
			Debug.Log("Call to Appodeal.disableLocationPermissionCheck on not supported platform");
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000055BD File Offset: 0x000037BD
		public void disableWriteExternalStoragePermissionCheck()
		{
			Debug.Log("Call to Appodeal.disableWriteExternalStoragePermissionCheck on not supported platform");
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000055C9 File Offset: 0x000037C9
		public void setTriggerOnLoadedOnPrecache(int adTypes, bool onLoadedTriggerBoth)
		{
			Debug.Log("Call to Appodeal.setTriggerOnLoadedOnPrecache on not supported platform");
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000055D5 File Offset: 0x000037D5
		public void muteVideosIfCallsMuted(bool value)
		{
			Debug.Log("Call to Appodeal.muteVideosIfCallsMuted on not supported platform");
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000055E1 File Offset: 0x000037E1
		public void showTestScreen()
		{
			Debug.Log("Call to Appodeal.showTestScreen on not supported platform");
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000496D File Offset: 0x00002B6D
		public string getVersion()
		{
			return "2.8.49";
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000055ED File Offset: 0x000037ED
		public bool canShow(int adTypes)
		{
			Debug.Log("Call to Appodeal.canShow on not supported platform");
			return false;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000055ED File Offset: 0x000037ED
		public bool canShow(int adTypes, string placement)
		{
			Debug.Log("Call to Appodeal.canShow on not supported platform");
			return false;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000055FA File Offset: 0x000037FA
		public void setSegmentFilter(string name, bool value)
		{
			Debug.Log("Call to Appodeal.setSegmentFilter on not supported platform");
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000055FA File Offset: 0x000037FA
		public void setSegmentFilter(string name, int value)
		{
			Debug.Log("Call to Appodeal.setSegmentFilter on not supported platform");
		}

		// Token: 0x06000128 RID: 296 RVA: 0x000055FA File Offset: 0x000037FA
		public void setSegmentFilter(string name, double value)
		{
			Debug.Log("Call to Appodeal.setSegmentFilter on not supported platform");
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000055FA File Offset: 0x000037FA
		public void setSegmentFilter(string name, string value)
		{
			Debug.Log("Call to Appodeal.setSegmentFilter on not supported platform");
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00005606 File Offset: 0x00003806
		public void trackInAppPurchase(double amount, string currency)
		{
			Debug.Log("Call to Appodeal.trackInAppPurchase on not supported platform");
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00005612 File Offset: 0x00003812
		public string getRewardCurrency(string placement)
		{
			Debug.Log("Call to Appodeal.getRewardCurrency on not supported platform");
			return "USD";
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00005623 File Offset: 0x00003823
		public double getRewardAmount(string placement)
		{
			Debug.Log("Call to Appodeal.getRewardAmount on not supported platform");
			return 0.0;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00005612 File Offset: 0x00003812
		public string getRewardCurrency()
		{
			Debug.Log("Call to Appodeal.getRewardCurrency on not supported platform");
			return "USD";
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00005623 File Offset: 0x00003823
		public double getRewardAmount()
		{
			Debug.Log("Call to Appodeal.getRewardAmount on not supported platform");
			return 0.0;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00005638 File Offset: 0x00003838
		public double getPredictedEcpm(int adType)
		{
			Debug.Log("Call to Appodeal.getPredictedEcpm on not supported platform");
			return 0.0;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000564D File Offset: 0x0000384D
		public void setExtraData(string key, bool value)
		{
			Debug.Log("Call to Appodeal.setExtraData(string key, bool value) on not supported platform");
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00005659 File Offset: 0x00003859
		public void setExtraData(string key, int value)
		{
			Debug.Log("Call to Appodeal.setExtraData(string key, int value) on not supported platform");
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00005665 File Offset: 0x00003865
		public void setExtraData(string key, double value)
		{
			Debug.Log("Call to Appodeal.setExtraDatastring key, double value) on not supported platform");
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00005671 File Offset: 0x00003871
		public void setExtraData(string key, string value)
		{
			Debug.Log("Call to Appodeal.setExtraData(string key, string value) on not supported platform");
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000567D File Offset: 0x0000387D
		public void setInterstitialCallbacks(IInterstitialAdListener listener)
		{
			Debug.Log("Call to Appodeal.setInterstitialCallbacks on not supported platform");
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00005689 File Offset: 0x00003889
		public void setNonSkippableVideoCallbacks(INonSkippableVideoAdListener listener)
		{
			Debug.Log("Call to Appodeal.setNonSkippableVideoCallbacks on not supported platform");
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00005695 File Offset: 0x00003895
		public void setRewardedVideoCallbacks(IRewardedVideoAdListener listener)
		{
			Debug.Log("Call to Appodeal.setRewardedVideoCallbacks on not supported platform");
		}

		// Token: 0x06000137 RID: 311 RVA: 0x000056A1 File Offset: 0x000038A1
		public void setBannerCallbacks(IBannerAdListener listener)
		{
			Debug.Log("Call to Appodeal.setBannerCallbacks on not supported platform");
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000056AD File Offset: 0x000038AD
		public void setMrecCallbacks(IMrecAdListener listener)
		{
			Debug.Log("Call to Appodeal.setMrecCallbacks on not supported platform");
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000056B9 File Offset: 0x000038B9
		public void requestAndroidMPermissions(IPermissionGrantedListener listener)
		{
			Debug.Log("Call to Appodeal.requestAndroidMPermissions on not supported platform");
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000056C5 File Offset: 0x000038C5
		public void getUserSettings()
		{
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000056C7 File Offset: 0x000038C7
		public void setUserId(string id)
		{
			Debug.Log("Call to Appodeal.setUserId on not supported platform");
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000056D3 File Offset: 0x000038D3
		public void setAge(int age)
		{
			Debug.Log("Call to Appodeal.setAge on not supported platform");
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000056DF File Offset: 0x000038DF
		public void setGender(UserSettings.Gender gender)
		{
			Debug.Log("Call to Appodeal.setGender on not supported platform");
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000056B9 File Offset: 0x000038B9
		public void requestAndroidMPermissions()
		{
			Debug.Log("Call to Appodeal.requestAndroidMPermissions on not supported platform");
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000056EB File Offset: 0x000038EB
		public void destroy(int adTypes)
		{
			Debug.Log("Call to Appodeal.destroy on not supported platform");
		}
	}
}
