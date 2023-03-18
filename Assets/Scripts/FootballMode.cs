using System;
using System.Collections.Generic;
using System.Linq;
using Photon;

// Token: 0x020003DD RID: 989
public class FootballMode : MonoBehaviour
{
	// Token: 0x060023A0 RID: 9120 RVA: 0x000D78B4 File Offset: 0x000D5AB4
	private void Start()
	{
		base.photonView.AddMessage("OnCreatePlayer", new PhotonView.MessageDelegate(this.OnCreatePlayer));
		base.photonView.AddMessage("AutoGoal", new PhotonView.MessageDelegate(this.AutoGoal));
		base.photonView.AddMessage("GoalPlayer", new PhotonView.MessageDelegate(this.GoalPlayer));
		base.photonView.AddMessage("OnFinishRound", new PhotonView.MessageDelegate(this.OnFinishRound));
		GameManager.maxScore = nValue.int0;
		UIScore.SetActiveScore(true, nValue.int0);
		GameManager.startDamageTime = (float)(-(float)nValue.int1);
		UIPanelManager.ShowPanel("Display");
		CameraManager.SetType(CameraType.Static, new object[0]);
		GameManager.player.PlayerWeapon.PushRigidbody = true;
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
		TimerManager.In(nValue.float05, delegate()
		{
			PlayerInput player = GameManager.player;
			player.BunnyHopEnabled = true;
			player.BunnyHopSpeed = nValue.float025;
			player.FPController.MotorJumpForce = nValue.float02;
			player.FPController.MotorAirSpeed = (float)nValue.int1;
		});
	}

