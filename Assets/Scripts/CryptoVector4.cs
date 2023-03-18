using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020004FF RID: 1279
[Serializable]
public struct CryptoVector4 : IEquatable<CryptoVector4>
{
	// Token: 0x06002BD9 RID: 11225 RVA: 0x00101E3C File Offset: 0x0010003C
	private CryptoVector4(Vector4 value)
	{
		this.cryptoKey = CryptoManager.random.Next(int.MinValue, int.MaxValue);
		CryptoVector4.FloatIntUnion floatIntUnion = default(CryptoVector4.FloatIntUnion);
		floatIntUnion.f = value.x;
		CryptoVector4.EncryptVector4 encryptVector;
		encryptVector.x = (floatIntUnion.i ^ this.cryptoKey);
		CryptoVector4.FloatIntUnion floatIntUnion2 = default(CryptoVector4.FloatIntUnion);
		floatIntUnion2.f = value.y;
		encryptVector.y = (floatIntUnion2.i ^ this.cryptoKey);
		CryptoVector4.FloatIntUnion floatIntUnion3 = default(CryptoVector4.FloatIntUnion);
		floatIntUnion3.f = value.z;
		encryptVector.z = (floatIntUnion3.i ^ this.cryptoKey);
		CryptoVector4.FloatIntUnion floatIntUnion4 = default(CryptoVector4.FloatIntUnion);
		floatIntUnion4.f = value.w;
		encryptVector.w = (floatIntUnion4.i ^ this.cryptoKey);
		this.hiddenValue = encryptVector;
		this.fakeValue = value;
		this.inited = true;
	}

	// Token: 0x1700048B RID: 1163
	// (get) Token: 0x06002BDA RID: 11226 RVA: 0x00101F24 File Offset: 0x00100124
	public float x
	{
		get
		{
			float num = this.DecryptFloat(this.hiddenValue.x);
			if (this.fakeValue.x != num)
			{
				CryptoManager.CheatingDetected();
			}
			return num;
		}
	}

	// Token: 0x1700048C RID: 1164
	// (get) Token: 0x06002BDB RID: 11227 RVA: 0x00101F5C File Offset: 0x0010015C
	public float y
	{
		get
		{
			float num = this.DecryptFloat(this.hiddenValue.y);
			if (this.fakeValue.y != num)
			{
				CryptoManager.CheatingDetected();
			}
			return num;
		}
	}

	// Token: 0x1700048D RID: 1165
	// (get) Token: 0x06002BDC RID: 11228 RVA: 0x00101F94 File Offset: 0x00100194
	public float z
	{
		get
		{
			float num = this.DecryptFloat(this.hiddenValue.z);
			if (this.fakeValue.z != num)
			{
				CryptoManager.CheatingDetected();
			}
			return num;
		}
	}

	// Token: 0x1700048E RID: 1166
	// (get) Token: 0x06002BDD RID: 11229 RVA: 0x00101FCC File Offset: 0x001001CC
	public float w
	{
		get
		{
			float num = this.DecryptFloat(this.hiddenValue.w);
			if (this.fakeValue.z != num)
			{
				CryptoManager.CheatingDetected();
			}
			return num;
		}
	}

	// Token: 0x06002BDE RID: 11230 RVA: 0x0001E883 File Offset: 0x0001CA83
	public void SetValue(Vector4 value)
	{
		this.cryptoKey = CryptoManager.random.Next(int.MinValue, int.MaxValue);
		this.hiddenValue = this.Encrypt(value);
		this.fakeValue = value;
	}

	// Token: 0x06002BDF RID: 11231 RVA: 0x00102004 File Offset: 0x00100204
	private Vector4 GetValue()
	{
		if (!this.inited)
		{
			this.cryptoKey = CryptoManager.random.Next(int.MinValue, int.MaxValue);
			this.hiddenValue = this.Encrypt(Vector4.zero);
			this.fakeValue = Vector4.zero;
			this.inited = true;
		}
		Vector4 vector = this.Decrypt(this.hiddenValue);
		if (this.fakeValue != vector)
		{
			CryptoManager.CheatingDetected();
		}
		return vector;
	}

