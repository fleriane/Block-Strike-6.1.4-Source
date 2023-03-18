using System;
using System.Collections.Generic;
using Photon;
using UnityEngine;

// Token: 0x02000387 RID: 903
public class BombMode : Photon.MonoBehaviour
{
	// Token: 0x060020E2 RID: 8418 RVA: 0x000170DB File Offset: 0x000152DB
	private void Awake()
	{
		if (PhotonNetwork.offlineMode)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		if (PhotonNetwork.room.GetGameMode() != GameMode.Bomb)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			BombMode.instance = this;
		}
	}

	// Token: 0x060020E3 RID: 8419 RVA: 0x000CB70C File Offset: 0x000C990C
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
		UIPanelManager.ShowPanel("Display");
		CameraManager.SetType(CameraType.Static, new object[0]);
		CameraManager.Team = true;
		CameraManager.ChangeType = true;
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

	// Token: 0x060020E4 RID: 8420 RVA: 0x000CB844 File Offset: 0x000C9A44
	private void OnEnable()
	{
		PhotonNetwork.onPhotonPlayerConnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Combine(PhotonNetwork.onPhotonPlayerConnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerConnected));
		PhotonNetwork.onPhotonPlayerDisconnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Combine(PhotonNetwork.onPhotonPlayerDisconnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerDisconnected));
	}

	// Token: 0x060020E5 RID: 8421 RVA: 0x000CB894 File Offset: 0x000C9A94
	private void OnDisable()
	{
		PhotonNetwork.onPhotonPlayerConnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Remove(PhotonNetwork.onPhotonPlayerConnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerConnected));
		PhotonNetwork.onPhotonPlayerDisconnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Remove(PhotonNetwork.onPhotonPlayerDisconnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerDisconnected));
	}

	// Token: 0x060020E6 RID: 8422 RVA: 0x000CB8E4 File Offset: 0x000C9AE4
	private void OnSelectTeam(Team team)
	{
		UIPanelManager.ShowPanel("Display");
		if (team == Team.None)
		{
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

	// Token: 0x060020E7 RID: 8423 RVA: 0x00017110 File Offset: 0x00015310
	private void ActivationWaitPlayer()
	{
		EventManager.Dispatch("WaitPlayer");
		GameManager.roundState = RoundState.WaitPlayer;
		GameManager.team = Team.Red;
		this.OnWaitPlayer();
		this.OnCreatePlayer();
	}

	// Token: 0x060020E8 RID: 8424 RVA: 0x00017134 File Offset: 0x00015334
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

	// Token: 0x060020E9 RID: 8425 RVA: 0x000CB960 File Offset: 0x000C9B60
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

	// Token: 0x060020EA RID: 8426 RVA: 0x0001715F File Offset: 0x0001535F
	private void OnPhotonPlayerDisconnected(PhotonPlayer playerDisconnect)
	{
		if (PhotonNetwork.isMasterClient)
		{
			this.CheckPlayers();
		}
	}

	// Token: 0x060020EB RID: 8427 RVA: 0x000CB9D8 File Offset: 0x000C9BD8
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

	// Token: 0x060020EC RID: 8428 RVA: 0x000CBA30 File Offset: 0x000C9C30
	[PunRPC]
	private void PhotonStartRound(PhotonMessage message)
	{
		EventManager.Dispatch("StartRound");
		float num = (float)nValue.int120;
		num -= (float)(PhotonNetwork.time - message.timestamp);
		UIScore2.StartTime(num, new Action(this.StopTimer));
		this.OnCreatePlayer();
	}

	// Token: 0x060020ED RID: 8429 RVA: 0x000CBA78 File Offset: 0x000C9C78
	private void StopTimer()
	{
		if (PhotonNetwork.isMasterClient)
		{
			GameManager.blueScore = ++GameManager.blueScore;
			GameManager.SetScore();
			UIMainStatus.Add("[@]", false, (float)nValue.int5, "Blue Win");
			base.photonView.RPC("OnFinishRound", PhotonTargets.All);
		}
	}

	// Token: 0x060020EE RID: 8430 RVA: 0x000CBACC File Offset: 0x000C9CCC
	public void Boom()
	{
		if (PhotonNetwork.isMasterClient && GameManager.roundState == RoundState.PlayRound)
		{
			GameManager.redScore = ++GameManager.redScore;
			GameManager.SetScore();
			UIMainStatus.Add("[@]", false, (float)nValue.int5, "Red Win");
			base.photonView.RPC("OnFinishRound", PhotonTargets.All);
		}
	}

	// Token: 0x060020EF RID: 8431 RVA: 0x000CBB2C File Offset: 0x000C9D2C
	public void DeactiveBoom()
	{
		if (PhotonNetwork.isMasterClient && GameManager.roundState == RoundState.PlayRound)
		{
			GameManager.blueScore = ++GameManager.blueScore;
			GameManager.SetScore();
			UIMainStatus.Add("[@]", false, (float)nValue.int5, "Blue Win");
			base.photonView.RPC("OnFinishRound", PhotonTargets.All);
		}
	}

	// Token: 0x060020F0 RID: 8432 RVA: 0x000CBB8C File Offset: 0x000C9D8C
	[PunRPC]
	private void UpdateTimer(PhotonMessage message)
	{
		float time = message.ReadFloat();
		TimerManager.In(nValue.float15, delegate()
		{
			time -= (float)(PhotonNetwork.time - message.timestamp);
			UIScore2.StartTime(time, new Action(this.StopTimer));
		});
	}

	// Token: 0x060020F1 RID: 8433 RVA: 0x000CBBD8 File Offset: 0x000C9DD8
	private void OnCreatePlayer()
	{
		if (PhotonNetwork.player.GetTeam() != Team.None)
		{
			UISpectator.SetActive(false);
			PlayerInput player = GameManager.player;
			player.SetHealth(nValue.int100);
			CameraManager.SetType(CameraType.None, new object[0]);
			GameManager.controller.ActivePlayer(SpawnManager.GetTeamSpawn().spawnPosition, SpawnManager.GetTeamSpawn().spawnRotation);
			player.PlayerWeapon.UpdateWeaponAll(WeaponType.Rifle);
		}
	}

	// Token: 0x060020F2 RID: 8434 RVA: 0x000CBC44 File Offset: 0x000C9E44
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
				UISpectator.SetActive(true);
				CameraManager.SetType(CameraType.Spectate, new object[0]);
			}
		});
		BombManager.DeadPlayer();
	}

	// Token: 0x060020F3 RID: 8435 RVA: 0x000CBD78 File Offset: 0x000C9F78
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

	// Token: 0x060020F4 RID: 8436 RVA: 0x000CBDFC File Offset: 0x000C9FFC
	[PunRPC]
	private void OnFinishRound(PhotonMessage message)
	{
		UIScore2.timeData.active = false;
		GameManager.roundState = RoundState.EndRound;
		GameManager.BalanceTeam(true);
		if (GameManager.blueScore + GameManager.redScore == GameManager.maxScore / nValue.int2)
		{
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

	// Token: 0x060020F5 RID: 8437 RVA: 0x00017171 File Offset: 0x00015371
	[PunRPC]
	private void CheckPlayers(PhotonMessage message)
	{
		this.CheckPlayers();
	}

	// Token: 0x060020F6 RID: 8438 RVA: 0x000CBEF4 File Offset: 0x000CA0F4
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

	// Token: 0x060020F7 RID: 8439 RVA: 0x000CC034 File Offset: 0x000CA234
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

	// Token: 0x04001434 RID: 5172
	public static BombMode instance;
}
