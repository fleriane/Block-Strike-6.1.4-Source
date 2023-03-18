using System;
using System.Collections.Generic;
using System.Linq;
using Photon;
using UnityEngine;

// Token: 0x0200039F RID: 927
public class HunterMode : Photon.MonoBehaviour
{
	// Token: 0x060021CB RID: 8651 RVA: 0x00017A0F File Offset: 0x00015C0F
	private void Awake()
	{
		if (PhotonNetwork.offlineMode)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		if (PhotonNetwork.room.GetGameMode() != GameMode.Hunter)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x060021CC RID: 8652 RVA: 0x000CF6D4 File Offset: 0x000CD8D4
	private void Start()
	{
		base.photonView.AddMessage("OnSendKillerInfo", new PhotonView.MessageDelegate(this.OnSendKillerInfo));
		base.photonView.AddMessage("UpdateTimer", new PhotonView.MessageDelegate(this.UpdateTimer));
		base.photonView.AddMessage("OnKilledPlayer", new PhotonView.MessageDelegate(this.OnKilledPlayer));
		base.photonView.AddMessage("OnFinishRound", new PhotonView.MessageDelegate(this.OnFinishRound));
		base.photonView.AddMessage("CheckPlayers", new PhotonView.MessageDelegate(this.CheckPlayers));
		UIScore.SetActiveScore(true, nValue.int20);
		GameManager.startDamageTime = (float)nValue.int1;
		UIPanelManager.ShowPanel("Display");
		GameManager.maxScore = nValue.int20;
		GameManager.changeWeapons = false;
		GameManager.globalChat = false;
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

	// Token: 0x060021CD RID: 8653 RVA: 0x000CF7E8 File Offset: 0x000CD9E8
	private void OnEnable()
	{
		PhotonNetwork.onPhotonPlayerConnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Combine(PhotonNetwork.onPhotonPlayerConnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerConnected));
		PhotonNetwork.onPhotonPlayerDisconnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Combine(PhotonNetwork.onPhotonPlayerDisconnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerDisconnected));
	}

