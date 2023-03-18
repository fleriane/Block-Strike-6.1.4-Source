using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003EB RID: 1003
public class MurderModeManager : MonoBehaviour
{
	// Token: 0x06002431 RID: 9265 RVA: 0x0001975A File Offset: 0x0001795A
	private void Awake()
	{
		if (PhotonNetwork.offlineMode)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		if (PhotonNetwork.room.GetGameMode() != GameMode.Murder)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			MurderModeManager.instance = this;
		}
	}

	// Token: 0x06002432 RID: 9266 RVA: 0x000D9EDC File Offset: 0x000D80DC
	private void Start()
	{
		PhotonRPC.AddMessage("PhotonMasterPickupPistol", new PhotonRPC.MessageDelegate(this.PhotonMasterPickupPistol));
		PhotonRPC.AddMessage("PhotonPickupPistol", new PhotonRPC.MessageDelegate(this.PhotonPickupPistol));
		PhotonRPC.AddMessage("PhotonSetPosition", new PhotonRPC.MessageDelegate(this.PhotonSetPosition));
		EventManager.AddListener("StartRound", new EventManager.Callback(this.StartRound));
	}

	// Token: 0x06002433 RID: 9267 RVA: 0x00019794 File Offset: 0x00017994
	private void StartRound()
	{
		this.Pistol.SetActive(false);
		this.canPickupPistol = true;
	}

	// Token: 0x06002434 RID: 9268 RVA: 0x000D9F44 File Offset: 0x000D8144
	public static void SetRandomPlayerPistol()
	{
		List<PhotonPlayer> list = new List<PhotonPlayer>();
		for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
		{
			if (!PhotonNetwork.playerList[i].GetDead() && PhotonNetwork.playerList[i].ID != MurderMode.Murder)
			{
				list.Add(PhotonNetwork.playerList[i]);
			}
		}
		if (list.Count > nValue.int0)
		{
			PhotonPlayer photonPlayer = list[UnityEngine.Random.Range(nValue.int0, list.Count)];
			PhotonRPC.RPC("PhotonPickupPistol", PhotonTargets.All, photonPlayer.ID);
		}
	}

	// Token: 0x06002435 RID: 9269 RVA: 0x000D9FDC File Offset: 0x000D81DC
	public static void DeadPlayer()
	{
		if (PhotonNetwork.player.ID == MurderMode.Detective)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(PlayerInput.instance.PlayerTransform.position, Vector3.down, out raycastHit, (float)nValue.int50))
			{
				PhotonDataWrite data = PhotonRPC.GetData();
				data.Write(raycastHit.point);
				data.Write(raycastHit.normal);
				PhotonRPC.RPC("PhotonSetPosition", PhotonTargets.All, data);
			}
			else
			{
				MurderModeManager.SetRandomPlayerPistol();
			}
		}
	}

	// Token: 0x06002436 RID: 9270 RVA: 0x000DA05C File Offset: 0x000D825C
	public static void DeadBystander()
	{
		MurderModeManager.instance.canPickupPistol = false;
		TimerManager.In(30f, delegate()
		{
			MurderModeManager.instance.canPickupPistol = true;
		});
		RaycastHit raycastHit;
		if (Physics.Raycast(PlayerInput.instance.PlayerTransform.position, Vector3.down, out raycastHit, (float)nValue.int50))
		{
			PhotonDataWrite data = PhotonRPC.GetData();
			data.Write(raycastHit.point);
			data.Write(raycastHit.normal);
			PhotonRPC.RPC("PhotonSetPosition", PhotonTargets.All, data);
		}
		else
		{
			MurderModeManager.SetRandomPlayerPistol();
		}
	}

	// Token: 0x06002437 RID: 9271 RVA: 0x000197A9 File Offset: 0x000179A9
	[PunRPC]
	private void PhotonSetPosition(PhotonMessage message)
	{
		MurderMode.Detective = -1;
		MurderModeManager.SetPosition(message.ReadVector3(), message.ReadVector3());
	}

	// Token: 0x06002438 RID: 9272 RVA: 0x000DA0F8 File Offset: 0x000D82F8
	public static void SetPosition(Vector3 pos, Vector3 normal)
	{
		MurderModeManager.instance.Pistol.transform.position = pos + normal * nValue.float001;
		MurderModeManager.instance.Pistol.transform.rotation = Quaternion.LookRotation(normal);
		MurderModeManager.instance.Pistol.SetActive(true);
	}

	// Token: 0x06002439 RID: 9273 RVA: 0x000DA154 File Offset: 0x000D8354
	public static void SetPosition(Vector3 pos)
	{
		MurderModeManager.instance.Pistol.transform.position = pos;
		MurderModeManager.instance.Pistol.transform.rotation = Quaternion.Euler((float)(-(float)nValue.int90), (float)UnityEngine.Random.Range(nValue.int0, nValue.int360), (float)nValue.int0);
		MurderModeManager.instance.Pistol.SetActive(true);
	}

	// Token: 0x0600243A RID: 9274 RVA: 0x000197C2 File Offset: 0x000179C2
	public void OnTriggerEnterPistol()
	{
		if (GameManager.roundState == RoundState.PlayRound && this.canPickupPistol && PhotonNetwork.player.ID != MurderMode.Murder)
		{
			PhotonRPC.RPC("PhotonMasterPickupPistol", PhotonTargets.MasterClient);
		}
	}

	// Token: 0x0600243B RID: 9275 RVA: 0x000197F9 File Offset: 0x000179F9
	[PunRPC]
	private void PhotonMasterPickupPistol(PhotonMessage message)
	{
		if (this.Pistol.activeSelf)
		{
			this.Pistol.SetActive(false);
			PhotonRPC.RPC("PhotonPickupPistol", PhotonTargets.All, message.sender.ID);
		}
	}

	// Token: 0x0600243C RID: 9276 RVA: 0x000DA1BC File Offset: 0x000D83BC
	[PunRPC]
	private void PhotonPickupPistol(PhotonMessage message)
	{
		this.Pistol.SetActive(false);
		int num = message.ReadInt();
		MurderMode.Detective = num;
		if (PhotonNetwork.player.ID == num)
		{
			WeaponManager.SetSelectWeapon(WeaponType.Knife, 0);
			WeaponManager.SetSelectWeapon(WeaponType.Pistol, 2);
			WeaponManager.SetSelectWeapon(WeaponType.Rifle, 47);
			WeaponCustomData weaponCustomData = new WeaponCustomData();
			weaponCustomData.Ammo = 1;
			weaponCustomData.AmmoTotal = 1;
			weaponCustomData.AmmoMax = 99;
			weaponCustomData.BodyDamage = 100;
			weaponCustomData.FaceDamage = 100;
			weaponCustomData.HandDamage = 100;
			weaponCustomData.LegDamage = 100;
			weaponCustomData.Skin = 0;
			weaponCustomData.CustomData = false;
			PlayerInput.instance.PlayerWeapon.UpdateWeaponAll(WeaponType.Rifle, null, weaponCustomData, null);
		}
	}

	// Token: 0x04001578 RID: 5496
	public GameObject Pistol;

	// Token: 0x04001579 RID: 5497
	private bool canPickupPistol = true;

	// Token: 0x0400157A RID: 5498
	private static MurderModeManager instance;
}
