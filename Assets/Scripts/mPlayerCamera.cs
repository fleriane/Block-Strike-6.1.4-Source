using System;
using DG.Tweening;
using UnityEngine;

// Token: 0x020004A4 RID: 1188
public class mPlayerCamera : MonoBehaviour
{
    // Token: 0x0600293F RID: 10559 RVA: 0x0001CC95 File Offset: 0x0001AE95
    private void Awake()
    {
        mPlayerCamera.instance = this;
        this.mCamera = base.GetComponent<Camera>();
        this.RotateSpeed = Mathf.Sqrt(this.RotateSpeed) / Mathf.Sqrt(Screen.dpi);
    }

    // Token: 0x06002940 RID: 10560 RVA: 0x0001CCC5 File Offset: 0x0001AEC5
    public static void Show()
    {
        mPlayerCamera.instance.mCamera.enabled = true;
        mPlayerCamera.instance.Player.SetActive(true);
    }

    // Token: 0x06002941 RID: 10561 RVA: 0x0001CCE7 File Offset: 0x0001AEE7
    public static void Close()
    {
        mPlayerCamera.instance.mCamera.enabled = false;
        mPlayerCamera.instance.Player.SetActive(false);
    }

    // Token: 0x06002942 RID: 10562 RVA: 0x0001CD09 File Offset: 0x0001AF09
    public static void Rotate(Vector2 rotate)
    {
        mPlayerCamera.instance.Point.Rotate(new Vector2(0f, -rotate.x * mPlayerCamera.instance.RotateSpeed));
    }

    // Token: 0x06002943 RID: 10563 RVA: 0x000F4D74 File Offset: 0x000F2F74
    public static void SetSkin(Team team, string head, string body, string leg)
    {
        UIAtlas atlas = (team != Team.Blue) ? GameSettings.instance.PlayerAtlasRed : GameSettings.instance.PlayerAtlasBlue;
        mPlayerCamera.instance.Head.atlas = atlas;
        mPlayerCamera.instance.Head.spriteName = "0-" + head;
        for (int i = 0; i < mPlayerCamera.instance.Body.Length; i++)
        {
            mPlayerCamera.instance.Body[i].atlas = atlas;
            mPlayerCamera.instance.Body[i].spriteName = "1-" + body;
        }
        for (int j = 0; j < mPlayerCamera.instance.Legs.Length; j++)
        {
            mPlayerCamera.instance.Legs[j].atlas = atlas;
            mPlayerCamera.instance.Legs[j].spriteName = "2-" + leg;
        }
    }

    // Token: 0x06002944 RID: 10564 RVA: 0x0001CD3C File Offset: 0x0001AF3C
    public static void ResetRotateX()
    {
        mPlayerCamera.instance.Point.DOLocalRotate(Vector3.zero, 0.2f, RotateMode.Fast);
    }

    // Token: 0x04001A4F RID: 6735
    public GameObject Player;

    // Token: 0x04001A50 RID: 6736
    public MeshAtlas Head;

    // Token: 0x04001A51 RID: 6737
    public MeshAtlas[] Body;

    // Token: 0x04001A52 RID: 6738
    public MeshAtlas[] Legs;

    // Token: 0x04001A53 RID: 6739
    public Transform Point;

    // Token: 0x04001A54 RID: 6740
    public float RotateSpeed = 200f;

    // Token: 0x04001A55 RID: 6741
    private Camera mCamera;

    // Token: 0x04001A56 RID: 6742
    private static mPlayerCamera instance;
}
