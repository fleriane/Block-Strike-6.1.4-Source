using System;
using UnityEngine;

// Token: 0x020004EC RID: 1260
[Serializable]
public struct CryptoFloatFast : IFormattable, IEquatable<CryptoFloatFast>
{
	// Token: 0x06002B24 RID: 11044 RVA: 0x0001DF76 File Offset: 0x0001C176
	private CryptoFloatFast(float value)
	{
		this.defaultValue = value;
		this.randomValue = CryptoManager.staticValue;
		this.hiddenValue = value + (float)this.randomValue;
		this.inited = true;
	}

	// Token: 0x06002B25 RID: 11045 RVA: 0x0001DFA0 File Offset: 0x0001C1A0
	public void SetValue(float value)
	{
		this.defaultValue = value;
		this.randomValue = CryptoManager.staticValue;
		this.hiddenValue = value + (float)this.randomValue;
	}

	// Token: 0x06002B26 RID: 11046 RVA: 0x0001DFC3 File Offset: 0x0001C1C3
	public void CheckValue()
	{
		if (this.defaultValue - (this.hiddenValue - (float)this.randomValue) >= nValue.float001)
		{
			CheckManager.Detected("Controller Error 23");
		}
	}

	// Token: 0x06002B27 RID: 11047 RVA: 0x0001DFEE File Offset: 0x0001C1EE
	public float GetValue()
	{
		if (!this.inited)
		{
			this.defaultValue = 0f;
			this.randomValue = CryptoManager.staticValue;
			this.hiddenValue = this.defaultValue + (float)this.randomValue;
		}
		return this.defaultValue;
	}

	// Token: 0x06002B28 RID: 11048 RVA: 0x0001E02B File Offset: 0x0001C22B
	public override bool Equals(object obj)
	{
		return obj is CryptoFloatFast && this.Equals((CryptoFloatFast)obj);
	}

	// Token: 0x06002B29 RID: 11049 RVA: 0x0001E046 File Offset: 0x0001C246
	public bool Equals(CryptoFloatFast obj)
	{
		return this.GetValue() == obj.GetValue();
	}

	// Token: 0x06002B2A RID: 11050 RVA: 0x001007D0 File Offset: 0x000FE9D0
	public override int GetHashCode()
	{
		return this.GetValue().GetHashCode();
	}

	// Token: 0x06002B2B RID: 11051 RVA: 0x001007EC File Offset: 0x000FE9EC
	public override string ToString()
	{
		return this.GetValue().ToString();
	}

	// Token: 0x06002B2C RID: 11052 RVA: 0x00100808 File Offset: 0x000FEA08
	public string ToString(string format)
	{
		return this.GetValue().ToString(format);
	}

	// Token: 0x06002B2D RID: 11053 RVA: 0x00100824 File Offset: 0x000FEA24
	public string ToString(IFormatProvider provider)
	{
		return this.GetValue().ToString(provider);
	}

	// Token: 0x06002B2E RID: 11054 RVA: 0x00100840 File Offset: 0x000FEA40
	public string ToString(string format, IFormatProvider provider)
	{
		return this.GetValue().ToString(format, provider);
	}

	// Token: 0x06002B2F RID: 11055 RVA: 0x00100860 File Offset: 0x000FEA60
	public static implicit operator CryptoFloatFast(float value)
	{
		CryptoFloatFast result = new CryptoFloatFast(value);
		return result;
	}

	// Token: 0x06002B30 RID: 11056 RVA: 0x0001E057 File Offset: 0x0001C257
	public static implicit operator float(CryptoFloatFast value)
	{
		return value.GetValue();
	}

	// Token: 0x06002B31 RID: 11057 RVA: 0x0001E060 File Offset: 0x0001C260
	public static CryptoFloatFast operator ++(CryptoFloatFast value)
	{
		value.SetValue(value.GetValue() + 1f);
		return value;
	}

	// Token: 0x06002B32 RID: 11058 RVA: 0x0001E077 File Offset: 0x0001C277
	public static CryptoFloatFast operator --(CryptoFloatFast value)
	{
		value.SetValue(value.GetValue() - 1f);
		return value;
	}

	// Token: 0x04001BD5 RID: 7125
	[SerializeField]
	private float defaultValue;

	// Token: 0x04001BD6 RID: 7126
	[SerializeField]
	private float hiddenValue;

	// Token: 0x04001BD7 RID: 7127
	[SerializeField]
	private int randomValue;

	// Token: 0x04001BD8 RID: 7128
	[SerializeField]
	private bool inited;
}
