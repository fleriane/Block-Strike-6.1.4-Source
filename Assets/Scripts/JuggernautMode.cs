using System;
using Photon;
using UnityEngine;

// Token: 0x020003A1 RID: 929
public class JuggernautMode : Photon.MonoBehaviour
{
	// Token: 0x060021E7 RID: 8679 RVA: 0x00017B9C File Offset: 0x00015D9C
	private void Awake()
	{
		if (PhotonNetwork.offlineMode)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		if (PhotonNetwork.room.GetGameMode() != GameMode.Juggernaut)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x060021E8 RID: 8680 RVA: 0x000CFEB8 File Offset: 0x000CE0B8
	private void Start()
	{
		base.photonView.AddMessage("OnSendKillerInfo", new PhotonView.MessageDelegate(this.OnSendKillerInfo));
		base.photonView.AddMessage("UpdateTimer", new PhotonView.MessageDelegate(this.UpdateTimer));
		base.photonView.AddMessage("OnKilledPlayer", new PhotonView.MessageDelegate(this.OnKilledPlayer));
		base.photonView.AddMessage("OnFinishRound", new PhotonView.MessageDelegate(this.OnFinishRound));
		base.photonView.AddMessage("CheckPlayers", new PhotonView.MessageDelegate(this.CheckPlayers));
		base.photonView.AddMessage("SetNextJuggernaut", new PhotonView.MessageDelegate(this.SetNextJuggernaut));
		UIScore.SetActiveScore(true, nValue.int20);
		GameManager.startDamageTime = (float)nValue.int1;
		UIPanelManager.ShowPanel("Display");
		GameManager.maxScore = nValue.int20;
		CameraManager.SetType(CameraType.Static, new object[0]);
		TimerManager.In(nValue.float05, delegate()
		{
			if (PhotonNetwork.isMasterClient)
			{
				this.ActivationWaitPlayer();
			}
			else if (GameManager.player.Dead)
			{
				CameraManager.SetType(CameraType.Spectate, new object[0]);
			}
		});
		EventManager.AddListener<DamageInfo>("DeadPlayer", new EventManager.Callback<DamageInfo>(this.OnDeadPlayer));
	}

	// Token: 0x060021E9 RID: 8681 RVA: 0x000CFFDC File Offset: 0x000CE1DC
	private void OnEnable()
	{
		PhotonNetwork.onPhotonPlayerConnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Combine(PhotonNetwork.onPhotonPlayerConnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerConnected));
		PhotonNetwork.onPhotonPlayerDisconnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Combine(PhotonNetwork.onPhotonPlayerDisconnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerDisconnected));
	}

	// Token: 0x060021EA RID: 8682 RVA: 0x000D002C File Offset: 0x000CE22C
	private void OnDisable()
	{
		PhotonNetwork.onPhotonPlayerConnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Remove(PhotonNetwork.onPhotonPlayerConnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerConnected));
		PhotonNetwork.onPhotonPlayerDisconnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Remove(PhotonNetwork.onPhotonPlayerDisconnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerDisconnected));
	}

	// Token: 0x060021EB RID: 8683 RVA: 0x00017BC6 File Offset: 0x00015DC6
	private void ActivationWaitPlayer()
	{
		EventManager.Dispatch("WaitPlayer");
		GameManager.roundState = RoundState.WaitPlayer;
		GameManager.team = Team.Blue;
		this.OnWaitPlayer();
		this.OnCreatePlayer();
	}

	// Token: 0x060021EC RID: 8684 RVA: 0x00017BEA File Offset: 0x00015DEA
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

	// Token: 0x060021ED RID: 8685 RVA: 0x000D007C File Offset: 0x000CE27C
	private void OnPhotonPlayerConnected(PhotonPlayer playerConnect)
	{
		if (PhotonNetwork.isMasterClient)
		{
			GameManager.SetScore(playerConnect);
			if (GameManager.roundState != RoundState.WaitPlayer)
			{
				this.CheckPlayers();
				if (UIScore.timeData.active)
				{
					TimerManager.In(nValue.float05, delegate()
					{
						PhotonDataWrite data = this.photonView.GetData();
						data.Write(UIScore.timeData.endTime - Time.time);
						this.photonView.RPC("UpdateTimer", playerConnect, data);
					});
				}
			}
		}
	}

	// Token: 0x060021EE RID: 8686 RVA: 0x00017C15 File Offset: 0x00015E15
	private void OnPhotonPlayerDisconnected(PhotonPlayer playerDisconnect)
	{
		if (PhotonNetwork.isMasterClient)
		{
			this.CheckPlayers();
		}
	}

	// Token: 0x060021EF RID: 8687 RVA: 0x000D00E8 File Offset: 0x000CE2E8
	private void OnStartRound()
	{
		UIDeathScreen.ClearAll();
		DecalsManager.ClearBulletHoles();
		if (PhotonNetwork.playerList.Length <= nValue.int1)
		{
			this.ActivationWaitPlayer();
		}
		else if (PhotonNetwork.isMasterClient)
		{
			GameManager.roundState = RoundState.PlayRound;
			if (!this.HasNextJuggernaut())
			{
				this.NextJuggernaut = PhotonNetwork.playerList[UnityEngine.Random.Range(nValue.int0, PhotonNetwork.playerList.Length)].ID;
			}
			PhotonDataWrite data = base.photonView.GetData();
			data.Write(this.NextJuggernaut);
			base.photonView.RPC("OnSendKillerInfo", PhotonTargets.All, data);
		}
	}

	// Token: 0x060021F0 RID: 8688 RVA: 0x000D0184 File Offset: 0x000CE384
	private bool HasNextJuggernaut()
	{
		for (int i = nValue.int0; i < PhotonNetwork.playerList.Length; i++)
		{
			if (this.NextJuggernaut == PhotonNetwork.playerList[i].ID)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060021F1 RID: 8689 RVA: 0x000D01C8 File Offset: 0x000CE3C8
	[PunRPC]
	private void OnSendKillerInfo(PhotonMessage message)
	{
		int num = message.ReadInt();
		float num2 = (float)nValue.int150;
		num2 -= (float)(PhotonNetwork.time - message.timestamp);
		UIScore.StartTime(num2, new Action(this.StopTimer));
		if (PhotonNetwork.player.ID == num)
		{
			GameManager.team = Team.Red;
		}
		else
		{
			GameManager.team = Team.Blue;
		}
		EventManager.Dispatch("StartRound");
		this.OnCreatePlayer();
		TimerManager.In((float)nValue.int3, delegate()
		{
			if (PhotonNetwork.isMasterClient)
			{
				this.CheckPlayers();
			}
		});
	}

	// Token: 0x060021F2 RID: 8690 RVA: 0x000D0250 File Offset: 0x000CE450
	private void StopTimer()
	{
		if (PhotonNetwork.isMasterClient)
		{
			GameManager.redScore = ++GameManager.redScore;
			GameManager.SetScore();
			UIMainStatus.Add("[@]", false, (float)nValue.int5, "Red Win");
			PhotonDataWrite data = base.photonView.GetData();
			data.Write(PhotonNetwork.playerList[UnityEngine.Random.Range(nValue.int0, PhotonNetwork.playerList.Length)].ID);
			base.photonView.RPC("SetNextJuggernaut", PhotonTargets.All, data);
			base.photonView.RPC("OnFinishRound", PhotonTargets.All);
		}
	}

	// Token: 0x060021F3 RID: 8691 RVA: 0x000D02E4 File Offset: 0x000CE4E4
	[PunRPC]
	private void UpdateTimer(PhotonMessage message)
	{
		float time = message.ReadFloat();
		double timestamp = message.timestamp;
		TimerManager.In(nValue.float15, delegate()
		{
			time -= (float)(PhotonNetwork.time - timestamp);
			UIScore.StartTime(time, new Action(this.StopTimer));
		});
	}

	// Token: 0x060021F4 RID: 8692 RVA: 0x000D0330 File Offset: 0x000CE530
	private void OnCreatePlayer()
	{
		PlayerInput playerInput = GameManager.player;
		CameraManager.SetType(CameraType.None, new object[0]);
		GameManager.controller.ActivePlayer(SpawnManager.GetTeamSpawn().spawnPosition, SpawnManager.GetTeamSpawn().spawnRotation);
		if (playerInput.PlayerTeam == Team.Red)
		{
			int num = nValue.int500 + nValue.int150 * PhotonNetwork.otherPlayers.Length;
			playerInput.MaxHealth = num;
			playerInput.SetHealth(num);
			WeaponManager.SetSelectWeapon(WeaponType.Knife, nValue.int0);
			WeaponManager.SetSelectWeapon(WeaponType.Pistol, nValue.int0);
			WeaponManager.SetSelectWeapon(WeaponType.Rifle, nValue.int23);
			playerInput.PlayerWeapon.UpdateWeaponAll(WeaponType.Rifle);
			TimerManager.In(nValue.float01, delegate()
			{
				playerInput.FPController.MotorAcceleration = nValue.float01;
			});
		}
		else
		{
			playerInput.MaxHealth = nValue.int100;
			playerInput.SetHealth(nValue.int100);
			WeaponManager.SetSelectWeapon(WeaponType.Knife, AccountManager.GetWeaponSelected(WeaponType.Knife));
			WeaponManager.SetSelectWeapon(WeaponType.Pistol, AccountManager.GetWeaponSelected(WeaponType.Pistol));
			WeaponManager.SetSelectWeapon(WeaponType.Rifle, AccountManager.GetWeaponSelected(WeaponType.Rifle));
			playerInput.PlayerWeapon.UpdateWeaponAll(WeaponType.Rifle);
		}
	}

	// Token: 0x060021F5 RID: 8693 RVA: 0x000D0464 File Offset: 0x000CE664
	private void OnDeadPlayer(DamageInfo damageInfo)
	{
		PhotonNetwork.player.SetDeaths1();
		PlayerRoundManager.SetDeaths1();
		UIStatus.Add(damageInfo);
		UIDeathScreen.Show(damageInfo);
		if (damageInfo.otherPlayer)
		{
			PhotonDataWrite data = base.photonView.GetData();
			data.Write(damageInfo.Deserialize());
			base.photonView.RPC("OnKilledPlayer", PhotonPlayer.Find(damageInfo.player), data);
			data = base.photonView.GetData();
			data.Write(damageInfo.player);
			base.photonView.RPC("SetNextJuggernaut", PhotonTargets.All, data);
		}
		Vector3 ragdollForce = Utils.GetRagdollForce(GameManager.player.PlayerTransform.position, damageInfo.position);
		CameraManager.SetType(CameraType.Dead, new object[]
		{
			GameManager.player.FPCamera.Transform.position,
			GameManager.player.FPCamera.Transform.eulerAngles,
			ragdollForce * (float)nValue.int100
		});
		GameManager.controller.DeactivePlayer(ragdollForce, damageInfo.headshot);
		base.photonView.RPC("CheckPlayers", PhotonTargets.MasterClient);
		TimerManager.In((float)nValue.int3, delegate()
		{
			if (GameManager.player.Dead)
			{
				CameraManager.SetType(CameraType.Spectate, new object[0]);
			}
		});
	}

	// Token: 0x060021F6 RID: 8694 RVA: 0x000CBD78 File Offset: 0x000C9F78
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
			PlayerRoundManager.SetXP(nValue.int12);
			PlayerRoundManager.SetMoney(nValue.int10);
			PlayerRoundManager.SetHeadshot1();
		}
		else
		{
			PlayerRoundManager.SetXP(nValue.int6);
			PlayerRoundManager.SetMoney(nValue.int5);
		}
	}

	// Token: 0x060021F7 RID: 8695 RVA: 0x000D05BC File Offset: 0x000CE7BC
	[PunRPC]
	private void OnFinishRound(PhotonMessage message)
	{
		UIScore.timeData.active = false;
		GameManager.roundState = RoundState.EndRound;
		if (GameManager.checkScore)
		{
			GameManager.LoadNextLevel(GameMode.Juggernaut);
		}
		else
		{
			float delay = (float)nValue.int8 - (float)(PhotonNetwork.time - message.timestamp);
			TimerManager.In(delay, delegate()
			{
				this.OnStartRound();
			});
		}
	}

	// Token: 0x060021F8 RID: 8696 RVA: 0x00017C27 File Offset: 0x00015E27
	[PunRPC]
	private void CheckPlayers(PhotonMessage message)
	{
		this.CheckPlayers();
	}

	// Token: 0x060021F9 RID: 8697 RVA: 0x000D0618 File Offset: 0x000CE818
	private void CheckPlayers()
	{
		if (PhotonNetwork.isMasterClient && GameManager.roundState != RoundState.EndRound)
		{
			PhotonPlayer[] playerList = PhotonNetwork.playerList;
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < playerList.Length; i++)
			{
				if (playerList[i].GetTeam() == Team.Blue && !playerList[i].GetDead())
				{
					flag = true;
					break;
				}
			}
			for (int j = 0; j < playerList.Length; j++)
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
				PhotonDataWrite data = base.photonView.GetData();
				data.Write(PhotonNetwork.playerList[UnityEngine.Random.Range(nValue.int0, PhotonNetwork.playerList.Length)].ID);
				base.photonView.RPC("SetNextJuggernaut", PhotonTargets.All, data);
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

	// Token: 0x060021FA RID: 8698 RVA: 0x00017C2F File Offset: 0x00015E2F
	[PunRPC]
	private void SetNextJuggernaut(PhotonMessage message)
	{
		this.NextJuggernaut = message.ReadInt();
	}

	// Token: 0x0400146B RID: 5227
	private int NextJuggernaut = -1;
}
