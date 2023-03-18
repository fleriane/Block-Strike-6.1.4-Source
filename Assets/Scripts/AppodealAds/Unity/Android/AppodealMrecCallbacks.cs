using System;
using AppodealAds.Unity.Common;
using UnityEngine;

namespace AppodealAds.Unity.Android
{
	// Token: 0x02000012 RID: 18
	public class AppodealMrecCallbacks : AndroidJavaProxy
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x00005301 File Offset: 0x00003501
		internal AppodealMrecCallbacks(IMrecAdListener listener) : base("com.appodeal.ads.MrecCallbacks")
		{
			this.listener = listener;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00005315 File Offset: 0x00003515
		private void onMrecLoaded(bool isPrecache)
		{
			this.listener.onMrecLoaded(isPrecache);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00005323 File Offset: 0x00003523
		private void onMrecFailedToLoad()
		{
			this.listener.onMrecFailedToLoad();
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00005330 File Offset: 0x00003530
		private void onMrecShown()
		{
			this.listener.onMrecShown();
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000533D File Offset: 0x0000353D
		private void onMrecClicked()
		{
			this.listener.onMrecClicked();
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000534A File Offset: 0x0000354A
		private void onMrecExpired()
		{
			this.listener.onMrecExpired();
		}

		// Token: 0x04000032 RID: 50
		private IMrecAdListener listener;
	}
}
