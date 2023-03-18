using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020004FB RID: 1275
[Serializable]
public struct CryptoVector3 : IEquatable<CryptoVector3>
{
	// Token: 0x06002BB5 RID: 11189 RVA: 0x00101AEC File Offset: 0x000FFCEC
	private CryptoVector3(Vector3 value)
	{
		this.cryptoKey = CryptoManager.random.Next(int.MinValue, int.MaxValue);
		CryptoVector3.FloatIntUnion floatIntUnion = default(CryptoVector3.FloatIntUnion);
		floatIntUnion.f = value.x;
		CryptoVector3.EncryptVector3 encryptVector;
		encryptVector.x = (floatIntUnion.i ^ this.cryptoKey);
		CryptoVector3.FloatIntUnion floatIntUnion2 = default(CryptoVector3.FloatIntUnion);
		floatIntUnion2.f = value.y;
		encryptVector.y = (floatIntUnion2.i ^ this.cryptoKey);
		CryptoVector3.FloatIntUnion floatIntUnion3 = default(CryptoVector3.FloatIntUnion);
		floatIntUnion3.f = value.z;
		encryptVector.z = (floatIntUnion3.i ^ this.cryptoKey);
		this.hiddenValue = encryptVector;
		this.fakeValue = ((!CryptoManager.fakeValue) ? Vector3.zero : value);
		this.inited = true;
	}

	// Token: 0x17000488 RID: 1160
	// (get) Token: 0x06002BB6 RID: 11190 RVA: 0x00101BC0 File Offset: 0x000FFDC0
	// (set) Token: 0x06002BB7 RID: 11191 RVA: 0x0001E644 File Offset: 0x0001C844
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

	// Token: 0x17000489 RID: 1161
	// (get) Token: 0x06002BB8 RID: 11192 RVA: 0x00101C00 File Offset: 0x000FFE00
	// (set) Token: 0x06002BB9 RID: 11193 RVA: 0x0001E66E File Offset: 0x0001C86E
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

	// Token: 0x1700048A RID: 1162
	// (get) Token: 0x06002BBA RID: 11194 RVA: 0x00101C40 File Offset: 0x000FFE40
	// (set) Token: 0x06002BBB RID: 11195 RVA: 0x0001E698 File Offset: 0x0001C898
	public float z
	{
		get
		{
			float num = this.DecryptFloat(this.hiddenValue.z);
			if (CryptoManager.fakeValue && this.fakeValue.z != num)
			{
				CryptoManager.CheatingDetected();
			}
			return num;
		}
		set
		{
			this.hiddenValue.z = this.EncryptFloat(value);
			if (CryptoManager.fakeValue)
			{
				this.fakeValue.z = value;
			}
		}
	}

	// Token: 0x06002BBC RID: 11196 RVA: 0x0001E6C2 File Offset: 0x0001C8C2
	public void UpdateValue()
	{
		this.SetValue(this.GetValue());
	}

	// Token: 0x06002BBD RID: 11197 RVA: 0x0001E6D0 File Offset: 0x0001C8D0
	public void SetValue(Vector3 value)
	{
		this.cryptoKey = CryptoManager.random.Next(int.MinValue, int.MaxValue);
		this.hiddenValue = this.Encrypt(value);
		if (CryptoManager.fakeValue)
		{
			this.fakeValue = value;
		}
	}

	// Token: 0x06002BBE RID: 11198 RVA: 0x00101C80 File Offset: 0x000FFE80
	private Vector3 GetValue()
	{
		if (!this.inited)
		{
			this.cryptoKey = CryptoManager.random.Next(int.MinValue, int.MaxValue);
			this.hiddenValue = this.Encrypt(Vector3.zero);
			this.fakeValue = Vector3.zero;
			this.inited = true;
		}
		Vector3 vector = this.Decrypt(this.hiddenValue);
		if (CryptoManager.fakeValue && this.fakeValue != vector)
		{
			CryptoManager.CheatingDetected();
		}
		return vector;
	}

