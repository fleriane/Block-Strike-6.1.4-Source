using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020004F8 RID: 1272
[Serializable]
public struct CryptoVector2 : IEquatable<CryptoVector2>
{
	// Token: 0x06002B93 RID: 11155 RVA: 0x00101830 File Offset: 0x000FFA30
	private CryptoVector2(Vector2 value)
	{
		this.cryptoKey = CryptoManager.random.Next(int.MinValue, int.MaxValue);
		CryptoVector2.FloatIntUnion floatIntUnion = default(CryptoVector2.FloatIntUnion);
		floatIntUnion.f = value.x;
		CryptoVector2.EncryptVector2 encryptVector;
		encryptVector.x = (floatIntUnion.i ^ this.cryptoKey);
		CryptoVector2.FloatIntUnion floatIntUnion2 = default(CryptoVector2.FloatIntUnion);
		floatIntUnion2.f = value.y;
		encryptVector.y = (floatIntUnion2.i ^ this.cryptoKey);
		this.hiddenValue = encryptVector;
		this.fakeValue = ((!CryptoManager.fakeValue) ? Vector2.zero : value);
		this.inited = true;
	}

	// Token: 0x17000486 RID: 1158
	// (get) Token: 0x06002B94 RID: 11156 RVA: 0x001018D8 File Offset: 0x000FFAD8
	// (set) Token: 0x06002B95 RID: 11157 RVA: 0x0001E42F File Offset: 0x0001C62F
	public float x
	{
		get
		{
			float num = this.DecryptFloat(this.hiddenValue.x);
			if (CryptoManager.fakeValue && this.fakeValue.x != num)
			{
				CryptoManager.CheatingDetected();
			}
			return num;
		}
		set
		{
			this.hiddenValue.x = this.EncryptFloat(value);
			if (CryptoManager.fakeValue)
			{
				this.fakeValue.x = value;
			}
		}
	}

	// Token: 0x17000487 RID: 1159
	// (get) Token: 0x06002B96 RID: 11158 RVA: 0x00101918 File Offset: 0x000FFB18
	// (set) Token: 0x06002B97 RID: 11159 RVA: 0x0001E459 File Offset: 0x0001C659
	public float y
	{
		get
		{
			float num = this.DecryptFloat(this.hiddenValue.y);
			if (CryptoManager.fakeValue && this.fakeValue.y != num)
			{
				CryptoManager.CheatingDetected();
			}
			return num;
		}
		set
		{
			this.hiddenValue.y = this.EncryptFloat(value);
			if (CryptoManager.fakeValue)
			{
				this.fakeValue.y = value;
			}
		}
	}

	// Token: 0x06002B98 RID: 11160 RVA: 0x0001E483 File Offset: 0x0001C683
	public void UpdateValue()
	{
		this.SetValue(this.GetValue());
	}

	// Token: 0x06002B99 RID: 11161 RVA: 0x0001E491 File Offset: 0x0001C691
	public void SetValue(Vector2 value)
	{
		this.cryptoKey = CryptoManager.random.Next(int.MinValue, int.MaxValue);
		this.hiddenValue = this.Encrypt(value);
		if (CryptoManager.fakeValue)
		{
			this.fakeValue = value;
		}
	}

	// Token: 0x06002B9A RID: 11162 RVA: 0x00101958 File Offset: 0x000FFB58
	private Vector2 GetValue()
	{
		if (!this.inited)
		{
			this.cryptoKey = CryptoManager.random.Next(int.MinValue, int.MaxValue);
			this.hiddenValue = this.Encrypt(Vector2.zero);
			this.fakeValue = Vector2.zero;
			this.inited = true;
		}
		Vector2 vector = this.Decrypt(this.hiddenValue);
		if (CryptoManager.fakeValue && this.fakeValue != vector)
		{
			CryptoManager.CheatingDetected();
		}
		return vector;
	}

