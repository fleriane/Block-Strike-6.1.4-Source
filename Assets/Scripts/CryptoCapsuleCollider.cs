using System;
using UnityEngine;

// Token: 0x020004E8 RID: 1256
[RequireComponent(typeof(CapsuleCollider))]
[ExecuteInEditMode]
public class CryptoCapsuleCollider : MonoBehaviour
{
	// Token: 0x17000483 RID: 1155
	// (get) Token: 0x06002B0F RID: 11023 RVA: 0x0001DED8 File Offset: 0x0001C0D8
	public CapsuleCollider cachedCapsuleCollider
	{
		get
		{
			if (this.mCollider == null)
			{
				this.mCollider = base.GetComponent<CapsuleCollider>();
			}
			return this.mCollider;
		}
	}

	// Token: 0x06002B10 RID: 11024 RVA: 0x0001DEFD File Offset: 0x0001C0FD
	private void OnDisable()
	{
		this.Check();
	}

	// Token: 0x06002B11 RID: 11025 RVA: 0x0001DEFD File Offset: 0x0001C0FD
	private void OnApplicationFocus(bool focus)
	{
		this.Check();
	}

	// Token: 0x06002B12 RID: 11026 RVA: 0x001004FC File Offset: 0x000FE6FC
	private void Check()
	{
		if (this.cachedCapsuleCollider.center != this.center || this.cachedCapsuleCollider.radius != this.radius || this.cachedCapsuleCollider.height != this.height)
		{
			CheckManager.Detected();
		}
	}

	// Token: 0x04001BC6 RID: 7110
	public CryptoVector3 center;

	// Token: 0x04001BC7 RID: 7111
	public CryptoFloat radius;

	// Token: 0x04001BC8 RID: 7112
	public CryptoFloat height;

	// Token: 0x04001BC9 RID: 7113
	private CapsuleCollider mCollider;
}
