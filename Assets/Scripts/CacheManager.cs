using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

// Token: 0x020004D6 RID: 1238
public class CacheManager
{
    // Token: 0x06002A99 RID: 10905 RVA: 0x0001DA7B File Offset: 0x0001BC7B
    public static void Save<T>(string name, string path, T data)
    {
        CacheManager.Save<T>(name, path, data, false, false, string.Empty);
    }

    // Token: 0x06002A9A RID: 10906 RVA: 0x0001DA8C File Offset: 0x0001BC8C
    public static void Save<T>(string name, string path, T data, bool md5)
    {
        CacheManager.Save<T>(name, path, data, md5, false, string.Empty);
    }

    // Token: 0x06002A9B RID: 10907 RVA: 0x0001DA9D File Offset: 0x0001BC9D
    public static void Save<T>(string name, string path, T data, bool md5, bool crypto)
    {
        CacheManager.Save<T>(name, path, data, md5, crypto, string.Empty);
    }

    // Token: 0x06002A9C RID: 10908 RVA: 0x000FDA00 File Offset: 0x000FBC00
    public static void Save<T>(string name, string path, T data, bool md5, bool crypto, string customCryptoKey)
    {
        if (Application.isEditor)
        {
            path = Directory.GetParent(Application.dataPath).FullName + "/Cache/" + path;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            string text = new AndroidJavaClass("android.os.Environment").CallStatic<AndroidJavaObject>("getExternalStorageDirectory", new object[0]).Call<string>("getAbsolutePath", new object[0]);
            text += "/Android/data/";
            text += new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity").Call<string>("getPackageName", new object[0]);
            if (Directory.Exists(text))
            {
                if (Directory.Exists(text + "/cache"))
                {
                    Directory.CreateDirectory(text + "/cache");
                }
                path = text + "/cache/" + path;
            }
            else
            {
                path = Application.dataPath;
            }
        }
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        if (md5)
        {
            name = CacheManager.Md5(name);
        }
        Stream stream = File.Open(path + "/" + name, FileMode.Create);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        if (crypto)
        {
            string text2 = CacheManager.cryptoKey;
            if (!string.IsNullOrEmpty(customCryptoKey))
            {
                text2 = customCryptoKey;
            }
            CryptoStream cryptoStream = new CryptoStream(stream, new DESCryptoServiceProvider
            {
                Key = Encoding.ASCII.GetBytes(text2.Substring(0, 8)),
                IV = Encoding.ASCII.GetBytes(text2.Substring(0, 8))
            }.CreateEncryptor(), CryptoStreamMode.Write);
            binaryFormatter.Serialize(cryptoStream, data);
            cryptoStream.Close();
        }
        else
        {
            binaryFormatter.Serialize(stream, data);
        }
        stream.Close();
    }

    // Token: 0x06002A9D RID: 10909 RVA: 0x0001DAAF File Offset: 0x0001BCAF
    public static void SaveAsync<T>(string name, string path, T data)
    {
        CacheManager.SaveAsync<T>(name, path, data, false, false, string.Empty);
    }

    // Token: 0x06002A9E RID: 10910 RVA: 0x0001DAC0 File Offset: 0x0001BCC0
    public static void SaveAsync<T>(string name, string path, T data, bool md5)
    {
        CacheManager.SaveAsync<T>(name, path, data, md5, false, string.Empty);
    }

    // Token: 0x06002A9F RID: 10911 RVA: 0x0001DAD1 File Offset: 0x0001BCD1
    public static void SaveAsync<T>(string name, string path, T data, bool md5, bool crypto)
    {
        CacheManager.SaveAsync<T>(name, path, data, md5, crypto, string.Empty);
    }

