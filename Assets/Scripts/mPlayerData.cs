using System;
using UnityEngine;

// Token: 0x020004A5 RID: 1189
public class mPlayerData : MonoBehaviour
{
    // Token: 0x06002946 RID: 10566 RVA: 0x0001CD59 File Offset: 0x0001AF59
    private void Start()
    {
        EventManager.AddListener("AccountUpdate", new EventManager.Callback(this.AccountUpdate));
        EventManager.AddListener("AvatarUpdate", new EventManager.Callback(this.AvatarUpdate));
        this.AccountUpdate();
        this.AvatarUpdate();
    }

    // Token: 0x06002947 RID: 10567 RVA: 0x000F4E64 File Offset: 0x000F3064
    private void AccountUpdate()
    {
        if (string.IsNullOrEmpty(AccountManager.instance.Data.Clan.ToString()))
        {
            this.PlayerNameLabel.text = AccountManager.instance.Data.AccountName;
        }
        else
        {
            this.PlayerNameLabel.text = AccountManager.instance.Data.AccountName + " - " + AccountManager.instance.Data.Clan;
        }
        this.PlayerLevelLabel.text = Localization.Get("Level", true) + " - " + AccountManager.GetLevel().ToString();
        this.PlayerXP.value = (float)AccountManager.GetXP() / (float)AccountManager.GetMaxXP();
        this.GoldLabel.text = AccountManager.GetGold().ToString("n0");
        this.MoneyLabel.text = AccountManager.GetMoney().ToString("n0");
    }

    // Token: 0x06002948 RID: 10568 RVA: 0x0001CD95 File Offset: 0x0001AF95
    private void AvatarUpdate()
    {
        this.AvatarTexture.mainTexture = AccountManager.instance.Data.Avatar;
    }

    // Token: 0x04001A57 RID: 6743
    public UILabel PlayerNameLabel;

    // Token: 0x04001A58 RID: 6744
    public UILabel PlayerLevelLabel;

    // Token: 0x04001A59 RID: 6745
    public UIProgressBar PlayerXP;

    // Token: 0x04001A5A RID: 6746
    public UILabel MoneyLabel;

    // Token: 0x04001A5B RID: 6747
    public UILabel GoldLabel;

    // Token: 0x04001A5C RID: 6748
    public UITexture AvatarTexture;
}
