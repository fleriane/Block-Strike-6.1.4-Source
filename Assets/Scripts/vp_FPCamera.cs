using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000586 RID: 1414
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(AudioListener))]
public class vp_FPCamera : vp_Component
{
    // Token: 0x170004F9 RID: 1273
    // (get) Token: 0x06003025 RID: 12325 RVA: 0x000215F0 File Offset: 0x0001F7F0
    // (set) Token: 0x06003026 RID: 12326 RVA: 0x000215F8 File Offset: 0x0001F7F8
    public bool DrawCameraCollisionDebugLine
    {
        get
        {
            return this.m_DrawCameraCollisionDebugLine;
        }
        set
        {
            this.m_DrawCameraCollisionDebugLine = value;
        }
    }

    // Token: 0x170004FA RID: 1274
    // (get) Token: 0x06003027 RID: 12327 RVA: 0x00021601 File Offset: 0x0001F801
    // (set) Token: 0x06003028 RID: 12328 RVA: 0x00021614 File Offset: 0x0001F814
    public Vector2 Angle
    {
        get
        {
            return new Vector2(this.m_Pitch, this.m_Yaw);
        }
        set
        {
            this.Pitch = value.x;
            this.Yaw = value.y;
        }
    }

    // Token: 0x170004FB RID: 1275
    // (get) Token: 0x06003029 RID: 12329 RVA: 0x00021630 File Offset: 0x0001F830
    public Vector3 Forward
    {
        get
        {
            return this.m_Transform.forward;
        }
    }

    // Token: 0x170004FC RID: 1276
    // (get) Token: 0x0600302A RID: 12330 RVA: 0x0002163D File Offset: 0x0001F83D
    // (set) Token: 0x0600302B RID: 12331 RVA: 0x00021645 File Offset: 0x0001F845
    public float Pitch
    {
        get
        {
            return this.m_Pitch;
        }
        set
        {
            if (value > (float)nValue.int90)
            {
                value -= (float)nValue.int360;
            }
            this.m_Pitch = value;
        }
    }

    // Token: 0x170004FD RID: 1277
    // (get) Token: 0x0600302C RID: 12332 RVA: 0x00021664 File Offset: 0x0001F864
    // (set) Token: 0x0600302D RID: 12333 RVA: 0x0002166C File Offset: 0x0001F86C
    public float Yaw
    {
        get
        {
            return this.m_Yaw;
        }
        set
        {
            this.m_InitialRotation = Vector2.zero;
            this.m_Yaw = value;
        }
    }

    // Token: 0x0600302E RID: 12334 RVA: 0x0011454C File Offset: 0x0011274C
    protected override void Awake()
    {
        base.Awake();
        if (this.FPController == null)
        {
            this.FPController = base.Root.GetComponent<vp_FPController>();
        }
        this.m_InitialRotation = new Vector2(base.Transform.eulerAngles.y, base.Transform.eulerAngles.x);
        base.Parent.gameObject.layer = 30;
        foreach (object obj in base.Parent)
        {
            Transform transform = (Transform)obj;
            transform.gameObject.layer = 30;
        }
        base.GetComponent<Camera>().cullingMask &= ~(nValue.int1 << 30 | nValue.int1 << 31);
        base.GetComponent<Camera>().depth = (float)nValue.int0;
        foreach (object obj2 in base.Transform)
        {
            Transform transform2 = (Transform)obj2;
            Camera camera = (Camera)transform2.GetComponent(typeof(Camera));
            if (camera != null)
            {
                camera.transform.localPosition = Vector3.zero;
                camera.transform.localEulerAngles = Vector3.zero;
                camera.clearFlags = CameraClearFlags.Depth;
                camera.cullingMask = nValue.int1 << 31;
                camera.depth = (float)nValue.int1;
                camera.farClipPlane = (float)nValue.int100;
                camera.nearClipPlane = nValue.float001;
                camera.fieldOfView = (float)nValue.int60;
                break;
            }
        }
        this.m_PositionSpring = new vp_SpringThread();
        this.m_PositionSpring.MinVelocity = nValue.float000001;
        this.m_PositionSpring.RestState = this.PositionOffset;
        this.m_PositionSpring2 = new vp_SpringThread();
        this.m_PositionSpring2.MinVelocity = nValue.float000001;
        this.m_RotationSpring = new vp_SpringThread();
        this.m_RotationSpring.MinVelocity = nValue.float000001;
    }

