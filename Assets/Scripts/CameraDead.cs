using System;
using UnityEngine;

// Token: 0x020002FB RID: 763
public class CameraDead : MonoBehaviour
{
    // Token: 0x06001CDA RID: 7386 RVA: 0x00014BC8 File Offset: 0x00012DC8
    private void Start()
    {
        this.cameraRigidbody.detectCollisions = false;
    }

    // Token: 0x06001CDB RID: 7387 RVA: 0x00014BD6 File Offset: 0x00012DD6
    public void Active(object[] parameters)
    {
        this.Active((Vector3)parameters[0], (Vector3)parameters[1], (Vector3)parameters[2]);
    }

    // Token: 0x06001CDC RID: 7388 RVA: 0x000B5CB8 File Offset: 0x000B3EB8
    public void Active(Vector3 position, Vector3 rotation, Vector3 force)
    {
        this.cameraTransform.gameObject.SetActive(true);
        this.cameraBoxCollider.isTrigger = false;
        this.cameraRigidbody.isKinematic = false;
        this.cameraRigidbody.detectCollisions = true;
        this.cameraTransform.position = position;
        this.cameraTransform.eulerAngles = rotation;
        this.cameraRigidbody.velocity = Vector3.zero;
        this.cameraRigidbody.AddForce(force);
        this.cameraRigidbody.AddRelativeForce(force);
        LODObject.Target = this.cameraTransform;
        SkyboxManager.GetCameraParent().localEulerAngles = Vector3.zero;
    }

    // Token: 0x06001CDD RID: 7389 RVA: 0x000B5D54 File Offset: 0x000B3F54
    public void Deactive()
    {
        if (CameraManager.type != CameraType.Dead)
        {
            return;
        }
        this.cameraRigidbody.isKinematic = true;
        this.cameraRigidbody.detectCollisions = false;
        this.cameraBoxCollider.isTrigger = true;
        this.cameraTransform.gameObject.SetActive(false);
    }

    // Token: 0x06001CDE RID: 7390 RVA: 0x00014BF6 File Offset: 0x00012DF6
    public void OnUpdate()
    {
        if (CameraManager.type != CameraType.Dead)
        {
            return;
        }
        SkyboxManager.GetCamera().rotation = this.cameraTransform.rotation;
    }

    // Token: 0x040010CB RID: 4299
    public CameraManager cameraManager;

    // Token: 0x040010CC RID: 4300
    public Rigidbody cameraRigidbody;

    // Token: 0x040010CD RID: 4301
    public Transform cameraTransform;

    // Token: 0x040010CE RID: 4302
    public BoxCollider cameraBoxCollider;
}