	// Token: 0x060021CE RID: 8654 RVA: 0x000CF838 File Offset: 0x000CDA38
	private void OnDisable()
	{
		PhotonNetwork.onPhotonPlayerConnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Remove(PhotonNetwork.onPhotonPlayerConnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerConnected));
		PhotonNetwork.onPhotonPlayerDisconnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Remove(PhotonNetwork.onPhotonPlayerDisconnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerDisconnected));
	}

	// Token: 0x060021CF RID: 8655 RVA: 0x00017A38 File Offset: 0x00015C38
	private void ActivationWaitPlayer()
	{
		EventManager.Dispatch("WaitPlayer");
		GameManager.roundState = RoundState.WaitPlayer;
		GameManager.team = Team.Blue;
		this.OnWaitPlayer();
		this.OnCreatePlayer();
	}

	// Token: 0x060021D0 RID: 8656 RVA: 0x00017A5C File Offset: 0x00015C5C
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

	// Token: 0x060021D1 RID: 8657 RVA: 0x000CF888 File Offset: 0x000CDA88
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
					PhotonDataWrite data = base.photonView.GetData();
					data.Write(UIScore.timeData.endTime - Time.time);
					base.photonView.RPC("UpdateTimer", playerConnect, data);
				}
			}
		}
	}

	// Token: 0x060021D2 RID: 8658 RVA: 0x00017A87 File Offset: 0x00015C87
	private void OnPhotonPlayerDisconnected(PhotonPlayer playerDisconnect)
	{
		if (PhotonNetwork.isMasterClient)
		{
			this.CheckPlayers();
		}
	}

	// Token: 0x060021D3 RID: 8659 RVA: 0x000CF8F8 File Offset: 0x000CDAF8
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
			List<PhotonPlayer> list = PhotonNetwork.playerList.ToList<PhotonPlayer>();
			int num = this.OnSelectMaxDeaths(list.Count);
			string text = string.Empty;
			for (int i = 0; i < num; i++)
			{
				int index = UnityEngine.Random.Range(nValue.int0, list.Count);
				text = text + list[index].ID + "#";
				list.RemoveAt(index);
			}
			GameManager.roundState = RoundState.PlayRound;
			PhotonDataWrite data = base.photonView.GetData();
			data.Write(text);
			base.photonView.RPC("OnSendKillerInfo", PhotonTargets.All, data);
		}
	}

	// Token: 0x060021D4 RID: 8660 RVA: 0x000CF9CC File Offset: 0x000CDBCC
	[PunRPC]
	private void OnSendKillerInfo(PhotonMessage message)
	{
		string[] array = message.ReadString().Split(new char[]
		{
			"#"[nValue.int0]
		});
		bool flag = false;
		for (int i = 0; i < array.Length - nValue.int1; i++)
		{
			if (PhotonNetwork.player.ID == int.Parse(array[i]))
			{
				flag = true;
				break;
			}
		}
		float num = (float)nValue.int120;
		num -= (float)(PhotonNetwork.time - message.timestamp);
		UIScore.StartTime(num, new Action(this.StopTimer));
		if (flag)
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

	// Token: 0x060021D5 RID: 8661 RVA: 0x000CFAA0 File Offset: 0x000CDCA0
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

	// Token: 0x060021D6 RID: 8662 RVA: 0x000CFAEC File Offset: 0x000CDCEC
	private void StopTimer()
	{
		if (PhotonNetwork.isMasterClient)
		{
			GameManager.redScore = ++GameManager.redScore;
			GameManager.SetScore();
			UIMainStatus.Add("[@]", false, (float)nValue.int5, "Red Win");
			base.photonView.RPC("OnFinishRound", PhotonTargets.All);
		}
	}

	// Token: 0x060021D7 RID: 8663 RVA: 0x000CFB40 File Offset: 0x000CDD40
	private void OnCreatePlayer()
	{
		PlayerInput player = GameManager.player;
		player.SetHealth(nValue.int100);
		CameraManager.SetType(CameraType.None, new object[0]);
		GameManager.controller.ActivePlayer(SpawnManager.GetTeamSpawn().spawnPosition, SpawnManager.GetTeamSpawn().spawnRotation);
		if (player.PlayerTeam == Team.Blue)
		{
			WeaponManager.SetSelectWeapon(WeaponType.Pistol, nValue.int6);
			WeaponManager.SetSelectWeapon(WeaponType.Rifle, nValue.int0);
			WeaponCustomData weaponCustomData = new WeaponCustomData();
			weaponCustomData.Ammo = nValue.int1;
			weaponCustomData.AmmoMax = nValue.int0;
			player.PlayerWeapon.UpdateWeaponAll(WeaponType.Pistol, null, weaponCustomData, null);
		}
		else
		{
			WeaponManager.SetSelectWeapon(WeaponType.Pistol, AccountManager.GetWeaponSelected(WeaponType.Pistol));
			WeaponManager.SetSelectWeapon(WeaponType.Rifle, AccountManager.GetWeaponSelected(WeaponType.Rifle));
			player.PlayerWeapon.UpdateWeaponAll(WeaponType.Rifle);
		}
	}

	// Token: 0x060021D8 RID: 8664 RVA: 0x000CFC08 File Offset: 0x000CDE08
	private void OnDeadPlayer(DamageInfo damageInfo)
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
		TimerManager.In((float)nValue.int3, delegate()
		{
			if (GameManager.player.Dead)
			{
				CameraManager.SetType(CameraType.Spectate, new object[0]);
			}
		});
	}

	// Token: 0x060021D9 RID: 8665 RVA: 0x000CBD78 File Offset: 0x000C9F78
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

	// Token: 0x060021DA RID: 8666 RVA: 0x000CFD34 File Offset: 0x000CDF34
	[PunRPC]
	private void OnFinishRound(PhotonMessage message)
	{
		GameManager.roundState = RoundState.EndRound;
		UIScore.timeData.active = false;
		if (GameManager.checkScore)
		{
			GameManager.LoadNextLevel(GameMode.Hunter);
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

	// Token: 0x060021DB RID: 8667 RVA: 0x00017A99 File Offset: 0x00015C99
	[PunRPC]
	private void CheckPlayers(PhotonMessage message)
	{
		this.CheckPlayers();
	}

	// Token: 0x060021DC RID: 8668 RVA: 0x000CFD90 File Offset: 0x000CDF90
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

	// Token: 0x060021DD RID: 8669 RVA: 0x00017AA1 File Offset: 0x00015CA1
	private int OnSelectMaxDeaths(int maxPlayers)
	{
		if (maxPlayers >= nValue.int10)
		{
			return nValue.int4;
		}
		if (maxPlayers >= nValue.int7)
		{
			return nValue.int3;
		}
		if (maxPlayers >= nValue.int4)
		{
			return nValue.int2;
		}
		return nValue.int1;
	}
}