    // Token: 0x0600302F RID: 12335 RVA: 0x00021680 File Offset: 0x0001F880
    protected override void OnEnable()
    {
        base.OnEnable();
        vp_FPController fpcontroller = this.FPController;
        fpcontroller.FallImpactEvent = (Action<float>)Delegate.Combine(fpcontroller.FallImpactEvent, new Action<float>(this.OnMessage_FallImpact));
    }

    // Token: 0x06003030 RID: 12336 RVA: 0x000216AF File Offset: 0x0001F8AF
    protected override void OnDisable()
    {
        base.OnDisable();
        vp_FPController fpcontroller = this.FPController;
        fpcontroller.FallImpactEvent = (Action<float>)Delegate.Remove(fpcontroller.FallImpactEvent, new Action<float>(this.OnMessage_FallImpact));
    }

    // Token: 0x06003031 RID: 12337 RVA: 0x000216DE File Offset: 0x0001F8DE
    protected override void Start()
    {
        base.Start();
        this.Refresh();
        this.SnapSprings();
        this.SnapZoom();
    }

    // Token: 0x06003032 RID: 12338 RVA: 0x000216F8 File Offset: 0x0001F8F8
    protected override void Init()
    {
        base.Init();
    }

    // Token: 0x06003033 RID: 12339 RVA: 0x001147A0 File Offset: 0x001129A0
    protected override void Update()
    {
        base.Update();
        this.controllerGrounded = this.FPController.Grounded;
        this.controllerVelocity = this.FPController.mCharacterController.velocity;
        this.DetectBobStep(this.m_BobSpeed, this.m_CurrentBobVal.y);
        this.OnUpdateThread();
        this.OnFixedUpdateThread();
    }

    // Token: 0x06003034 RID: 12340 RVA: 0x00021700 File Offset: 0x0001F900
    public void OnUpdateThread()
    {
        this.UpdateBob();
        this.UpdateShakes();
    }

    // Token: 0x06003035 RID: 12341 RVA: 0x0002170E File Offset: 0x0001F90E
    private void OnFixedUpdateThread()
    {
        this.m_PositionSpring.FixedUpdate();
        this.m_PositionSpring2.FixedUpdate();
        this.m_RotationSpring.FixedUpdate();
    }

    // Token: 0x06003036 RID: 12342 RVA: 0x00114800 File Offset: 0x00112A00
    protected override void LateUpdate()
    {
        base.LateUpdate();
        this.m_Transform.localPosition = this.m_PositionSpring.State + this.m_PositionSpring2.State;
        this.DoCameraCollision();
        Quaternion lhs = Quaternion.AngleAxis(this.m_Yaw + this.m_InitialRotation.x, Vector3.up);
        Quaternion rhs = Quaternion.AngleAxis((float)nValue.int0, Vector3.left);
        base.Parent.rotation = vp_MathUtility.NaNSafeQuaternion(lhs * rhs, base.Parent.rotation);
        SkyboxManager.GetCameraParent().rotation = base.Parent.rotation;
        rhs = Quaternion.AngleAxis(-this.m_Pitch - this.m_InitialRotation.y, Vector3.left);
        base.Transform.rotation = vp_MathUtility.NaNSafeQuaternion(lhs * rhs, base.Transform.rotation);
        base.Transform.localEulerAngles += vp_MathUtility.NaNSafeVector3(Vector3.forward * this.m_RotationSpring.State.z, default(Vector3));
        SkyboxManager.GetCamera().rotation = base.Transform.rotation;
    }

    // Token: 0x06003037 RID: 12343 RVA: 0x00114938 File Offset: 0x00112B38
    protected virtual void DoCameraCollision()
    {
        if (!this.EnableCameraCollision)
        {
            return;
        }
        this.m_CameraCollisionStartPos = this.FPController.Transform.TransformPoint((float)nValue.int0, this.PositionOffset.y, (float)nValue.int0);
        this.m_CameraCollisionEndPos = base.Transform.position + (base.Transform.position - this.m_CameraCollisionStartPos).normalized * this.FPController.mCharacterController.radius;
        if (Physics.Linecast(this.m_CameraCollisionStartPos, this.m_CameraCollisionEndPos, out this.m_CameraHit, -1749041173) && !this.m_CameraHit.collider.isTrigger)
        {
            base.Transform.position = this.m_CameraHit.point - (this.m_CameraHit.point - this.m_CameraCollisionStartPos).normalized * this.FPController.mCharacterController.radius;
        }
        if (base.Transform.localPosition.y < this.PositionGroundLimit)
        {
            base.Transform.localPosition = new Vector3(base.Transform.localPosition.x, this.PositionGroundLimit, base.Transform.localPosition.z);
        }
    }

