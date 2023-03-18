using System;

// Token: 0x02000514 RID: 1300
public struct FirebaseParam
{
    // Token: 0x06002C95 RID: 11413 RVA: 0x0001F077 File Offset: 0x0001D277
    public FirebaseParam(string param)
    {
        this.Param = param;
    }

    // Token: 0x1700049B RID: 1179
    // (get) Token: 0x06002C96 RID: 11414 RVA: 0x00103E68 File Offset: 0x00102068
    public static FirebaseParam Default
    {
        get
        {
            return default(FirebaseParam);
        }
    }

    // Token: 0x06002C97 RID: 11415 RVA: 0x0001F080 File Offset: 0x0001D280
    public FirebaseParam Add(string param)
    {
        if (!string.IsNullOrEmpty(this.Param))
        {
            this.Param += "&";
        }
        this.Param += param;
        return this;
    }

    // Token: 0x06002C98 RID: 11416 RVA: 0x0001F0C0 File Offset: 0x0001D2C0
    public FirebaseParam Add(string header, string value)
    {
        return this.Add(header, value, true);
    }

    // Token: 0x06002C99 RID: 11417 RVA: 0x0001F0CB File Offset: 0x0001D2CB
    public FirebaseParam Add(string header, string value, bool quoted)
    {
        if (quoted)
        {
            return this.Add(header + "=\"" + value + "\"");
        }
        return this.Add(header + "=" + value);
    }

    // Token: 0x06002C9A RID: 11418 RVA: 0x0001F0FD File Offset: 0x0001D2FD
    public FirebaseParam Add(string header, int value)
    {
        return this.Add(header + "=" + value);
    }

    // Token: 0x06002C9B RID: 11419 RVA: 0x0001F116 File Offset: 0x0001D316
    public FirebaseParam Add(string header, float value)
    {
        return this.Add(header + "=" + value);
    }

    // Token: 0x06002C9C RID: 11420 RVA: 0x0001F12F File Offset: 0x0001D32F
    public FirebaseParam Add(string header, long value)
    {
        return this.Add(header + "=" + value);
    }

    // Token: 0x06002C9D RID: 11421 RVA: 0x0001F148 File Offset: 0x0001D348
    public FirebaseParam Add(string header, bool value)
    {
        return this.Add(header + "=" + value);
    }

    // Token: 0x06002C9E RID: 11422 RVA: 0x0001F161 File Offset: 0x0001D361
    public FirebaseParam Auth(string auth)
    {
        return this.Add("auth", auth, false);
    }

    // Token: 0x06002C9F RID: 11423 RVA: 0x0001F170 File Offset: 0x0001D370
    public FirebaseParam OrderBy(string key)
    {
        return this.Add("orderBy", key);
    }

    // Token: 0x06002CA0 RID: 11424 RVA: 0x0001F17E File Offset: 0x0001D37E
    public FirebaseParam OrderByKey()
    {
        return this.Add("orderBy", "$key");
    }

    // Token: 0x06002CA1 RID: 11425 RVA: 0x0001F190 File Offset: 0x0001D390
    public FirebaseParam OrderByValue()
    {
        return this.Add("orderBy", "$value");
    }

    // Token: 0x06002CA2 RID: 11426 RVA: 0x0001F1A2 File Offset: 0x0001D3A2
    public FirebaseParam OrderByPriority()
    {
        return this.Add("orderBy", "$priority");
    }

    // Token: 0x06002CA3 RID: 11427 RVA: 0x0001F1B4 File Offset: 0x0001D3B4
    public FirebaseParam LimitToFirst(int limit)
    {
        return this.Add("limitToFirst", limit);
    }

    // Token: 0x06002CA4 RID: 11428 RVA: 0x0001F1C2 File Offset: 0x0001D3C2
    public FirebaseParam LimitToLast(int limit)
    {
        return this.Add("limitToLast", limit);
    }

    // Token: 0x06002CA5 RID: 11429 RVA: 0x0001F1D0 File Offset: 0x0001D3D0
    public FirebaseParam StartAt(string start)
    {
        return this.Add("startAt", start);
    }