	// Token: 0x06002BBF RID: 11199 RVA: 0x00101D04 File Offset: 0x000FFF04
	private CryptoVector3.EncryptVector3 Encrypt(Vector3 value)
	{
		CryptoVector3.EncryptVector3 result;
		result.x = this.EncryptFloat(value.x);
		result.y = this.EncryptFloat(value.y);
		result.z = this.EncryptFloat(value.z);
		return result;
	}

	// Token: 0x06002BC0 RID: 11200 RVA: 0x00101D50 File Offset: 0x000FFF50
	private int EncryptFloat(float value)
	{
		CryptoVector3.FloatIntUnion floatIntUnion = default(CryptoVector3.FloatIntUnion);
		floatIntUnion.f = value;
		floatIntUnion.i ^= this.cryptoKey;
		return floatIntUnion.i;
	}

	// Token: 0x06002BC1 RID: 11201 RVA: 0x00101D8C File Offset: 0x000FFF8C
	private Vector3 Decrypt(CryptoVector3.EncryptVector3 value)
	{
		Vector3 result;
		result.x = this.DecryptFloat(value.x);
		result.y = this.DecryptFloat(value.y);
		result.z = this.DecryptFloat(value.z);
		return result;
	}

	// Token: 0x06002BC2 RID: 11202 RVA: 0x00101DD8 File Offset: 0x000FFFD8
	private float DecryptFloat(int value)
	{
		CryptoVector3.FloatIntUnion floatIntUnion = default(CryptoVector3.FloatIntUnion);
		floatIntUnion.i = (value ^ this.cryptoKey);
		return floatIntUnion.f;
	}

	// Token: 0x06002BC3 RID: 11203 RVA: 0x0001E70A File Offset: 0x0001C90A
	public override bool Equals(object obj)
	{
		return obj is CryptoVector3 && this.Equals((CryptoVector3)obj);
	}

	// Token: 0x06002BC4 RID: 11204 RVA: 0x0001E725 File Offset: 0x0001C925
	public bool Equals(CryptoVector3 obj)
	{
		return this.GetValue() == obj.GetValue();
	}

	// Token: 0x06002BC5 RID: 11205 RVA: 0x00101E04 File Offset: 0x00100004
	public override int GetHashCode()
	{
		return this.GetValue().GetHashCode();
	}

	// Token: 0x06002BC6 RID: 11206 RVA: 0x00101E20 File Offset: 0x00100020
	public override string ToString()
	{
		return this.GetValue().ToString();
	}

	// Token: 0x06002BC7 RID: 11207 RVA: 0x0001E739 File Offset: 0x0001C939
	public static implicit operator CryptoVector3(Vector3 value)
	{
		return new CryptoVector3(value);
	}

	// Token: 0x06002BC8 RID: 11208 RVA: 0x0001E741 File Offset: 0x0001C941
	public static implicit operator Vector3(CryptoVector3 value)
	{
		return value.GetValue();
	}

	// Token: 0x06002BC9 RID: 11209 RVA: 0x0001E74A File Offset: 0x0001C94A
	public static CryptoVector3 operator +(CryptoVector3 a, CryptoVector3 b)
	{
		return a.GetValue() + b.GetValue();
	}

	// Token: 0x06002BCA RID: 11210 RVA: 0x0001E764 File Offset: 0x0001C964
	public static CryptoVector3 operator +(Vector3 a, CryptoVector3 b)
	{
		return a + b.GetValue();
	}

	// Token: 0x06002BCB RID: 11211 RVA: 0x0001E778 File Offset: 0x0001C978
	public static CryptoVector3 operator +(CryptoVector3 a, Vector3 b)
	{
		return a.GetValue() + b;
	}

	// Token: 0x06002BCC RID: 11212 RVA: 0x0001E78C File Offset: 0x0001C98C
	public static CryptoVector3 operator -(CryptoVector3 a, CryptoVector3 b)
	{
		return a.GetValue() - b.GetValue();
	}

	// Token: 0x06002BCD RID: 11213 RVA: 0x0001E7A6 File Offset: 0x0001C9A6
	public static CryptoVector3 operator -(Vector3 a, CryptoVector3 b)
	{
		return a - b.GetValue();
	}

