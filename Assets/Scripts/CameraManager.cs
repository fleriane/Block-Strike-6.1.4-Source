using System;
using UnityEngine;

// Token: 0x020002FE RID: 766
public class CameraManager : MonoBehaviour
{
    // Token: 0x1400001A RID: 26
    // (add) Token: 0x06001CF1 RID: 7409 RVA: 0x00014CBB File Offset: 0x00012EBB
    // (remove) Token: 0x06001CF2 RID: 7410 RVA: 0x00014CD2 File Offset: 0x00012ED2
    public static event Action<int> selectPlayerEvent;

    // Token: 0x1700041C RID: 1052
    // (get) Token: 0x06001CF3 RID: 7411 RVA: 0x00014CE9 File Offset: 0x00012EE9
    // (set) Token: 0x06001CF4 RID: 7412 RVA: 0x000B67D8 File Offset: 0x000B49D8
    public static int selectPlayer
    {
        get
        {
            return CameraManager.instance.playerID;
        }
        set
        {
            CameraType type = CameraManager.type;
            if (type != CameraType.Spectate)
            {
                if (type == CameraType.FirstPerson)
                {
                    CameraManager.instance.firstPerson.UpdateSelectPlayer(value);
                }
            }
            else
            {
                CameraManager.instance.spectate.UpdateSelectPlayer(value);
            }
        }
    }

    // Token: 0x1700041D RID: 1053
    // (get) Token: 0x06001CF5 RID: 7413 RVA: 0x00014CF5 File Offset: 0x00012EF5
    public static CameraType type
    {
        get
        {
            return CameraManager.instance.cameraType;
        }
    }

    // Token: 0x1700041E RID: 1054
    // (get) Token: 0x06001CF6 RID: 7414 RVA: 0x00014D01 File Offset: 0x00012F01
    public static Transform ActiveCamera
    {
        get
        {
            if (CameraManager.type != CameraType.None)
            {
                return CameraManager.instance.cameraTransform;
            }
            if (PlayerInput.instance == null)
            {
                return null;
            }
            return PlayerInput.instance.FPCamera.Transform;
        }
    }

    // Token: 0x1700041F RID: 1055
    // (get) Token: 0x06001CF7 RID: 7415 RVA: 0x00014D39 File Offset: 0x00012F39
    public static CameraManager main
    {
        get
        {
            return CameraManager.instance;
        }
    }

    // Token: 0x06001CF8 RID: 7416 RVA: 0x00014D40 File Offset: 0x00012F40
    private void Awake()
    {
        CameraManager.instance = this;
    }

    // Token: 0x06001CF9 RID: 7417 RVA: 0x00014D48 File Offset: 0x00012F48
    private void OnDisable()
    {
        CameraManager.Team = false;
        CameraManager.ChangeType = false;
    }

    // Token: 0x06001CFA RID: 7418 RVA: 0x000B6828 File Offset: 0x000B4A28
    private void LateUpdate()
    {
        switch (this.cameraType)
        {
            case CameraType.Dead:
                this.dead.OnUpdate();
                break;
            case CameraType.Spectate:
                this.spectate.OnUpdate();
                break;
            case CameraType.FirstPerson:
                this.firstPerson.OnUpdate();
                break;
        }
    }

    // Token: 0x06001CFB RID: 7419 RVA: 0x00014D56 File Offset: 0x00012F56
    public void OnSelectPlayer(int id)
    {
        if (CameraManager.selectPlayerEvent != null)
        {
            this.playerID = id;
            CameraManager.selectPlayerEvent(this.playerID);
        }
    }

    // Token: 0x06001CFC RID: 7420 RVA: 0x000B688C File Offset: 0x000B4A8C
    public static void SetType(CameraType type, params object[] parameters)
    {
        CameraManager.instance.DeactiveAll();
        CameraManager.instance.cameraType = type;
        switch (type)
        {
            case CameraType.Dead:
                CameraManager.instance.dead.Active(parameters);
                break;
            case CameraType.Static:
                CameraManager.instance.statiс.Active();
                break;
            case CameraType.Spectate:
                CameraManager.instance.spectate.Active(parameters);
                break;
            case CameraType.FirstPerson:
                CameraManager.instance.firstPerson.Active(parameters);
                break;
        }
    }

    // Token: 0x06001CFD RID: 7421 RVA: 0x00014D79 File Offset: 0x00012F79
    private void DeactiveAll()
    {
        this.dead.Deactive();
        this.statiс.Deactive();
        this.spectate.Deactive();
        this.firstPerson.Deactive();
        this.cameraType = CameraType.None;
    }

    // Token: 0x040010DE RID: 4318
    public CameraType cameraType;

    // Token: 0x040010DF RID: 4319
    public Transform cameraTransform;

    // Token: 0x040010E0 RID: 4320
    public Transform cameraStaticPoint;

    // Token: 0x040010E1 RID: 4321
    public CameraDead dead;

    // Token: 0x040010E2 RID: 4322
    public CameraStatic statiс;

    // Token: 0x040010E3 RID: 4323
    public CameraSpectate spectate;

    // Token: 0x040010E4 RID: 4324
    public CameraFirstPerson firstPerson;

    // Token: 0x040010E5 RID: 4325
    private int playerID = -1;

    // Token: 0x040010E6 RID: 4326
    private static CameraManager instance;

    // Token: 0x040010E7 RID: 4327
    public static bool Team;

    // Token: 0x040010E8 RID: 4328
    public static bool ChangeType;
}
