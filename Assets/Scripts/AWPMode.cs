using System;
using Photon;
using UnityEngine;

// Token: 0x02000386 RID: 902
public class AWPMode : Photon.MonoBehaviour
{
	// Token: 0x060020D3 RID: 8403 RVA: 0x0001702C File Offset: 0x0001522C
	private void Awake()
	{
		if (PhotonNetwork.offlineMode)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		if (PhotonNetwork.room.GetGameMode() != GameMode.AWPMode)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x060020D4 RID: 8404 RVA: 0x000CB318 File Offset: 0x000C9518
	private void Start()
	{
		base.photonView.AddMessage("OnKilledPlayer", new PhotonView.MessageDelegate(this.OnKilledPlayer));
		base.photonView.AddMessage("PhotonOnScore", new PhotonView.MessageDelegate(this.PhotonOnScore));
		base.photonView.AddMessage("PhotonNextLevel", new PhotonView.MessageDelegate(this.PhotonNextLevel));
		GameManager.roundState = RoundState.PlayRound;
		CameraManager.SetType(CameraType.Static, new object[0]);
		UIScore.SetActiveScore(true, nValue.int80);
		WeaponManager.SetSelectWeapon(WeaponType.Knife, nValue.int0);
		WeaponManager.SetSelectWeapon(WeaponType.Pistol, nValue.int0);
		WeaponManager.SetSelectWeapon(WeaponType.Rifle, nValue.int8);
		GameManager.changeWeapons = false;
		GameManager.maxScore = nValue.int80;
		GameManager.StartAutoBalance();
		UISelectTeam.OnStart(new Action<Team>(this.OnSelectTeam));
		EventManager.AddListener<DamageInfo>("DeadPlayer", new EventManager.Callback<DamageInfo>(this.OnDeadPlayer));
		EventManager.AddListener<Team>("AutoBalance", new EventManager.Callback<Team>(this.OnAutoBalance));
	}

	// Token: 0x060020D5 RID: 8405 RVA: 0x00017055 File Offset: 0x00015255
	private void OnEnable()
	{
		PhotonNetwork.onPhotonPlayerConnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Combine(PhotonNetwork.onPhotonPlayerConnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerConnected));
	}

	// Token: 0x060020D6 RID: 8406 RVA: 0x00017077 File Offset: 0x00015277
	private void OnDisable()
	{
		PhotonNetwork.onPhotonPlayerConnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Remove(PhotonNetwork.onPhotonPlayerConnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerConnected));
	}

	// Token: 0x060020D7 RID: 8407 RVA: 0x00017099 File Offset: 0x00015299
	private void OnSelectTeam(Team team)
	{
		UIPanelManager.ShowPanel("Display");
		this.OnRevivalPlayer();
	}

	// Token: 0x060020D8 RID: 8408 RVA: 0x000170AB File Offset: 0x000152AB
	private void OnAutoBalance(Team team)
	{
		GameManager.team = team;
		this.OnRevivalPlayer();
	}

	// Token: 0x060020D9 RID: 8409 RVA: 0x000CB410 File Offset: 0x000C9610
	private void OnRevivalPlayer()
	{
		WeaponManager.SetSelectWeapon(WeaponType.Knife, nValue.int0);
		WeaponManager.SetSelectWeapon(WeaponType.Pistol, nValue.int0);
		WeaponManager.SetSelectWeapon(WeaponType.Rifle, nValue.int8);
		PlayerInput player = GameManager.player;
		player.SetHealth(nValue.int100);
		CameraManager.SetType(CameraType.None, new object[0]);
		GameManager.controller.ActivePlayer(SpawnManager.GetTeamSpawn().spawnPosition, SpawnManager.GetTeamSpawn().spawnRotation);
		player.PlayerWeapon.UpdateWeaponAll(WeaponType.Rifle);
	}

	// Token: 0x060020DA RID: 8410 RVA: 0x000CB488 File Offset: 0x000C9688
	private void OnDeadPlayer(DamageInfo damageInfo)
	{
		PhotonNetwork.player.SetDeaths1();
		PlayerRoundManager.SetDeaths1();
		UIStatus.Add(damageInfo);
		UIDeathScreen.Show(damageInfo);
		if (damageInfo.otherPlayer)
		{
			this.OnScore(damageInfo.team);
			PhotonDataWrite data = base.photonView.GetData();
			data.Write(damageInfo.Deserialize());
			base.photonView.RPC("OnKilledPlayer", PhotonPlayer.Find(damageInfo.player), data);
		}
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

	// Token: 0x060020DB RID: 8411 RVA: 0x000170B9 File Offset: 0x000152B9
	private void OnPhotonPlayerConnected(PhotonPlayer playerConnect)
	{
		if (PhotonNetwork.isMasterClient)
		{
			GameManager.SetScore(playerConnect);
		}
	}

	// Token: 0x060020DC RID: 8412 RVA: 0x000CB5A0 File Offset: 0x000C97A0
	[PunRPC]
	private void OnKilledPlayer(PhotonMessage message)
	{
		DamageInfo e = DamageInfo.Serialize(message.ReadBytes());
		EventManager.Dispatch<DamageInfo>("KillPlayer", e);
		PhotonNetwork.player.SetKills1();
		PlayerRoundManager.SetKills1();
		UIDeathScreen.AddKill(message.sender.ID);
		if (e.headshot)
		{
			PlayerRoundManager.SetXP(nValue.int10);
			PlayerRoundManager.SetMoney(nValue.int9);
			PlayerRoundManager.SetHeadshot1();
		}
		else
		{
			PlayerRoundManager.SetXP(nValue.int5);
			PlayerRoundManager.SetMoney(nValue.int3);
		}
	}

	// Token: 0x060020DD RID: 8413 RVA: 0x000CB624 File Offset: 0x000C9824
	public void OnScore(Team team)
	{
		PhotonDataWrite data = base.photonView.GetData();
		data.Write((byte)team);
		base.photonView.RPC("PhotonOnScore", PhotonTargets.MasterClient, data);
	}

	// Token: 0x060020DE RID: 8414 RVA: 0x000CB658 File Offset: 0x000C9858
	[PunRPC]
	private void PhotonOnScore(PhotonMessage message)
	{
		Team team = (Team)message.ReadByte();
		if (team == Team.Blue)
		{
			GameManager.blueScore = ++GameManager.blueScore;
		}
		else if (team == Team.Red)
		{
			GameManager.redScore = ++GameManager.redScore;
		}
		GameManager.SetScore();
		if (GameManager.checkScore)
		{
			GameManager.roundState = RoundState.EndRound;
			if (GameManager.winTeam == Team.Blue)
			{
				UIMainStatus.Add("[@]", false, (float)nValue.int5, "Blue Win");
			}
			else if (GameManager.winTeam == Team.Red)
			{
				UIMainStatus.Add("[@]", false, (float)nValue.int5, "Red Win");
			}
			base.photonView.RPC("PhotonNextLevel", PhotonTargets.All);
		}
	}

	// Token: 0x060020DF RID: 8415 RVA: 0x000170CB File Offset: 0x000152CB
	[PunRPC]
	private void PhotonNextLevel(PhotonMessage message)
	{
		GameManager.LoadNextLevel(GameMode.AWPMode);
	}
}