	// Token: 0x06002B9B RID: 11163 RVA: 0x001019DC File Offset: 0x000FFBDC
	private CryptoVector2.EncryptVector2 Encrypt(Vector2 value)
	{
		CryptoVector2.EncryptVector2 result;
		result.x = this.EncryptFloat(value.x);
		result.y = this.EncryptFloat(value.y);
		return result;
	}

	// Token: 0x06002B9C RID: 11164 RVA: 0x00101A14 File Offset: 0x000FFC14
	private int EncryptFloat(float value)
	{
		CryptoVector2.FloatIntUnion floatIntUnion = default(CryptoVector2.FloatIntUnion);
		floatIntUnion.f = value;
		floatIntUnion.i ^= this.cryptoKey;
		return floatIntUnion.i;
	}

	// Token: 0x06002B9D RID: 11165 RVA: 0x00101A50 File Offset: 0x000FFC50
	private Vector2 Decrypt(CryptoVector2.EncryptVector2 value)
	{
		Vector2 result;
		result.x = this.DecryptFloat(value.x);
		result.y = this.DecryptFloat(value.y);
		return result;
	}

	// Token: 0x06002B9E RID: 11166 RVA: 0x00101A88 File Offset: 0x000FFC88
	private float DecryptFloat(int value)
	{
		CryptoVector2.FloatIntUnion floatIntUnion = default(CryptoVector2.FloatIntUnion);
		floatIntUnion.i = (value ^ this.cryptoKey);
		return floatIntUnion.f;
	}

	// Token: 0x06002B9F RID: 11167 RVA: 0x0001E4CB File Offset: 0x0001C6CB
	public override bool Equals(object obj)
	{
		return obj is CryptoVector2 && this.Equals((CryptoVector2)obj);
	}

	// Token: 0x06002BA0 RID: 11168 RVA: 0x0001E4E6 File Offset: 0x0001C6E6
	public bool Equals(CryptoVector2 obj)
	{
		return this.GetValue() == obj.GetValue();
	}

	// Token: 0x06002BA1 RID: 11169 RVA: 0x00101AB4 File Offset: 0x000FFCB4
	public override int GetHashCode()
	{
		return this.GetValue().GetHashCode();
	}

	// Token: 0x06002BA2 RID: 11170 RVA: 0x00101AD0 File Offset: 0x000FFCD0
	public override string ToString()
	{
		return this.GetValue().ToString();
	}

	// Token: 0x06002BA3 RID: 11171 RVA: 0x0001E4FA File Offset: 0x0001C6FA
	public static implicit operator CryptoVector2(Vector2 value)
	{
		return new CryptoVector2(value);
	}

	// Token: 0x06002BA4 RID: 11172 RVA: 0x0001E502 File Offset: 0x0001C702
	public static implicit operator Vector2(CryptoVector2 value)
	{
		return value.GetValue();
	}

	// Token: 0x06002BA5 RID: 11173 RVA: 0x0001E50B File Offset: 0x0001C70B
	public static CryptoVector2 operator +(CryptoVector2 a, CryptoVector2 b)
	{
		return a.GetValue() + b.GetValue();
	}

	// Token: 0x06002BA6 RID: 11174 RVA: 0x0001E525 File Offset: 0x0001C725
	public static CryptoVector2 operator +(Vector2 a, CryptoVector2 b)
	{
		return a + b.GetValue();
	}

	// Token: 0x06002BA7 RID: 11175 RVA: 0x0001E539 File Offset: 0x0001C739
	public static CryptoVector2 operator +(CryptoVector2 a, Vector2 b)
	{
		return a.GetValue() + b;
	}

	// Token: 0x06002BA8 RID: 11176 RVA: 0x0001E54D File Offset: 0x0001C74D
	public static CryptoVector2 operator -(CryptoVector2 a, CryptoVector2 b)
	{
		return a.GetValue() - b.GetValue();
	}

