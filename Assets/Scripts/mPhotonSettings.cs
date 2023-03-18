using System;
using System.Collections.Generic;
using BSCM;
using ExitGames.Client.Photon;
using FreeJSON;
using UnityEngine;

// Token: 0x020004A0 RID: 1184
public class mPhotonSettings : MonoBehaviour
{
    // Token: 0x17000471 RID: 1137
    // (get) Token: 0x0600291F RID: 10527 RVA: 0x0001CB28 File Offset: 0x0001AD28
    // (set) Token: 0x06002920 RID: 10528 RVA: 0x0001CB39 File Offset: 0x0001AD39
    public static string region
    {
        get
        {
            return nPlayerPrefs.GetString("Region", "Best");
        }
        set
        {
            nPlayerPrefs.SetString("Region", value);
        }
    }

    // Token: 0x06002921 RID: 10529 RVA: 0x0001CB46 File Offset: 0x0001AD46
    private void Start()
    {
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.offlineMode = false;
        PhotonNetwork.UseRpcMonoBehaviourCache = true;
    }

    // Token: 0x06002922 RID: 10530 RVA: 0x000F4258 File Offset: 0x000F2458
    private void OnEnable()
    {
        PhotonNetwork.onConnectedToPhoton = (PhotonNetwork.VoidDelegate)Delegate.Combine(PhotonNetwork.onConnectedToPhoton, new PhotonNetwork.VoidDelegate(this.OnConnectedToPhoton));
        PhotonNetwork.onDisconnectedFromPhoton = (PhotonNetwork.VoidDelegate)Delegate.Combine(PhotonNetwork.onDisconnectedFromPhoton, new PhotonNetwork.VoidDelegate(this.OnDisconnectedFromPhoton));
        PhotonNetwork.onConnectionFail = (PhotonNetwork.DisconnectCauseDelegate)Delegate.Combine(PhotonNetwork.onConnectionFail, new PhotonNetwork.DisconnectCauseDelegate(this.OnConnectionFail));
        PhotonNetwork.onJoinedRoom = (PhotonNetwork.VoidDelegate)Delegate.Combine(PhotonNetwork.onJoinedRoom, new PhotonNetwork.VoidDelegate(this.OnJoinedRoom));
        PhotonNetwork.onPhotonJoinRoomFailed = (PhotonNetwork.ResponseDelegate)Delegate.Combine(PhotonNetwork.onPhotonJoinRoomFailed, new PhotonNetwork.ResponseDelegate(this.OnPhotonJoinRoomFailed));
        PhotonNetwork.onPhotonCreateRoomFailed = (PhotonNetwork.ResponseDelegate)Delegate.Combine(PhotonNetwork.onPhotonCreateRoomFailed, new PhotonNetwork.ResponseDelegate(this.OnPhotonCreateRoomFailed));
        PhotonNetwork.onCustomAuthenticationFailed = (PhotonNetwork.StringDelegate)Delegate.Combine(PhotonNetwork.onCustomAuthenticationFailed, new PhotonNetwork.StringDelegate(this.OnCustomAuthenticationFailed));
        PhotonNetwork.onCustomAuthenticationResponse = (PhotonNetwork.ObjectsDelegate)Delegate.Combine(PhotonNetwork.onCustomAuthenticationResponse, new PhotonNetwork.ObjectsDelegate(this.OnCustomAuthenticationResponse));
    }