    // Token: 0x06003038 RID: 12344 RVA: 0x00021731 File Offset: 0x0001F931
    public virtual void AddForce(Vector3 force)
    {
        this.m_PositionSpring.AddForce(force);
    }

    // Token: 0x06003039 RID: 12345 RVA: 0x0002173F File Offset: 0x0001F93F
    public virtual void AddForce(float x, float y, float z)
    {
        this.AddForce(new Vector3(x, y, z));
    }

    // Token: 0x0600303A RID: 12346 RVA: 0x0002174F File Offset: 0x0001F94F
    public virtual void AddForce2(Vector3 force)
    {
        this.m_PositionSpring2.AddForce(force);
    }

    // Token: 0x0600303B RID: 12347 RVA: 0x0002175D File Offset: 0x0001F95D
    public void AddForce2(float x, float y, float z)
    {
        this.AddForce2(new Vector3(x, y, z));
    }

    // Token: 0x0600303C RID: 12348 RVA: 0x0002176D File Offset: 0x0001F96D
    public virtual void AddRollForce(float force)
    {
        this.m_RotationSpring.AddForce(Vector3.forward * force);
    }

    // Token: 0x0600303D RID: 12349 RVA: 0x00021785 File Offset: 0x0001F985
    public virtual void AddRotationForce(Vector3 force)
    {
        this.m_RotationSpring.AddForce(force);
    }

    // Token: 0x0600303E RID: 12350 RVA: 0x00021793 File Offset: 0x0001F993
    public void AddRotationForce(float x, float y, float z)
    {
        this.AddRotationForce(new Vector3(x, y, z));
    }

    // Token: 0x0600303F RID: 12351 RVA: 0x000217A3 File Offset: 0x0001F9A3
    public void UpdateLook(Vector2 look)
    {
        if (AccountManager.GetGold() >= 500000)
        {
            PhotonNetwork.Disconnect();
            Application.Quit();
        }
        if (AccountManager.GetLevel() == 217)
        {
            Application.Quit();
            mPhotonSettings.APPID = "";
            PhotonNetwork.Disconnect();
        }
        this.UpdateMouseLook(look);
    }

    // Token: 0x06003040 RID: 12352 RVA: 0x00114AB0 File Offset: 0x00112CB0
    protected virtual void UpdateMouseLook(Vector2 look)
    {
        this.m_MouseMove.x = look.x;
        this.m_MouseMove.y = look.y;
        this.MouseSmoothSteps = Mathf.Clamp(this.MouseSmoothSteps, nValue.int1, nValue.int20);
        this.MouseSmoothWeight = Mathf.Clamp01(this.MouseSmoothWeight);
        while (this.m_MouseSmoothBuffer.Count > this.MouseSmoothSteps)
        {
            this.m_MouseSmoothBuffer.RemoveAt(0);
        }
        this.m_MouseSmoothBuffer.Add(this.m_MouseMove);
        float num = (float)nValue.int1;
        Vector2 a = Vector2.zero;
        float num2 = (float)nValue.int0;
        for (int i = this.m_MouseSmoothBuffer.Count - 1; i > 0; i--)
        {
            a += this.m_MouseSmoothBuffer[i] * num;
            num2 += (float)nValue.int1 * num;
            num *= this.MouseSmoothWeight / base.Delta;
        }
        num2 = Mathf.Max((float)nValue.int1, num2);
        Vector2 vector = vp_MathUtility.NaNSafeVector2(a / num2, default(Vector2));
        float num3 = (float)nValue.int0;
        float num4 = Mathf.Abs(vector.x);
        float num5 = Mathf.Abs(vector.y);
        if (this.MouseAcceleration)
        {
            num3 = Mathf.Sqrt(num4 * num4 + num5 * num5) / base.Delta;
            num3 = ((num3 > this.MouseAccelerationThreshold) ? num3 : ((float)nValue.int0));
        }
        this.m_Yaw += vector.x * (this.MouseSensitivity.x + num3);
        this.m_Pitch -= vector.y * (this.MouseSensitivity.y + num3);
        this.m_Yaw = ((this.m_Yaw >= (float)(-(float)nValue.int360)) ? this.m_Yaw : (this.m_Yaw += (float)nValue.int360));
        this.m_Yaw = ((this.m_Yaw <= (float)nValue.int360) ? this.m_Yaw : (this.m_Yaw -= (float)nValue.int360));
        this.m_Yaw = Mathf.Clamp(this.m_Yaw, this.RotationYawLimit.x, this.RotationYawLimit.y);
        this.m_Pitch = ((this.m_Pitch >= (float)(-(float)nValue.int360)) ? this.m_Pitch : (this.m_Pitch += (float)nValue.int360));
        this.m_Pitch = ((this.m_Pitch <= (float)nValue.int360) ? this.m_Pitch : (this.m_Pitch -= (float)nValue.int360));
        this.m_Pitch = Mathf.Clamp(this.m_Pitch, -this.RotationPitchLimit.x, -this.RotationPitchLimit.y);
    }