    // Token: 0x06002AA0 RID: 10912 RVA: 0x000FDBB8 File Offset: 0x000FBDB8
    public static void SaveAsync<T>(string name, string path, T data, bool md5, bool crypto, string customCryptoKey)
    {
        if (SystemInfo.processorCount <= 1)
        {
            CacheManager.Save<T>(name, path, data, md5, crypto, customCryptoKey);
            return;
        }
        if (Application.isEditor)
        {
            path = Directory.GetParent(Application.dataPath).FullName + "/Cache/" + path;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            string text = new AndroidJavaClass("android.os.Environment").CallStatic<AndroidJavaObject>("getExternalStorageDirectory", new object[0]).Call<string>("getAbsolutePath", new object[0]);
            text += "/Android/data/";
            text += new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity").Call<string>("getPackageName", new object[0]);
            if (Directory.Exists(text))
            {
                if (Directory.Exists(text + "/cache"))
                {
                    Directory.CreateDirectory(text + "/cache");
                }
                path = text + "/cache/" + path;
            }
            else
            {
                path = Application.dataPath;
            }
        }
        Loom.RunAsync(delegate
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (md5)
            {
                name = CacheManager.Md5(name);
            }
            Stream stream = File.Open(path + "/" + name, FileMode.Create);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            if (crypto)
            {
                string customCryptoKey2 = CacheManager.cryptoKey;
                if (!string.IsNullOrEmpty(customCryptoKey))
                {
                    customCryptoKey2 = customCryptoKey;
                }
                CryptoStream cryptoStream = new CryptoStream(stream, new DESCryptoServiceProvider
                {
                    Key = Encoding.ASCII.GetBytes(customCryptoKey2.Substring(0, 8)),
                    IV = Encoding.ASCII.GetBytes(customCryptoKey2.Substring(0, 8))
                }.CreateEncryptor(), CryptoStreamMode.Write);
                binaryFormatter.Serialize(cryptoStream, data);
                cryptoStream.Close();
            }
            else
            {
                binaryFormatter.Serialize(stream, data);
            }
            stream.Close();
        });
    }

    // Token: 0x06002AA1 RID: 10913 RVA: 0x0001DAE3 File Offset: 0x0001BCE3
    public static T Load<T>(string name, string path)
    {
        return CacheManager.Load<T>(name, path, false, false, string.Empty);
    }

    // Token: 0x06002AA2 RID: 10914 RVA: 0x0001DAF3 File Offset: 0x0001BCF3
    public static T Load<T>(string name, string path, bool md5)
    {
        return CacheManager.Load<T>(name, path, md5, false, string.Empty);
    }

    // Token: 0x06002AA3 RID: 10915 RVA: 0x0001DB03 File Offset: 0x0001BD03
    public static T Load<T>(string name, string path, bool md5, bool crypto)
    {
        return CacheManager.Load<T>(name, path, md5, crypto, string.Empty);
    }

    // Token: 0x06002AA4 RID: 10916 RVA: 0x000FDD34 File Offset: 0x000FBF34
    public static T Load<T>(string name, string path, bool md5, bool crypto, string customCryptoKey)
    {
        if (Application.isEditor)
        {
            path = Directory.GetParent(Application.dataPath).FullName + "/Cache/" + path;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            string text = new AndroidJavaClass("android.os.Environment").CallStatic<AndroidJavaObject>("getExternalStorageDirectory", new object[0]).Call<string>("getAbsolutePath", new object[0]);
            text += "/Android/data/";
            text += new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity").Call<string>("getPackageName", new object[0]);
            if (Directory.Exists(text))
            {
                if (Directory.Exists(text + "/cache"))
                {
                    Directory.CreateDirectory(text + "/cache");
                }
                path = text + "/cache/" + path;
            }
            else
            {
                path = Application.dataPath;
            }
        }
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        if (md5)
        {
            name = CacheManager.Md5(name);
        }
        Stream stream = File.Open(path + "/" + name, FileMode.Open);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        object obj;
        if (crypto)
        {
            string text2 = CacheManager.cryptoKey;
            if (!string.IsNullOrEmpty(customCryptoKey))
            {
                text2 = customCryptoKey;
            }
            CryptoStream cryptoStream = new CryptoStream(stream, new DESCryptoServiceProvider
            {
                Key = Encoding.ASCII.GetBytes(text2.Substring(0, 8)),
                IV = Encoding.ASCII.GetBytes(text2.Substring(0, 8))
            }.CreateDecryptor(), CryptoStreamMode.Read);
            obj = binaryFormatter.Deserialize(cryptoStream);
            cryptoStream.Close();
        }
        else
        {
            obj = binaryFormatter.Deserialize(stream);
        }
        stream.Close();
        return (T)((object)obj);
    }

    // Token: 0x06002AA5 RID: 10917 RVA: 0x0001DB13 File Offset: 0x0001BD13
    public static void LoadAsync<T>(Action<T> callback, string name, string path)
    {
        CacheManager.LoadAsync<T>(callback, name, path, false, false, string.Empty);
    }

    // Token: 0x06002AA6 RID: 10918 RVA: 0x0001DB24 File Offset: 0x0001BD24
    public static void LoadAsync<T>(Action<T> callback, string name, string path, bool md5)
    {
        CacheManager.LoadAsync<T>(callback, name, path, md5, false, string.Empty);
    }

    // Token: 0x06002AA7 RID: 10919 RVA: 0x0001DB35 File Offset: 0x0001BD35
    public static void LoadAsync<T>(Action<T> callback, string name, string path, bool md5, bool crypto)
    {
        CacheManager.LoadAsync<T>(callback, name, path, md5, crypto, string.Empty);
    }

    // Token: 0x06002AA8 RID: 10920 RVA: 0x000FDEEC File Offset: 0x000FC0EC
    public static void LoadAsync<T>(Action<T> callback, string name, string path, bool md5, bool crypto, string customCryptoKey)
    {
        if (SystemInfo.processorCount <= 1)
        {
            callback(CacheManager.Load<T>(name, path, md5, crypto, customCryptoKey));
        }
        if (Application.isEditor)
        {
            path = Directory.GetParent(Application.dataPath).FullName + "/Cache/" + path;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            string text = new AndroidJavaClass("android.os.Environment").CallStatic<AndroidJavaObject>("getExternalStorageDirectory", new object[0]).Call<string>("getAbsolutePath", new object[0]);
            text += "/Android/data/";
            text += new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity").Call<string>("getPackageName", new object[0]);
            if (Directory.Exists(text))
            {
                if (Directory.Exists(text + "/cache"))
                {
                    Directory.CreateDirectory(text + "/cache");
                }
                path = text + "/cache/" + path;
            }
            else
            {
                path = Application.dataPath;
            }
        }
        Loom.RunAsync(delegate
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (md5)
            {
                name = CacheManager.Md5(name);
            }
            Stream stream = File.Open(path + "/" + name, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            object value = null;
            if (crypto)
            {
                string customCryptoKey2 = CacheManager.cryptoKey;
                if (!string.IsNullOrEmpty(customCryptoKey))
                {
                    customCryptoKey2 = customCryptoKey;
                }
                CryptoStream cryptoStream = new CryptoStream(stream, new DESCryptoServiceProvider
                {
                    Key = Encoding.ASCII.GetBytes(customCryptoKey2.Substring(0, 8)),
                    IV = Encoding.ASCII.GetBytes(customCryptoKey2.Substring(0, 8))
                }.CreateDecryptor(), CryptoStreamMode.Read);
                value = binaryFormatter.Deserialize(cryptoStream);
                cryptoStream.Close();
            }
            else
            {
                value = binaryFormatter.Deserialize(stream);
            }
            stream.Close();
            Loom.QueueOnMainThread(delegate ()
            {
                callback((T)((object)value));
            });
        });
    }

    // Token: 0x06002AA9 RID: 10921 RVA: 0x000FE06C File Offset: 0x000FC26C
    public static bool Exists(string name, string path, bool md5)
    {
        if (Application.isEditor)
        {
            path = Directory.GetParent(Application.dataPath).FullName + "/Cache/" + path;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            string text = new AndroidJavaClass("android.os.Environment").CallStatic<AndroidJavaObject>("getExternalStorageDirectory", new object[0]).Call<string>("getAbsolutePath", new object[0]);
            text += "/Android/data/";
            text += new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity").Call<string>("getPackageName", new object[0]);
            if (Directory.Exists(text))
            {
                if (Directory.Exists(text + "/cache"))
                {
                    Directory.CreateDirectory(text + "/cache");
                }
                path = text + "/cache/" + path;
            }
            else
            {
                path = Application.dataPath;
            }
        }
        if (md5)
        {
            name = CacheManager.Md5(name);
        }
        return File.Exists(path + "/" + name);
    }

    // Token: 0x06002AAA RID: 10922 RVA: 0x000FE178 File Offset: 0x000FC378
    private static string Md5(string text)
    {
        UTF8Encoding utf8Encoding = new UTF8Encoding();
        byte[] bytes = utf8Encoding.GetBytes(text);
        MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
        byte[] array = md5CryptoServiceProvider.ComputeHash(bytes);
        string text2 = string.Empty;
        for (int i = 0; i < array.Length; i++)
        {
            text2 += Convert.ToString(array[i], 16).PadLeft(2, '0');
        }
        return text2.PadLeft(32, '0');
    }

    // Token: 0x04001B5C RID: 7004
    public static string cryptoKey = "1gf4f4hy";
}
