using System;
using UnityEngine;

// Token: 0x020004EE RID: 1262
[Serializable]
public struct CryptoInt2 : IFormattable, IEquatable<CryptoInt2>
{
	// Token: 0x06002B42 RID: 11074 RVA: 0x00100A64 File Offset: 0x000FEC64
	private CryptoInt2(int value)
	{
		do
		{
			this.cryptoKey = CryptoManager.random.Next(int.MinValue, int.MaxValue);
		}
		while (this.cryptoKey == 0);
		this.hiddenValue = CryptoManager.MD5(value ^ this.cryptoKey);
		this.fakeValue = value;
		this.inited = true;
	}

	// Token: 0x06002B43 RID: 11075 RVA: 0x00100AB8 File Offset: 0x000FECB8
	public void SetValue(int value)
	{
		do
		{
			this.cryptoKey = CryptoManager.random.Next(int.MinValue, int.MaxValue);
		}
		while (this.cryptoKey == 0);
		this.hiddenValue = CryptoManager.MD5(value ^ this.cryptoKey);
		this.fakeValue = value;
	}

	// Token: 0x06002B44 RID: 11076 RVA: 0x0001E0F7 File Offset: 0x0001C2F7
	public void UpdateValue()
	{
		this.SetValue(this.GetValue());
	}

	// Token: 0x06002B45 RID: 11077 RVA: 0x00100B04 File Offset: 0x000FED04
	private int GetValue()
	{
		if (!this.inited)
		{
			do
			{
				this.cryptoKey = CryptoManager.random.Next(int.MinValue, int.MaxValue);
			}
			while (this.cryptoKey == 0);
			this.hiddenValue = CryptoManager.MD5(this.cryptoKey);
			this.fakeValue = 0;
			this.inited = true;
		}
		if (CryptoManager.MD5(this.fakeValue ^ this.cryptoKey) != this.hiddenValue || this.cryptoKey == 0)
		{
			CryptoManager.CheatingDetected();
			return 0;
		}
		return this.fakeValue;
	}

	// Token: 0x06002B46 RID: 11078 RVA: 0x0001E105 File Offset: 0x0001C305
	public override bool Equals(object obj)
	{
		return obj is CryptoInt2 && this.Equals((CryptoInt2)obj);
	}

	// Token: 0x06002B47 RID: 11079 RVA: 0x0001E120 File Offset: 0x0001C320
	public bool Equals(CryptoInt2 obj)
	{
		return this.GetValue() == obj.GetValue();
	}

	// Token: 0x06002B48 RID: 11080 RVA: 0x00100B9C File Offset: 0x000FED9C
	public override int GetHashCode()
	{
		return this.GetValue().GetHashCode();
	}

	// Token: 0x06002B49 RID: 11081 RVA: 0x00100BB8 File Offset: 0x000FEDB8
	public override string ToString()
	{
		return this.GetValue().ToString();
	}

	// Token: 0x06002B4A RID: 11082 RVA: 0x00100BD4 File Offset: 0x000FEDD4
	public string ToString(string format)
	{
		return this.GetValue().ToString(format);
	}

	// Token: 0x06002B4B RID: 11083 RVA: 0x00100BF0 File Offset: 0x000FEDF0
	public string ToString(IFormatProvider provider)
	{
		return this.GetValue().ToString(provider);
	}

	// Token: 0x06002B4C RID: 11084 RVA: 0x00100C0C File Offset: 0x000FEE0C
	public string ToString(string format, IFormatProvider provider)
	{
		return this.GetValue().ToString(format, provider);
	}

	// Token: 0x06002B4D RID: 11085 RVA: 0x00100C2C File Offset: 0x000FEE2C
	public static implicit operator CryptoInt2(int value)
	{
		CryptoInt2 result = new CryptoInt2(value);
		return result;
	}

	// Token: 0x06002B4E RID: 11086 RVA: 0x0001E131 File Offset: 0x0001C331
	public static implicit operator int(CryptoInt2 value)
	{
		return value.GetValue();
	}

	// Token: 0x06002B4F RID: 11087 RVA: 0x0001E13A File Offset: 0x0001C33A
	public static CryptoInt2 operator ++(CryptoInt2 value)
	{
		value.SetValue(value.GetValue() + 1);
		return value;
	}

	// Token: 0x06002B50 RID: 11088 RVA: 0x0001E14D File Offset: 0x0001C34D
	public static CryptoInt2 operator --(CryptoInt2 value)
	{
		value.SetValue(value.GetValue() - 1);
		return value;
	}

	// Token: 0x04001BDD RID: 7133
	[SerializeField]
	private int cryptoKey;

	// Token: 0x04001BDE RID: 7134
	[SerializeField]
	private string hiddenValue;

	// Token: 0x04001BDF RID: 7135
	[SerializeField]
	private int fakeValue;

	// Token: 0x04001BE0 RID: 7136
	[SerializeField]
	private bool inited;
}