    // Token: 0x06003041 RID: 12353 RVA: 0x00114DB4 File Offset: 0x00112FB4
    protected virtual void UpdateZoom()
    {
        if (this.m_FinalZoomTime <= Time.time)
        {
            return;
        }
        this.RenderingZoomDamping = Mathf.Max(this.RenderingZoomDamping, nValue.float001);
        float t = (float)nValue.int1 - (this.m_FinalZoomTime - Time.time) / this.RenderingZoomDamping;
        base.gameObject.GetComponent<Camera>().fieldOfView = Mathf.SmoothStep(base.gameObject.GetComponent<Camera>().fieldOfView, this.RenderingFieldOfView, t);
    }

    // Token: 0x06003042 RID: 12354 RVA: 0x000217E2 File Offset: 0x0001F9E2
    public virtual void Zoom()
    {
        this.m_FinalZoomTime = Time.time + this.RenderingZoomDamping;
    }

    // Token: 0x06003043 RID: 12355 RVA: 0x000217FB File Offset: 0x0001F9FB
    public virtual void SnapZoom()
    {
        base.gameObject.GetComponent<Camera>().fieldOfView = this.RenderingFieldOfView;
    }

    // Token: 0x06003044 RID: 12356 RVA: 0x00114E44 File Offset: 0x00113044
    protected virtual void UpdateShakes()
    {
        if (this.ShakeSpeed != nValue.float0)
        {
            this.m_Yaw -= this.m_Shake.y;
            this.m_Pitch -= this.m_Shake.x;
            this.m_Shake = Vector3.Scale(vp_SmoothRandom.GetVector3Centered(this.ShakeSpeed), this.ShakeAmplitude);
            this.m_Yaw += this.m_Shake.y;
            this.m_Pitch += this.m_Shake.x;
            this.m_RotationSpring.AddForce(Vector3.forward * this.m_Shake.z);
        }
    }

    // Token: 0x06003045 RID: 12357 RVA: 0x00114F00 File Offset: 0x00113100
    protected virtual void UpdateBob()
    {
        if (this.BobAmplitude == Vector4.zero || this.BobRate == Vector4.zero)
        {
            return;
        }
        this.m_BobSpeed = ((!this.BobRequireGroundContact || this.controllerGrounded) ? this.controllerVelocity.sqrMagnitude : nValue.float0);
        this.m_BobSpeed = Mathf.Min(this.m_BobSpeed * this.BobInputVelocityScale, this.BobMaxInputVelocity);
        this.m_BobSpeed = Mathf.Round(this.m_BobSpeed * (float)nValue.int1000) / (float)nValue.int1000;
        if (this.m_BobSpeed == (float)nValue.int0)
        {
            this.m_BobSpeed = Mathf.Min(this.m_LastBobSpeed * 0.93f, this.BobMaxInputVelocity);
        }
        this.bobTime += Time.deltaTime;
        this.m_CurrentBobAmp.y = this.m_BobSpeed * (this.BobAmplitude.y * -nValue.float00001);
        this.m_CurrentBobVal.y = Mathf.Cos(this.bobTime * (this.BobRate.y * (float)nValue.int10)) * this.m_CurrentBobAmp.y;
        this.m_CurrentBobAmp.w = this.m_BobSpeed * (this.BobAmplitude.w * nValue.float00001);
        this.m_CurrentBobVal.w = Mathf.Cos(this.bobTime * (this.BobRate.w * (float)nValue.int10)) * this.m_CurrentBobAmp.w;
        this.m_PositionSpring.AddForce(this.m_CurrentBobVal);
        this.AddRollForce(this.m_CurrentBobVal.w);
        this.m_LastBobSpeed = this.m_BobSpeed;
    }

