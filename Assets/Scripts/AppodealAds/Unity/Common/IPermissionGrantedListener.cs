using System;

namespace AppodealAds.Unity.Common
{
	// Token: 0x0200000C RID: 12
	public interface IPermissionGrantedListener
	{
		// Token: 0x0600008C RID: 140
		void writeExternalStorageResponse(int result);

		// Token: 0x0600008D RID: 141
		void accessCoarseLocationResponse(int result);
	}
}
