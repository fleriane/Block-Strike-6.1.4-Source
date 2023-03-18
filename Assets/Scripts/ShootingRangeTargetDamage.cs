using System;
using UnityEngine;

// Token: 0x02000403 RID: 1027
public class ShootingRangeTargetDamage : MonoBehaviour
{
	// Token: 0x06002491 RID: 9361 RVA: 0x000DB400 File Offset: 0x000D9600
	private void Damage(DamageInfo damageInfo)
	{
		if (this.Member == PlayerSkinMember.Face)
		{
			damageInfo.headshot = true;
		}
		damageInfo.damage = WeaponManager.GetMemberDamage(this.Member, damageInfo.weapon);
		if (this.Target.GetActive())
		{
			UICrosshair.Hit();
		}
		this.Target.Damage(damageInfo);
		if (ShootingRangeManager.ShowDamage)
		{
			UIToast.Show(Localization.Get("Damage", true) + ": " + damageInfo.damage, 2f);
		}
	}

	// Token: 0x040015D5 RID: 5589
	public PlayerSkinMember Member;

	// Token: 0x040015D6 RID: 5590
	public ShootingRangeTarget Target;
}