    // Token: 0x06002923 RID: 10531 RVA: 0x000F4368 File Offset: 0x000F2568
    private void OnDisable()
    {
        PhotonNetwork.onConnectedToPhoton = (PhotonNetwork.VoidDelegate)Delegate.Remove(PhotonNetwork.onConnectedToPhoton, new PhotonNetwork.VoidDelegate(this.OnConnectedToPhoton));
        PhotonNetwork.onDisconnectedFromPhoton = (PhotonNetwork.VoidDelegate)Delegate.Remove(PhotonNetwork.onDisconnectedFromPhoton, new PhotonNetwork.VoidDelegate(this.OnDisconnectedFromPhoton));
        PhotonNetwork.onConnectionFail = (PhotonNetwork.DisconnectCauseDelegate)Delegate.Remove(PhotonNetwork.onConnectionFail, new PhotonNetwork.DisconnectCauseDelegate(this.OnConnectionFail));
        PhotonNetwork.onJoinedRoom = (PhotonNetwork.VoidDelegate)Delegate.Remove(PhotonNetwork.onJoinedRoom, new PhotonNetwork.VoidDelegate(this.OnJoinedRoom));
        PhotonNetwork.onPhotonJoinRoomFailed = (PhotonNetwork.ResponseDelegate)Delegate.Remove(PhotonNetwork.onPhotonJoinRoomFailed, new PhotonNetwork.ResponseDelegate(this.OnPhotonJoinRoomFailed));
        PhotonNetwork.onPhotonCreateRoomFailed = (PhotonNetwork.ResponseDelegate)Delegate.Remove(PhotonNetwork.onPhotonCreateRoomFailed, new PhotonNetwork.ResponseDelegate(this.OnPhotonCreateRoomFailed));
        PhotonNetwork.onCustomAuthenticationFailed = (PhotonNetwork.StringDelegate)Delegate.Remove(PhotonNetwork.onCustomAuthenticationFailed, new PhotonNetwork.StringDelegate(this.OnCustomAuthenticationFailed));
        PhotonNetwork.onCustomAuthenticationResponse = (PhotonNetwork.ObjectsDelegate)Delegate.Remove(PhotonNetwork.onCustomAuthenticationResponse, new PhotonNetwork.ObjectsDelegate(this.OnCustomAuthenticationResponse));
    }

