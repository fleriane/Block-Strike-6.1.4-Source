using System;

// Token: 0x02000516 RID: 1302
public class FirebaseTokenOptions
{
    // Token: 0x06002CC2 RID: 11458 RVA: 0x0001F2EE File Offset: 0x0001D4EE
    public FirebaseTokenOptions(DateTime? notBefore = null, DateTime? expires = null, bool admin = false, bool debug = false)
    {
        this.notBefore = notBefore;
        this.expires = expires;
        this.admin = admin;
        this.debug = debug;
    }

    // Token: 0x1700049C RID: 1180
    // (get) Token: 0x06002CC3 RID: 11459 RVA: 0x0001F313 File Offset: 0x0001D513
    // (set) Token: 0x06002CC4 RID: 11460 RVA: 0x0001F31B File Offset: 0x0001D51B
    public DateTime? expires { get; private set; }

    // Token: 0x1700049D RID: 1181
    // (get) Token: 0x06002CC5 RID: 11461 RVA: 0x0001F324 File Offset: 0x0001D524
    // (set) Token: 0x06002CC6 RID: 11462 RVA: 0x0001F32C File Offset: 0x0001D52C
    public DateTime? notBefore { get; private set; }

    // Token: 0x1700049E RID: 1182
    // (get) Token: 0x06002CC7 RID: 11463 RVA: 0x0001F335 File Offset: 0x0001D535
    // (set) Token: 0x06002CC8 RID: 11464 RVA: 0x0001F33D File Offset: 0x0001D53D
    public bool admin { get; private set; }

    // Token: 0x1700049F RID: 1183
    // (get) Token: 0x06002CC9 RID: 11465 RVA: 0x0001F346 File Offset: 0x0001D546
    // (set) Token: 0x06002CCA RID: 11466 RVA: 0x0001F34E File Offset: 0x0001D54E
    public bool debug { get; private set; }
}