	// Token: 0x06002BE0 RID: 11232 RVA: 0x00102080 File Offset: 0x00100280
	private CryptoVector4.EncryptVector4 Encrypt(Vector4 value)
	{
		CryptoVector4.EncryptVector4 result;
		result.x = this.EncryptFloat(value.x);
		result.y = this.EncryptFloat(value.y);
		result.z = this.EncryptFloat(value.z);
		result.w = this.EncryptFloat(value.w);
		return result;
	}

	// Token: 0x06002BE1 RID: 11233 RVA: 0x001020E0 File Offset: 0x001002E0
	private int EncryptFloat(float value)
	{
		CryptoVector4.FloatIntUnion floatIntUnion = default(CryptoVector4.FloatIntUnion);
		floatIntUnion.f = value;
		floatIntUnion.i ^= this.cryptoKey;
		return floatIntUnion.i;
	}

	// Token: 0x06002BE2 RID: 11234 RVA: 0x0010211C File Offset: 0x0010031C
	private Vector4 Decrypt(CryptoVector4.EncryptVector4 value)
	{
		Vector4 result;
		result.x = this.DecryptFloat(value.x);
		result.y = this.DecryptFloat(value.y);
		result.z = this.DecryptFloat(value.z);
		result.w = this.DecryptFloat(value.w);
		return result;
	}

	// Token: 0x06002BE3 RID: 11235 RVA: 0x0010217C File Offset: 0x0010037C
	private float DecryptFloat(int value)
	{
		CryptoVector4.FloatIntUnion floatIntUnion = default(CryptoVector4.FloatIntUnion);
		floatIntUnion.i = (value ^ this.cryptoKey);
		return floatIntUnion.f;
	}

	// Token: 0x06002BE4 RID: 11236 RVA: 0x0001E8B3 File Offset: 0x0001CAB3
	public override bool Equals(object obj)
	{
		return obj is CryptoVector4 && this.Equals((CryptoVector4)obj);
	}

	// Token: 0x06002BE5 RID: 11237 RVA: 0x0001E8CE File Offset: 0x0001CACE
	public bool Equals(CryptoVector4 obj)
	{
		return this.GetValue() == obj.GetValue();
	}

	// Token: 0x06002BE6 RID: 11238 RVA: 0x001021A8 File Offset: 0x001003A8
	public override int GetHashCode()
	{
		return this.GetValue().GetHashCode();
	}

	// Token: 0x06002BE7 RID: 11239 RVA: 0x001021C4 File Offset: 0x001003C4
	public override string ToString()
	{
		return this.GetValue().ToString();
	}

	// Token: 0x06002BE8 RID: 11240 RVA: 0x0001E8E2 File Offset: 0x0001CAE2
	public static implicit operator CryptoVector4(Vector4 value)
	{
		return new CryptoVector4(value);
	}

	// Token: 0x06002BE9 RID: 11241 RVA: 0x0001E8EA File Offset: 0x0001CAEA
	public static implicit operator Vector4(CryptoVector4 value)
	{
		return value.GetValue();
	}

	// Token: 0x06002BEA RID: 11242 RVA: 0x0001E8F3 File Offset: 0x0001CAF3
	public static Vector4 operator +(CryptoVector4 a, CryptoVector4 b)
	{
		return a.GetValue() + b.GetValue();
	}

	// Token: 0x06002BEB RID: 11243 RVA: 0x0001E908 File Offset: 0x0001CB08
	public static Vector4 operator +(Vector4 a, CryptoVector4 b)
	{
		return a + b.GetValue();
	}

	// Token: 0x06002BEC RID: 11244 RVA: 0x0001E917 File Offset: 0x0001CB17
	public static Vector4 operator +(CryptoVector4 a, Vector4 b)
	{
		return a.GetValue() + b;
	}

	// Token: 0x06002BED RID: 11245 RVA: 0x0001E926 File Offset: 0x0001CB26
	public static Vector4 operator -(CryptoVector4 a, CryptoVector4 b)
	{
		return a.GetValue() - b.GetValue();
	}

