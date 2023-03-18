using System;
using AppodealAds.Unity.Common;

namespace AppodealAds.Unity.Api
{
	// Token: 0x02000005 RID: 5
	public class UserSettings
	{
		// Token: 0x06000038 RID: 56 RVA: 0x00004A20 File Offset: 0x00002C20
		public UserSettings()
		{
			UserSettings.getInstance().getUserSettings();
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00004A32 File Offset: 0x00002C32
		private static IAppodealAdsClient getInstance()
		{
			if (UserSettings.client == null)
			{
				UserSettings.client = AppodealAdsClientFactory.GetAppodealAdsClient();
			}
			return UserSettings.client;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00004A4D File Offset: 0x00002C4D
		public UserSettings setUserId(string id)
		{
			UserSettings.getInstance().setUserId(id);
			return this;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00004A5B File Offset: 0x00002C5B
		public UserSettings setAge(int age)
		{
			UserSettings.getInstance().setAge(age);
			return this;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00004A69 File Offset: 0x00002C69
		public UserSettings setGender(UserSettings.Gender gender)
		{
			UserSettings.getInstance().setGender(gender);
			return this;
		}

		// Token: 0x04000015 RID: 21
		private static IAppodealAdsClient client;

		// Token: 0x02000006 RID: 6
		public enum Gender
		{
			// Token: 0x04000017 RID: 23
			OTHER,
			// Token: 0x04000018 RID: 24
			MALE,
			// Token: 0x04000019 RID: 25
			FEMALE
		}
	}
}
