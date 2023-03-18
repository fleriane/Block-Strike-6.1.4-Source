using System;
using UnityEngine;

// Token: 0x020004E7 RID: 1255
[RequireComponent(typeof(BoxCollider))]
[ExecuteInEditMode]
public class CryptoBoxCollider : MonoBehaviour
{
	// Token: 0x17000482 RID: 1154
	// (get) Token: 0x06002B09 RID: 11017 RVA: 0x0001DE9C File Offset: 0x0001C09C
	public BoxCollider cachedBoxCollider
	{
		get
		{
			if (this.mCollider == null)
			{
				this.mCollider = base.GetComponent<BoxCollider>();
			}
			return this.mCollider;
		}
	}

	// Token: 0x06002B0A RID: 11018 RVA: 0x0001DEC1 File Offset: 0x0001C0C1
	private void OnEnable()
	{
		this.Check();
	}

	// Token: 0x06002B0B RID: 11019 RVA: 0x0001DEC1 File Offset: 0x0001C0C1
	private void OnDisable()
	{
		this.Check();
	}

	// Token: 0x06002B0C RID: 11020 RVA: 0x0001DEC9 File Offset: 0x0001C0C9
	private void OnApplicationFocus(bool focus)
	{
		if (!focus)
		{
			return;
		}
		this.Check();
	}

	// Token: 0x06002B0D RID: 11021 RVA: 0x001004AC File Offset: 0x000FE6AC
	private void Check()
	{
		if (this.cachedBoxCollider.size != this.size)
		{
			CheckManager.Detected();
		}
		if (this.cachedBoxCollider.center != this.center)
		{
			CheckManager.Detected();
		}
	}

	// Token: 0x04001BC3 RID: 7107
	public CryptoVector3 center;

	// Token: 0x04001BC4 RID: 7108
	public CryptoVector3 size;

	// Token: 0x04001BC5 RID: 7109
	private BoxCollider mCollider;
}
