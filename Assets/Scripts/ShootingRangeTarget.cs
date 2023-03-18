using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020003FF RID: 1023
public class ShootingRangeTarget : MonoBehaviour
{
	// Token: 0x06002489 RID: 9353 RVA: 0x00019C42 File Offset: 0x00017E42
	private void Start()
	{
		this.CacheTransform = base.transform;
	}

	// Token: 0x0600248A RID: 9354 RVA: 0x000DB1B8 File Offset: 0x000D93B8
	public void SetActive(bool active)
	{
		this.Activated = active;
		this.Health = 100;
		if (this.CacheTransform == null)
		{
			this.CacheTransform = base.transform;
		}
		if (this.Activated)
		{
			if (this.Position.Use)
			{
				this.CacheTransform.position = this.Position.Default;
				if (this.Position.Loop)
				{
					this.CacheTransform.DOMove(this.Position.Activated, this.Position.Duration, false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
				}
				else
				{
					this.CacheTransform.DOMove(this.Position.Activated, this.Position.Duration, false);
				}
			}
			if (this.Rotation.Use)
			{
				this.CacheTransform.eulerAngles = this.Rotation.Default;
				this.CacheTransform.DORotate(this.Rotation.Activated, this.Rotation.Duration, RotateMode.Fast);
			}
			if (this.RandomPosition.Use)
			{
				this.CacheTransform.position = this.RandomPosition.Positions[UnityEngine.Random.Range(0, this.RandomPosition.Positions.Length)];
			}
		}
		else
		{
			if (this.Position.Use)
			{
				if (this.Position.Loop)
				{
					this.CacheTransform.DOKill(false);
				}
				else
				{
					this.CacheTransform.DOMove(this.Position.Default, this.Position.Duration, false);
				}
			}
			if (this.Rotation.Use)
			{
				this.CacheTransform.DORotate(this.Rotation.Default, this.Rotation.Duration, RotateMode.Fast);
			}
		}
	}

	// Token: 0x0600248B RID: 9355 RVA: 0x00019C50 File Offset: 0x00017E50
	public bool GetActive()
	{
		return this.Activated;
	}

	// Token: 0x0600248C RID: 9356 RVA: 0x000DB3A4 File Offset: 0x000D95A4
	public void Damage(DamageInfo damageInfo)
	{
		if (this.Activated)
		{
			this.Health -= damageInfo.damage;
			this.Health = Mathf.Max(this.Health, 0);
			if (this.Health == 0)
			{
				this.DeadCallback.Invoke();
				this.SetActive(false);
			}
		}
	}

	// Token: 0x040015C1 RID: 5569
	public ShootingRangeTarget.PositionClass Position;

	// Token: 0x040015C2 RID: 5570
	public ShootingRangeTarget.RotationClass Rotation;

	// Token: 0x040015C3 RID: 5571
	public ShootingRangeTarget.RandomPositionClass RandomPosition;

	// Token: 0x040015C4 RID: 5572
	public int Health = 100;

	// Token: 0x040015C5 RID: 5573
	public UnityEvent DeadCallback;

	// Token: 0x040015C6 RID: 5574
	private bool Activated;

	// Token: 0x040015C7 RID: 5575
	private Transform CacheTransform;

	// Token: 0x02000400 RID: 1024
	[Serializable]
	public class PositionClass
	{
		// Token: 0x040015C8 RID: 5576
		public bool Use;

		// Token: 0x040015C9 RID: 5577
		public Vector3 Default;

		// Token: 0x040015CA RID: 5578
		public Vector3 Activated;

		// Token: 0x040015CB RID: 5579
		public float Duration = 1f;

		// Token: 0x040015CC RID: 5580
		public bool Loop;

		// Token: 0x040015CD RID: 5581
		public Tweener Tween;
	}

	// Token: 0x02000401 RID: 1025
	[Serializable]
	public class RotationClass
	{
		// Token: 0x040015CE RID: 5582
		public bool Use;

		// Token: 0x040015CF RID: 5583
		public Vector3 Default;

		// Token: 0x040015D0 RID: 5584
		public Vector3 Activated;

		// Token: 0x040015D1 RID: 5585
		public float Duration = 1f;

		// Token: 0x040015D2 RID: 5586
		public Tweener Tween;
	}

	// Token: 0x02000402 RID: 1026
	[Serializable]
	public class RandomPositionClass
	{
		// Token: 0x040015D3 RID: 5587
		public bool Use;

		// Token: 0x040015D4 RID: 5588
		public Vector3[] Positions;
	}
}
