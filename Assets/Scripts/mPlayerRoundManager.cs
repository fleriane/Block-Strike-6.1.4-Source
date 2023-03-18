using System;
using UnityEngine;

// Token: 0x020004A6 RID: 1190
public class mPlayerRoundManager : MonoBehaviour
{
    // Token: 0x0600294A RID: 10570 RVA: 0x0001CDB1 File Offset: 0x0001AFB1
    private void Start()
    {
        mPlayerRoundManager.instance = this;
    }

    // Token: 0x0600294B RID: 10571 RVA: 0x000F4F70 File Offset: 0x000F3170
    public static void Show()
    {
        mPlayerRoundManager.instance.panel.SetActive(true);
        mPlayerRoundManager.instance.modeLabel.text = Localization.Get(PlayerRoundManager.GetMode().ToString(), true);
        mPlayerRoundManager.instance.moneyLabel.text = PlayerRoundManager.GetMoney().ToString();
        mPlayerRoundManager.instance.xpLabel.text = PlayerRoundManager.GetXP().ToString();
        mPlayerRoundManager.instance.killsLabel.text = PlayerRoundManager.GetKills().ToString();
        mPlayerRoundManager.instance.headshotLabel.text = PlayerRoundManager.GetHeadshot().ToString();
        mPlayerRoundManager.instance.deathsLabel.text = PlayerRoundManager.GetDeaths().ToString();
        mPlayerRoundManager.instance.timeLabel.text = mPlayerRoundManager.ConvertTime(PlayerRoundManager.GetTime());
    }

    // Token: 0x0600294C RID: 10572 RVA: 0x0001CDB9 File Offset: 0x0001AFB9
    public void Close()
    {
        this.panel.SetActive(false);
        PlayerRoundManager.Clear();
        EventManager.Dispatch("AccountUpdate");
    }

    // Token: 0x0600294D RID: 10573 RVA: 0x000F5058 File Offset: 0x000F3258
    private static string ConvertTime(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds((double)time);
        return string.Format("{0:0}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    // Token: 0x04001A5D RID: 6749
    public GameObject panel;

    // Token: 0x04001A5E RID: 6750
    public UILabel modeLabel;

    // Token: 0x04001A5F RID: 6751
    public UILabel moneyLabel;

    // Token: 0x04001A60 RID: 6752
    public UILabel xpLabel;

    // Token: 0x04001A61 RID: 6753
    public UILabel killsLabel;

    // Token: 0x04001A62 RID: 6754
    public UILabel headshotLabel;

    // Token: 0x04001A63 RID: 6755
    public UILabel deathsLabel;

    // Token: 0x04001A64 RID: 6756
    public UILabel timeLabel;

    // Token: 0x04001A65 RID: 6757
    private static mPlayerRoundManager instance;
}
