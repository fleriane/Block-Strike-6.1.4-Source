using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003FB RID: 1019
public class ShootingRangeFreeFire : MonoBehaviour
{
	// Token: 0x0600247A RID: 9338 RVA: 0x000DABFC File Offset: 0x000D8DFC
	private void Start()
	{
		for (int i = 0; i < this.Targets.Count; i++)
		{
			this.DeactiveTarget.Add(this.Targets[i]);
		}
		this.SetActive(true);
	}

	// Token: 0x0600247B RID: 9339 RVA: 0x000DAC44 File Offset: 0x000D8E44
	private void SetActive(bool active)
	{
		if (this.Activated == active)
		{
			return;
		}
		this.Activated = !this.Activated;
		if (this.Activated)
		{
			for (int i = 0; i < this.MaxTargets; i++)
			{
				ShootingRangeTarget shootingRangeTarget = this.DeactiveTarget[UnityEngine.Random.Range(0, this.DeactiveTarget.Count)];
				shootingRangeTarget.SetActive(true);
				this.DeactiveTarget.Remove(shootingRangeTarget);
				this.ActivateTarget.Add(shootingRangeTarget);
			}
		}
		else
		{
			this.ActivateTarget.Clear();
			this.DeactiveTarget.Clear();
			for (int j = 0; j < this.Targets.Count; j++)
			{
				this.Targets[j].SetActive(false);
				this.DeactiveTarget.Add(this.Targets[j]);
			}
		}
	}

	// Token: 0x0600247C RID: 9340 RVA: 0x000DAD2C File Offset: 0x000D8F2C
	public void DeadTarget(ShootingRangeTarget target)
	{
		ShootingRangeTarget shootingRangeTarget = this.DeactiveTarget[UnityEngine.Random.Range(0, this.DeactiveTarget.Count)];
		shootingRangeTarget.SetActive(true);
		this.DeactiveTarget.Remove(shootingRangeTarget);
		this.ActivateTarget.Add(shootingRangeTarget);
		if (this.ActivateTarget.Contains(target))
		{
			this.ActivateTarget.Remove(target);
			this.DeactiveTarget.Add(target);
		}
	}

	// Token: 0x040015AF RID: 5551
	public List<ShootingRangeTarget> Targets;

	// Token: 0x040015B0 RID: 5552
	private List<ShootingRangeTarget> ActivateTarget = new List<ShootingRangeTarget>();

	// Token: 0x040015B1 RID: 5553
	private List<ShootingRangeTarget> DeactiveTarget = new List<ShootingRangeTarget>();

	// Token: 0x040015B2 RID: 5554
	public int MaxTargets = 5;

	// Token: 0x040015B3 RID: 5555
	private bool Activated;
}
