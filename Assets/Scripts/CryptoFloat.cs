using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020004E9 RID: 1257
[Serializable]
public struct CryptoFloat : IFormattable, IEquatable<CryptoFloat>
{
	// Token: 0x06002B13 RID: 11027 RVA: 0x00100560 File Offset: 0x000FE760
	private CryptoFloat(float value)
	{
		this.cryptoKey = CryptoManager.random.Next(int.MinValue, int.MaxValue);
		CryptoFloat.FloatIntBytesUnion floatIntBytesUnion = default(CryptoFloat.FloatIntBytesUnion);
		floatIntBytesUnion.f = value;
		floatIntBytesUnion.i ^= this.cryptoKey;
		this.hiddenValue = floatIntBytesUnion.b4;
		this.fakeValue = ((!CryptoManager.fakeValue) ? 0f : value);
		this.inited = true;
	}

	// Token: 0x06002B14 RID: 11028 RVA: 0x0001DF05 File Offset: 0x0001C105
	public void UpdateValue()
	{
		this.SetValue(this.GetValue());
	}

	// Token: 0x06002B15 RID: 11029 RVA: 0x001005DC File Offset: 0x000FE7DC
	public void SetValue(float value)
	{
		this.cryptoKey = CryptoManager.random.Next(int.MinValue, int.MaxValue);
		this.hiddenValue = this.Encrypt(value);
		this.fakeValue = ((!CryptoManager.fakeValue) ? 0f : value);
	}

	// Token: 0x06002B16 RID: 11030 RVA: 0x0010062C File Offset: 0x000FE82C
	private float GetValue()
	{
		if (!this.inited)
		{
			this.cryptoKey = CryptoManager.random.Next(int.MinValue, int.MaxValue);
			this.hiddenValue = this.Encrypt(0f);
			this.fakeValue = 0f;
			this.inited = true;
		}
		float num = this.Decrypt(this.hiddenValue);
		if (CryptoManager.fakeValue && this.fakeValue != num)
		{
			CryptoManager.CheatingDetected();
		}
		return num;
	}

	// Token: 0x06002B17 RID: 11031 RVA: 0x001006AC File Offset: 0x000FE8AC
	private CryptoFloat.Byte4 Encrypt(float value)
	{
		CryptoFloat.FloatIntBytesUnion floatIntBytesUnion = default(CryptoFloat.FloatIntBytesUnion);
		floatIntBytesUnion.f = value;
		floatIntBytesUnion.i ^= this.cryptoKey;
		return floatIntBytesUnion.b4;
	}

	// Token: 0x06002B18 RID: 11032 RVA: 0x001006E8 File Offset: 0x000FE8E8
	private float Decrypt(CryptoFloat.Byte4 bytes)
	{
		CryptoFloat.FloatIntBytesUnion floatIntBytesUnion = default(CryptoFloat.FloatIntBytesUnion);
		floatIntBytesUnion.b4 = this.hiddenValue;
		floatIntBytesUnion.i ^= this.cryptoKey;
		return floatIntBytesUnion.f;
	}

	// Token: 0x06002B19 RID: 11033 RVA: 0x0001DF13 File Offset: 0x0001C113
	public override bool Equals(object obj)
	{
		return obj is CryptoFloat && this.Equals((CryptoFloat)obj);
	}

	// Token: 0x06002B1A RID: 11034 RVA: 0x0001DF2E File Offset: 0x0001C12E
	public bool Equals(CryptoFloat obj)
	{
		return this.GetValue() == obj.GetValue();
	}

	// Token: 0x06002B1B RID: 11035 RVA: 0x00100728 File Offset: 0x000FE928
	public override int GetHashCode()
	{
		return this.GetValue().GetHashCode();
	}

	// Token: 0x06002B1C RID: 11036 RVA: 0x00100744 File Offset: 0x000FE944
	public override string ToString()
	{
		return this.GetValue().ToString();
	}

	// Token: 0x06002B1D RID: 11037 RVA: 0x00100760 File Offset: 0x000FE960
	public string ToString(string format)
	{
		return this.GetValue().ToString(format);
	}

	// Token: 0x06002B1E RID: 11038 RVA: 0x0010077C File Offset: 0x000FE97C
	public string ToString(IFormatProvider provider)
	{
		return this.GetValue().ToString(provider);
	}

	// Token: 0x06002B1F RID: 11039 RVA: 0x00100798 File Offset: 0x000FE998
	public string ToString(string format, IFormatProvider provider)
	{
		return this.GetValue().ToString(format, provider);
	}

	// Token: 0x06002B20 RID: 11040 RVA: 0x001007B8 File Offset: 0x000FE9B8
	public static implicit operator CryptoFloat(float value)
	{
		CryptoFloat result = new CryptoFloat(value);
		return result;
	}

	// Token: 0x06002B21 RID: 11041 RVA: 0x0001DF3F File Offset: 0x0001C13F
	public static implicit operator float(CryptoFloat value)
	{
		return value.GetValue();
	}

	// Token: 0x06002B22 RID: 11042 RVA: 0x0001DF48 File Offset: 0x0001C148
	public static CryptoFloat operator ++(CryptoFloat value)
	{
		value.SetValue(value.GetValue() + 1f);
		return value;
	}

	// Token: 0x06002B23 RID: 11043 RVA: 0x0001DF5F File Offset: 0x0001C15F
	public static CryptoFloat operator --(CryptoFloat value)
	{
		value.SetValue(value.GetValue() - 1f);
		return value;
	}

	// Token: 0x04001BCA RID: 7114
	[SerializeField]
	private int cryptoKey;

	// Token: 0x04001BCB RID: 7115
	[SerializeField]
	private CryptoFloat.Byte4 hiddenValue;

	// Token: 0x04001BCC RID: 7116
	[SerializeField]
	private float fakeValue;

	// Token: 0x04001BCD RID: 7117
	[SerializeField]
	private bool inited;

	// Token: 0x020004EA RID: 1258
	[Serializable]
	private struct Byte4
	{
		// Token: 0x04001BCE RID: 7118
		public byte b1;

		// Token: 0x04001BCF RID: 7119
		public byte b2;

		// Token: 0x04001BD0 RID: 7120
		public byte b3;

		// Token: 0x04001BD1 RID: 7121
		public byte b4;
	}

	// Token: 0x020004EB RID: 1259
	[StructLayout(LayoutKind.Explicit)]
	private struct FloatIntBytesUnion
	{
		// Token: 0x04001BD2 RID: 7122
		[FieldOffset(0)]
		public float f;

		// Token: 0x04001BD3 RID: 7123
		[FieldOffset(0)]
		public int i;

		// Token: 0x04001BD4 RID: 7124
		[FieldOffset(0)]
		public CryptoFloat.Byte4 b4;
	}
}