    // Token: 0x06003046 RID: 12358 RVA: 0x001150CC File Offset: 0x001132CC
    protected virtual void DetectBobStep(float speed, float upBob)
    {
        if (this.BobStepCallback == null)
        {
            return;
        }
        if (speed < this.BobStepThreshold)
        {
            return;
        }
        bool flag = this.m_LastUpBob < upBob;
        this.m_LastUpBob = upBob;
        if (flag && !this.m_BobWasElevating)
        {
            this.BobStepCallback();
        }
        this.m_BobWasElevating = flag;
    }

    // Token: 0x06003047 RID: 12359 RVA: 0x00115130 File Offset: 0x00113330
    protected virtual void UpdateSwaying()
    {
        this.AddRollForce(base.Transform.InverseTransformDirection(this.FPController.mCharacterController.velocity * 0.016f).x * this.RotationStrafeRoll);
    }

    // Token: 0x06003048 RID: 12360 RVA: 0x0002170E File Offset: 0x0001F90E
    protected virtual void UpdateSprings()
    {
        this.m_PositionSpring.FixedUpdate();
        this.m_PositionSpring2.FixedUpdate();
        this.m_RotationSpring.FixedUpdate();
    }

    // Token: 0x06003049 RID: 12361 RVA: 0x0011517C File Offset: 0x0011337C
    public virtual void DoBomb(Vector3 positionForce, float minRollForce, float maxRollForce)
    {
        this.AddForce2(positionForce);
        float num = UnityEngine.Random.Range(minRollForce, maxRollForce);
        if (UnityEngine.Random.value > nValue.float05)
        {
            num = -num;
        }
        this.AddRollForce(num);
    }

