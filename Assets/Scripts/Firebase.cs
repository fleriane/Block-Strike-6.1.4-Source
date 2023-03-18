using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x0200050E RID: 1294
public class Firebase
{
    // Token: 0x06002C50 RID: 11344 RVA: 0x0001ECD2 File Offset: 0x0001CED2
    public Firebase()
    {
        this.Key = string.Empty;
        this.FullKey = string.Empty;
        this.Parent = null;
        this.DataBase = "blockstrikeq-98a3c-default-rtdb.firebaseio.com";
    }

    // Token: 0x06002C51 RID: 11345 RVA: 0x0001ED02 File Offset: 0x0001CF02
    public Firebase(string databaseURL)
    {
        this.DataBase = databaseURL;
    }

    // Token: 0x06002C52 RID: 11346 RVA: 0x0001ED11 File Offset: 0x0001CF11
    public Firebase(string databaseURL, string auth)
    {
        this.DataBase = databaseURL;
        this.Auth = auth;
    }

    // Token: 0x06002C53 RID: 11347 RVA: 0x0001ED27 File Offset: 0x0001CF27
    private Firebase(Firebase parent, string key, string auth)
    {
        this.Parent = parent;
        this.Key = key;
        this.Auth = auth;
        this.FullKey = parent.FullKey + "/" + key;
        this.DataBase = parent.DataBase;
    }

    // Token: 0x17000490 RID: 1168
    // (get) Token: 0x06002C54 RID: 11348 RVA: 0x0001ED67 File Offset: 0x0001CF67
    public string FullURL
    {
        get
        {
            return "https://" + this.DataBase + this.FullKey + ".json";
        }
    }

    // Token: 0x06002C55 RID: 11349 RVA: 0x0001ED84 File Offset: 0x0001CF84
    public Firebase Child(string key)
    {
        return new Firebase(this, key, this.Auth);
    }

    // Token: 0x06002C56 RID: 11350 RVA: 0x0010350C File Offset: 0x0010170C
    public Firebase Copy()
    {
        return new Firebase
        {
            Key = this.Key,
            Auth = this.Auth,
            FullKey = this.FullKey,
            Parent = this.Parent,
            DataBase = this.DataBase
        };
    }

    // Token: 0x06002C57 RID: 11351 RVA: 0x0001ED93 File Offset: 0x0001CF93
    public void SetTimeStamp(string key)
    {
        this.Child(key).SetValue(Firebase.GetTimeStamp());
    }

    // Token: 0x06002C58 RID: 11352 RVA: 0x0001EDA6 File Offset: 0x0001CFA6
    public static string GetTimeStamp()
    {
        return "{\".sv\": \"timestamp\"}";
    }

    // Token: 0x06002C59 RID: 11353 RVA: 0x0001EDAD File Offset: 0x0001CFAD
    public void GetValue()
    {
        this.GetValue(string.Empty, null, null);
    }

    // Token: 0x06002C5A RID: 11354 RVA: 0x0001EDBC File Offset: 0x0001CFBC
    public void GetValue(Action<string> success, Action<string> failed)
    {
        this.GetValue(string.Empty, success, failed);
    }

    // Token: 0x06002C5B RID: 11355 RVA: 0x0001EDCB File Offset: 0x0001CFCB
    public void GetValue(FirebaseParam param)
    {
        this.GetValue(param.ToString(), null, null);
    }

    // Token: 0x06002C5C RID: 11356 RVA: 0x0001EDE2 File Offset: 0x0001CFE2
    public void GetValue(FirebaseParam param, Action<string> success, Action<string> failed)
    {
        this.GetValue(param.ToString(), success, failed);
    }

    // Token: 0x06002C5D RID: 11357 RVA: 0x0010355C File Offset: 0x0010175C
    public void GetValue(string param, Action<string> success, Action<string> failed)
    {
        if (!string.IsNullOrEmpty(this.Auth))
        {
            FirebaseParam firebaseParam = new FirebaseParam(param);
            param = firebaseParam.Auth(this.Auth).ToString();
        }
        string text = this.FullURL;
        param = WWW.EscapeURL(param);
        if (!string.IsNullOrEmpty(param))
        {
            text = text + "?" + param;
        }
        FirebaseManager.Instance.StartCoroutine(this.GetValueCoroutine(text, success, failed));
    }