	// Token: 0x06002BCE RID: 11214 RVA: 0x0001E7BA File Offset: 0x0001C9BA
	public static CryptoVector3 operator -(CryptoVector3 a, Vector3 b)
	{
		return a.GetValue() - b;
	}

	// Token: 0x06002BCF RID: 11215 RVA: 0x0001E7CE File Offset: 0x0001C9CE
	public static CryptoVector3 operator -(CryptoVector3 a)
	{
		return -a.GetValue();
	}

	// Token: 0x06002BD0 RID: 11216 RVA: 0x0001E7E1 File Offset: 0x0001C9E1
	public static CryptoVector3 operator *(CryptoVector3 a, float d)
	{
		return a.GetValue() * d;
	}

	// Token: 0x06002BD1 RID: 11217 RVA: 0x0001E7F5 File Offset: 0x0001C9F5
	public static CryptoVector3 operator *(float d, CryptoVector3 a)
	{
		return d * a.GetValue();
	}

	// Token: 0x06002BD2 RID: 11218 RVA: 0x0001E809 File Offset: 0x0001CA09
	public static CryptoVector3 operator /(CryptoVector3 a, float d)
	{
		return a.GetValue() / d;
	}

	// Token: 0x06002BD3 RID: 11219 RVA: 0x0001E81D File Offset: 0x0001CA1D
	public static bool operator ==(CryptoVector3 lhs, CryptoVector3 rhs)
	{
		return lhs.GetValue() == rhs.GetValue();
	}

	// Token: 0x06002BD4 RID: 11220 RVA: 0x0001E832 File Offset: 0x0001CA32
	public static bool operator ==(Vector3 lhs, CryptoVector3 rhs)
	{
		return lhs == rhs.GetValue();
	}

	// Token: 0x06002BD5 RID: 11221 RVA: 0x0001E841 File Offset: 0x0001CA41
	public static bool operator ==(CryptoVector3 lhs, Vector3 rhs)
	{
		return lhs.GetValue() == rhs;
	}

	// Token: 0x06002BD6 RID: 11222 RVA: 0x0001E850 File Offset: 0x0001CA50
	public static bool operator !=(CryptoVector3 lhs, CryptoVector3 rhs)
	{
		return lhs.GetValue() != rhs.GetValue();
	}

	// Token: 0x06002BD7 RID: 11223 RVA: 0x0001E865 File Offset: 0x0001CA65
	public static bool operator !=(Vector3 lhs, CryptoVector3 rhs)
	{
		return lhs != rhs.GetValue();
	}

	// Token: 0x06002BD8 RID: 11224 RVA: 0x0001E874 File Offset: 0x0001CA74
	public static bool operator !=(CryptoVector3 lhs, Vector3 rhs)
	{
		return lhs.GetValue() != rhs;
	}

	// Token: 0x04001C0E RID: 7182
	[SerializeField]
	private int cryptoKey;

	// Token: 0x04001C0F RID: 7183
	[SerializeField]
	private CryptoVector3.EncryptVector3 hiddenValue;

	// Token: 0x04001C10 RID: 7184
	[SerializeField]
	private Vector3 fakeValue;

	// Token: 0x04001C11 RID: 7185
	[SerializeField]
	private bool inited;

	// Token: 0x020004FC RID: 1276
	[Serializable]
	public struct EncryptVector3
	{
		// Token: 0x04001C12 RID: 7186
		public int x;

		// Token: 0x04001C13 RID: 7187
		public int y;

		// Token: 0x04001C14 RID: 7188
		public int z;
	}

	// Token: 0x020004FD RID: 1277
	[Serializable]
	private struct Byte4
	{
		// Token: 0x04001C15 RID: 7189
		public byte b1;

		// Token: 0x04001C16 RID: 7190
		public byte b2;

		// Token: 0x04001C17 RID: 7191
		public byte b3;

		// Token: 0x04001C18 RID: 7192
		public byte b4;
	}

	// Token: 0x020004FE RID: 1278
	[StructLayout(LayoutKind.Explicit)]
	private struct FloatIntUnion
	{
		// Token: 0x04001C19 RID: 7193
		[FieldOffset(0)]
		public float f;

		// Token: 0x04001C1A RID: 7194
		[FieldOffset(0)]
		public int i;
	}
}
