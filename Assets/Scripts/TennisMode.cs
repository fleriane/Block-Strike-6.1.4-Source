using System;
using Photon;
using UnityEngine;

// Token: 0x020003E8 RID: 1000
public class TennisMode : Photon.MonoBehaviour
{
	// Token: 0x06002419 RID: 9241 RVA: 0x000D98F0 File Offset: 0x000D7AF0
	private void Start()
	{
		base.photonView.AddMessage("OnCreatePlayer", new PhotonView.MessageDelegate(this.OnCreatePlayer));
		base.photonView.AddMessage("OnKilledPlayer", new PhotonView.MessageDelegate(this.OnKilledPlayer));
		base.photonView.AddMessage("OnFinishRound", new PhotonView.MessageDelegate(this.OnFinishRound));
		base.photonView.AddMessage("CheckPlayers", new PhotonView.MessageDelegate(this.CheckPlayers));
		GameManager.maxScore = 0;
		UIScore.SetActiveScore(true, 0);
		GameManager.startDamageTime = (float)nValue.int1;
		UIPanelManager.ShowPanel("Display");
		CameraManager.SetType(CameraType.Static, new object[0]);
		GameManager.changeWeapons = false;
		if (PhotonNetwork.isMasterClient)
		{
			TimerManager.In(nValue.float05, delegate()
			{
				this.ActivationWaitPlayer();
			});
		}
		else
		{
			UISelectTeam.OnStart(new Action<Team>(this.OnSelectTeam));
		}
		EventManager.AddListener<DamageInfo>("DeadPlayer", new EventManager.Callback<DamageInfo>(this.OnDeadPlayer));
	}

