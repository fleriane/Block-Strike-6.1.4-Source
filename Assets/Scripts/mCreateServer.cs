using System;
using System.Collections.Generic;
using BSCM;
using UnityEngine;

// Token: 0x0200048A RID: 1162
public class mCreateServer : MonoBehaviour
{
    // Token: 0x17000467 RID: 1127
    // (get) Token: 0x06002874 RID: 10356 RVA: 0x0001C482 File Offset: 0x0001A682
    public static GameMode mode
    {
        get
        {
            return mCreateServer.instance.selectMode;
        }
    }

    // Token: 0x17000468 RID: 1128
    // (get) Token: 0x06002875 RID: 10357 RVA: 0x0001C48E File Offset: 0x0001A68E
    public static string map
    {
        get
        {
            return mCreateServer.instance.mapPopupList.value;
        }
    }

    // Token: 0x17000469 RID: 1129
    // (get) Token: 0x06002876 RID: 10358 RVA: 0x0001C49F File Offset: 0x0001A69F
    // (set) Token: 0x06002877 RID: 10359 RVA: 0x0001C4B0 File Offset: 0x0001A6B0
    public static string serverName
    {
        get
        {
            return mCreateServer.instance.serverNameInput.value;
        }
        set
        {
            mCreateServer.instance.serverNameInput.value = value;
        }
    }

    // Token: 0x1700046A RID: 1130
    // (get) Token: 0x06002878 RID: 10360 RVA: 0x0001C4C2 File Offset: 0x0001A6C2
    public static int maxPlayers
    {
        get
        {
            return mCreateServer.instance.selectMaxPlayers;
        }
    }

    // Token: 0x1700046B RID: 1131
    // (get) Token: 0x06002879 RID: 10361 RVA: 0x0001C4CE File Offset: 0x0001A6CE
    public static string password
    {
        get
        {
            return mCreateServer.instance.passwordInput.value;
        }
    }

    // Token: 0x1700046C RID: 1132
    // (get) Token: 0x0600287A RID: 10362 RVA: 0x0001C4DF File Offset: 0x0001A6DF
    public static bool custom
    {
        get
        {
            return mCreateServer.instance.customMapsToggle.value;
        }
    }

    // Token: 0x0600287B RID: 10363 RVA: 0x0001C4F0 File Offset: 0x0001A6F0
    private void Start()
    {
        mCreateServer.instance = this;
    }

    // Token: 0x0600287C RID: 10364 RVA: 0x000F11D0 File Offset: 0x000EF3D0
    public void Open()
    {
        if (mCreateServer.custom)
        {
            Manager.Start();
        }
        this.customMaps.SetActive(Manager.enabled && AccountManager.GetLevel() >= 10);
        mCreateServer.serverName = Localization.Get("Room", true) + " " + UnityEngine.Random.Range(0, 99999);
        this.modePopupList.Clear();
        GameMode[] array = (!mCreateServer.custom) ? GameModeManager.gameMode : GameModeManager.customGameMode;
        for (int i = 0; i < array.Length; i++)
        {
            this.modePopupList.AddItem(array[i].ToString());
        }
        this.modePopupList.value = this.modePopupList.items[0];
        this.UpdateMaps();
    }

    // Token: 0x0600287D RID: 10365 RVA: 0x0001C4F8 File Offset: 0x0001A6F8
    public static void OpenPanel()
    {
        mCreateServer.instance.Open();
    }

    // Token: 0x0600287E RID: 10366 RVA: 0x000F12B0 File Offset: 0x000EF4B0
    private void UpdateMaps()
    {
        if (!mCreateServer.custom)
        {
            List<string> gameModeScenes = LevelManager.GetGameModeScenes(this.selectMode);
            this.mapPopupList.Clear();
            for (int i = 0; i < gameModeScenes.Count; i++)
            {
                this.mapPopupList.AddItem(gameModeScenes[i]);
            }
            this.mapPopupList.value = this.mapPopupList.items[0];
        }
        else
        {
            string[] mapsList = Manager.GetMapsList(mCreateServer.mode);
            this.mapPopupList.Clear();
            for (int j = 0; j < mapsList.Length; j++)
            {
                this.mapPopupList.AddItem(Manager.GetBundleName(mapsList[j]), mapsList[j], null);
            }
            if (mapsList.Length > 0)
            {
                this.mapPopupList.value = this.mapPopupList.items[0];
            }
            else
            {
                this.mapPopupList.AddItem("-----");
                this.mapPopupList.value = this.mapPopupList.items[0];
                this.mapPopupList.Clear();
            }
        }
    }

    // Token: 0x0600287F RID: 10367 RVA: 0x0001C504 File Offset: 0x0001A704
    public void SelectGameMode()
    {
        this.selectMode = (GameMode)((int)Enum.Parse(typeof(GameMode), this.modePopupList.value));
        this.UpdateMaps();
        mServerSettings.Check(this.selectMode, mCreateServer.map);
    }

