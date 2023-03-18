using System;
using Photon;
using UnityEngine;

// Token: 0x020003AC RID: 940
public class OnlyMode : Photon.MonoBehaviour
{
	// Token: 0x06002246 RID: 8774 RVA: 0x00017F80 File Offset: 0x00016180
	private void Awake()
	{
		if (PhotonNetwork.offlineMode)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		if (PhotonNetwork.room.GetGameMode() != GameMode.Only)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06002247 RID: 8775 RVA: 0x000D1790 File Offset: 0x000CF990
	private void Start()
	{
		base.photonView.AddMessage("PhotonOnScore", new PhotonView.MessageDelegate(this.PhotonOnScore));
		base.photonView.AddMessage("OnKilledPlayer", new PhotonView.MessageDelegate(this.OnKilledPlayer));
		base.photonView.AddMessage("PhotonNextLevel", new PhotonView.MessageDelegate(this.PhotonNextLevel));
		GameManager.roundState = RoundState.PlayRound;
		CameraManager.SetType(CameraType.Static, new object[0]);
		UIScore.SetActiveScore(true, this.MaxScore);
		WeaponManager.SetSelectWeapon(WeaponType.Knife, nValue.int0);
		WeaponManager.SetSelectWeapon(WeaponType.Pistol, nValue.int0);
		WeaponManager.SetSelectWeapon(WeaponType.Rifle, nValue.int0);
		int weaponID = PhotonNetwork.room.GetOnlyWeapon();
		WeaponData weaponData = WeaponManager.GetWeaponData(weaponID);
		if (weaponData.Secret || weaponData.Lock)
		{
			weaponID = nValue.int4;
			PhotonNetwork.LeaveRoom(true);
		}
		this.Weapon = WeaponManager.GetWeaponData(weaponID);
		WeaponManager.SetSelectWeapon(this.Weapon.ID);
		UISelectTeam.OnStart(new Action<Team>(this.OnSelectTeam));
		GameManager.maxScore = this.MaxScore;
		GameManager.changeWeapons = false;
		GameManager.StartAutoBalance();
		EventManager.AddListener<DamageInfo>("DeadPlayer", new EventManager.Callback<DamageInfo>(this.OnDeadPlayer));
		EventManager.AddListener<Team>("AutoBalance", new EventManager.Callback<Team>(this.OnAutoBalance));
	}

	// Token: 0x06002248 RID: 8776 RVA: 0x00017FAA File Offset: 0x000161AA
	private void OnEnable()
	{
		PhotonNetwork.onPhotonPlayerConnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Combine(PhotonNetwork.onPhotonPlayerConnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerConnected));
	}

	// Token: 0x06002249 RID: 8777 RVA: 0x00017FCC File Offset: 0x000161CC
	private void OnDisable()
	{
		PhotonNetwork.onPhotonPlayerConnected = (PhotonNetwork.PhotonPlayerDelegate)Delegate.Remove(PhotonNetwork.onPhotonPlayerConnected, new PhotonNetwork.PhotonPlayerDelegate(this.OnPhotonPlayerConnected));
	}

	// Token: 0x0600224A RID: 8778 RVA: 0x00017FEE File Offset: 0x000161EE
	private void OnSelectTeam(Team team)
	{
		UIPanelManager.ShowPanel("Display");
		this.OnRevivalPlayer();
	}

	// Token: 0x0600224B RID: 8779 RVA: 0x00018000 File Offset: 0x00016200
	private void OnAutoBalance(Team team)
	{
		GameManager.team = team;
		this.OnRevivalPlayer();
	}

	// Token: 0x0600224C RID: 8780 RVA: 0x000D18EC File Offset: 0x000CFAEC
	private void OnRevivalPlayer()
	{
		PlayerInput player = GameManager.player;
		player.SetHealth(nValue.int100);
		CameraManager.SetType(CameraType.None, new object[0]);
		GameManager.controller.ActivePlayer(SpawnManager.GetTeamSpawn().spawnPosition, SpawnManager.GetTeamSpawn().spawnRotation);
		player.PlayerWeapon.UpdateWeaponAll(this.Weapon.Type);
	}

	// Token: 0x0600224D RID: 8781 RVA: 0x000D194C File Offset: 0x000CFB4C
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

	// Token: 0x0600224E RID: 8782 RVA: 0x000170B9 File Offset: 0x000152B9
	private void OnPhotonPlayerConnected(PhotonPlayer playerConnect)
	{
		if (PhotonNetwork.isMasterClient)
		{
			GameManager.SetScore(playerConnect);
		}
	}

	// Token: 0x0600224F RID: 8783 RVA: 0x000CBD78 File Offset: 0x000C9F78
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

	// Token: 0x06002250 RID: 8784 RVA: 0x000CB624 File Offset: 0x000C9824
	public void OnScore(Team team)
	{
		PhotonDataWrite data = base.photonView.GetData();
		data.Write((byte)team);
		base.photonView.RPC("PhotonOnScore", PhotonTargets.MasterClient, data);
	}

	// Token: 0x06002251 RID: 8785 RVA: 0x000CB658 File Offset: 0x000C9858
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

	// Token: 0x06002252 RID: 8786 RVA: 0x0001800E File Offset: 0x0001620E
	[PunRPC]
	private void PhotonNextLevel(PhotonMessage message)
	{
		GameManager.LoadNextLevel(GameMode.Only);
	}

	// Token: 0x0400147C RID: 5244
	public CryptoInt MaxScore = 100;

	// Token: 0x0400147D RID: 5245
	private WeaponData Weapon;
}
