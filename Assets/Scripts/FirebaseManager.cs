using System;
using UnityEngine;

// Token: 0x0200050D RID: 1293
public class FirebaseManager : MonoBehaviour
{
    // Token: 0x1700048F RID: 1167
    // (get) Token: 0x06002C4F RID: 11343 RVA: 0x001034CC File Offset: 0x001016CC
    public static FirebaseManager Instance
    {
        get
        {
            if (FirebaseManager.instance == null)
            {
                GameObject gameObject = new GameObject("Firebase");
                FirebaseManager.instance = gameObject.AddComponent<FirebaseManager>();
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
            }
            return FirebaseManager.instance;
        }
    }

    // Token: 0x04001C5A RID: 7258
    public static bool DebugAction;

    // Token: 0x04001C5B RID: 7259
    private static FirebaseManager instance;
}
