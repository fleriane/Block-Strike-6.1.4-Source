using System;
using System.Collections.Generic;
using Photon;
using UnityEngine;

// Token: 0x0200038A RID: 906
public class BombMode2 : Photon.MonoBehaviour
{
	// Token: 0x06002102 RID: 8450 RVA: 0x000171E9 File Offset: 0x000153E9
	private void Awake()
	{
		if (PhotonNetwork.offlineMode)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		if (PhotonNetwork.room.GetGameMode() != GameMode.Bomb2)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			BombMode2.instance = this;
		}
	}

	// Token: 0x06002103 RID: 8451 RVA: 0x000CC11C File Offset: 0x000CA31C
	private void Start()
	{
		base.photonView.AddMessage("PhotonStartRound", new PhotonView.MessageDelegate(this.PhotonStartRound));
		base.photonView.AddMessage("UpdateTimer", new PhotonView.MessageDelegate(this.UpdateTimer));
		base.photonView.AddMessage("OnKilledPlayer", new PhotonView.MessageDelegate(this.OnKilledPlayer));
		base.photonView.AddMessage("OnFinishRound", new PhotonView.MessageDelegate(this.OnFinishRound));
		base.photonView.AddMessage("CheckPlayers", new PhotonView.MessageDelegate(this.CheckPlayers));
		UIScore2.SetActiveScore(true, nValue.int20);
		GameManager.startDamageTime = (float)nValue.int1;
		GameManager.globalChat = false;
		WeaponManager.SetSelectWeapon(WeaponType.Knife, AccountManager.GetWeaponSelected(WeaponType.Knife));
		WeaponManager.SetSelectWeapon(WeaponType.Pistol, nValue.int3);
		WeaponManager.SetSelectWeapon(WeaponType.Rifle, nValue.int0);
		UIPanelManager.ShowPanel("Display");
		CameraManager.SetType(CameraType.Static, new object[0]);
		CameraManager.Team = true;
		CameraManager.ChangeType = true;
		GameManager.changeWeapons = false;
		DropWeaponManager.enable = true;
		UIBuyWeapon.SetActive(true);
		UIBuyWeapon.Money = nValue.int500;
		UIChangeTeam.SetChangeTeam(true, true);
		if (PhotonNetwork.isMasterClient)
		{
			TimerManager.In(nValue.float05, delegate()
			{
				this.ActivationWaitPlayer();
			});
		}
		else
		{
			UISelectTeam.OnSpectator();
			UISelectTeam.OnStart(new Action<Team>(this.OnSelectTeam));
		}
		EventManager.AddListener<DamageInfo>("DeadPlayer", new EventManager.Callback<DamageInfo>(this.OnDeadPlayer));
	}

	// Token: 0x06002104 RID: 8452 RVA: 0x000CC290 File Offset: 0x000CA490
	private void OnEnable()
	{
		PhotonNetwork.onPhotonPlayerConnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Combine(PhotonNetwork.onPhotonPlayerConnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerConnected));
		PhotonNetwork.onPhotonPlayerDisconnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Combine(PhotonNetwork.onPhotonPlayerDisconnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerDisconnected));
	}

	// Token: 0x06002105 RID: 8453 RVA: 0x000CC2E0 File Offset: 0x000CA4E0
	private void OnDisable()
	{
		PhotonNetwork.onPhotonPlayerConnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Remove(PhotonNetwork.onPhotonPlayerConnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerConnected));
		PhotonNetwork.onPhotonPlayerDisconnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Remove(PhotonNetwork.onPhotonPlayerDisconnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerDisconnected));
	}

	// Token: 0x06002106 RID: 8454 RVA: 0x000CC330 File Offset: 0x000CA530
	private void OnSelectTeam(Team team)
	{
		UIPanelManager.ShowPanel("Display");
		if (team == Team.None)
		{
			UIBuyWeapon.SetActive(false);
			CameraManager.Team = false;
			CameraManager.SetType(CameraType.Spectate, new object[0]);
			UIControllerList.Chat.cachedGameObject.SetActive(false);
			UIControllerList.SelectWeapon.cachedGameObject.SetActive(false);
			UISpectator.SetActive(true);
		}
		else if (GameManager.player.Dead)
		{
			CameraManager.SetType(CameraType.Spectate, new object[0]);
			UISpectator.SetActive(true);
		}
	}

	// Token: 0x06002107 RID: 8455 RVA: 0x0001721E File Offset: 0x0001541E
	private void ActivationWaitPlayer()
	{
		EventManager.Dispatch("WaitPlayer");
		GameManager.roundState = RoundState.WaitPlayer;
		GameManager.team = Team.Red;
		this.OnWaitPlayer();
		this.OnCreatePlayer();
		PlayerInput.instance.SetMove(true);
	}

	// Token: 0x06002108 RID: 8456 RVA: 0x0001724D File Offset: 0x0001544D
	private void OnWaitPlayer()
	{
		UIStatus.Add(Localization.Get("Waiting for other players", true), true);
		TimerManager.In((float)nValue.int4, delegate()
		{
			if (GameManager.roundState == RoundState.WaitPlayer)
			{
				if (this.GetPlayers().Length <= nValue.int1)
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

	// Token: 0x06002109 RID: 8457 RVA: 0x000CC3B4 File Offset: 0x000CA5B4
	private void OnPhotonPlayerConnected(PhotonPlayer playerConnect)
	{
		if (PhotonNetwork.isMasterClient)
		{
			GameManager.SetScore(playerConnect);
			if (GameManager.roundState != RoundState.WaitPlayer)
			{
				this.CheckPlayers();
				if (UIScore2.timeData.active && !BombManager.BombPlaced)
				{
					TimerManager.In(nValue.float05, delegate()
					{
						PhotonDataWrite data = this.photonView.GetData();
						data.Write(UIScore2.timeData.endTime - Time.time);
						this.photonView.RPC("UpdateTimer", playerConnect, data);
					});
				}
			}
		}
	}

	// Token: 0x0600210A RID: 8458 RVA: 0x00017278 File Offset: 0x00015478
	private void OnPhotonPlayerDisconnected(PhotonPlayer playerDisconnect)
	{
		if (PhotonNetwork.isMasterClient)
		{
			this.CheckPlayers();
		}
	}

	// Token: 0x0600210B RID: 8459 RVA: 0x000CC42C File Offset: 0x000CA62C
	private void OnStartRound()
	{
		UIDeathScreen.ClearAll();
		DecalsManager.ClearBulletHoles();
		if (this.GetPlayers().Length <= nValue.int1)
		{
			this.ActivationWaitPlayer();
		}
		else if (PhotonNetwork.isMasterClient)
		{
			GameManager.roundState = RoundState.PlayRound;
			base.photonView.RPC("PhotonStartRound", PhotonTargets.All);
		}
	}

	// Token: 0x0600210C RID: 8460 RVA: 0x000CC484 File Offset: 0x000CA684
	[PunRPC]
	private void PhotonStartRound(PhotonMessage message)
	{
		DropWeaponManager.ClearScene();
		EventManager.Dispatch("StartRound");
		float num = (float)nValue.int7;
		num -= (float)(PhotonNetwork.time - message.timestamp);
		UIScore2.StartTime(num, new Action(this.StartTimer));
		this.OnCreatePlayer(false);
		BombManager.BuyTime = true;
	}

	// Token: 0x0600210D RID: 8461 RVA: 0x000CC4D8 File Offset: 0x000CA6D8
	private void StartTimer()
	{
		if (GameManager.roundState != RoundState.EndRound)
		{
			float time = (float)nValue.int120;
			UIScore2.StartTime(time, new Action(this.StopTimer));
			PlayerInput.instance.SetMove(true);
		}
		BombManager.BuyTime = false;
	}

	// Token: 0x0600210E RID: 8462 RVA: 0x000CC51C File Offset: 0x000CA71C
	private void StopTimer()
	{
		if (PhotonNetwork.isMasterClient)
		{
			GameManager.blueScore = ++GameManager.blueScore;
			GameManager.SetScore();
			UIMainStatus.Add("[@]", false, (float)nValue.int5, "Blue Win");
			PhotonDataWrite data = base.photonView.GetData();
			data.Write(1);
			data.Write(false);
			base.photonView.RPC("OnFinishRound", PhotonTargets.All, data);
		}
	}

	// Token: 0x0600210F RID: 8463 RVA: 0x000CC58C File Offset: 0x000CA78C
	public void Boom()
	{
		if (PhotonNetwork.isMasterClient && GameManager.roundState == RoundState.PlayRound)
		{
			GameManager.redScore = ++GameManager.redScore;
			GameManager.SetScore();
			UIMainStatus.Add("[@]", false, (float)nValue.int5, "Red Win");
			PhotonDataWrite data = base.photonView.GetData();
			data.Write(2);
			data.Write(true);
			base.photonView.RPC("OnFinishRound", PhotonTargets.All, data);
		}
	}

	// Token: 0x06002110 RID: 8464 RVA: 0x000CC604 File Offset: 0x000CA804
	public void DeactiveBoom()
	{
		if (PhotonNetwork.isMasterClient && GameManager.roundState == RoundState.PlayRound)
		{
			GameManager.blueScore = ++GameManager.blueScore;
			GameManager.SetScore();
			UIMainStatus.Add("[@]", false, (float)nValue.int5, "Blue Win");
			PhotonDataWrite data = base.photonView.GetData();
			data.Write(1);
			data.Write(true);
			base.photonView.RPC("OnFinishRound", PhotonTargets.All, data);
		}
	}

	// Token: 0x06002111 RID: 8465 RVA: 0x000CC67C File Offset: 0x000CA87C
	[PunRPC]
	private void UpdateTimer(PhotonMessage message)
	{
		float time = message.ReadFloat();
		double timestamp = message.timestamp;
		TimerManager.In(nValue.float15, delegate()
		{
			time -= (float)(PhotonNetwork.time - timestamp);
			UIScore2.StartTime(time, new Action(this.StopTimer));
		});
	}

	// Token: 0x06002112 RID: 8466 RVA: 0x0001728A File Offset: 0x0001548A
	private void OnCreatePlayer()
	{
		this.OnCreatePlayer(true);
	}

	// Token: 0x06002113 RID: 8467 RVA: 0x000CC6C8 File Offset: 0x000CA8C8
	private void OnCreatePlayer(bool move)
	{
		if (PhotonNetwork.player.GetTeam() != Team.None)
		{
			bool dead = GameManager.player.Dead;
			UISpectator.SetActive(false);
			PlayerInput playerInput = GameManager.player;
			playerInput.SetHealth(nValue.int100);
			CameraManager.SetType(CameraType.None, new object[0]);
			GameManager.controller.ActivePlayer(SpawnManager.GetTeamSpawn().spawnPosition, SpawnManager.GetTeamSpawn().spawnRotation);
			if (dead || this.changeTeam)
			{
				playerInput.PlayerWeapon.UpdateWeaponAll(WeaponType.Pistol);
			}
			else
			{
				playerInput.PlayerWeapon.UpdateWeaponAmmoAll();
			}
			this.changeTeam = false;
			TimerManager.In(0.1f, delegate()
			{
				playerInput.SetMove(move);
			});
		}
	}

	// Token: 0x06002114 RID: 8468 RVA: 0x000CC7AC File Offset: 0x000CA9AC
	private void OnDeadPlayer(DamageInfo damageInfo)
	{
		PhotonNetwork.player.SetDeaths1();
		PlayerRoundManager.SetDeaths1();
		UIStatus.Add(damageInfo);
		UIDeathScreen.Show(damageInfo);
		PlayerInput player = GameManager.player;
		Vector3 ragdollForce = Utils.GetRagdollForce(player.PlayerTransform.position, damageInfo.position);
		CameraManager.SetType(CameraType.Dead, new object[]
		{
			GameManager.player.FPCamera.Transform.position,
			player.FPCamera.Transform.eulerAngles,
			ragdollForce * (float)nValue.int100
		});
		GameManager.controller.DeactivePlayer(ragdollForce, damageInfo.headshot);
		player.PlayerWeapon.DropWeapon();
		UIDefuseKit.defuseKit = false;
		if (damageInfo.otherPlayer)
		{
			PhotonDataWrite data = base.photonView.GetData();
			data.Write(damageInfo.Deserialize());
			base.photonView.RPC("OnKilledPlayer", PhotonPlayer.Find(damageInfo.player), data);
		}
		base.photonView.RPC("CheckPlayers", PhotonTargets.MasterClient);
		GameManager.BalanceTeam(true);
		WeaponManager.SetSelectWeapon(WeaponType.Pistol, nValue.int3);
		WeaponManager.SetSelectWeapon(WeaponType.Rifle, nValue.int0);
		TimerManager.In((float)nValue.int3, delegate()
		{
			if (GameManager.player.Dead)
			{
				UISpectator.SetActive(true);
				CameraManager.SetType(CameraType.Spectate, new object[0]);
			}
		});
		BombManager.DeadPlayer();
	}

	// Token: 0x06002115 RID: 8469 RVA: 0x000CC908 File Offset: 0x000CAB08
	[PunRPC]
	private void OnKilledPlayer(PhotonMessage message)
	{
		DamageInfo e = DamageInfo.Serialize(message.ReadBytes());
		EventManager.Dispatch<DamageInfo>("KillPlayer", e);
		PhotonNetwork.player.SetKills1();
		PlayerRoundManager.SetKills1();
		UIDeathScreen.AddKill(message.sender.ID);
		UIBuyWeapon.Money += nValue.int150;
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

	// Token: 0x06002116 RID: 8470 RVA: 0x000CC99C File Offset: 0x000CAB9C
	[PunRPC]
	private void OnFinishRound(PhotonMessage message)
	{
		byte b = message.ReadByte();
		bool flag = message.ReadBool();
		UIScore2.timeData.active = false;
		GameManager.roundState = RoundState.EndRound;
		GameManager.BalanceTeam(true);
		if ((Team)b == PhotonNetwork.player.GetTeam())
		{
			UIBuyWeapon.Money += ((!flag) ? 2500 : 2750);
		}
		else
		{
			UIBuyWeapon.Money += ((!flag) ? 1750 : 2000);
		}
		if (GameManager.blueScore + GameManager.redScore == GameManager.maxScore / nValue.int2)
		{
			UIDeathScreen.ClearAll();
			if (PhotonNetwork.isMasterClient)
			{
				int value = GameManager.blueScore;
				GameManager.blueScore = GameManager.redScore;
				GameManager.redScore = value;
				GameManager.SetScore();
			}
			if (PhotonNetwork.player.GetTeam() == Team.Blue)
			{
				GameManager.team = Team.Red;
			}
			else if (PhotonNetwork.player.GetTeam() == Team.Red)
			{
				GameManager.team = Team.Blue;
			}
			UIBuyWeapon.Money = nValue.int500;
			this.changeTeam = true;
			WeaponManager.SetSelectWeapon(WeaponType.Knife, AccountManager.GetWeaponSelected(WeaponType.Knife));
			WeaponManager.SetSelectWeapon(WeaponType.Pistol, nValue.int3);
			WeaponManager.SetSelectWeapon(WeaponType.Rifle, nValue.int0);
			PlayerInput.instance.PlayerWeapon.UpdateWeaponAll(WeaponType.Pistol);
		}
		else
		{
			GameManager.BalanceTeam(true);
		}
		if (GameManager.checkScore)
		{
			GameManager.LoadNextLevel(GameMode.Bomb);
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

	// Token: 0x06002117 RID: 8471 RVA: 0x00017293 File Offset: 0x00015493
	[PunRPC]
	private void CheckPlayers(PhotonMessage message)
	{
		this.CheckPlayers();
	}

	// Token: 0x06002118 RID: 8472 RVA: 0x000CCB40 File Offset: 0x000CAD40
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
			if (!BombManager.BombPlaced)
			{
				for (int j = nValue.int0; j < playerList.Length; j++)
				{
					if (playerList[j].GetTeam() == Team.Red && !playerList[j].GetDead())
					{
						flag2 = true;
						break;
					}
				}
			}
			else
			{
				flag2 = true;
			}
			if (!flag)
			{
				GameManager.redScore = ++GameManager.redScore;
				GameManager.SetScore();
				UIMainStatus.Add("[@]", false, (float)nValue.int5, "Red Win");
				PhotonDataWrite data = base.photonView.GetData();
				data.Write(2);
				data.Write(false);
				base.photonView.RPC("OnFinishRound", PhotonTargets.All, data);
			}
			else if (!flag2)
			{
				GameManager.blueScore = ++GameManager.blueScore;
				GameManager.SetScore();
				UIMainStatus.Add("[@]", false, (float)nValue.int5, "Blue Win");
				PhotonDataWrite data2 = base.photonView.GetData();
				data2.Write(1);
				data2.Write(false);
				base.photonView.RPC("OnFinishRound", PhotonTargets.All, data2);
			}
		}
	}

	// Token: 0x06002119 RID: 8473 RVA: 0x000CC034 File Offset: 0x000CA234
	private PhotonPlayer[] GetPlayers()
	{
		PhotonPlayer[] playerList = PhotonNetwork.playerList;
		List<PhotonPlayer> list = new List<PhotonPlayer>();
		for (int i = 0; i < playerList.Length; i++)
		{
			if (playerList[i].GetTeam() != Team.None)
			{
				list.Add(playerList[i]);
			}
		}
		return list.ToArray();
	}

	// Token: 0x0400143B RID: 5179
	public static BombMode2 instance;

	// Token: 0x0400143C RID: 5180
	private bool changeTeam;
}
