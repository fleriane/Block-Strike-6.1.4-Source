using System;
using DG.Tweening;
using UnityEngine;

// Token: 0x020003FD RID: 1021
public class ShootingRangeSpeedFire : MonoBehaviour
{
	// Token: 0x06002484 RID: 9348 RVA: 0x00019C10 File Offset: 0x00017E10
	public void StartRoom()
	{
		if (this.isStarting)
		{
			return;
		}
		this.index = 0;
		this.isStarting = true;
		this.Next();
	}

	// Token: 0x06002485 RID: 9349 RVA: 0x000DB0E4 File Offset: 0x000D92E4
	public void Dead()
	{
		this.dead++;
		if (this.Data[this.index].Targets.Length == this.dead)
		{
			this.dead = 0;
			this.index++;
			this.Next();
		}
	}

	// Token: 0x06002486 RID: 9350 RVA: 0x000DB13C File Offset: 0x000D933C
	public void Next()
	{
		MonoBehaviour.print("Next");
		this.Data[this.index].Gate.DOLocalMoveY(-5f, 0.25f, false);
		for (int i = 0; i < this.Data[this.index].Targets.Length; i++)
		{
			this.Data[this.index].Targets[i].SetActive(true);
		}
	}

	// Token: 0x040015BB RID: 5563
	public ShootingRangeSpeedFire.RoomData[] Data;

	// Token: 0x040015BC RID: 5564
	public int dead;

	// Token: 0x040015BD RID: 5565
	public int index;

	// Token: 0x040015BE RID: 5566
	public bool isStarting;

	// Token: 0x020003FE RID: 1022
	[Serializable]
	public class RoomData
	{
		// Token: 0x040015BF RID: 5567
		public Transform Gate;

		// Token: 0x040015C0 RID: 5568
		public ShootingRangeTarget[] Targets;
	}
}
