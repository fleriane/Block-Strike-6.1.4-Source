using System;
using UnityEngine;

// Token: 0x02000300 RID: 768
public class CameraStatic : MonoBehaviour
{
    // Token: 0x06001D0B RID: 7435 RVA: 0x000B6F18 File Offset: 0x000B5118
    public void Active()
    {
        this.cameraTransform = this.cameraManager.cameraTransform;
        this.cameraTransform.gameObject.SetActive(true);
        this.cameraTransform.position = this.cameraManager.cameraStaticPoint.position;
        this.cameraTransform.rotation = this.cameraManager.cameraStaticPoint.rotation;
        LODObject.Target = this.cameraTransform;
        SkyboxManager.GetCameraParent().localEulerAngles = Vector3.zero;
        SkyboxManager.GetCamera().rotation = this.cameraTransform.rotation;
    }

    // Token: 0x06001D0C RID: 7436 RVA: 0x00014E54 File Offset: 0x00013054
    public void Deactive()
    {
        if (CameraManager.type != CameraType.Static)
        {
            return;
        }
        this.cameraTransform = this.cameraManager.cameraTransform;
        this.cameraTransform.gameObject.SetActive(false);
    }

    // Token: 0x040010F7 RID: 4343
    public CameraManager cameraManager;

    // Token: 0x040010F8 RID: 4344
    private Transform cameraTransform;
}
