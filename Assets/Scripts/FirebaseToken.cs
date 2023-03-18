using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

// Token: 0x02000515 RID: 1301
public class FirebaseToken
{
    // Token: 0x06002CB8 RID: 11448 RVA: 0x0001F2D1 File Offset: 0x0001D4D1
    public FirebaseToken(string firebaseSecret)
    {
        this._firebaseSecret = firebaseSecret;
    }

    // Token: 0x06002CBA RID: 11450 RVA: 0x00103E80 File Offset: 0x00102080
    public string CreateToken(string uid)
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            {
                "uid",
                uid
            }
        };
        return this.CreateToken(data);
    }

    // Token: 0x06002CBB RID: 11451 RVA: 0x00103EA8 File Offset: 0x001020A8
    public string CreateToken(Dictionary<string, object> data)
    {
        return this.CreateToken(data, new FirebaseTokenOptions(null, null, false, false));
    }

    // Token: 0x06002CBC RID: 11452 RVA: 0x00103ED8 File Offset: 0x001020D8
    public string CreateToken(Dictionary<string, object> data, FirebaseTokenOptions options)
    {
        bool flag = data == null || data.Count == 0;
        if (flag && (options == null || (!options.admin && !options.debug)))
        {
            throw new Exception("data is empty and no options are set.  This token will have no effect on Firebase.");
        }
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        dictionary["v"] = FirebaseToken.TOKEN_VERSION;
        dictionary["iat"] = FirebaseToken.secondsSinceEpoch(DateTime.Now);
        FirebaseToken.validateToken(data, false);
        if (!flag)
        {
            dictionary["d"] = data;
        }
        if (options != null)
        {
            if (options.expires != null)
            {
                dictionary["exp"] = FirebaseToken.secondsSinceEpoch(options.expires.Value);
            }
            if (options.notBefore != null)
            {
                dictionary["nbf"] = FirebaseToken.secondsSinceEpoch(options.notBefore.Value);
            }
            if (options.admin)
            {
                dictionary["admin"] = true;
            }
            if (options.debug)
            {
                dictionary["debug"] = true;
            }
        }
        string text = this.computeToken(dictionary);
        if (text.Length > 1024)
        {
            throw new Exception("Generated token is too long. The token cannot be longer than 1024 bytes.");
        }
        return text;
    }

    // Token: 0x06002CBD RID: 11453 RVA: 0x0001F2E0 File Offset: 0x0001D4E0
    private string computeToken(Dictionary<string, object> claims)
    {
        return FirebaseToken.Encode(claims, this._firebaseSecret);
    }

    // Token: 0x06002CBE RID: 11454 RVA: 0x00104044 File Offset: 0x00102244
    private static long secondsSinceEpoch(DateTime dt)
    {
        return (long)(dt.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
    }

    // Token: 0x06002CBF RID: 11455 RVA: 0x00104074 File Offset: 0x00102274
    private static void validateToken(Dictionary<string, object> data, bool isAdminToken)
    {
        bool flag = data != null && data.ContainsKey("uid");
        if ((!flag && !isAdminToken) || (flag && !(data["uid"] is string)))
        {
            throw new Exception("Data payload must contain a \"uid\" key that must not be a string.");
        }
        if (flag && data["uid"].ToString().Length > 256)
        {
            throw new Exception("Data payload must contain a \"uid\" key that must not be longer than 256 characters.");
        }
    }

    // Token: 0x06002CC0 RID: 11456 RVA: 0x001040F8 File Offset: 0x001022F8
    private static string Encode(Dictionary<string, object> payload, string key)
    {
        List<string> list = new List<string>();
        Dictionary<string, object> json = new Dictionary<string, object>
        {
            {
                "typ",
                "JWT"
            },
            {
                "alg",
                "HS256"
            }
        };
        byte[] bytes = Encoding.UTF8.GetBytes(FirebaseMiniJson.Serialize(json));
        byte[] bytes2 = Encoding.UTF8.GetBytes(FirebaseMiniJson.Serialize(payload));
        list.Add(FirebaseToken.Base64UrlEncode(bytes));
        list.Add(FirebaseToken.Base64UrlEncode(bytes2));
        string s = string.Join(".", list.ToArray());
        byte[] bytes3 = Encoding.UTF8.GetBytes(s);
        HMACSHA256 hmacsha = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        byte[] input = hmacsha.ComputeHash(bytes3);
        list.Add(FirebaseToken.Base64UrlEncode(input));
        return string.Join(".", list.ToArray());
    }

    // Token: 0x06002CC1 RID: 11457 RVA: 0x001041CC File Offset: 0x001023CC
    private static string Base64UrlEncode(byte[] input)
    {
        string text = Convert.ToBase64String(input);
        text = text.Split(new char[]
        {
            '='
        })[0];
        text = text.Replace('+', '-');
        return text.Replace('/', '_');
    }

    // Token: 0x04001C8A RID: 7306
    private static int TOKEN_VERSION;

    // Token: 0x04001C8B RID: 7307
    private string _firebaseSecret;
}