    // Token: 0x06002880 RID: 10368 RVA: 0x0001C541 File Offset: 0x0001A741
    public void SelectMap()
    {
        mServerSettings.Check(this.selectMode, mCreateServer.map);
    }

    // Token: 0x06002881 RID: 10369 RVA: 0x000F13CC File Offset: 0x000EF5CC
    public void CheckServerName()
    {
        if (mCreateServer.serverName.Length < 4 || Utils.IsNullOrWhiteSpace(mCreateServer.serverName) || mCreateServer.serverName.Contains("\n") || BadWordsManager.Contains(mCreateServer.serverName))
        {
            mCreateServer.serverName = Localization.Get("Room", true) + " " + UnityEngine.Random.Range(0, 99999);
        }
        mCreateServer.serverName = NGUIText.StripSymbols(mCreateServer.serverName);
        RoomInfo[] roomList = PhotonNetwork.GetRoomList();
        for (int i = 0; i < roomList.Length; i++)
        {
            if (roomList[i].Name == mCreateServer.serverName)
            {
                mCreateServer.serverName = Localization.Get("Room", true) + " " + UnityEngine.Random.Range(0, 99999);
                UIToast.Show(Localization.Get("Name already taken", true));
                break;
            }
        }
    }

    // Token: 0x06002882 RID: 10370 RVA: 0x000F14C4 File Offset: 0x000EF6C4
    public void SetMaxPlayer(GameObject go)
    {
        this.selectMaxPlayers = int.Parse(go.name);
        for (int i = 0; i < this.maxPlayersSprite.Length; i++)
        {
            if (this.maxPlayersLabel[i].text != go.name)
            {
                this.maxPlayersSprite[i].color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 40);
            }
            else
            {
                this.maxPlayersSprite[i].color = new Color32(235, 242, byte.MaxValue, byte.MaxValue);
            }
        }
    }

    // Token: 0x06002883 RID: 10371 RVA: 0x0001C553 File Offset: 0x0001A753
    public void SetDefaultMaxPlayers()
    {
        this.SetMaxPlayers(new int[]
        {
            4,
            6,
            8,
            10,
            12
        });
    }

    // Token: 0x06002884 RID: 10372 RVA: 0x000F1570 File Offset: 0x000EF770
    public void SetMaxPlayers(int[] list)
    {
        if (list.Length > 5)
        {
            Debug.LogError("Max list  <=5");
            return;
        }
        for (int i = 0; i < this.maxPlayersSprite.Length; i++)
        {
            this.maxPlayersSprite[i].cachedGameObject.SetActive(false);
        }
        for (int j = 0; j < list.Length; j++)
        {
            this.maxPlayersSprite[j].cachedGameObject.SetActive(true);
            this.maxPlayersSprite[j].cachedGameObject.name = list[j].ToString();
            this.maxPlayersLabel[j].text = list[j].ToString();
        }
        this.SetMaxPlayer(this.maxPlayersSprite[0].cachedGameObject);
    }

    // Token: 0x06002885 RID: 10373 RVA: 0x000F1630 File Offset: 0x000EF830
    public void CreateServer()
    {
        if (!mCreateServer.custom)
        {
            mPhotonSettings.CreateServer(mCreateServer.serverName, mCreateServer.map, mCreateServer.mode, mCreateServer.password, mCreateServer.maxPlayers, false);
        }
        else
        {
            if (this.mapPopupList.items.Count <= 0)
            {
                return;
            }
            Manager.LoadBundle((string)this.mapPopupList.data);
            mPhotonSettings.CreateServer(mCreateServer.serverName, mCreateServer.map, mCreateServer.mode, mCreateServer.password, mCreateServer.maxPlayers, true);
        }
    }

    // Token: 0x040019C7 RID: 6599
    public GameMode selectMode;

    // Token: 0x040019C8 RID: 6600
    public UIPopupList modePopupList;

    // Token: 0x040019C9 RID: 6601
    public UIPopupList mapPopupList;

    // Token: 0x040019CA RID: 6602
    public UIInput serverNameInput;

    // Token: 0x040019CB RID: 6603
    public UISprite[] maxPlayersSprite;

    // Token: 0x040019CC RID: 6604
    public int selectMaxPlayers = 4;

    // Token: 0x040019CD RID: 6605
    public UILabel[] maxPlayersLabel;

    // Token: 0x040019CE RID: 6606
    public UIInput passwordInput;

    // Token: 0x040019CF RID: 6607
    public GameObject customMaps;

    // Token: 0x040019D0 RID: 6608
    public UIToggle customMapsToggle;

    // Token: 0x040019D1 RID: 6609
    private static mCreateServer instance;
}