	// Token: 0x06002BA9 RID: 11177 RVA: 0x0001E567 File Offset: 0x0001C767
	public static CryptoVector2 operator -(Vector2 a, CryptoVector2 b)
	{
		return a - b.GetValue();
	}

	// Token: 0x06002BAA RID: 11178 RVA: 0x0001E57B File Offset: 0x0001C77B
	public static CryptoVector2 operator -(CryptoVector2 a, Vector2 b)
	{
		return a.GetValue() - b;
	}

	// Token: 0x06002BAB RID: 11179 RVA: 0x0001E58F File Offset: 0x0001C78F
	public static CryptoVector2 operator -(CryptoVector2 a)
	{
		return -a.GetValue();
	}

	// Token: 0x06002BAC RID: 11180 RVA: 0x0001E5A2 File Offset: 0x0001C7A2
	public static CryptoVector2 operator *(CryptoVector2 a, float d)
	{
		return a.GetValue() * d;
	}

	// Token: 0x06002BAD RID: 11181 RVA: 0x0001E5B6 File Offset: 0x0001C7B6
	public static CryptoVector2 operator *(float d, CryptoVector2 a)
	{
		return d * a.GetValue();
	}

	// Token: 0x06002BAE RID: 11182 RVA: 0x0001E5CA File Offset: 0x0001C7CA
	public static CryptoVector2 operator /(CryptoVector2 a, float d)
	{
		return a.GetValue() / d;
	}

	// Token: 0x06002BAF RID: 11183 RVA: 0x0001E5DE File Offset: 0x0001C7DE
	public static bool operator ==(CryptoVector2 lhs, CryptoVector2 rhs)
	{
		return lhs.GetValue() == rhs.GetValue();
	}

	// Token: 0x06002BB0 RID: 11184 RVA: 0x0001E5F3 File Offset: 0x0001C7F3
	public static bool operator ==(Vector2 lhs, CryptoVector2 rhs)
	{
		return lhs == rhs.GetValue();
	}

	// Token: 0x06002BB1 RID: 11185 RVA: 0x0001E602 File Offset: 0x0001C802
	public static bool operator ==(CryptoVector2 lhs, Vector2 rhs)
	{
		return lhs.GetValue() == rhs;
	}

	// Token: 0x06002BB2 RID: 11186 RVA: 0x0001E611 File Offset: 0x0001C811
	public static bool operator !=(CryptoVector2 lhs, CryptoVector2 rhs)
	{
		return lhs.GetValue() != rhs.GetValue();
	}

	// Token: 0x06002BB3 RID: 11187 RVA: 0x0001E626 File Offset: 0x0001C826
	public static bool operator !=(Vector2 lhs, CryptoVector2 rhs)
	{
		return lhs != rhs.GetValue();
	}

	// Token: 0x06002BB4 RID: 11188 RVA: 0x0001E635 File Offset: 0x0001C835
	public static bool operator !=(CryptoVector2 lhs, Vector2 rhs)
	{
		return lhs.GetValue() != rhs;
	}

	// Token: 0x04001C06 RID: 7174
	[SerializeField]
	private int cryptoKey;

	// Token: 0x04001C07 RID: 7175
	[SerializeField]
	private CryptoVector2.EncryptVector2 hiddenValue;

	// Token: 0x04001C08 RID: 7176
	[SerializeField]
	private Vector2 fakeValue;

	// Token: 0x04001C09 RID: 7177
	[SerializeField]
	private bool inited;

	// Token: 0x020004F9 RID: 1273
	[Serializable]
	public struct EncryptVector2
	{
		// Token: 0x04001C0A RID: 7178
		public int x;

		// Token: 0x04001C0B RID: 7179
		public int y;
	}

	// Token: 0x020004FA RID: 1274
	[StructLayout(LayoutKind.Explicit)]
	private struct FloatIntUnion
	{
		// Token: 0x04001C0C RID: 7180
		[FieldOffset(0)]
		public float f;

		// Token: 0x04001C0D RID: 7181
		[FieldOffset(0)]
		public int i;
	}
}