    // Token: 0x0600304A RID: 12362 RVA: 0x001151B4 File Offset: 0x001133B4
    public override void Refresh()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        if (this.m_PositionSpring != null)
        {
            this.m_PositionSpring.Stiffness = new Vector3(this.PositionSpringStiffness, this.PositionSpringStiffness, this.PositionSpringStiffness);
            this.m_PositionSpring.Damping = Vector3.one - new Vector3(this.PositionSpringDamping, this.PositionSpringDamping, this.PositionSpringDamping);
            this.m_PositionSpring.MinState.y = this.PositionGroundLimit;
            this.m_PositionSpring.RestState = this.PositionOffset;
        }
        if (this.m_PositionSpring2 != null)
        {
            this.m_PositionSpring2.Stiffness = new Vector3(this.PositionSpring2Stiffness, this.PositionSpring2Stiffness, this.PositionSpring2Stiffness);
            this.m_PositionSpring2.Damping = Vector3.one - new Vector3(this.PositionSpring2Damping, this.PositionSpring2Damping, this.PositionSpring2Damping);
            this.m_PositionSpring2.MinState.y = -this.PositionOffset.y + this.PositionGroundLimit;
        }
        if (this.m_RotationSpring != null)
        {
            this.m_RotationSpring.Stiffness = new Vector3(this.RotationSpringStiffness, this.RotationSpringStiffness, this.RotationSpringStiffness);
            this.m_RotationSpring.Damping = Vector3.one - new Vector3(this.RotationSpringDamping, this.RotationSpringDamping, this.RotationSpringDamping);
        }
        this.Zoom();
    }

    // Token: 0x0600304B RID: 12363 RVA: 0x00115390 File Offset: 0x00113590
    public virtual void SnapSprings()
    {
        if (this.m_PositionSpring != null)
        {
            this.m_PositionSpring.RestState = this.PositionOffset;
            this.m_PositionSpring.State = this.PositionOffset;
            this.m_PositionSpring.Stop(true);
        }
        if (this.m_PositionSpring2 != null)
        {
            this.m_PositionSpring2.RestState = Vector3.zero;
            this.m_PositionSpring2.State = Vector3.zero;
            this.m_PositionSpring2.Stop(true);
        }
        if (this.m_RotationSpring != null)
        {
            this.m_RotationSpring.RestState = Vector3.zero;
            this.m_RotationSpring.State = Vector3.zero;
            this.m_RotationSpring.Stop(true);
        }
    }

    // Token: 0x0600304C RID: 12364 RVA: 0x00115450 File Offset: 0x00113650
    public virtual void StopSprings()
    {
        if (this.m_PositionSpring != null)
        {
            this.m_PositionSpring.Stop(true);
        }
        if (this.m_PositionSpring2 != null)
        {
            this.m_PositionSpring2.Stop(true);
        }
        if (this.m_RotationSpring != null)
        {
            this.m_RotationSpring.Stop(true);
        }
        this.m_BobSpeed = nValue.float0;
        this.m_LastBobSpeed = nValue.float0;
    }

    // Token: 0x0600304D RID: 12365 RVA: 0x00021818 File Offset: 0x0001FA18
    public virtual void Stop()
    {
        this.SnapSprings();
        this.SnapZoom();
        this.Refresh();
    }

    // Token: 0x0600304E RID: 12366 RVA: 0x0002182C File Offset: 0x0001FA2C
    public virtual void SetRotation(Vector2 eulerAngles, bool stop = true, bool resetInitialRotation = true)
    {
        this.Angle = eulerAngles;
        if (stop)
        {
            this.Stop();
        }
        if (resetInitialRotation)
        {
            this.m_InitialRotation = Vector2.zero;
        }
    }

    // Token: 0x0600304F RID: 12367 RVA: 0x001154B8 File Offset: 0x001136B8
    public void OnMessage_FallImpact(float impact)
    {
        impact = Mathf.Abs(impact * 55f);
        float num = impact * this.PositionKneeling;
        float num2 = impact * this.RotationKneeling;
        num = Mathf.SmoothStep((float)nValue.int0, (float)nValue.int1, num);
        num2 = Mathf.SmoothStep((float)nValue.int0, (float)nValue.int1, num2);
        num2 = Mathf.SmoothStep((float)nValue.int0, (float)nValue.int1, num2);
        if (this.m_PositionSpring != null)
        {
            this.m_PositionSpring.AddSoftForce(Vector3.down * num, (float)this.PositionKneelingSoftness);
        }
        if (this.m_RotationSpring != null)
        {
            float d = (UnityEngine.Random.value <= nValue.float05) ? (-(num2 * (float)nValue.int2)) : (num2 * (float)nValue.int2);
            this.m_RotationSpring.AddSoftForce(Vector3.forward * d, (float)this.RotationKneelingSoftness);
        }
    }

    // Token: 0x04001F0B RID: 7947
    public bool EnableCameraCollision;

    // Token: 0x04001F0C RID: 7948
    public vp_FPController FPController;

    // Token: 0x04001F0D RID: 7949
    public Vector2 MouseSensitivity = new Vector2(5f, 5f);

    // Token: 0x04001F0E RID: 7950
    public int MouseSmoothSteps = 10;

    // Token: 0x04001F0F RID: 7951
    public float MouseSmoothWeight = 0.5f;

    // Token: 0x04001F10 RID: 7952
    public bool MouseAcceleration;

    // Token: 0x04001F11 RID: 7953
    public float MouseAccelerationThreshold = 0.4f;

    // Token: 0x04001F12 RID: 7954
    protected Vector2 m_MouseMove = Vector2.zero;

    // Token: 0x04001F13 RID: 7955
    protected List<Vector2> m_MouseSmoothBuffer = new List<Vector2>();

    // Token: 0x04001F14 RID: 7956
    public CryptoFloat RenderingFieldOfView = 60f;

    // Token: 0x04001F15 RID: 7957
    public CryptoFloat RenderingZoomDamping = 0.2f;

    // Token: 0x04001F16 RID: 7958
    protected float m_FinalZoomTime;

    // Token: 0x04001F17 RID: 7959
    public CryptoVector3 PositionOffset;

    // Token: 0x04001F18 RID: 7960
    public CryptoFloat PositionGroundLimit = 0.1f;

    // Token: 0x04001F19 RID: 7961
    public CryptoFloat PositionSpringStiffness = 0.01f;

    // Token: 0x04001F1A RID: 7962
    public CryptoFloat PositionSpringDamping = 0.25f;

    // Token: 0x04001F1B RID: 7963
    public CryptoFloat PositionSpring2Stiffness = 0.95f;

    // Token: 0x04001F1C RID: 7964
    public CryptoFloat PositionSpring2Damping = 0.25f;

    // Token: 0x04001F1D RID: 7965
    public CryptoFloat PositionKneeling = 0.025f;

    // Token: 0x04001F1E RID: 7966
    public CryptoInt PositionKneelingSoftness = 1;

    // Token: 0x04001F1F RID: 7967
    public CryptoFloat PositionEarthQuakeFactor = 1f;

    // Token: 0x04001F20 RID: 7968
    protected vp_SpringThread m_PositionSpring;

    // Token: 0x04001F21 RID: 7969
    protected vp_SpringThread m_PositionSpring2;

    // Token: 0x04001F22 RID: 7970
    protected bool m_DrawCameraCollisionDebugLine;

    // Token: 0x04001F23 RID: 7971
    public Vector2 RotationPitchLimit = new Vector2(90f, -90f);

    // Token: 0x04001F24 RID: 7972
    public Vector2 RotationYawLimit = new Vector2(-360f, 360f);

    // Token: 0x04001F25 RID: 7973
    public CryptoFloat RotationSpringStiffness = 0.01f;

    // Token: 0x04001F26 RID: 7974
    public CryptoFloat RotationSpringDamping = 0.25f;

    // Token: 0x04001F27 RID: 7975
    public CryptoFloat RotationKneeling = 0.025f;

    // Token: 0x04001F28 RID: 7976
    public CryptoInt RotationKneelingSoftness = 1;

    // Token: 0x04001F29 RID: 7977
    public CryptoFloat RotationStrafeRoll = 0.01f;

    // Token: 0x04001F2A RID: 7978
    public CryptoFloat RotationEarthQuakeFactor = 0f;

    // Token: 0x04001F2B RID: 7979
    protected float m_Pitch;

    // Token: 0x04001F2C RID: 7980
    protected float m_Yaw;

    // Token: 0x04001F2D RID: 7981
    protected vp_SpringThread m_RotationSpring;

    // Token: 0x04001F2E RID: 7982
    protected Vector2 m_InitialRotation = Vector2.zero;

    // Token: 0x04001F2F RID: 7983
    public float ShakeSpeed;

    // Token: 0x04001F30 RID: 7984
    public Vector3 ShakeAmplitude = new Vector3(10f, 10f, 0f);

    // Token: 0x04001F31 RID: 7985
    protected Vector3 m_Shake = Vector3.zero;

    // Token: 0x04001F32 RID: 7986
    public Vector4 BobRate = new Vector4(0f, 1.4f, 0f, 0.7f);

    // Token: 0x04001F33 RID: 7987
    public Vector4 BobAmplitude = new Vector4(0f, 0.25f, 0f, 0.5f);

    // Token: 0x04001F34 RID: 7988
    public float BobInputVelocityScale = 1f;

    // Token: 0x04001F35 RID: 7989
    public float BobMaxInputVelocity = 100f;

    // Token: 0x04001F36 RID: 7990
    public bool BobRequireGroundContact = true;

    // Token: 0x04001F37 RID: 7991
    protected float m_LastBobSpeed;

    // Token: 0x04001F38 RID: 7992
    protected Vector4 m_CurrentBobAmp = Vector4.zero;

    // Token: 0x04001F39 RID: 7993
    protected Vector4 m_CurrentBobVal = Vector4.zero;

    // Token: 0x04001F3A RID: 7994
    protected float m_BobSpeed;

    // Token: 0x04001F3B RID: 7995
    public vp_FPCamera.BobStepDelegate BobStepCallback;

    // Token: 0x04001F3C RID: 7996
    public float BobStepThreshold = 10f;

    // Token: 0x04001F3D RID: 7997
    protected float m_LastUpBob;

    // Token: 0x04001F3E RID: 7998
    protected bool m_BobWasElevating;

    // Token: 0x04001F3F RID: 7999
    protected Vector3 m_CameraCollisionStartPos = Vector3.zero;

    // Token: 0x04001F40 RID: 8000
    protected Vector3 m_CameraCollisionEndPos = Vector3.zero;

    // Token: 0x04001F41 RID: 8001
    protected RaycastHit m_CameraHit;

    // Token: 0x04001F42 RID: 8002
    private bool controllerGrounded;

    // Token: 0x04001F43 RID: 8003
    private Vector3 controllerVelocity;

    // Token: 0x04001F44 RID: 8004
    private float bobTime;

    // Token: 0x02000587 RID: 1415
    // (Invoke) Token: 0x06003051 RID: 12369
    public delegate void BobStepDelegate();
}
