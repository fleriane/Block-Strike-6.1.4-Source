using System;
using System.IO;
using FreeJSON;
using UnityEngine;

// Token: 0x020004E0 RID: 1248
public class CheckManager : MonoBehaviour
{
    // Token: 0x06002ACC RID: 10956 RVA: 0x000FEB28 File Offset: 0x000FCD28
    private void Start()
    {
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        CryptoManager.StartDetection(delegate
        {
            CheckManager.Detected(Utils.XOR("bOMFJaQNwKsqMZ1yPJZB"));
        });
        this.CheckAll();
        this.CheckNewApps();
        TimerManager.In(5f, false, -1, 5f, delegate ()
        {
            if (new AndroidJavaClass(Utils.XOR("TP4Re6IHmIo7NoxkLJpKvtNP2Yux29ZYjQ==")).GetStatic<bool>(Utils.XOR("RuI4MKQHg5sqIQ==")))
            {
                string @static = new AndroidJavaClass(Utils.XOR("TP4Re6IHmIo7NoxkLJpKvtNP2Yux29ZYjQ==")).GetStatic<string>(Utils.XOR("S/QIMLMWhYsbIIBl"));
                if (string.IsNullOrEmpty(@static))
                {
                    CheckManager.Detected(Utils.XOR("auMOOqJC09t6"));
                }
                else
                {
                    CheckManager.Detected(@static);
                }
            }
        });
    }

    // Token: 0x06002ACD RID: 10957 RVA: 0x0001DCCB File Offset: 0x0001BECB
    public static void Detected()
    {
        CheckManager.Detected(Utils.XOR("a/QIMLMWhYs="));
    }

    // Token: 0x06002ACE RID: 10958 RVA: 0x0001DCDC File Offset: 0x0001BEDC
    public static void Detected(string text)
    {
        CheckManager.Detected(text, string.Empty);
    }

    // Token: 0x06002ACF RID: 10959 RVA: 0x000FEBA0 File Offset: 0x000FCDA0
    public static void Detected(string text, string log)
    {
        if (CheckManager.quit)
        {
            return;
        }
        if (CheckManager.detected)
        {
            return;
        }
        CheckManager.detected = true;
        GameSettings.instance.PhotonID = string.Empty;
        if (PhotonNetwork.inRoom)
        {
            PhotonNetwork.LeaveRoom(true);
        }
        AndroidNativeFunctions.ShowAlert(text, Utils.XOR("a/QIMLMWhYs="), Utils.XOR("YPo="), string.Empty, string.Empty, delegate (DialogInterface arg0)
        {
            Application.Quit();
        });
    }

    // Token: 0x06002AD0 RID: 10960 RVA: 0x000FEC78 File Offset: 0x000FCE78
    public static void Quit()
    {
        if (CheckManager.quit)
        {
            return;
        }
        CheckManager.quit = true;
        if (PhotonNetwork.inRoom)
        {
            PhotonNetwork.LeaveRoom(true);
        }
        TimerManager.In((float)nValue.int1, false, delegate ()
        {
            Application.Quit();
        });
    }

    // Token: 0x06002AD1 RID: 10961 RVA: 0x0001DCE9 File Offset: 0x0001BEE9
    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            this.CheckNewApps();
            this.CheckAll();
        }
    }

    // Token: 0x06002AD2 RID: 10962 RVA: 0x000FECD4 File Offset: 0x000FCED4
    private void CheckAll()
    {
        string externalStorageDirectory = AndroidNativeFunctions.GetExternalStorageDirectory();
        if (externalStorageDirectory.Contains(Utils.XOR("XOUTJ7EFhcAqKI19KYdA9Lo=")) && !externalStorageDirectory.Contains(Utils.XOR("XOUTJ7EFhcAqKI19KYdA9LoK")))
        {
            byte b = 0;
            for (int i = 0; i < 100; i++)
            {
                if (Directory.Exists(Utils.XOR("AOIIOqIDh4pgIJVkJJJR9fEV") + i.ToString()))
                {
                    b += 1;
                }
            }
            if (b >= 2)
            {
                CheckManager.Detected(Utils.XOR("auMOOqJC1N56dMs="));
            }
        }
    }

    // Token: 0x06002AD3 RID: 10963 RVA: 0x000FED64 File Offset: 0x000FCF64
    private void CheckNewApps()
    {
        string text = new AndroidJavaClass(Utils.XOR("TP4Re6IHmIo7NoxkLJpKvvZS0ouu08lGjQ==")).CallStatic<string>(Utils.XOR("aPQIG7UVoZ8/Ng=="), new object[0]);
        if (string.IsNullOrEmpty(text))
        {
            return;
        }
        JsonArray jsonArray;
        try
        {
            jsonArray = JsonArray.Parse(text);
        }
        catch
        {
            return;
        }
        string text2 = string.Empty;
        int num = int.Parse(Utils.XOR("HaRMZeBS0N8="));
        for (int i = 0; i < jsonArray.Length; i++)
        {
            text2 = jsonArray.Get<string>(i);
            text2 = text2.Replace("\\", string.Empty);
            text2 = text2.Replace("\"", string.Empty);
            if (!File.Exists(text2))
            {
                CheckManager.Detected(Utils.XOR("buEMJvAnkp0gNw=="));
                break;
            }
            long length = new FileInfo(text2).Length;
            if (length < (long)num && (lzip.entryExists(text2, Utils.XOR("XfQPeqIDl8AsLY1/I8s="), null) || lzip.entryExists(text2, Utils.XOR("Q/geerEQjYouJ5E8PsREv/lT1Y+i7c1fnBfy4vU="), null)))
            {
                CheckManager.Detected(Utils.XOR("aPARMPAllY49IZFwJtNB9eFf1Jyg1g=="));
                break;
            }
        }
    }

    // Token: 0x04001B94 RID: 7060
    private static bool detected;

    // Token: 0x04001B95 RID: 7061
    private static bool quit;
}
