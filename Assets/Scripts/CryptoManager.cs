using System;
using System.Security.Cryptography;
using System.Text;

// Token: 0x020004F0 RID: 1264
public static class CryptoManager
{
	// Token: 0x06002B61 RID: 11105 RVA: 0x00100D8C File Offset: 0x000FEF8C
	static CryptoManager()
	{
		CryptoManager.seed = DateTime.Now.Millisecond;
		CryptoManager.randValue = CryptoManager.seed;
	}

	// Token: 0x17000484 RID: 1156
	// (get) Token: 0x06002B62 RID: 11106 RVA: 0x0001E238 File Offset: 0x0001C438
	public static int staticValue
	{
		get
		{
			if (CryptoManager._staticValue == 0)
			{
				CryptoManager._staticValue = CryptoManager.Next(1, 10);
			}
			return CryptoManager._staticValue;
		}
	}

	// Token: 0x06002B63 RID: 11107 RVA: 0x0001E256 File Offset: 0x0001C456
	public static void StartDetection(Action callback)
	{
		CryptoManager.DetectorAction = callback;
	}

	// Token: 0x06002B64 RID: 11108 RVA: 0x0001E25E File Offset: 0x0001C45E
	public static void CheatingDetected()
	{
		if (CryptoManager.DetectorAction != null)
		{
			CryptoManager.DetectorAction();
		}
	}

	// Token: 0x06002B65 RID: 11109 RVA: 0x0001E274 File Offset: 0x0001C474
	public static string MD5(int value)
	{
		return CryptoManager.MD5(BitConverter.GetBytes(value));
	}

	// Token: 0x06002B66 RID: 11110 RVA: 0x00100DEC File Offset: 0x000FEFEC
	public static string MD5(string value)
	{
		UTF8Encoding utf8Encoding = new UTF8Encoding();
		return CryptoManager.MD5(utf8Encoding.GetBytes(value));
	}

	// Token: 0x06002B67 RID: 11111 RVA: 0x00100E0C File Offset: 0x000FF00C
	public static string MD5(byte[] bytes)
	{
		MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
		byte[] array = md5CryptoServiceProvider.ComputeHash(bytes);
		string text = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			text += Convert.ToString(array[i], 16).PadLeft(2, '0');
		}
		return text.PadLeft(32, '0');
	}

	// Token: 0x06002B68 RID: 11112 RVA: 0x0001E281 File Offset: 0x0001C481
	public static int Next()
	{
		CryptoManager.randValue ^= CryptoManager.randValue << 21;
		CryptoManager.randValue ^= CryptoManager.randValue >> 3;
		CryptoManager.randValue ^= CryptoManager.randValue << 4;
		return CryptoManager.randValue;
	}

	// Token: 0x06002B69 RID: 11113 RVA: 0x00100E64 File Offset: 0x000FF064
	public static int Next(int min, int max)
	{
		CryptoManager.randValue ^= CryptoManager.randValue << 21;
		CryptoManager.randValue ^= CryptoManager.randValue >> 3;
		CryptoManager.randValue ^= CryptoManager.randValue << 4;
		return (int)(((float)CryptoManager.randValue / 2.14748365E+09f + 1f) / 2f * (float)(max - min) + (float)min);
	}

	// Token: 0x04001BE5 RID: 7141
	public static string cryptoKey = "^8b1v]x4";

	// Token: 0x04001BE6 RID: 7142
	public static byte[] cryptoKeyBytes = new byte[]
	{
		1,
		61,
		42,
		250,
		125
	};

	// Token: 0x04001BE7 RID: 7143
	public static Random random = new Random();

	// Token: 0x04001BE8 RID: 7144
	public static bool fakeValue = false;

	// Token: 0x04001BE9 RID: 7145
	private static int seed;

	// Token: 0x04001BEA RID: 7146
	private static int randValue;

	// Token: 0x04001BEB RID: 7147
	private static int _staticValue = 0;

	// Token: 0x04001BEC RID: 7148
	private static Action DetectorAction;
}
