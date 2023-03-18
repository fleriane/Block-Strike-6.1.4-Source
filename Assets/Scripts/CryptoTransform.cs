using System;
using UnityEngine;

// Token: 0x020004F4 RID: 1268
public static class CryptoTransform
{
	// Token: 0x06002B8B RID: 11147 RVA: 0x00101658 File Offset: 0x000FF858
	public static void HackDetector(this Transform target, nVector position, nVector rotation, nVector scale)
	{
		if (!target.hasChanged)
		{
			return;
		}
		if (CryptoTransform.Detected(target.localPosition, position))
		{
			CheckManager.Detected();
		}
		else if (CryptoTransform.Detected(target.localEulerAngles, rotation))
		{
			CheckManager.Detected();
		}
		else if (CryptoTransform.Detected(target.localScale, scale))
		{
			CheckManager.Detected();
		}
	}

	// Token: 0x06002B8C RID: 11148 RVA: 0x001016C0 File Offset: 0x000FF8C0
	private static bool Detected(Vector3 v, nVector a)
	{
		if (a != nVector.Zero)
		{
			return a == nVector.One && v != CryptoTransform.vOne;
		}
		return v != CryptoTransform.vZero;
	}

	// Token: 0x04001BF9 RID: 7161
	private static Vector3 vZero = new Vector3((float)nValue.int0, (float)nValue.int0, (float)nValue.int0);

	// Token: 0x04001BFA RID: 7162
	private static Vector3 vOne = new Vector3((float)nValue.int1, (float)nValue.int1, (float)nValue.int1);
}