    // Token: 0x06002924 RID: 10532 RVA: 0x000F4478 File Offset: 0x000F2678
    public static void Connect()
    {
        if (!AccountManager.isConnect)
        {
            UIToast.Show(Localization.Get("Connection account", true));
            return;
        }
        if (PhotonNetwork.connected && !mPhotonSettings.newRegion)
        {
            mPhotonSettings.connectedCallback();
            return;
        }
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.Disconnect();
        }
        mPopUp.ShowText(Localization.Get("Connecting to the region", true) + "...");
        string bundleVersion = VersionManager.bundleVersion;
        string appid = mPhotonSettings.APPID;
        PhotonNetwork.AuthValues = new AuthenticationValues();
        PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Custom;
        PhotonNetwork.AuthValues.UserId = AccountManager.instance.Data.AccountName;
        PhotonNetwork.AuthValues.AddAuthParameter("f", "7");
        JsonObject jsonObject = new JsonObject();
        jsonObject.Add("v", bundleVersion);
        jsonObject.Add("m", NetworkingPeer.AppToken);
        jsonObject.Add("e", AccountManager.AccountID);
        jsonObject.Add("s", AccountManager.instance.Data.Session);
        PhotonNetwork.AuthValues.AddAuthParameter("v", Utils.XOR(jsonObject.ToString(), true));
        if (nPlayerPrefs.HasKey("Region"))
        {
            PhotonNetwork.ConnectToRegion(Region.Parse(mPhotonSettings.region), bundleVersion, appid);
        }
        else
        {
            PhotonNetwork.ConnectToBestCloudServer(bundleVersion, appid);
        }
        mPhotonSettings.newRegion = false;
    }

    // Token: 0x06002925 RID: 10533 RVA: 0x0001CB5A File Offset: 0x0001AD5A
    public void SelectRegion(string region)
    {
        if (mPhotonSettings.region == region)
        {
            return;
        }
        mPhotonSettings.region = region;
        mPhotonSettings.newRegion = true;
        mVersionManager.UpdateRegion();
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.Disconnect();
        }
    }

    // Token: 0x06002926 RID: 10534 RVA: 0x000F45D0 File Offset: 0x000F27D0
    private void OnConnectedToPhoton()
    {
        if (!PhotonNetwork.connected)
        {
            return;
        }
        mVersionManager.UpdateRegion();
        PhotonNetwork.player.SetLevel(AccountManager.GetLevel());
        PhotonNetwork.player.SetClan(AccountManager.GetClan());
        PhotonNetwork.player.SetAvatarUrl(AccountManager.instance.Data.AvatarUrl);
        TimerManager.In("PhotonConnected", 1.5f, delegate ()
        {
            mPhotonSettings.connectedCallback();
        });
    }

    // Token: 0x06002927 RID: 10535 RVA: 0x0001CA25 File Offset: 0x0001AC25
    private void OnDisconnectedFromPhoton()
    {
        mPopUp.HideAll("Menu");
    }

    // Token: 0x06002928 RID: 10536 RVA: 0x0001CB87 File Offset: 0x0001AD87
    private void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        TimerManager.Cancel("PhotonConnected");
        UIToast.Show("Failed: " + cause.ToString());
    }

    // Token: 0x06002929 RID: 10537 RVA: 0x0001CBAF File Offset: 0x0001ADAF
    private void OnConnectionFail(DisconnectCause cause)
    {
        TimerManager.Cancel("PhotonConnected");
        UIToast.Show("Fail: " + cause.ToString());
    }

    // Token: 0x0600292A RID: 10538 RVA: 0x000056C5 File Offset: 0x000038C5
    private void OnCustomAuthenticationFailed(string error)
    {
    }

    // Token: 0x0600292B RID: 10539 RVA: 0x000F4650 File Offset: 0x000F2850
    private void OnCustomAuthenticationResponse(object[] obj)
    {
        TimerManager.In(0.1f, delegate ()
        {
            MonoBehaviour.print(obj.Length);
            object[] obj2 = obj;
            for (int i = 0; i < obj2.Length; i++)
            {
                MonoBehaviour.print(((Dictionary<string, object>)obj2[i])["Test"]);
            }
        });
    }

    // Token: 0x0600292C RID: 10540 RVA: 0x0001CBD7 File Offset: 0x0001ADD7
    private void OnJoinedRoom()
    {
        PlayerRoundManager.Clear();
        PlayerRoundManager.SetMode(PhotonNetwork.room.GetGameMode());
        if (PhotonNetwork.offlineMode)
        {
            LevelManager.LoadLevel(mPhotonSettings.selectMap);
            return;
        }
        PhotonNetwork.isMessageQueueRunning = false;
        PhotonNetwork.LoadLevel(mPhotonSettings.selectMap);
    }

    // Token: 0x0600292D RID: 10541 RVA: 0x000F4684 File Offset: 0x000F2884
    private void OnPhotonJoinRoomFailed(short code, string message)
    {
        mJoinServer.onBack();
        if (message == "Game full")
        {
            UIToast.Show(Localization.Get("The server is full", true));
            return;
        }
        UIToast.Show(string.Concat(new object[]
        {
            "Error Code: ",
            code,
            " Message: ",
            message
        }));
    }

    // Token: 0x0600292E RID: 10542 RVA: 0x000F46E8 File Offset: 0x000F28E8
    private void OnPhotonCreateRoomFailed(short code, string message)
    {
        mPopUp.HideAll("CreateServer");
        if (code == 32766)
        {
            UIToast.Show(Localization.Get("Server with this name already exists", true));
            return;
        }
        UIToast.Show(string.Concat(new object[]
        {
            "Error Code: ",
            code,
            " Message: ",
            message
        }));
    }

    // Token: 0x0600292F RID: 10543 RVA: 0x000F4748 File Offset: 0x000F2948
    public static void CreateOfficialServer(string name, string map, GameMode mode)
    {
        if (!AccountManager.isConnect)
        {
            UIToast.Show(Localization.Get("Connection account", true));
            return;
        }
        mPopUp.ShowText(Localization.Get("Connecting", true) + "...");
        PhotonNetwork.player.SetPlayerID(AccountManager.instance.Data.ID);
        PhotonNetwork.player.ClearProperties();
        mPhotonSettings.selectMap = map;
        Hashtable hashtable = PhotonNetwork.room.CreateRoomHashtable("off", mode, true);
        hashtable[PhotonCustomValue.minLevelKey] = (byte)AccountManager.GetLevel();
        PhotonNetwork.CreateRoom(name, new RoomOptions
        {
            MaxPlayers = 12,
            IsOpen = true,
            IsVisible = true,
            PublishUserId = true,
            CustomRoomProperties = hashtable,
            CustomRoomPropertiesForLobby = new string[]
            {
                PhotonCustomValue.sceneNameKey,
                PhotonCustomValue.passwordKey,
                PhotonCustomValue.gameModeKey,
                PhotonCustomValue.officialServerKey,
                PhotonCustomValue.minLevelKey
            }
        }, null);
    }

    // Token: 0x06002930 RID: 10544 RVA: 0x000F4868 File Offset: 0x000F2A68
    public static void CreateServer(string name, string map, GameMode mode, string password, int maxPlayers, bool custom)
    {
        if (!AccountManager.isConnect)
        {
            UIToast.Show(Localization.Get("Connection account", true));
            return;
        }
        mPopUp.ShowText(Localization.Get("Creating Server", true) + "...");
        PhotonNetwork.player.SetPlayerID(AccountManager.instance.Data.ID);
        PhotonNetwork.player.ClearProperties();
        if (map == "50Traps")
        {
            maxPlayers = Mathf.Clamp(maxPlayers, 4, 32);
        }
        else
        {
            maxPlayers = Mathf.Clamp(maxPlayers, 4, 12);
        }
        mPhotonSettings.selectMap = map;
        Hashtable hashtable = PhotonNetwork.room.CreateRoomHashtable(password, mode, false);
        if (mode == GameMode.Only)
        {
            hashtable[PhotonCustomValue.onlyWeaponKey] = (byte)mServerSettings.GetOnlyWeapon();
        }
        if (custom)
        {
            hashtable[PhotonCustomValue.customMapHash] = Manager.hash;
            hashtable[PhotonCustomValue.customMapUrl] = Manager.bundleUrl;
            hashtable[PhotonCustomValue.customMapModes] = Manager.modes;
        }
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)maxPlayers;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.PublishUserId = true;
        roomOptions.CustomRoomProperties = hashtable;
        if (custom)
        {
            roomOptions.CustomRoomPropertiesForLobby = new string[]
            {
                PhotonCustomValue.sceneNameKey,
                PhotonCustomValue.passwordKey,
                PhotonCustomValue.gameModeKey,
                PhotonCustomValue.customMapHash,
                PhotonCustomValue.customMapUrl,
                PhotonCustomValue.customMapModes
            };
        }
        else if (mode == GameMode.Only)
        {
            roomOptions.CustomRoomPropertiesForLobby = new string[]
            {
                PhotonCustomValue.sceneNameKey,
                PhotonCustomValue.passwordKey,
                PhotonCustomValue.gameModeKey,
                PhotonCustomValue.onlyWeaponKey
            };
        }
        else
        {
            roomOptions.CustomRoomPropertiesForLobby = new string[]
            {
                PhotonCustomValue.sceneNameKey,
                PhotonCustomValue.passwordKey,
                PhotonCustomValue.gameModeKey
            };
        }
        LevelManager.customScene = custom;
        PhotonNetwork.CreateRoom(name, roomOptions, null);
    }

    // Token: 0x06002931 RID: 10545 RVA: 0x000F4A60 File Offset: 0x000F2C60
    public static void QueueServer(RoomInfo room)
    {
        mPhotonSettings.queueRoom = room;
        mPopUp.ShowPopup(Localization.Get("Please wait", true) + "...", Localization.Get("Queue", true), Localization.Get("Exit", true), delegate ()
        {
            mPhotonSettings.queueRoom = null;
            TimerManager.Cancel("QueueTimer");
            mJoinServer.onBack();
        });
        TimerManager.In("QueueTimer", 1f, -1, 1f, delegate ()
        {
            if (mPhotonSettings.queueRoom == null)
            {
                TimerManager.Cancel("QueueTimer");
                return;
            }
            RoomInfo[] roomList = PhotonNetwork.GetRoomList();
            int i = 0;
            while (i < roomList.Length)
            {
                if (roomList[i].Name == mPhotonSettings.queueRoom.Name)
                {
                    if (roomList[i].PlayerCount != (int)roomList[i].MaxPlayers)
                    {
                        TimerManager.Cancel("QueueTimer");
                        mPhotonSettings.JoinServer(mPhotonSettings.queueRoom);
                        return;
                    }
                    break;
                }
                else
                {
                    i++;
                }
            }
        });
    }

    // Token: 0x06002932 RID: 10546 RVA: 0x000F4AF8 File Offset: 0x000F2CF8
    public void CreateServerOffline(string map)
    {
        if (!AccountManager.isConnect)
        {
            UIToast.Show(Localization.Get("Connection account", true));
            return;
        }
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.Disconnect();
        }
        mPopUp.ShowText(Localization.Get("Loading", true) + "...");
        TimerManager.In(0.2f, delegate ()
        {
            mPopUp.ShowText(Localization.Get("Loading", true) + "...");
            mPhotonSettings.selectMap = map;
            PhotonNetwork.offlineMode = true;
            LevelManager.customScene = false;
            PhotonNetwork.CreateRoom(map);
        });
    }

    // Token: 0x06002933 RID: 10547 RVA: 0x000F4B68 File Offset: 0x000F2D68
    public static void CreateCustomServerOffline(string map, GameMode mode)
    {
        if (!AccountManager.isConnect)
        {
            UIToast.Show(Localization.Get("Connection account", true));
            return;
        }
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.Disconnect();
        }
        mPopUp.ShowText(Localization.Get("Loading", true) + "...");
        Hashtable customRoomProperties = PhotonNetwork.room.CreateRoomHashtable(string.Empty, mode, false);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 1;
        roomOptions.IsOpen = false;
        roomOptions.IsVisible = false;
        roomOptions.CustomRoomProperties = customRoomProperties;
        mPhotonSettings.selectMap = map;
        PhotonNetwork.offlineMode = true;
        LevelManager.customScene = true;
        PhotonNetwork.CreateRoom(map, roomOptions, null);
    }

    // Token: 0x06002934 RID: 10548 RVA: 0x000F4C04 File Offset: 0x000F2E04
    public static void JoinServer(RoomInfo room)
    {
        if (!AccountManager.isConnect)
        {
            UIToast.Show(Localization.Get("Connection account", true));
            return;
        }
        if (mPhotonSettings.queueRoom == null)
        {
            mPopUp.ShowText(Localization.Get("Connecting", true) + "...");
        }
        PhotonNetwork.player.SetPlayerID(AccountManager.instance.Data.ID);
        PhotonNetwork.player.ClearProperties();
        mPhotonSettings.selectMap = room.GetSceneName();
        PhotonNetwork.JoinRoom(room.Name);
    }

    // Token: 0x04001A44 RID: 6724
    private static string selectMap;

    // Token: 0x04001A45 RID: 6725
    private static bool newRegion;

    // Token: 0x04001A46 RID: 6726
    private static RoomInfo queueRoom;

    // Token: 0x04001A47 RID: 6727
    public static Action connectedCallback;

    // Token: 0x04001A48 RID: 6728
    public static string APPID = "b5c3c122-c361-4ad0-9a7a-ac797d02fcb6";
}