	// Token: 0x06002BEE RID: 11246 RVA: 0x0001E93B File Offset: 0x0001CB3B
	public static Vector4 operator -(Vector4 a, CryptoVector4 b)
	{
		return a - b.GetValue();
	}

	// Token: 0x06002BEF RID: 11247 RVA: 0x0001E94A File Offset: 0x0001CB4A
	public static Vector4 operator -(CryptoVector4 a, Vector4 b)
	{
		return a.GetValue() - b;
	}

	// Token: 0x06002BF0 RID: 11248 RVA: 0x0001E959 File Offset: 0x0001CB59
	public static Vector4 operator -(CryptoVector4 a)
	{
		return -a.GetValue();
	}

	// Token: 0x06002BF1 RID: 11249 RVA: 0x0001E967 File Offset: 0x0001CB67
	public static Vector4 operator *(CryptoVector4 a, float d)
	{
		return a.GetValue() * d;
	}

	// Token: 0x06002BF2 RID: 11250 RVA: 0x0001E976 File Offset: 0x0001CB76
	public static Vector4 operator *(float d, CryptoVector4 a)
	{
		return d * a.GetValue();
	}

	// Token: 0x06002BF3 RID: 11251 RVA: 0x0001E985 File Offset: 0x0001CB85
	public static Vector4 operator /(CryptoVector4 a, float d)
	{
		return a.GetValue() / d;
	}

	// Token: 0x06002BF4 RID: 11252 RVA: 0x0001E994 File Offset: 0x0001CB94
	public static bool operator ==(CryptoVector4 lhs, CryptoVector4 rhs)
	{
		return lhs.GetValue() == rhs.GetValue();
	}

	// Token: 0x06002BF5 RID: 11253 RVA: 0x0001E9A9 File Offset: 0x0001CBA9
	public static bool operator ==(Vector4 lhs, CryptoVector4 rhs)
	{
		return lhs == rhs.GetValue();
	}

	// Token: 0x06002BF6 RID: 11254 RVA: 0x0001E9B8 File Offset: 0x0001CBB8
	public static bool operator ==(CryptoVector4 lhs, Vector4 rhs)
	{
		return lhs.GetValue() == rhs;
	}

	// Token: 0x06002BF7 RID: 11255 RVA: 0x0001E9C7 File Offset: 0x0001CBC7
	public static bool operator !=(CryptoVector4 lhs, CryptoVector4 rhs)
	{
		return lhs.GetValue() != rhs.GetValue();
	}

	// Token: 0x06002BF8 RID: 11256 RVA: 0x0001E9DC File Offset: 0x0001CBDC
	public static bool operator !=(Vector4 lhs, CryptoVector4 rhs)
	{
		return lhs != rhs.GetValue();
	}

	// Token: 0x06002BF9 RID: 11257 RVA: 0x0001E9EB File Offset: 0x0001CBEB
	public static bool operator !=(CryptoVector4 lhs, Vector4 rhs)
	{
		return lhs.GetValue() != rhs;
	}

	// Token: 0x04001C1B RID: 7195
	[SerializeField]
	private int cryptoKey;

	// Token: 0x04001C1C RID: 7196
	[SerializeField]
	private CryptoVector4.EncryptVector4 hiddenValue;

	// Token: 0x04001C1D RID: 7197
	[SerializeField]
	private Vector4 fakeValue;

	// Token: 0x04001C1E RID: 7198
	[SerializeField]
	private bool inited;

	// Token: 0x02000500 RID: 1280
	[Serializable]
	public struct EncryptVector4
	{
		// Token: 0x04001C1F RID: 7199
		public int x;

		// Token: 0x04001C20 RID: 7200
		public int y;

		// Token: 0x04001C21 RID: 7201
		public int z;

		// Token: 0x04001C22 RID: 7202
		public int w;
	}

	// Token: 0x02000501 RID: 1281
	[StructLayout(LayoutKind.Explicit)]
	private struct FloatIntUnion
	{
		// Token: 0x04001C23 RID: 7203
		[FieldOffset(0)]
		public float f;

		// Token: 0x04001C24 RID: 7204
		[FieldOffset(0)]
		public int i;
	}
}
