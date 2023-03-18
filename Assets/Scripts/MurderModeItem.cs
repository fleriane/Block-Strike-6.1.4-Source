using System;
using UnityEngine;

// Token: 0x020003E9 RID: 1001
public class MurderModeItem : MonoBehaviour
{
	// Token: 0x0600242F RID: 9263 RVA: 0x000D9EA0 File Offset: 0x000D80A0
	private void OnTriggerEnter(Collider other)
	{
		if (GameManager.roundState == RoundState.EndRound)
		{
			return;
		}
		if (other.CompareTag("Player"))
		{
			PlayerInput component = other.GetComponent<PlayerInput>();
			if (component != null)
			{
			}
		}
	}

	// Token: 0x04001572 RID: 5490
	public int ID;

	// Token: 0x04001573 RID: 5491
	public MurderModeItem.ItemList Item;

	// Token: 0x04001574 RID: 5492
	public bool Active;

	// Token: 0x020003EA RID: 1002
	public enum ItemList
	{
		// Token: 0x04001576 RID: 5494
		Weapon,
		// Token: 0x04001577 RID: 5495
		Clue
	}
}