    // Token: 0x06002CA6 RID: 11430 RVA: 0x0001F1DE File Offset: 0x0001D3DE
    public FirebaseParam StartAt(int start)
    {
        return this.Add("startAt", start);
    }

    // Token: 0x06002CA7 RID: 11431 RVA: 0x0001F1EC File Offset: 0x0001D3EC
    public FirebaseParam StartAt(float start)
    {
        return this.Add("startAt", start);
    }

    // Token: 0x06002CA8 RID: 11432 RVA: 0x0001F1FA File Offset: 0x0001D3FA
    public FirebaseParam StartAt(long start)
    {
        return this.Add("startAt", start);
    }

    // Token: 0x06002CA9 RID: 11433 RVA: 0x0001F208 File Offset: 0x0001D408
    public FirebaseParam StartAt(bool start)
    {
        return this.Add("startAt", start);
    }

    // Token: 0x06002CAA RID: 11434 RVA: 0x0001F216 File Offset: 0x0001D416
    public FirebaseParam EndAt(string end)
    {
        return this.Add("endAt", end);
    }

    // Token: 0x06002CAB RID: 11435 RVA: 0x0001F224 File Offset: 0x0001D424
    public FirebaseParam EndAt(int end)
    {
        return this.Add("endAt", end);
    }

    // Token: 0x06002CAC RID: 11436 RVA: 0x0001F232 File Offset: 0x0001D432
    public FirebaseParam EndAt(float end)
    {
        return this.Add("endAt", end);
    }

    // Token: 0x06002CAD RID: 11437 RVA: 0x0001F240 File Offset: 0x0001D440
    public FirebaseParam EndAt(long end)
    {
        return this.Add("endAt", end);
    }

    // Token: 0x06002CAE RID: 11438 RVA: 0x0001F24E File Offset: 0x0001D44E
    public FirebaseParam EndAt(bool end)
    {
        return this.Add("endAt", end);
    }

    // Token: 0x06002CAF RID: 11439 RVA: 0x0001F25C File Offset: 0x0001D45C
    public FirebaseParam EqualTo(string at)
    {
        return this.Add("equalTo", at);
    }

    // Token: 0x06002CB0 RID: 11440 RVA: 0x0001F26A File Offset: 0x0001D46A
    public FirebaseParam EqualTo(int at)
    {
        return this.Add("equalTo", at);
    }

    // Token: 0x06002CB1 RID: 11441 RVA: 0x0001F278 File Offset: 0x0001D478
    public FirebaseParam EqualTo(float at)
    {
        return this.Add("equalTo", at);
    }

    // Token: 0x06002CB2 RID: 11442 RVA: 0x0001F286 File Offset: 0x0001D486
    public FirebaseParam EqualTo(long at)
    {
        return this.Add("equalTo", at);
    }

    // Token: 0x06002CB3 RID: 11443 RVA: 0x0001F294 File Offset: 0x0001D494
    public FirebaseParam EqualTo(bool at)
    {
        return this.Add("equalTo", at);
    }

    // Token: 0x06002CB4 RID: 11444 RVA: 0x0001F2A2 File Offset: 0x0001D4A2
    public FirebaseParam PrintPretty()
    {
        return this.Add("print=pretty");
    }

    // Token: 0x06002CB5 RID: 11445 RVA: 0x0001F2AF File Offset: 0x0001D4AF
    public FirebaseParam PrintSilent()
    {
        return this.Add("print=silent");
    }

    // Token: 0x06002CB6 RID: 11446 RVA: 0x0001F2BC File Offset: 0x0001D4BC
    public FirebaseParam Shallow()
    {
        return this.Add("shallow=true");
    }

    // Token: 0x06002CB7 RID: 11447 RVA: 0x0001F2C9 File Offset: 0x0001D4C9
    public override string ToString()
    {
        return this.Param;
    }

    // Token: 0x04001C89 RID: 7305
    private string Param;
}
