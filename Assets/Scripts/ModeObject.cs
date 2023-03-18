using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000310 RID: 784
public class ModeObject : MonoBehaviour
{
	// Token: 0x06001D22 RID: 7458 RVA: 0x00014EBB File Offset: 0x000130BB
	private void Start()
	{
		if (PhotonNetwork.room.GetGameMode() == this.Mode)
		{
			base.StartCoroutine(this.LoadSync());
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06001D23 RID: 7459 RVA: 0x000B7470 File Offset: 0x000B5670
	private IEnumerator LoadSync()
	{
		for (int i = 0; i < this.Targets.Length; i++)
		{
			this.Targets[i].SetActive(true);
			yield return new WaitForSeconds(0.01f);
		}
		EventManager.Dispatch("ModeObject_Finish");
		yield break;
	}

	// Token: 0x0400115A RID: 4442
	public GameMode Mode;

	// Token: 0x0400115B RID: 4443
	public GameObject[] Targets;
}