	// Token: 0x0600241A RID: 9242 RVA: 0x000D99FC File Offset: 0x000D7BFC
	private void OnEnable()
	{
		PhotonNetwork.onPhotonPlayerConnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Combine(PhotonNetwork.onPhotonPlayerConnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerConnected));
		PhotonNetwork.onPhotonPlayerDisconnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Combine(PhotonNetwork.onPhotonPlayerDisconnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerDisconnected));
	}

	// Token: 0x0600241B RID: 9243 RVA: 0x000D9A4C File Offset: 0x000D7C4C
	private void OnDisable()
	{
		PhotonNetwork.onPhotonPlayerConnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Remove(PhotonNetwork.onPhotonPlayerConnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerConnected));
		PhotonNetwork.onPhotonPlayerDisconnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Remove(PhotonNetwork.onPhotonPlayerDisconnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerDisconnected));
	}

	// Token: 0x0600241C RID: 9244 RVA: 0x00019182 File Offset: 0x00017382
	private void OnSelectTeam(Team team)
	{
		UIPanelManager.ShowPanel("Display");
		if (GameManager.player.Dead)
		{
			CameraManager.SetType(CameraType.Spectate, new object[0]);
		}
	}

	// Token: 0x0600241D RID: 9245 RVA: 0x00019668 File Offset: 0x00017868
	private void ActivationWaitPlayer()
	{
		EventManager.Dispatch("WaitPlayer");
		GameManager.roundState = RoundState.WaitPlayer;
		GameManager.team = Team.Blue;
		this.OnWaitPlayer();
		this.OnCreatePlayer();
	}

	// Token: 0x0600241E RID: 9246 RVA: 0x0001968C File Offset: 0x0001788C
	private void OnWaitPlayer()
	{
		UIStatus.Add(Localization.Get("Waiting for other players", true), true);
		TimerManager.In((float)nValue.int4, delegate()
		{
			if (GameManager.roundState == RoundState.WaitPlayer)
			{
				if (PhotonNetwork.playerList.Length <= nValue.int1)
				{
					this.OnWaitPlayer();
				}
				else
				{
					TimerManager.In((float)nValue.int4, delegate()
					{
						this.OnStartRound();
					});
				}
			}
		});
	}

	// Token: 0x0600241F RID: 9247 RVA: 0x000196B7 File Offset: 0x000178B7
	private void OnPhotonPlayerConnected(PhotonPlayer playerConnect)
	{
		if (PhotonNetwork.isMasterClient)
		{
			GameManager.SetScore(playerConnect);
			if (GameManager.roundState != RoundState.WaitPlayer)
			{
				this.CheckPlayers();
			}
		}
	}

	// Token: 0x06002420 RID: 9248 RVA: 0x000196D9 File Offset: 0x000178D9
	private void OnPhotonPlayerDisconnected(PhotonPlayer playerDisconnect)
	{
		if (PhotonNetwork.isMasterClient)
		{
			this.CheckPlayers();
		}
	}

	// Token: 0x06002421 RID: 9249 RVA: 0x000D9A9C File Offset: 0x000D7C9C
	private void OnStartRound()
	{
		DecalsManager.ClearBulletHoles();
		if (PhotonNetwork.playerList.Length <= nValue.int1)
		{
			this.ActivationWaitPlayer();
		}
		else if (PhotonNetwork.isMasterClient)
		{
			GameManager.roundState = RoundState.PlayRound;
			base.photonView.RPC("OnCreatePlayer", PhotonTargets.All);
		}
	}

	// Token: 0x06002422 RID: 9250 RVA: 0x000196EB File Offset: 0x000178EB
	[PunRPC]
	private void OnCreatePlayer(PhotonMessage message)
	{
		this.OnCreatePlayer();
	}

	// Token: 0x06002423 RID: 9251 RVA: 0x000D9AEC File Offset: 0x000D7CEC
	private void OnCreatePlayer()
	{
		if (PhotonNetwork.player.GetTeam() != Team.None)
		{
			PlayerInput player = GameManager.player;
			player.SetHealth(nValue.int100);
			CameraManager.SetType(CameraType.None, new object[0]);
			if (PhotonNetwork.room.PlayerCount <= 4)
			{
				if (player.PlayerTeam == Team.Blue)
				{
					GameManager.controller.ActivePlayer(this.Spawns[0].spawnPosition, this.Spawns[0].spawnRotation);
				}
				else if (player.PlayerTeam == Team.Red)
				{
					GameManager.controller.ActivePlayer(this.Spawns[1].spawnPosition, this.Spawns[1].spawnRotation);
				}
			}
			else if (PhotonNetwork.room.PlayerCount <= 8)
			{
				if (player.PlayerTeam == Team.Blue)
				{
					GameManager.controller.ActivePlayer(this.Spawns[2].spawnPosition, this.Spawns[2].spawnRotation);
				}
				else if (player.PlayerTeam == Team.Red)
				{
					GameManager.controller.ActivePlayer(this.Spawns[3].spawnPosition, this.Spawns[3].spawnRotation);
				}
			}
			else if (PhotonNetwork.room.PlayerCount <= 12)
			{
				if (player.PlayerTeam == Team.Blue)
				{
					GameManager.controller.ActivePlayer(this.Spawns[4].spawnPosition, this.Spawns[4].spawnRotation);
				}
				else if (player.PlayerTeam == Team.Red)
				{
					GameManager.controller.ActivePlayer(this.Spawns[5].spawnPosition, this.Spawns[5].spawnRotation);
				}
			}
			WeaponManager.SetSelectWeapon(WeaponType.Knife, 46);
			WeaponManager.SetSelectWeapon(WeaponType.Pistol, nValue.int0);
			WeaponManager.SetSelectWeapon(WeaponType.Rifle, nValue.int0);
			player.PlayerWeapon.UpdateWeaponAll(WeaponType.Knife);
		}
	}

	// Token: 0x06002424 RID: 9252 RVA: 0x000D9CBC File Offset: 0x000D7EBC
	private void OnDeadPlayer(DamageInfo damageInfo)
	{
		if (GameManager.roundState == RoundState.PlayRound)
		{
			PhotonNetwork.player.SetDeaths1();
			PlayerRoundManager.SetDeaths1();
			UIStatus.Add(damageInfo);
			UIDeathScreen.Show(damageInfo);
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
			}
			base.photonView.RPC("CheckPlayers", PhotonTargets.MasterClient);
			GameManager.BalanceTeam(true);
			TimerManager.In((float)nValue.int3, delegate()
			{
				if (GameManager.player.Dead)
				{
					CameraManager.SetType(CameraType.Spectate, new object[0]);
				}
			});
		}
		else
		{
			this.OnCreatePlayer();
		}
	}

	// Token: 0x06002425 RID: 9253 RVA: 0x000D9E04 File Offset: 0x000D8004
	[PunRPC]
	private void OnKilledPlayer(PhotonMessage message)
	{
		DamageInfo e = DamageInfo.Serialize(message.ReadBytes());
		EventManager.Dispatch<DamageInfo>("KillPlayer", e);
		PhotonNetwork.player.SetKills1();
		PlayerRoundManager.SetKills1();
		UIDeathScreen.AddKill(message.sender.ID);
		PlayerRoundManager.SetXP(nValue.int7);
		PlayerRoundManager.SetMoney(nValue.int5);
	}

	// Token: 0x06002426 RID: 9254 RVA: 0x000D9E5C File Offset: 0x000D805C
	[PunRPC]
	private void OnFinishRound(PhotonMessage message)
	{
		GameManager.roundState = RoundState.EndRound;
		GameManager.BalanceTeam(true);
		float delay = (float)nValue.int8 - (float)(PhotonNetwork.time - message.timestamp);
		TimerManager.In(delay, delegate()
		{
			this.OnStartRound();
		});
	}

	// Token: 0x06002427 RID: 9255 RVA: 0x000196F3 File Offset: 0x000178F3
	[PunRPC]
	private void CheckPlayers(PhotonMessage message)
	{
		this.CheckPlayers();
	}

	// Token: 0x06002428 RID: 9256 RVA: 0x000CD698 File Offset: 0x000CB898
	private void CheckPlayers()
	{
		if (PhotonNetwork.isMasterClient && GameManager.roundState != RoundState.EndRound)
		{
			PhotonPlayer[] playerList = PhotonNetwork.playerList;
			bool flag = false;
			bool flag2 = false;
			for (int i = nValue.int0; i < playerList.Length; i++)
			{
				if (playerList[i].GetTeam() == Team.Blue && !playerList[i].GetDead())
				{
					flag = true;
					break;
				}
			}
			for (int j = nValue.int0; j < playerList.Length; j++)
			{
				if (playerList[j].GetTeam() == Team.Red && !playerList[j].GetDead())
				{
					flag2 = true;
					break;
				}
			}
			if (!flag)
			{
				GameManager.redScore = ++GameManager.redScore;
				GameManager.SetScore();
				UIMainStatus.Add("[@]", false, (float)nValue.int5, "Red Win");
				base.photonView.RPC("OnFinishRound", PhotonTargets.All);
			}
			else if (!flag2)
			{
				GameManager.blueScore = ++GameManager.blueScore;
				GameManager.SetScore();
				UIMainStatus.Add("[@]", false, (float)nValue.int5, "Blue Win");
				base.photonView.RPC("OnFinishRound", PhotonTargets.All);
			}
		}
	}

	// Token: 0x04001570 RID: 5488
	public SpawnPoint[] Spawns;
}