	// Token: 0x060023A1 RID: 9121 RVA: 0x000D79E4 File Offset: 0x000D5BE4
	private void OnEnable()
	{
		PhotonNetwork.onPhotonPlayerConnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Combine(PhotonNetwork.onPhotonPlayerConnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerConnected));
		PhotonNetwork.onPhotonPlayerDisconnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Combine(PhotonNetwork.onPhotonPlayerDisconnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerDisconnected));
	}

	// Token: 0x060023A2 RID: 9122 RVA: 0x000D7A34 File Offset: 0x000D5C34
	private void OnDisable()
	{
		PhotonNetwork.onPhotonPlayerConnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Remove(PhotonNetwork.onPhotonPlayerConnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerConnected));
		PhotonNetwork.onPhotonPlayerDisconnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Remove(PhotonNetwork.onPhotonPlayerDisconnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerDisconnected));
	}

	// Token: 0x060023A3 RID: 9123 RVA: 0x00019182 File Offset: 0x00017382
	private void OnSelectTeam(Team team)
	{
		UIPanelManager.ShowPanel("Display");
		if (GameManager.player.Dead)
		{
			CameraManager.SetType(CameraType.Spectate, new object[0]);
		}
	}

	// Token: 0x060023A4 RID: 9124 RVA: 0x000191A9 File Offset: 0x000173A9
	private void ActivationWaitPlayer()
	{
		EventManager.Dispatch("WaitPlayer");
		GameManager.roundState = RoundState.WaitPlayer;
		GameManager.team = Team.Blue;
		this.OnWaitPlayer();
		this.OnCreatePlayer();
	}

	// Token: 0x060023A5 RID: 9125 RVA: 0x000191CD File Offset: 0x000173CD
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

	// Token: 0x060023A6 RID: 9126 RVA: 0x000170B9 File Offset: 0x000152B9
	private void OnPhotonPlayerConnected(PhotonPlayer playerConnect)
	{
		if (PhotonNetwork.isMasterClient)
		{
			GameManager.SetScore(playerConnect);
		}
	}

	// Token: 0x060023A7 RID: 9127 RVA: 0x000191F8 File Offset: 0x000173F8
	private void OnPhotonPlayerDisconnected(PhotonPlayer playerDisconnect)
	{
		if (PhotonNetwork.isMasterClient)
		{
			this.CheckPlayers();
		}
	}

	// Token: 0x060023A8 RID: 9128 RVA: 0x000D7A84 File Offset: 0x000D5C84
	private void OnStartRound()
	{
		DecalsManager.ClearBulletHoles();
		FootballManager.StartRound();
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

	// Token: 0x060023A9 RID: 9129 RVA: 0x0001920A File Offset: 0x0001740A
	public void OnCreatePlayer()
	{
		this.OnCreatePlayer(new PhotonMessage());
	}

	// Token: 0x060023AA RID: 9130 RVA: 0x000D7AD8 File Offset: 0x000D5CD8
	[PunRPC]
	private void OnCreatePlayer(PhotonMessage message)
	{
		if (PhotonNetwork.player.GetTeam() != Team.None)
		{
			WeaponManager.SetSelectWeapon(WeaponType.Pistol, nValue.int0);
			WeaponManager.SetSelectWeapon(WeaponType.Rifle, nValue.int0);
			PlayerInput player = GameManager.player;
			player.SetHealth(nValue.int100);
			if (message.timestamp != (double)nValue.int0)
			{
				float duration = (float)(PhotonNetwork.time - message.timestamp + (double)nValue.int3);
				player.SetMove(false, duration);
			}
			CameraManager.SetType(CameraType.None, new object[0]);
			int num = UIPlayerStatistics.GetPlayerStatsPosition(PhotonNetwork.player) - nValue.int1;
			if (PhotonNetwork.player.GetTeam() == Team.Red)
			{
				num += nValue.int6;
			}
			GameManager.controller.ActivePlayer(SpawnManager.GetSpawn(num).spawnPosition, SpawnManager.GetSpawn(num).spawnRotation);
			player.PlayerWeapon.UpdateWeaponAll(WeaponType.Knife);
		}
	}

	// Token: 0x060023AB RID: 9131 RVA: 0x000D7BAC File Offset: 0x000D5DAC
	public void Goal(PhotonPlayer player, Team gateTeam)
	{
		bool flag = false;
		UIMainStatus.Add(player.UserId + " [@]", false, (float)nValue.int5, "scored a goal");
		if (player.GetTeam() == Team.Red)
		{
			if (gateTeam == Team.Blue)
			{
				GameManager.redScore = ++GameManager.redScore;
				GameManager.SetScore();
			}
			else
			{
				GameManager.blueScore = ++GameManager.blueScore;
				GameManager.SetScore();
				flag = true;
			}
		}
		else if (gateTeam == Team.Red)
		{
			GameManager.blueScore = ++GameManager.blueScore;
			GameManager.SetScore();
		}
		else
		{
			GameManager.redScore = ++GameManager.redScore;
			GameManager.SetScore();
			flag = true;
		}
		base.photonView.RPC("OnFinishRound", PhotonTargets.All);
		if (flag)
		{
			TimerManager.In(nValue.float15, delegate()
			{
				UIMainStatus.Add("[@]", false, (float)nValue.int3, "Autogoal");
			});
			base.photonView.RPC("AutoGoal", player);
		}
		else
		{
			base.photonView.RPC("GoalPlayer", player);
		}
	}

	// Token: 0x060023AC RID: 9132 RVA: 0x00019217 File Offset: 0x00017417
	[PunRPC]
	private void GoalPlayer(PhotonMessage message)
	{
		PhotonNetwork.player.SetKills1();
		PlayerRoundManager.SetXP(nValue.int17);
		PlayerRoundManager.SetMoney(nValue.int15);
	}

	// Token: 0x060023AD RID: 9133 RVA: 0x00019237 File Offset: 0x00017437
	[PunRPC]
	private void AutoGoal(PhotonMessage message)
	{
		PhotonNetwork.player.SetDeaths1();
	}

	// Token: 0x060023AE RID: 9134 RVA: 0x000D7CC0 File Offset: 0x000D5EC0
	[PunRPC]
	private void OnFinishRound(PhotonMessage message)
	{
		GameManager.roundState = RoundState.EndRound;
		GameManager.BalanceTeam(true);
		float delay = (float)nValue.int5 - (float)(PhotonNetwork.time - message.timestamp);
		TimerManager.In(delay, delegate()
		{
			this.OnStartRound();
		});
	}

	// Token: 0x060023AF RID: 9135 RVA: 0x000D7D04 File Offset: 0x000D5F04
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
			if (!flag || !flag2)
			{
				base.photonView.RPC("OnFinishRound", PhotonTargets.All);
			}
		}
	}

	// Token: 0x060023B0 RID: 9136 RVA: 0x000D7DC8 File Offset: 0x000D5FC8
	private void UpdateMasterServer()
	{
		if (PhotonNetwork.isMasterClient && GameManager.roundState == RoundState.PlayRound)
		{
			List<PhotonPlayer> list = PhotonNetwork.playerList.ToList<PhotonPlayer>();
			list.Sort(new Comparison<PhotonPlayer>(FootballMode.SortByPing));
			if (list[nValue.int0].ID != PhotonNetwork.player.ID)
			{
				PhotonNetwork.SetMasterClient(list[nValue.int0]);
			}
		}
	}

	// Token: 0x060023B1 RID: 9137 RVA: 0x000D7E38 File Offset: 0x000D6038
	public static int SortByPing(PhotonPlayer a, PhotonPlayer b)
	{
		return a.GetPing().CompareTo(b.GetPing());
	}
}
