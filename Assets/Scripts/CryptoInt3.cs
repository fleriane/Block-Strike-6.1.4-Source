using System;
using UnityEngine;

// Token: 0x020004EF RID: 1263
[Serializable]
public struct CryptoInt3 : IFormattable, IEquatable<CryptoInt3>
{
	// Token: 0x06002B51 RID: 11089 RVA: 0x0001E160 File Offset: 0x0001C360
	private CryptoInt3(int value)
	{
		do
		{
			this.cryptoKey = CryptoManager.Next();
		}
		while (this.cryptoKey == 0);
		this.hiddenValue = value + this.cryptoKey;
		this.fakeValue = value - this.cryptoKey;
		this.inited = true;
	}

	// Token: 0x06002B52 RID: 11090 RVA: 0x0001E19B File Offset: 0x0001C39B
	public void SetValue(int value)
	{
		do
		{
			this.cryptoKey = CryptoManager.Next();
		}
		while (this.cryptoKey == 0);
		this.hiddenValue = value + this.cryptoKey;
		this.fakeValue = value - this.cryptoKey;
	}

	// Token: 0x06002B53 RID: 11091 RVA: 0x0001E1CF File Offset: 0x0001C3CF
	public void UpdateValue()
	{
		this.SetValue(this.GetValue());
	}

	// Token: 0x06002B54 RID: 11092 RVA: 0x00100C44 File Offset: 0x000FEE44
	private int GetValue()
	{
		if (!this.inited)
		{
			do
			{
				this.cryptoKey = CryptoManager.Next();
			}
			while (this.cryptoKey == 0);
			this.hiddenValue = this.cryptoKey;
			this.fakeValue = -this.cryptoKey;
			this.inited = true;
		}
		if (this.hiddenValue - this.cryptoKey != this.fakeValue + this.cryptoKey)
		{
			CryptoManager.CheatingDetected();
			return 0;
		}
		return this.hiddenValue - this.cryptoKey;
	}

	// Token: 0x06002B55 RID: 11093 RVA: 0x0001E1DD File Offset: 0x0001C3DD
	public override bool Equals(object obj)
	{
		return obj is CryptoInt3 && this.Equals((CryptoInt3)obj);
	}

	// Token: 0x06002B56 RID: 11094 RVA: 0x0001E1F8 File Offset: 0x0001C3F8
	public bool Equals(CryptoInt3 obj)
	{
		return this.GetValue() == obj.GetValue();
	}

	// Token: 0x06002B57 RID: 11095 RVA: 0x00100CC8 File Offset: 0x000FEEC8
	public override int GetHashCode()
	{
		return this.GetValue().GetHashCode();
	}

	// Token: 0x06002B58 RID: 11096 RVA: 0x00100CE4 File Offset: 0x000FEEE4
	public override string ToString()
	{
		return this.GetValue().ToString();
	}

	// Token: 0x06002B59 RID: 11097 RVA: 0x00100D00 File Offset: 0x000FEF00
	public string ToString(string format)
	{
		return this.GetValue().ToString(format);
	}

	// Token: 0x06002B5A RID: 11098 RVA: 0x00100D1C File Offset: 0x000FEF1C
	public string ToString(IFormatProvider provider)
	{
		return this.GetValue().ToString(provider);
	}

	// Token: 0x06002B5B RID: 11099 RVA: 0x00100D38 File Offset: 0x000FEF38
	public string ToString(string format, IFormatProvider provider)
	{
		return this.GetValue().ToString(format, provider);
	}

	// Token: 0x06002B5C RID: 11100 RVA: 0x00100D58 File Offset: 0x000FEF58
	public static implicit operator CryptoInt3(int value)
	{
		CryptoInt3 result = new CryptoInt3(value);
		return result;
	}

	// Token: 0x06002B5D RID: 11101 RVA: 0x00100D70 File Offset: 0x000FEF70
	public static implicit operator CryptoInt3(CryptoInt value)
	{
		CryptoInt3 result = new CryptoInt3(value);
		return result;
	}

	// Token: 0x06002B5E RID: 11102 RVA: 0x0001E209 File Offset: 0x0001C409
	public static implicit operator int(CryptoInt3 value)
	{
		return value.GetValue();
	}

	// Token: 0x06002B5F RID: 11103 RVA: 0x0001E212 File Offset: 0x0001C412
	public static CryptoInt3 operator ++(CryptoInt3 value)
	{
		value.SetValue(value.GetValue() + 1);
		return value;
	}

	// Token: 0x06002B60 RID: 11104 RVA: 0x0001E225 File Offset: 0x0001C425
	public static CryptoInt3 operator --(CryptoInt3 value)
	{
		value.SetValue(value.GetValue() - 1);
		return value;
	}

	// Token: 0x04001BE1 RID: 7137
	[SerializeField]
	private int cryptoKey;

	// Token: 0x04001BE2 RID: 7138
	[SerializeField]
	private int hiddenValue;

	// Token: 0x04001BE3 RID: 7139
	[SerializeField]
	private int fakeValue;

	// Token: 0x04001BE4 RID: 7140
	[SerializeField]
	private bool inited;
}
