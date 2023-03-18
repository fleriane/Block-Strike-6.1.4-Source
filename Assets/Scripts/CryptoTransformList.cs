using System;
using UnityEngine;

// Token: 0x020004F5 RID: 1269
public class CryptoTransformList : MonoBehaviour
{
	// Token: 0x06002B8F RID: 11151 RVA: 0x0001E418 File Offset: 0x0001C618
	private void OnEnable()
	{
		this.CheckTransform();
	}

	// Token: 0x06002B90 RID: 11152 RVA: 0x001016FC File Offset: 0x000FF8FC
	public void CheckTransform()
	{
		for (int i = 0; i < this.transforms.Length; i++)
		{
			if (!(this.transforms[i].target == null))
			{
				if (this.transforms[i].target.hasChanged)
				{
					if (this.Detected(this.transforms[i].target.localPosition, this.transforms[i].Position))
					{
						CheckManager.Detected();
					}
					else if (this.Detected(this.transforms[i].target.localEulerAngles, this.transforms[i].Rotation))
					{
						CheckManager.Detected();
					}
					else if (this.Detected(this.transforms[i].target.localScale, this.transforms[i].Scale))
					{
						CheckManager.Detected();
					}
				}
			}
		}
	}

	// Token: 0x06002B91 RID: 11153 RVA: 0x001017F4 File Offset: 0x000FF9F4
	private bool Detected(Vector3 v, CryptoTransformList.Axis a)
	{
		if (a != CryptoTransformList.Axis.Zero)
		{
			return a == CryptoTransformList.Axis.One && v != CryptoTransformList.vOne;
		}
		return v != CryptoTransformList.vZero;
	}

	// Token: 0x04001BFB RID: 7163
	public CryptoTransformList.TransformData[] transforms = new CryptoTransformList.TransformData[0];

	// Token: 0x04001BFC RID: 7164
	private static Vector3 vZero = new Vector3((float)nValue.int0, (float)nValue.int0, (float)nValue.int0);

	// Token: 0x04001BFD RID: 7165
	private static Vector3 vOne = new Vector3((float)nValue.int1, (float)nValue.int1, (float)nValue.int1);

	// Token: 0x020004F6 RID: 1270
	public enum Axis
	{
		// Token: 0x04001BFF RID: 7167
		Off,
		// Token: 0x04001C00 RID: 7168
		Zero,
		// Token: 0x04001C01 RID: 7169
		One
	}

	// Token: 0x020004F7 RID: 1271
	[Serializable]
	public class TransformData
	{
		// Token: 0x04001C02 RID: 7170
		public Transform target;

		// Token: 0x04001C03 RID: 7171
		public CryptoTransformList.Axis Position;

		// Token: 0x04001C04 RID: 7172
		public CryptoTransformList.Axis Rotation;

		// Token: 0x04001C05 RID: 7173
		public CryptoTransformList.Axis Scale = CryptoTransformList.Axis.One;
	}
}
