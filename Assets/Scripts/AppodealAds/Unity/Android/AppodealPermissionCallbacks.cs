using System;
using AppodealAds.Unity.Common;
using UnityEngine;

namespace AppodealAds.Unity.Android
{
	// Token: 0x02000014 RID: 20
	public class AppodealPermissionCallbacks : AndroidJavaProxy
	{
		// Token: 0x060000F2 RID: 242 RVA: 0x000053BB File Offset: 0x000035BB
		internal AppodealPermissionCallbacks(IPermissionGrantedListener listener) : base("com.appodeal.ads.utils.PermissionsHelper$AppodealPermissionCallbacks")
		{
			this.listener = listener;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000053CF File Offset: 0x000035CF
		private void writeExternalStorageResponse(int result)
		{
			this.listener.writeExternalStorageResponse(result);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000053DD File Offset: 0x000035DD
		private void accessCoarseLocationResponse(int result)
		{
			this.listener.accessCoarseLocationResponse(result);
		}

		// Token: 0x04000034 RID: 52
		private IPermissionGrantedListener listener;
	}
}
