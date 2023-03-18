using System;
using UnityEngine;

// Token: 0x020004A7 RID: 1191
public class mPlayerStats : MonoBehaviour
{
    // Token: 0x0600294F RID: 10575 RVA: 0x000F509C File Offset: 0x000F329C
    public void Open()
    {
        if (!AccountManager.isConnect)
        {
            UIToast.Show(Localization.Get("Connection account", true));
            return;
        }
        this.Panel.SetActive(true);
        this.avatarTexture.mainTexture = AccountManager.instance.Data.Avatar;
        this.NameLabel.text = AccountManager.instance.Data.AccountName;
        this.IDLabel.text = "ID: " + AccountManager.instance.Data.ID.ToString();
        this.LevelLabel.text = AccountManager.GetLevel().ToString();
        this.XPLabel.text = AccountManager.GetXP() + "/" + AccountManager.GetMaxXP();
        this.DeathsLabel.text = AccountManager.GetDeaths().ToString();
        this.KillsLabel.text = AccountManager.GetKills().ToString();
        this.HeadshotKillsLabel.text = AccountManager.GetHeadshot().ToString();
        this.TimeLabel.text = Localization.Get("Time in the game", true) + ": " + this.ConvertTime(AccountManager.instance.Data.Time);
        this.TotalSkinLabel.text = this.GetOpenSkins(WeaponSkinQuality.Default) + "/" + this.GetTotalSkins(WeaponSkinQuality.Default);
        this.MoneyLabel.text = AccountManager.GetMoney().ToString("n0");
        this.GoldLabel.text = AccountManager.GetGold().ToString("n0");
        this.LegendarySkinLabel.text = this.GetOpenSkins(WeaponSkinQuality.Legendary) + "/" + this.GetTotalSkins(WeaponSkinQuality.Legendary);
        this.ProfessionalSkinLabel.text = this.GetOpenSkins(WeaponSkinQuality.Professional) + "/" + this.GetTotalSkins(WeaponSkinQuality.Professional);
        this.BasicSkinLabel.text = this.GetOpenSkins(WeaponSkinQuality.Basic) + "/" + this.GetTotalSkins(WeaponSkinQuality.Basic);
        this.NormalSkinLabel.text = this.GetOpenSkins(WeaponSkinQuality.Normal) + "/" + this.GetTotalSkins(WeaponSkinQuality.Normal);
    }

    // Token: 0x06002950 RID: 10576 RVA: 0x000F5310 File Offset: 0x000F3510
    private int GetOpenSkins(WeaponSkinQuality quality)
    {
        int num = 0;
        for (int i = 0; i < GameSettings.instance.Weapons.Count; i++)
        {
            for (int j = 1; j < GameSettings.instance.WeaponsStore[i].Skins.Count; j++)
            {
                if ((GameSettings.instance.WeaponsStore[i].Skins[j].Quality == quality || quality == WeaponSkinQuality.Default) && AccountManager.GetWeaponSkin(i + 1, j))
                {
                    num++;
                }
            }
        }
        return num;
    }

    // Token: 0x06002951 RID: 10577 RVA: 0x000F53AC File Offset: 0x000F35AC
    private int GetTotalSkins(WeaponSkinQuality quality)
    {
        int num = 0;
        for (int i = 0; i < GameSettings.instance.Weapons.Count; i++)
        {
            for (int j = 1; j < GameSettings.instance.WeaponsStore[i].Skins.Count; j++)
            {
                if (GameSettings.instance.WeaponsStore[i].Skins[j].Quality == quality || quality == WeaponSkinQuality.Default)
                {
                    num++;
                }
            }
        }
        return num;
    }

    // Token: 0x06002952 RID: 10578 RVA: 0x000EAEE0 File Offset: 0x000E90E0
    private string ConvertTime(long time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds((double)time);
        if (timeSpan.Days * 24 + timeSpan.Hours > 0)
        {
            return string.Concat(new object[]
            {
                timeSpan.Days * 24 + timeSpan.Hours,
                " ",
                Localization.Get("h", true),
                "."
            });
        }
        if (timeSpan.Minutes > 0)
        {
            return string.Concat(new object[]
            {
                timeSpan.Minutes,
                " ",
                Localization.Get("m", true),
                "."
            });
        }
        return string.Concat(new object[]
        {
            timeSpan.Seconds,
            " ",
            Localization.Get("s", true),
            "."
        });
    }

    // Token: 0x04001A66 RID: 6758
    public UITexture avatarTexture;

    // Token: 0x04001A67 RID: 6759
    public UILabel NameLabel;

    // Token: 0x04001A68 RID: 6760
    public UILabel IDLabel;

    // Token: 0x04001A69 RID: 6761
    public UILabel LevelLabel;

    // Token: 0x04001A6A RID: 6762
    public UILabel XPLabel;

    // Token: 0x04001A6B RID: 6763
    public UILabel DeathsLabel;

    // Token: 0x04001A6C RID: 6764
    public UILabel KillsLabel;

    // Token: 0x04001A6D RID: 6765
    public UILabel HeadshotKillsLabel;

    // Token: 0x04001A6E RID: 6766
    public UILabel TimeLabel;

    // Token: 0x04001A6F RID: 6767
    public UILabel TotalSkinLabel;

    // Token: 0x04001A70 RID: 6768
    public UILabel LegendarySkinLabel;

    // Token: 0x04001A71 RID: 6769
    public UILabel ProfessionalSkinLabel;

    // Token: 0x04001A72 RID: 6770
    public UILabel BasicSkinLabel;

    // Token: 0x04001A73 RID: 6771
    public UILabel NormalSkinLabel;

    // Token: 0x04001A74 RID: 6772
    public UILabel MoneyLabel;

    // Token: 0x04001A75 RID: 6773
    public UILabel GoldLabel;

    // Token: 0x04001A76 RID: 6774
    public GameObject Panel;
}