    // Token: 0x06002C5E RID: 11358 RVA: 0x0001EDF9 File Offset: 0x0001CFF9
    private IEnumerator GetValueCoroutine(string url, Action<string> success, Action<string> failed)
    {
        WWW www = new WWW(url);
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            if (success != null)
            {
                success(www.text);
            }
            if (FirebaseManager.DebugAction)
            {
                Debug.Log("OnGetSuccess");
                Debug.Log("Firebase: " + this.FullURL);
                Debug.Log("Json: " + www.text);
            }
        }
        else
        {
            if (failed != null)
            {
                failed(www.error);
            }
            if (FirebaseManager.DebugAction)
            {
                Debug.Log("OnGetFailed");
                Debug.Log("Firebase: " + this.FullURL);
                Debug.Log("Json: " + www.error);
            }
        }
        yield break;
    }

    // Token: 0x06002C5F RID: 11359 RVA: 0x0001EE1D File Offset: 0x0001D01D
    public void SetValue(string json)
    {
        this.SetValue(json, string.Empty, null, null);
    }

    // Token: 0x06002C60 RID: 11360 RVA: 0x0001EE2D File Offset: 0x0001D02D
    public void SetValue(string json, Action<string> success, Action<string, string> failed)
    {
        this.SetValue(json, string.Empty, success, failed);
    }

    // Token: 0x06002C61 RID: 11361 RVA: 0x0001EE3D File Offset: 0x0001D03D
    public void SetValue(string json, FirebaseParam param)
    {
        this.SetValue(json, param.ToString(), null, null);
    }

    // Token: 0x06002C62 RID: 11362 RVA: 0x0001EE55 File Offset: 0x0001D055
    public void SetValue(string json, FirebaseParam param, Action<string> success, Action<string, string> failed)
    {
        this.SetValue(json, param.ToString(), success, failed);
    }

    // Token: 0x06002C63 RID: 11363 RVA: 0x001035D4 File Offset: 0x001017D4
    public void SetValue(string json, string param, Action<string> success, Action<string, string> failed)
    {
        if (!string.IsNullOrEmpty(this.Auth))
        {
            FirebaseParam firebaseParam = new FirebaseParam(param);
            param = firebaseParam.Auth(this.Auth).ToString();
        }
        string text = this.FullURL;
        param = WWW.EscapeURL(param);
        if (!string.IsNullOrEmpty(param))
        {
            text = text + "?" + param;
        }
        FirebaseManager.Instance.StartCoroutine(this.SetValueCoroutine(text, json, success, failed));
    }

    // Token: 0x06002C64 RID: 11364 RVA: 0x0001EE6E File Offset: 0x0001D06E
    private IEnumerator SetValueCoroutine(string url, string json, Action<string> success, Action<string, string> failed)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary.Add("Content-Type", "application/json");
        dictionary.Add("X-HTTP-Method-Override", "PUT");
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        WWW www = new WWW(url, bytes, dictionary);
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            if (success != null)
            {
                success(www.text);
            }
            if (FirebaseManager.DebugAction)
            {
                Debug.Log("OnSetSuccess");
                Debug.Log("Firebase: " + this.FullURL);
                Debug.Log("Json: " + www.text);
            }
        }
        else
        {
            if (failed != null)
            {
                failed(www.error, json);
            }
            if (FirebaseManager.DebugAction)
            {
                Debug.Log("OnSetFailed");
                Debug.Log("Firebase: " + this.FullURL);
                Debug.Log("Json: " + www.error);
            }
        }
        yield break;
    }

    // Token: 0x06002C65 RID: 11365 RVA: 0x0001EE9A File Offset: 0x0001D09A
    public void UpdateValue(string json)
    {
        this.UpdateValue(json, string.Empty, null, null);
    }

    // Token: 0x06002C66 RID: 11366 RVA: 0x0001EEAA File Offset: 0x0001D0AA
    public void UpdateValue(string json, Action<string> success, Action<string, string> failed)
    {
        this.UpdateValue(json, string.Empty, success, failed);
    }

    // Token: 0x06002C67 RID: 11367 RVA: 0x0001EEBA File Offset: 0x0001D0BA
    public void UpdateValue(string json, FirebaseParam param)
    {
        this.UpdateValue(json, param.ToString(), null, null);
    }

    // Token: 0x06002C68 RID: 11368 RVA: 0x0001EED2 File Offset: 0x0001D0D2
    public void UpdateValue(string json, FirebaseParam param, Action<string> success, Action<string, string> failed)
    {
        this.UpdateValue(json, param.ToString(), success, failed);
    }

    // Token: 0x06002C69 RID: 11369 RVA: 0x00103650 File Offset: 0x00101850
    public void UpdateValue(string json, string param, Action<string> success, Action<string, string> failed)
    {
        if (!string.IsNullOrEmpty(this.Auth))
        {
            FirebaseParam firebaseParam = new FirebaseParam(param);
            param = firebaseParam.Auth(this.Auth).ToString();
        }
        string text = this.FullURL;
        param = WWW.EscapeURL(param);
        if (!string.IsNullOrEmpty(param))
        {
            text = text + "?" + param;
        }
        FirebaseManager.Instance.StartCoroutine(this.UpdateValueCoroutine(text, json, success, failed));
    }

    // Token: 0x06002C6A RID: 11370 RVA: 0x0001EEEB File Offset: 0x0001D0EB
    private IEnumerator UpdateValueCoroutine(string url, string json, Action<string> success, Action<string, string> failed)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary.Add("Content-Type", "application/json");
        dictionary.Add("X-HTTP-Method-Override", "PATCH");
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        WWW www = new WWW(url, bytes, dictionary);
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            if (success != null)
            {
                success(www.text);
            }
            if (FirebaseManager.DebugAction)
            {
                Debug.Log("OnUpdateSuccess");
                Debug.Log("Firebase: " + this.FullURL);
                Debug.Log("Json: " + www.text);
            }
        }
        else
        {
            if (failed != null)
            {
                failed(www.error, json);
            }
            if (FirebaseManager.DebugAction)
            {
                Debug.Log("OnUpdateFailed");
                Debug.Log("Firebase: " + this.FullURL);
                Debug.Log("Json: " + www.error);
            }
        }
        yield break;
    }

    // Token: 0x06002C6B RID: 11371 RVA: 0x0001EF17 File Offset: 0x0001D117
    public void Push(string json)
    {
        this.Push(json, string.Empty, null, null);
    }

    // Token: 0x06002C6C RID: 11372 RVA: 0x0001EF27 File Offset: 0x0001D127
    public void Push(string json, Action<string> success, Action<string> failed)
    {
        this.Push(json, string.Empty, success, failed);
    }

    // Token: 0x06002C6D RID: 11373 RVA: 0x0001EF37 File Offset: 0x0001D137
    public void Push(string json, FirebaseParam param)
    {
        this.Push(json, param.ToString(), null, null);
    }

    // Token: 0x06002C6E RID: 11374 RVA: 0x0001EF4F File Offset: 0x0001D14F
    public void Push(string json, FirebaseParam param, Action<string> success, Action<string> failed)
    {
        this.Push(json, param.ToString(), success, failed);
    }

    // Token: 0x06002C6F RID: 11375 RVA: 0x001036CC File Offset: 0x001018CC
    public void Push(string json, string param, Action<string> success, Action<string> failed)
    {
        if (!string.IsNullOrEmpty(this.Auth))
        {
            FirebaseParam firebaseParam = new FirebaseParam(param);
            param = firebaseParam.Auth(this.Auth).ToString();
        }
        string text = this.FullURL;
        param = WWW.EscapeURL(param);
        if (!string.IsNullOrEmpty(param))
        {
            text = text + "?" + param;
        }
        FirebaseManager.Instance.StartCoroutine(this.PushCoroutine(text, json, success, failed));
    }

    // Token: 0x06002C70 RID: 11376 RVA: 0x0001EF68 File Offset: 0x0001D168
    private IEnumerator PushCoroutine(string url, string json, Action<string> success, Action<string> failed)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        WWW www = new WWW(url, bytes);
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            if (success != null)
            {
                success(www.text);
            }
            if (FirebaseManager.DebugAction)
            {
                Debug.Log("OnPushSuccess");
                Debug.Log("Firebase: " + this.FullURL);
                Debug.Log("Json: " + www.text);
            }
        }
        else
        {
            if (failed != null)
            {
                failed(www.error);
            }
            if (FirebaseManager.DebugAction)
            {
                Debug.Log("OnPushFailed");
                Debug.Log("Firebase: " + this.FullURL);
                Debug.Log("Json: " + www.error);
            }
        }
        yield break;
    }

    // Token: 0x06002C71 RID: 11377 RVA: 0x0001EF94 File Offset: 0x0001D194
    public void Delete()
    {
        this.Delete(string.Empty, null, null);
    }

    // Token: 0x06002C72 RID: 11378 RVA: 0x0001EFA3 File Offset: 0x0001D1A3
    public void Delete(Action<string> success, Action<string> failed)
    {
        this.Delete(string.Empty, success, failed);
    }

    // Token: 0x06002C73 RID: 11379 RVA: 0x0001EFB2 File Offset: 0x0001D1B2
    public void Delete(FirebaseParam param)
    {
        this.Delete(param.ToString(), null, null);
    }

    // Token: 0x06002C74 RID: 11380 RVA: 0x0001EFC9 File Offset: 0x0001D1C9
    public void Delete(FirebaseParam param, Action<string> success, Action<string> failed)
    {
        this.Delete(param.ToString(), success, failed);
    }

    // Token: 0x06002C75 RID: 11381 RVA: 0x00103748 File Offset: 0x00101948
    public void Delete(string param, Action<string> success, Action<string> failed)
    {
        if (!string.IsNullOrEmpty(this.Auth))
        {
            FirebaseParam firebaseParam = new FirebaseParam(param);
            param = firebaseParam.Auth(this.Auth).ToString();
        }
        string text = this.FullURL;
        param = WWW.EscapeURL(param);
        if (!string.IsNullOrEmpty(param))
        {
            text = text + "?" + param;
        }
        FirebaseManager.Instance.StartCoroutine(this.DeleteCoroutine(text, success, failed));
    }

    // Token: 0x06002C76 RID: 11382 RVA: 0x0001EFE0 File Offset: 0x0001D1E0
    private IEnumerator DeleteCoroutine(string url, Action<string> success, Action<string> failed)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary.Add("Content-Type", "application/json");
        dictionary.Add("X-HTTP-Method-Override", "DELETE");
        byte[] bytes = Encoding.UTF8.GetBytes("{ \"dummy\" : \"dummies\"}");
        WWW www = new WWW(url, bytes, dictionary);
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            if (success != null)
            {
                success(www.text);
            }
            if (FirebaseManager.DebugAction)
            {
                Debug.Log("OnDeleteSuccess");
                Debug.Log("Firebase: " + this.FullURL);
                Debug.Log("Json: " + www.text);
            }
        }
        else
        {
            if (failed != null)
            {
                failed(www.error);
            }
            if (FirebaseManager.DebugAction)
            {
                Debug.Log("OnDeleteFailed");
                Debug.Log("Firebase: " + this.FullURL);
                Debug.Log("Json: " + www.error);
            }
        }
        yield break;
    }

    // Token: 0x04001C5C RID: 7260
    public string Key;

    // Token: 0x04001C5D RID: 7261
    public string Auth;

    // Token: 0x04001C5E RID: 7262
    public string FullKey;

    // Token: 0x04001C5F RID: 7263
    public Firebase Parent;

    // Token: 0x04001C60 RID: 7264
    public string DataBase;

    // Token: 0x04001C61 RID: 7265
    private static bool isServerCertificate;

    // Token: 0x04001C62 RID: 7266
    public static string Base;
}
