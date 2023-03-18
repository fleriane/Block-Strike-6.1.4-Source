using System;
using UnityEngine;

// Token: 0x020003FA RID: 1018
public class ShootingRangeAmmo : MonoBehaviour
{
	// Token: 0x06002478 RID: 9336 RVA: 0x00019B78 File Offset: 0x00017D78
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			PlayerInput.instance.PlayerWeapon.UpdateWeaponAll(PlayerInput.instance.PlayerWeapon.SelectedWeapon);
		}
	}
}
