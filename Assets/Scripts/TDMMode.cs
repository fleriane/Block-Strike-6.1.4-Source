using System;
using Photon;
using UnityEngine;

// Token: 0x020003B2 RID: 946
public class TDMMode : Photon.MonoBehaviour
{
	// Token: 0x0600227A RID: 8826 RVA: 0x00018227 File Offset: 0x00016427
	private void Awake()
	{
		if (PhotonNetwork.offlineMode)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		if (PhotonNetwork.room.GetGameMode() != GameMode.TeamDeathmatch)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x0600227B RID: 8827 RVA: 0x000D2240 File Offset: 0x000D0440
	private void Start()
	{
		base.photonView.AddMessage("PhotonOnScore", new PhotonView.MessageDelegate(this.PhotonOnScore));
		base.photonView.AddMessage("OnKilledPlayer", new PhotonView.MessageDelegate(this.OnKilledPlayer));
		base.photonView.AddMessage("PhotonNextLevel", new PhotonView.MessageDelegate(this.PhotonNextLevel));
		GameManager.roundState = RoundState.PlayRound;
		CameraManager.SetType(CameraType.Static, new object[0]);
		UIScore.SetActiveScore(true, this.MaxScore);
		UISelectTeam.OnSpectator();
		UISelectTeam.OnStart(new Action<Team>(this.OnSelectTeam));
		GameManager.maxScore = this.MaxScore;
		GameManager.StartAutoBalance();
		EventManager.AddListener<DamageInfo>("DeadPlayer", new EventManager.Callback<DamageInfo>(this.OnDeadPlayer));
		EventManager.AddListener<Team>("AutoBalance", new EventManager.Callback<Team>(this.OnAutoBalance));
	}

	// Token: 0x0600227C RID: 8828 RVA: 0x0001824F File Offset: 0x0001644F
	private void OnEnable()
	{
		PhotonNetwork.onPhotonPlayerConnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Combine(PhotonNetwork.onPhotonPlayerConnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerConnected));
	}

	// Token: 0x0600227D RID: 8829 RVA: 0x00018271 File Offset: 0x00016471
	private void OnDisable()
	{
		PhotonNetwork.onPhotonPlayerConnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Remove(PhotonNetwork.onPhotonPlayerConnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerConnected));
	}

	// Token: 0x0600227E RID: 8830 RVA: 0x000D2318 File Offset: 0x000D0518
	private void OnSelectTeam(Team team)
	{
		UIPanelManager.ShowPanel("Display");
		if (team == Team.None)
		{
			CameraManager.SetType(CameraType.Spectate, new object[0]);
			CameraManager.ChangeType = true;
			UIControllerList.Chat.cachedGameObject.SetActive(false);
			UIControllerList.SelectWeapon.cachedGameObject.SetActive(false);
			UISpectator.SetActive(true);
		}
		else
		{
			this.OnRevivalPlayer();
		}
	}

	// Token: 0x0600227F RID: 8831 RVA: 0x00018293 File Offset: 0x00016493
	private void OnAutoBalance(Team team)
	{
		GameManager.team = team;
		this.OnRevivalPlayer();
	}

	// Token: 0x06002280 RID: 8832 RVA: 0x000D2378 File Offset: 0x000D0578
	private void OnRevivalPlayer()
	{
		PlayerInput player = GameManager.player;
		player.SetHealth(nValue.int100);
		CameraManager.SetType(CameraType.None, new object[0]);
		GameManager.controller.ActivePlayer(SpawnManager.GetTeamSpawn().spawnPosition, SpawnManager.GetTeamSpawn().spawnRotation);
		player.PlayerWeapon.UpdateWeaponAll(WeaponType.Rifle);
	}

	// Token: 0x06002281 RID: 8833 RVA: 0x000D23CC File Offset: 0x000D05CC
	private void OnDeadPlayer(DamageInfo damageInfo)
	{
		PhotonNetwork.player.SetDeaths1();
		PlayerRoundManager.SetDeaths1();
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
		if (damageInfo.otherPlayer)
		{
			PhotonDataWrite data = base.photonView.GetData();
			data.Write(damageInfo.Deserialize());
			base.photonView.RPC("OnKilledPlayer", PhotonPlayer.Find(damageInfo.player), data);
			this.OnScore(damageInfo.team);
		}
		TimerManager.In((float)nValue.int3, delegate()
		{
			this.OnRevivalPlayer();
		});
	}

	// Token: 0x06002282 RID: 8834 RVA: 0x000D24E4 File Offset: 0x000D06E4
	private void OnPhotonPlayerConnected(PhotonPlayer playerConnect)
	{
		if (PhotonNetwork.isMasterClient)
		{
			TimerManager.In(0.5f, delegate()
			{
				GameManager.SetScore(playerConnect);
			});
		}
	}

	// Token: 0x06002283 RID: 8835 RVA: 0x000D2520 File Offset: 0x000D0720
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
			PlayerRoundManager.SetMoney(nValue.int8);
			PlayerRoundManager.SetHeadshot1();
		}
		else
		{
			PlayerRoundManager.SetXP(nValue.int5);
			PlayerRoundManager.SetMoney(nValue.int4);
		}
	}

	// Token: 0x06002284 RID: 8836 RVA: 0x000CB624 File Offset: 0x000C9824
	public void OnScore(Team team)
	{
		PhotonDataWrite data = base.photonView.GetData();
		data.Write((byte)team);
		base.photonView.RPC("PhotonOnScore", PhotonTargets.MasterClient, data);
	}

	// Token: 0x06002285 RID: 8837 RVA: 0x000CB658 File Offset: 0x000C9858
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

	// Token: 0x06002286 RID: 8838 RVA: 0x000182A1 File Offset: 0x000164A1
	[PunRPC]
	private void PhotonNextLevel(PhotonMessage message)
	{
		GameManager.LoadNextLevel(GameMode.TeamDeathmatch);
	}

	// Token: 0x04001487 RID: 5255
	public CryptoInt MaxScore = 100;
}
