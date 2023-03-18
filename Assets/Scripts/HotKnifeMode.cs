using System;
using System.Collections.Generic;
using Photon;
using UnityEngine;

// Token: 0x020003DF RID: 991
public class HotKnifeMode : Photon.MonoBehaviour
{
	// Token: 0x060023BB RID: 9147 RVA: 0x000192AB File Offset: 0x000174AB
	private void Awake()
	{
		if (PhotonNetwork.offlineMode)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
	}

	// Token: 0x060023BC RID: 9148 RVA: 0x000D7F18 File Offset: 0x000D6118
	private void Start()
	{
		base.photonView.AddMessage("StartTimer", new PhotonView.MessageDelegate(this.StartTimer));
		base.photonView.AddMessage("HitPlayer", new PhotonView.MessageDelegate(this.HitPlayer));
		base.photonView.AddMessage("OnFinishRound", new PhotonView.MessageDelegate(this.OnFinishRound));
		base.photonView.AddMessage("CheckPlayers", new PhotonView.MessageDelegate(this.CheckPlayers));
		PlayerTriggerDetector.isKick = true;
		UIScore.SetActiveScore(true);
		GameManager.startDamageTime = 1f;
		UIPanelManager.ShowPanel("Display");
		WeaponManager.MaxDamage = true;
		GameManager.changeWeapons = false;
		CameraManager.SetType(CameraType.Static, new object[0]);
		TimerManager.In(0.5f, delegate()
		{
			GameManager.team = Team.Blue;
			if (PhotonNetwork.isMasterClient)
			{
				this.ActivationWaitPlayer();
				return;
			}
			CameraManager.SetType(CameraType.Spectate, new object[0]);
		});
		EventManager.AddListener<DamageInfo>("DeadPlayer", new EventManager.Callback<DamageInfo>(this.OnDeadPlayer));
	}

