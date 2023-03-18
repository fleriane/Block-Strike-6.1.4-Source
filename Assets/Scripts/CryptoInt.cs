using System;
using UnityEngine;

// Token: 0x020004ED RID: 1261
[Serializable]
public struct CryptoInt : IFormattable, IEquatable<CryptoInt>
{
	// Token: 0x06002B33 RID: 11059 RVA: 0x00100878 File Offset: 0x000FEA78
	private CryptoInt(int value)
	{
		do
		{
			this.cryptoKey = CryptoManager.random.Next(int.MinValue, int.MaxValue);
		}
		while (this.cryptoKey == 0);
		this.hiddenValue = (value ^ this.cryptoKey);
		this.fakeValue = ((!CryptoManager.fakeValue) ? 0 : value);
		this.inited = true;
	}

	// Token: 0x06002B34 RID: 11060 RVA: 0x001008D8 File Offset: 0x000FEAD8
	public void SetValue(int value)
	{
		do
		{
			this.cryptoKey = CryptoManager.random.Next(int.MinValue, int.MaxValue);
		}
		while (this.cryptoKey == 0);
		this.fakeValue = ((!CryptoManager.fakeValue) ? 0 : value);
		this.hiddenValue = (value ^ this.cryptoKey);
	}

	// Token: 0x06002B35 RID: 11061 RVA: 0x0001E08E File Offset: 0x0001C28E
	public void UpdateValue()
	{
		this.SetValue(this.GetValue());
	}

	// Token: 0x06002B36 RID: 11062 RVA: 0x00100930 File Offset: 0x000FEB30
	private int GetValue()
	{
		if (!this.inited)
		{
			do
			{
				this.cryptoKey = CryptoManager.random.Next(int.MinValue, int.MaxValue);
			}
			while (this.cryptoKey == 0);
			this.hiddenValue = this.cryptoKey;
			this.fakeValue = 0;
			this.inited = true;
		}
		int num = this.hiddenValue ^ this.cryptoKey;
		if ((CryptoManager.fakeValue && this.fakeValue != num) || this.cryptoKey == 0)
		{
			CryptoManager.CheatingDetected();
		}
		return num;
	}

	// Token: 0x06002B37 RID: 11063 RVA: 0x0001E09C File Offset: 0x0001C29C
	public override bool Equals(object obj)
	{
		return obj is CryptoInt && this.Equals((CryptoInt)obj);
	}

	// Token: 0x06002B38 RID: 11064 RVA: 0x0001E0B7 File Offset: 0x0001C2B7
	public bool Equals(CryptoInt obj)
	{
		return this.GetValue() == obj.GetValue();
	}

	// Token: 0x06002B39 RID: 11065 RVA: 0x001009BC File Offset: 0x000FEBBC
	public override int GetHashCode()
	{
		return this.GetValue().GetHashCode();
	}

	// Token: 0x06002B3A RID: 11066 RVA: 0x001009D8 File Offset: 0x000FEBD8
	public override string ToString()
	{
		return this.GetValue().ToString();
	}

	// Token: 0x06002B3B RID: 11067 RVA: 0x001009F4 File Offset: 0x000FEBF4
	public string ToString(string format)
	{
		return this.GetValue().ToString(format);
	}

	// Token: 0x06002B3C RID: 11068 RVA: 0x00100A10 File Offset: 0x000FEC10
	public string ToString(IFormatProvider provider)
	{
		return this.GetValue().ToString(provider);
	}

	// Token: 0x06002B3D RID: 11069 RVA: 0x00100A2C File Offset: 0x000FEC2C
	public string ToString(string format, IFormatProvider provider)
	{
		return this.GetValue().ToString(format, provider);
	}

	// Token: 0x06002B3E RID: 11070 RVA: 0x00100A4C File Offset: 0x000FEC4C
	public static implicit operator CryptoInt(int value)
	{
		CryptoInt result = new CryptoInt(value);
		return result;
	}

	// Token: 0x06002B3F RID: 11071 RVA: 0x0001E0C8 File Offset: 0x0001C2C8
	public static implicit operator int(CryptoInt value)
	{
		return value.GetValue();
	}

	// Token: 0x06002B40 RID: 11072 RVA: 0x0001E0D1 File Offset: 0x0001C2D1
	public static CryptoInt operator ++(CryptoInt value)
	{
		value.SetValue(value.GetValue() + 1);
		return value;
	}

	// Token: 0x06002B41 RID: 11073 RVA: 0x0001E0E4 File Offset: 0x0001C2E4
	public static CryptoInt operator --(CryptoInt value)
	{
		value.SetValue(value.GetValue() - 1);
		return value;
	}

	// Token: 0x04001BD9 RID: 7129
	[SerializeField]
	private int cryptoKey;

	// Token: 0x04001BDA RID: 7130
	[SerializeField]
	private int hiddenValue;

	// Token: 0x04001BDB RID: 7131
	[SerializeField]
	private int fakeValue;

	// Token: 0x04001BDC RID: 7132
	[SerializeField]
	private bool inited;
}
