using System;
using UnityEngine;

// Token: 0x020003A7 RID: 935
public class MeetingMode : MonoBehaviour
{
	// Token: 0x06002218 RID: 8728 RVA: 0x00017DA8 File Offset: 0x00015FA8
	private void Awake()
	{
		if (PhotonNetwork.offlineMode)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		if (PhotonNetwork.room.GetGameMode() != GameMode.Meeting)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06002219 RID: 8729 RVA: 0x000D0A54 File Offset: 0x000CEC54
	private void Start()
	{
		GameManager.roundState = RoundState.PlayRound;
		GameManager.startDamageTime = (float)nValue.int2;
		UIScore.SetActiveScore(false);
		UIPanelManager.ShowPanel("Display");
		UIPlayerStatistics.isOnlyBluePanel = true;
		CameraManager.SetType(CameraType.Static, new object[0]);
		TimerManager.In(nValue.float05, delegate()
		{
			GameManager.team = Team.Blue;
			this.OnRevivalPlayer();
		});
		EventManager.AddListener<DamageInfo>("DeadPlayer", new EventManager.Callback<DamageInfo>(this.OnDeadPlayer));
	}

	// Token: 0x0600221A RID: 8730 RVA: 0x000D0AC8 File Offset: 0x000CECC8
	private void OnRevivalPlayer()
	{
		PlayerInput player = GameManager.player;
		player.SetHealth(nValue.int100);
		CameraManager.SetType(CameraType.None, new object[0]);
		GameManager.controller.ActivePlayer(SpawnManager.GetTeamSpawn().spawnPosition, SpawnManager.GetTeamSpawn().spawnRotation);
		WeaponManager.SetSelectWeapon(WeaponType.Pistol, nValue.int0);
		WeaponManager.SetSelectWeapon(WeaponType.Rifle, nValue.int0);
		player.PlayerWeapon.UpdateWeaponAll(WeaponType.Knife);
		player.PlayerWeapon.CanFire = false;
	}

	// Token: 0x0600221B RID: 8731 RVA: 0x000D0B40 File Offset: 0x000CED40
	private void OnDeadPlayer(DamageInfo damageInfo)
	{
		UIDeathScreen.Show(damageInfo);
		UIStatus.Add(damageInfo);
		Vector3 ragdollForce = Utils.GetRagdollForce(GameManager.player.PlayerTransform.position, damageInfo.position);
		CameraManager.SetType(CameraType.Dead, new object[]
		{
			GameManager.player.FPCamera.Transform.position,
			GameManager.player.FPCamera.Transform.eulerAngles,
			ragdollForce * (float)nValue.int100
		});
		GameManager.controller.DeactivePlayer(ragdollForce, damageInfo.headshot);
		TimerManager.In((float)nValue.int3, delegate()
		{
			this.OnRevivalPlayer();
		});
	}

	// Token: 0x0600221C RID: 8732 RVA: 0x00017DD2 File Offset: 0x00015FD2
	public void OnClimb(bool active)
	{
		PlayerInput.instance.SetClimb(active);
	}

	// Token: 0x0600221D RID: 8733 RVA: 0x00017DDF File Offset: 0x00015FDF
	public void OnIce(bool active)
	{
		PlayerInput.instance.SetMoveIce(active);
	}

	// Token: 0x0600221E RID: 8734 RVA: 0x00017DEC File Offset: 0x00015FEC
	public void OnTrampoline(float force)
	{
		PlayerInput.instance.FPController.AddForce(new Vector3(0f, force, 0f));
	}

	// Token: 0x0600221F RID: 8735 RVA: 0x000D0BF8 File Offset: 0x000CEDF8
	public void OnSize(float size)
	{
		PlayerSkin[] array = UnityEngine.Object.FindObjectsOfType<PlayerSkin>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Controller.transform.localScale = Vector3.one * size;
		}
	}

	// Token: 0x06002220 RID: 8736 RVA: 0x000D0C3C File Offset: 0x000CEE3C
	public void GiveKnife(int weaponID)
	{
		WeaponManager.SetSelectWeapon(WeaponType.Knife, weaponID);
		PlayerInput player = GameManager.player;
		player.PlayerWeapon.UpdateWeaponAll(WeaponType.Knife);
	}
}
