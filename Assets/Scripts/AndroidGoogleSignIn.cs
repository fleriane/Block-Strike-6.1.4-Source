using System;
using FreeJSON;
using UnityEngine;

// Token: 0x02000130 RID: 304
public class AndroidGoogleSignIn : MonoBehaviour
{
    // Token: 0x06000AB6 RID: 2742 RVA: 0x0000BCF2 File Offset: 0x00009EF2
    private void Awake()
    {
        if (AndroidGoogleSignIn.instance == null)
        {
            AndroidGoogleSignIn.instance = this;
        }
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
    }

    // Token: 0x06000AB7 RID: 2743 RVA: 0x0005B384 File Offset: 0x00059584
    private static void Init()
    {
        if (AndroidGoogleSignIn.instance != null)
        {
            return;
        }
        GameObject gameObject = new GameObject("GoogleSignIn");
        gameObject.AddComponent<AndroidGoogleSignIn>();
    }

    // Token: 0x06000AB8 RID: 2744 RVA: 0x0005B3B4 File Offset: 0x000595B4
    public static void SignIn(string webClientId, Action success, Action<string> error)
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }
        AndroidGoogleSignIn.Init();
        AndroidGoogleSignIn.successCallback = success;
        AndroidGoogleSignIn.errorCallback = error;
        using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.rexetstudio.google.GoogleSignInFragment"))
                {
                    androidJavaClass2.SetStatic<string>("UnityGameObjectName", AndroidGoogleSignIn.instance.gameObject.name);
                    androidJavaClass2.CallStatic("SignIn", new object[]
                    {
                        @static,
                        webClientId
                    });
                }
            }
        }
    }

    // Token: 0x06000AB9 RID: 2745 RVA: 0x0005B494 File Offset: 0x00059694
    public void UnityGoogleSignInSuccessCallback(string googleSignInAccountJson)
    {
        googleSignInAccountJson = googleSignInAccountJson.Replace(", ", ",");
        googleSignInAccountJson = googleSignInAccountJson.Replace(": ", ":");
        AndroidGoogleSignInAccount androidGoogleSignInAccount = new AndroidGoogleSignInAccount();
        JsonObject jsonObject = JsonObject.Parse(googleSignInAccountJson);
        androidGoogleSignInAccount.DisplayName = jsonObject.Get<string>("DisplayName");
        androidGoogleSignInAccount.Email = jsonObject.Get<string>("Email");
        androidGoogleSignInAccount.FamilyName = jsonObject.Get<string>("FamilyName");
        androidGoogleSignInAccount.Id = jsonObject.Get<string>("Id");
        androidGoogleSignInAccount.PhotoUrl = jsonObject.Get<string>("PhotoUrl");
        androidGoogleSignInAccount.Token = jsonObject.Get<string>("Token");
        if (androidGoogleSignInAccount == null)
        {
            this.UnityGoogleSignInErrorCallback(string.Empty);
            return;
        }
        if (string.IsNullOrEmpty(androidGoogleSignInAccount.Email))
        {
            this.UnityGoogleSignInErrorCallback(string.Empty);
            return;
        }
        AndroidGoogleSignIn.Account = androidGoogleSignInAccount;
        if (AndroidGoogleSignIn.successCallback != null)
        {
            AndroidGoogleSignIn.successCallback();
        }
        this.ClearReferences();
    }

    // Token: 0x06000ABA RID: 2746 RVA: 0x0000BD15 File Offset: 0x00009F15
    public void UnityGoogleSignInErrorCallback(string errorMsg)
    {
        if (AndroidGoogleSignIn.errorCallback != null)
        {
            AndroidGoogleSignIn.errorCallback(errorMsg);
        }
        this.ClearReferences();
    }

    // Token: 0x06000ABB RID: 2747 RVA: 0x0000BD32 File Offset: 0x00009F32
    private void ClearReferences()
    {
        AndroidGoogleSignIn.successCallback = null;
        AndroidGoogleSignIn.errorCallback = null;
    }

    // Token: 0x04000765 RID: 1893
    public static AndroidGoogleSignInAccount Account = new AndroidGoogleSignInAccount();

    // Token: 0x04000766 RID: 1894
    private static AndroidGoogleSignIn instance;

    // Token: 0x04000767 RID: 1895
    private static Action successCallback;

    // Token: 0x04000768 RID: 1896
    private static Action<string> errorCallback;
}