	// Token: 0x060023BD RID: 9149 RVA: 0x000D8008 File Offset: 0x000D6208
	private void OnEnable()
	{
		PhotonNetwork.onPhotonPlayerConnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Combine(PhotonNetwork.onPhotonPlayerConnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerConnected));
		PhotonNetwork.onPhotonPlayerDisconnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Combine(PhotonNetwork.onPhotonPlayerDisconnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerDisconnected));
	}

	// Token: 0x060023BE RID: 9150 RVA: 0x000D8058 File Offset: 0x000D6258
	private void OnDisable()
	{
		PhotonNetwork.onPhotonPlayerConnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Remove(PhotonNetwork.onPhotonPlayerConnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerConnected));
		PhotonNetwork.onPhotonPlayerDisconnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Remove(PhotonNetwork.onPhotonPlayerDisconnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerDisconnected));
	}

	// Token: 0x060023BF RID: 9151 RVA: 0x000192BB File Offset: 0x000174BB
	private void ActivationWaitPlayer()
	{
		EventManager.Dispatch("WaitPlayer");
		GameManager.roundState = RoundState.WaitPlayer;
		this.OnWaitPlayer();
		this.OnCreatePlayer(true);
		this.HitPlayer(-2);
	}

	// Token: 0x060023C0 RID: 9152 RVA: 0x000192E2 File Offset: 0x000174E2
	private void OnWaitPlayer()
	{
		UIStatus.Add(Localization.Get("Waiting for other players", true), true);
		TimerManager.In(4f, delegate()
		{
			if (GameManager.roundState == RoundState.WaitPlayer)
			{
				if (PhotonNetwork.playerList.Length <= 1)
				{
					this.OnWaitPlayer();
					return;
				}
				TimerManager.In(4f, delegate()
				{
					this.OnStartRound();
				});
			}
		});
	}

	// Token: 0x060023C1 RID: 9153 RVA: 0x0001930C File Offset: 0x0001750C
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

	// Token: 0x060023C2 RID: 9154 RVA: 0x00019328 File Offset: 0x00017528
	private void OnPhotonPlayerDisconnected(PhotonPlayer playerDisconnect)
	{
		if (PhotonNetwork.isMasterClient)
		{
			this.CheckPlayers();
		}
	}

	// Token: 0x060023C3 RID: 9155 RVA: 0x000D80A8 File Offset: 0x000D62A8
	private void OnStartRound()
	{
		DecalsManager.ClearBulletHoles();
		if (PhotonNetwork.playerList.Length <= 1)
		{
			this.ActivationWaitPlayer();
			return;
		}
		if (PhotonNetwork.isMasterClient)
		{
			GameManager.roundState = RoundState.PlayRound;
			int id = PhotonNetwork.playerList[UnityEngine.Random.Range(0, PhotonNetwork.playerList.Length)].ID;
			PhotonDataWrite data = base.photonView.GetData();
			data.Write(id);
			data.Write(true);
			base.photonView.RPC("StartTimer", PhotonTargets.All, data);
		}
	}

	// Token: 0x060023C4 RID: 9156 RVA: 0x000D8120 File Offset: 0x000D6320
	[PunRPC]
	private void StartTimer(PhotonMessage message)
	{
		int knifeID = message.ReadInt();
		bool spawn = message.ReadBool();
		UIScore.timeData.active = false;
		this.KnifeID = knifeID;
		UIScore.StartTime(45f - (float)(PhotonNetwork.time - message.timestamp), new Action(this.StopTimer));
		this.OnCreatePlayer(spawn);
	}

	// Token: 0x060023C5 RID: 9157 RVA: 0x000D8178 File Offset: 0x000D6378
	private void StopTimer()
	{
		if (PhotonNetwork.player.ID == this.KnifeID)
		{
			PlayerInput player = GameManager.player;
			DamageInfo damageInfo = DamageInfo.Get(101, Vector3.zero, Team.Blue, player.PlayerWeapon.GetSelectedWeaponData().ID, 0, PhotonNetwork.player.ID, false);
			player.Damage(damageInfo);
		}
	}

	// Token: 0x060023C6 RID: 9158 RVA: 0x000D81D4 File Offset: 0x000D63D4
	private void OnCreatePlayer(bool spawn)
	{
		if (PhotonNetwork.player.GetTeam() != Team.None)
		{
			PlayerInput player = GameManager.player;
			if (spawn)
			{
				player.SetHealth(nValue.int100);
				CameraManager.SetType(CameraType.None, new object[0]);
				SpawnPoint playerIDSpawn = SpawnManager.GetPlayerIDSpawn();
				GameManager.controller.ActivePlayer(playerIDSpawn.spawnPosition, playerIDSpawn.spawnRotation);
				this.HitPlayer(this.KnifeID);
				return;
			}
			if (!PhotonNetwork.player.GetDead())
			{
				this.HitPlayer(this.KnifeID);
			}
		}
	}

	// Token: 0x060023C7 RID: 9159 RVA: 0x000D8250 File Offset: 0x000D6450
	private void OnDeadPlayer(DamageInfo damageInfo)
	{
		if (damageInfo.player == PhotonNetwork.player.ID)
		{
			GameManager.redScore = (GameManager.redScore = ++GameManager.redScore);
			PhotonNetwork.player.SetDeaths1();
			PlayerRoundManager.SetDeaths1();
			UIStatus.Add(damageInfo);
			Vector3 ragdollForce = Utils.GetRagdollForce(GameManager.player.PlayerTransform.position, damageInfo.position);
			CameraManager.SetType(CameraType.Dead, new object[]
			{
				GameManager.player.FPCamera.Transform.position,
				GameManager.player.FPCamera.Transform.eulerAngles,
				ragdollForce * 100f
			});
			GameManager.controller.DeactivePlayer(ragdollForce, damageInfo.headshot);
			base.photonView.RPC("CheckPlayers", PhotonTargets.MasterClient);
			TimerManager.In(3f, delegate()
			{
				if (GameManager.player.Dead)
				{
					CameraManager.SetType(CameraType.Spectate, new object[0]);
				}
			});
			return;
		}
		UIStatus.Add("[@]: " + UIStatus.GetTeamHexColor(PhotonNetwork.player), false, "HotKnife");
		PhotonDataWrite data = base.photonView.GetData();
		data.Write(PhotonNetwork.player.ID);
		base.photonView.RPC("HitPlayer", PhotonTargets.All, data);
	}

	// Token: 0x060023C8 RID: 9160 RVA: 0x00019337 File Offset: 0x00017537
	[PunRPC]
	private void HitPlayer(PhotonMessage message)
	{
		this.HitPlayer(message.ReadInt());
	}

	// Token: 0x060023C9 RID: 9161 RVA: 0x000D83A8 File Offset: 0x000D65A8
	private void HitPlayer(int knife)
	{
		this.KnifeID = knife;
		if (PhotonNetwork.player.GetTeam() != Team.None && !PhotonNetwork.player.GetDead())
		{
			PlayerInput playerInput = GameManager.player;
			playerInput.SetHealth(nValue.int100);
			if (PhotonNetwork.player.ID == knife)
			{
				GameManager.team = Team.Red;
				GameManager.controller.SetTeam(Team.Red);
				WeaponManager.SetSelectWeapon(WeaponType.Knife, 4);
				WeaponManager.SetSelectWeapon(WeaponType.Pistol, 0);
				WeaponManager.SetSelectWeapon(WeaponType.Rifle, 0);
				playerInput.PlayerWeapon.UpdateWeaponAll(WeaponType.Knife);
				UIHealth.SetHealth(0);
			}
			else if (WeaponManager.HasSelectWeapon(WeaponType.Knife))
			{
				GameManager.team = Team.Blue;
				GameManager.controller.SetTeam(Team.Blue);
				WeaponManager.SetSelectWeapon(WeaponType.Knife, 0);
				WeaponManager.SetSelectWeapon(WeaponType.Pistol, 0);
				WeaponManager.SetSelectWeapon(WeaponType.Rifle, 47);
				TimerManager.In(0.1f, delegate()
				{
					playerInput.PlayerWeapon.GetWeaponData(WeaponType.Rifle).AmmoMax = 0;
					playerInput.PlayerWeapon.GetWeaponData(WeaponType.Rifle).Ammo = 0;
					UIAmmo.SetAmmo(0, -1);
					UIHealth.SetHealth(0);
				});
				playerInput.PlayerWeapon.UpdateWeaponAll(WeaponType.Rifle);
			}
		}
		if (PhotonNetwork.isMasterClient && !UIScore.timeData.active && GameManager.roundState == RoundState.PlayRound)
		{
			this.CheckPlayers();
		}
	}

	// Token: 0x060023CA RID: 9162 RVA: 0x000D84C0 File Offset: 0x000D66C0
	[PunRPC]
	private void OnFinishRound(PhotonMessage message)
	{
		int num = message.ReadInt();
		UIScore.timeData.active = false;
		GameManager.roundState = RoundState.EndRound;
		if (PhotonNetwork.player.ID == num)
		{
			PhotonNetwork.player.SetKills1();
			PlayerRoundManager.SetXP(nValue.int12);
			PlayerRoundManager.SetMoney(nValue.int7);
		}
		if (GameManager.checkScore)
		{
			GameManager.LoadNextLevel(GameMode.Classic);
			TimerManager.In(8f, delegate()
			{
				PhotonNetwork.LeaveRoom(true);
			});
			return;
		}
		TimerManager.In(8f - (float)(PhotonNetwork.time - message.timestamp), delegate()
		{
			this.OnStartRound();
		});
	}

	// Token: 0x060023CB RID: 9163 RVA: 0x000D8570 File Offset: 0x000D6770
	private void SelectKnife()
	{
		PhotonPlayer[] playerList = PhotonNetwork.playerList;
		List<int> list = new List<int>();
		for (int i = 0; i < playerList.Length; i++)
		{
			if (!playerList[i].GetDead())
			{
				list.Add(i);
			}
		}
		int id = playerList[list[UnityEngine.Random.Range(0, list.Count)]].ID;
		PhotonDataWrite data = base.photonView.GetData();
		data.Write(id);
		data.Write(false);
		base.photonView.RPC("StartTimer", PhotonTargets.All, data);
		UIStatus.Add("[@]: " + UIStatus.GetTeamHexColor(PhotonPlayer.Find(id)), false, "HotKnife");
	}

	// Token: 0x060023CC RID: 9164 RVA: 0x00019345 File Offset: 0x00017545
	[PunRPC]
	private void CheckPlayers(PhotonMessage message)
	{
		this.CheckPlayers();
	}

	// Token: 0x060023CD RID: 9165 RVA: 0x000D8614 File Offset: 0x000D6814
	private void CheckPlayers()
	{
		if (PhotonNetwork.isMasterClient && GameManager.roundState != RoundState.EndRound)
		{
			PhotonPlayer[] playerList = PhotonNetwork.playerList;
			int num = -1;
			for (int i = 0; i < playerList.Length; i++)
			{
				if (!playerList[i].GetDead())
				{
					if (num == -1)
					{
						num = playerList[i].ID;
					}
					else
					{
						num = -2;
					}
				}
			}
			if (num == -1)
			{
				UIMainStatus.Add("[@]", false, 5f, "Draw");
				PhotonDataWrite data = base.photonView.GetData();
				data.Write(-2);
				base.photonView.RPC("OnFinishRound", PhotonTargets.All, data);
				return;
			}
			if (num >= 0)
			{
				GameManager.blueScore = (GameManager.blueScore = ++GameManager.blueScore);
				PhotonPlayer photonPlayer = PhotonPlayer.Find(num);
				if (photonPlayer.ID == PhotonNetwork.player.ID)
				{
					GameManager.blueScore = (GameManager.blueScore = ++GameManager.blueScore);
				}
				UIMainStatus.Add(photonPlayer.UserId + " [@]", false, 5f, "Win");
				PhotonDataWrite data2 = base.photonView.GetData();
				data2.Write(photonPlayer.ID);
				base.photonView.RPC("OnFinishRound", PhotonTargets.All, data2);
				return;
			}
			bool flag = false;
			for (int j = 0; j < playerList.Length; j++)
			{
				if (playerList[j].ID == this.KnifeID && !playerList[j].GetDead())
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.SelectKnife();
			}
		}
	}

	// Token: 0x04001551 RID: 5457
	private int KnifeID;

	// Token: 0x04001552 RID: 5458
	public static int Enable;
}
