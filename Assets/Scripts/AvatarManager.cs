using System;
using System.Collections.Generic;
using BestHTTP;
using UnityEngine;

// Token: 0x0200031B RID: 795
public class AvatarManager
{
    // Token: 0x06001D72 RID: 7538 RVA: 0x0001525B File Offset: 0x0001345B
    public static bool Contains(string url)
    {
        return !string.IsNullOrEmpty(url) && CacheManager.Exists(url, "Avatars", true);
    }

    // Token: 0x06001D73 RID: 7539 RVA: 0x000B7E3C File Offset: 0x000B603C
    public static void Get(string url, Action<Texture2D> callback)
    {
        if (string.IsNullOrEmpty(url))
        {
            return;
        }
        if (!AvatarManager.Contains(url))
        {
            new HTTPRequest(new Uri(url), delegate (HTTPRequest req, HTTPResponse res)
            {
                if (res.IsSuccess)
                {
                    Texture2D texture2D2 = new Texture2D(96, 96);
                    try
                    {
                        texture2D2.LoadImage(res.Data);
                        texture2D2.Apply();
                        AvatarManager.avatars.Add(url, texture2D2);
                        CacheManager.Save<byte[]>(url, "Avatars", res.Data, true);
                        callback(texture2D2);
                    }
                    catch
                    {
                        callback(null);
                    }
                }
            }).Send();
            return;
        }
        if (AvatarManager.avatars.ContainsKey(url))
        {
            callback(AvatarManager.avatars[url]);
            return;
        }
        Texture2D texture2D = new Texture2D(96, 96);
        try
        {
            texture2D.LoadImage(CacheManager.Load<byte[]>(url, "Avatars", true));
            texture2D.Apply();
            AvatarManager.avatars.Add(url, texture2D);
            callback(texture2D);
        }
        catch
        {
            callback(null);
        }
    }

    // Token: 0x06001D74 RID: 7540 RVA: 0x000B7F40 File Offset: 0x000B6140
    public static Texture2D Get(string url)
    {
        if (AvatarManager.avatars.ContainsKey(url))
        {
            return AvatarManager.avatars[url];
        }
        if (string.IsNullOrEmpty(url))
        {
            return GameSettings.instance.NoAvatarTexture;
        }
        if (AvatarManager.Contains(url))
        {
            try
            {
                Texture2D texture2D = new Texture2D(96, 96);
                texture2D.LoadImage(CacheManager.Load<byte[]>(url, "Avatars", true));
                texture2D.Apply();
                AvatarManager.avatars.Add(url, texture2D);
                return texture2D;
            }
            catch
            {
            }
        }
        return GameSettings.instance.NoAvatarTexture;
    }

    // Token: 0x06001D75 RID: 7541 RVA: 0x000B7FE8 File Offset: 0x000B61E8
    public static void Load(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return;
        }
        if (AvatarManager.Contains(url))
        {
            return;
        }
        new HTTPRequest(new Uri(url), delegate (HTTPRequest req, HTTPResponse res)
        {
            if (res.IsSuccess)
            {
                CacheManager.Save<byte[]>(url, "Avatars", res.Data, true);
            }
        }).Send();
    }

    // Token: 0x04001187 RID: 4487
    private static Dictionary<string, Texture2D> avatars = new Dictionary<string, Texture2D>();

    // Token: 0x04001188 RID: 4488
    public static Texture2D avatarsAtlas;

    // Token: 0x04001189 RID: 4489
    public static Rect[] avatarsAtlasRect;
}
