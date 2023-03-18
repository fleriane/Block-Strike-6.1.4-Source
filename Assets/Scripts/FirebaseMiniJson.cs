using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

// Token: 0x02000517 RID: 1303
public static class FirebaseMiniJson
{
    // Token: 0x06002CCC RID: 11468 RVA: 0x0010420C File Offset: 0x0010240C
    public static object Deserialize(string json)
    {
        FirebaseMiniJson.lastDecode = json;
        if (json != null)
        {
            char[] json2 = json.ToCharArray();
            int num = 0;
            bool flag = true;
            object result = FirebaseMiniJson.ParseValue(json2, ref num, ref flag);
            if (flag)
            {
                FirebaseMiniJson.lastErrorIndex = -1;
            }
            else
            {
                FirebaseMiniJson.lastErrorIndex = num;
            }
            return result;
        }
        return null;
    }

    // Token: 0x06002CCD RID: 11469 RVA: 0x00104258 File Offset: 0x00102458
    public static string Serialize(object json)
    {
        StringBuilder stringBuilder = new StringBuilder(2000);
        bool flag = FirebaseMiniJson.SerializeValue(json, stringBuilder);
        return (!flag) ? null : stringBuilder.ToString();
    }

    // Token: 0x06002CCE RID: 11470 RVA: 0x0001F35F File Offset: 0x0001D55F
    public static bool LastDecodeSuccessful()
    {
        return FirebaseMiniJson.lastErrorIndex == -1;
    }

    // Token: 0x06002CCF RID: 11471 RVA: 0x0001F369 File Offset: 0x0001D569
    public static int GetLastErrorIndex()
    {
        return FirebaseMiniJson.lastErrorIndex;
    }

    // Token: 0x06002CD0 RID: 11472 RVA: 0x0010428C File Offset: 0x0010248C
    public static string GetLastErrorSnippet()
    {
        if (FirebaseMiniJson.lastErrorIndex == -1)
        {
            return string.Empty;
        }
        int num = FirebaseMiniJson.lastErrorIndex - 5;
        int num2 = FirebaseMiniJson.lastErrorIndex + 15;
        if (num < 0)
        {
            num = 0;
        }
        if (num2 >= FirebaseMiniJson.lastDecode.Length)
        {
            num2 = FirebaseMiniJson.lastDecode.Length - 1;
        }
        return FirebaseMiniJson.lastDecode.Substring(num, num2 - num + 1);
    }

    // Token: 0x06002CD1 RID: 11473 RVA: 0x001042F4 File Offset: 0x001024F4
    private static Dictionary<string, object> ParseObject(char[] json, ref int index)
    {
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        FirebaseMiniJson.NextToken(json, ref index);
        for (; ; )
        {
            FirebaseMiniJson.TOKEN token = FirebaseMiniJson.LookAhead(json, index);
            if (token == FirebaseMiniJson.TOKEN.NONE)
            {
                break;
            }
            if (token == FirebaseMiniJson.TOKEN.COMMA)
            {
                FirebaseMiniJson.NextToken(json, ref index);
            }
            else
            {
                if (token == FirebaseMiniJson.TOKEN.CURLY_CLOSE)
                {
                    goto Block_3;
                }
                string text = FirebaseMiniJson.ParseString(json, ref index);
                if (text == null)
                {
                    goto Block_4;
                }
                token = FirebaseMiniJson.NextToken(json, ref index);
                if (token != FirebaseMiniJson.TOKEN.COLON)
                {
                    goto Block_5;
                }
                bool flag = true;
                object value = FirebaseMiniJson.ParseValue(json, ref index, ref flag);
                if (!flag)
                {
                    goto Block_6;
                }
                dictionary[text] = value;
            }
        }
        return null;
    Block_3:
        FirebaseMiniJson.NextToken(json, ref index);
        return dictionary;
    Block_4:
        return null;
    Block_5:
        return null;
    Block_6:
        return null;
    }

    // Token: 0x06002CD2 RID: 11474 RVA: 0x00104388 File Offset: 0x00102588
    private static List<object> ParseArray(char[] json, ref int index)
    {
        List<object> list = new List<object>();
        FirebaseMiniJson.NextToken(json, ref index);
        for (; ; )
        {
            FirebaseMiniJson.TOKEN token = FirebaseMiniJson.LookAhead(json, index);
            if (token == FirebaseMiniJson.TOKEN.NONE)
            {
                break;
            }
            if (token == FirebaseMiniJson.TOKEN.COMMA)
            {
                FirebaseMiniJson.NextToken(json, ref index);
            }
            else
            {
                if (token == FirebaseMiniJson.TOKEN.SQUARED_CLOSE)
                {
                    goto Block_3;
                }
                bool flag = true;
                object item = FirebaseMiniJson.ParseValue(json, ref index, ref flag);
                if (!flag)
                {
                    goto Block_4;
                }
                list.Add(item);
            }
        }
        return null;
    Block_3:
        FirebaseMiniJson.NextToken(json, ref index);
        return list;
    Block_4:
        return null;
    }

    // Token: 0x06002CD3 RID: 11475 RVA: 0x00104400 File Offset: 0x00102600
    private static object ParseValue(char[] json, ref int index, ref bool success)
    {
        switch (FirebaseMiniJson.LookAhead(json, index))
        {
            case FirebaseMiniJson.TOKEN.CURLY_OPEN:
                return FirebaseMiniJson.ParseObject(json, ref index);
            case FirebaseMiniJson.TOKEN.SQUARED_OPEN:
                return FirebaseMiniJson.ParseArray(json, ref index);
            case FirebaseMiniJson.TOKEN.STRING:
                return FirebaseMiniJson.ParseString(json, ref index);
            case FirebaseMiniJson.TOKEN.NUMBER:
                return FirebaseMiniJson.ParseNumber(json, ref index);
            case FirebaseMiniJson.TOKEN.TRUE:
                FirebaseMiniJson.NextToken(json, ref index);
                return true;
            case FirebaseMiniJson.TOKEN.FALSE:
                FirebaseMiniJson.NextToken(json, ref index);
                return false;
            case FirebaseMiniJson.TOKEN.NULL:
                FirebaseMiniJson.NextToken(json, ref index);
                return null;
        }
        success = false;
        return null;
    }

    // Token: 0x06002CD4 RID: 11476 RVA: 0x001044A4 File Offset: 0x001026A4
    private static string ParseString(char[] json, ref int index)
    {
        StringBuilder stringBuilder = new StringBuilder();
        FirebaseMiniJson.EatWhitespace(json, ref index);
        char c = json[index++];
        bool flag = false;
        while (!flag)
        {
            if (index == json.Length)
            {
                break;
            }
            c = json[index++];
            if (c == '"')
            {
                flag = true;
                break;
            }
            if (c == '\\')
            {
                if (index == json.Length)
                {
                    break;
                }
                c = json[index++];
                if (c == '"')
                {
                    stringBuilder.Append('"');
                }
                else if (c == '\\')
                {
                    stringBuilder.Append('\\');
                }
                else if (c == '/')
                {
                    stringBuilder.Append('/');
                }
                else if (c == 'b')
                {
                    stringBuilder.Append('\b');
                }
                else if (c == 'f')
                {
                    stringBuilder.Append('\f');
                }
                else if (c == 'n')
                {
                    stringBuilder.Append('\n');
                }
                else if (c == 'r')
                {
                    stringBuilder.Append('\r');
                }
                else if (c == 't')
                {
                    stringBuilder.Append('\t');
                }
                else if (c == 'u')
                {
                    int num = json.Length - index;
                    if (num < 4)
                    {
                        break;
                    }
                    char[] array = new char[4];
                    Array.Copy(json, index, array, 0, 4);
                    stringBuilder.AppendFormat(string.Format("&#x{0};", array), new object[0]);
                    index += 4;
                }
            }
            else
            {
                stringBuilder.Append(c);
            }
        }
        if (!flag)
        {
            return null;
        }
        return stringBuilder.ToString();
    }

    // Token: 0x06002CD5 RID: 11477 RVA: 0x00104648 File Offset: 0x00102848
    private static object ParseNumber(char[] json, ref int index)
    {
        FirebaseMiniJson.EatWhitespace(json, ref index);
        int lastIndexOfNumber = FirebaseMiniJson.GetLastIndexOfNumber(json, index);
        int num = lastIndexOfNumber - index + 1;
        char[] array = new char[num];
        Array.Copy(json, index, array, 0, num);
        index = lastIndexOfNumber + 1;
        string text = new string(array);
        if (text.IndexOf('.') == -1)
        {
            return long.Parse(text);
        }
        return double.Parse(text);
    }

    // Token: 0x06002CD6 RID: 11478 RVA: 0x0006CD28 File Offset: 0x0006AF28
    private static int GetLastIndexOfNumber(char[] json, int index)
    {
        int i;
        for (i = index; i < json.Length; i++)
        {
            if ("0123456789+-.eE".IndexOf(json[i]) == -1)
            {
                break;
            }
        }
        return i - 1;
    }

    // Token: 0x06002CD7 RID: 11479 RVA: 0x0000E4AF File Offset: 0x0000C6AF
    private static void EatWhitespace(char[] json, ref int index)
    {
        while (index < json.Length)
        {
            if (" \t\n\r".IndexOf(json[index]) == -1)
            {
                break;
            }
            index++;
        }
    }

    // Token: 0x06002CD8 RID: 11480 RVA: 0x001046B0 File Offset: 0x001028B0
    private static FirebaseMiniJson.TOKEN LookAhead(char[] json, int index)
    {
        int num = index;
        return FirebaseMiniJson.NextToken(json, ref num);
    }

    // Token: 0x06002CD9 RID: 11481 RVA: 0x001046C8 File Offset: 0x001028C8
    private static FirebaseMiniJson.TOKEN NextToken(char[] json, ref int index)
    {
        FirebaseMiniJson.EatWhitespace(json, ref index);
        if (index == json.Length)
        {
            return FirebaseMiniJson.TOKEN.NONE;
        }
        char c = json[index];
        index++;
        char c2 = c;
        switch (c2)
        {
            case '"':
                return FirebaseMiniJson.TOKEN.STRING;
            default:
                switch (c2)
                {
                    case '[':
                        return FirebaseMiniJson.TOKEN.SQUARED_OPEN;
                    default:
                        {
                            switch (c2)
                            {
                                case '{':
                                    return FirebaseMiniJson.TOKEN.CURLY_OPEN;
                                case '}':
                                    return FirebaseMiniJson.TOKEN.CURLY_CLOSE;
                            }
                            index--;
                            int num = json.Length - index;
                            if (num >= 5 && json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
                            {
                                index += 5;
                                return FirebaseMiniJson.TOKEN.FALSE;
                            }
                            if (num >= 4)
                            {
                                if (json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
                                {
                                    index += 4;
                                    return FirebaseMiniJson.TOKEN.TRUE;
                                }
                                if (json[index] == 'n' && json[index + 1] == 'u' && json[index + 2] == 'l' && json[index + 3] == 'l')
                                {
                                    index += 4;
                                    return FirebaseMiniJson.TOKEN.NULL;
                                }
                            }
                            return FirebaseMiniJson.TOKEN.NONE;
                        }
                    case ']':
                        return FirebaseMiniJson.TOKEN.SQUARED_CLOSE;
                }
                break;
            case ',':
                return FirebaseMiniJson.TOKEN.COMMA;
            case '-':
            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
                return FirebaseMiniJson.TOKEN.NUMBER;
            case ':':
                return FirebaseMiniJson.TOKEN.COLON;
        }
    }

    // Token: 0x06002CDA RID: 11482 RVA: 0x0010487C File Offset: 0x00102A7C
    private static bool SerializeObject(IDictionary anObject, StringBuilder builder)
    {
        bool flag = true;
        builder.Append('{');
        foreach (object obj in anObject.Keys)
        {
            if (!flag)
            {
                builder.Append(',');
            }
            FirebaseMiniJson.SerializeString(obj.ToString(), builder);
            builder.Append(':');
            if (!FirebaseMiniJson.SerializeValue(anObject[obj], builder))
            {
                return false;
            }
            flag = false;
        }
        builder.Append('}');
        return true;
    }

    // Token: 0x06002CDB RID: 11483 RVA: 0x0010492C File Offset: 0x00102B2C
    private static bool SerializeArray(IList anArray, StringBuilder builder)
    {
        builder.Append('[');
        bool flag = true;
        foreach (object value in anArray)
        {
            if (!flag)
            {
                builder.Append(',');
            }
            if (!FirebaseMiniJson.SerializeValue(value, builder))
            {
                return false;
            }
            flag = false;
        }
        builder.Append(']');
        return true;
    }

    // Token: 0x06002CDC RID: 11484 RVA: 0x001049BC File Offset: 0x00102BBC
    private static bool SerializeValue(object value, StringBuilder builder)
    {
        if (value == null)
        {
            builder.Append("null");
        }
        else if (value.GetType().IsArray)
        {
            FirebaseMiniJson.SerializeArray((IList)value, builder);
        }
        else if (value is string)
        {
            FirebaseMiniJson.SerializeString((string)value, builder);
        }
        else if (value is char)
        {
            FirebaseMiniJson.SerializeString(Convert.ToString((char)value), builder);
        }
        else if (value is IDictionary)
        {
            FirebaseMiniJson.SerializeObject((IDictionary)value, builder);
        }
        else if (value is IList)
        {
            FirebaseMiniJson.SerializeArray((IList)value, builder);
        }
        else if (value is bool)
        {
            builder.Append((!(bool)value) ? "false" : "true");
        }
        else
        {
            if (!value.GetType().IsPrimitive)
            {
                return false;
            }
            FirebaseMiniJson.SerializeNumber(Convert.ToDouble(value), builder);
        }
        return true;
    }

    // Token: 0x06002CDD RID: 11485 RVA: 0x00104AD0 File Offset: 0x00102CD0
    private static void SerializeString(string aString, StringBuilder builder)
    {
        builder.Append('"');
        char[] array = aString.ToCharArray();
        foreach (char c in array)
        {
            if (c == '"')
            {
                builder.Append("\\\"");
            }
            else if (c == '\\')
            {
                builder.Append("\\\\");
            }
            else if (c == '\b')
            {
                builder.Append("\\b");
            }
            else if (c == '\f')
            {
                builder.Append("\\f");
            }
            else if (c == '\n')
            {
                builder.Append("\\n");
            }
            else if (c == '\r')
            {
                builder.Append("\\r");
            }
            else if (c == '\t')
            {
                builder.Append("\\t");
            }
            else
            {
                int num = Convert.ToInt32(c);
                if (num >= 32 && num <= 126)
                {
                    builder.Append(c);
                }
                else
                {
                    builder.Append("\\u" + Convert.ToString(num, 16).PadLeft(4, '0'));
                }
            }
        }
        builder.Append('"');
    }

    // Token: 0x06002CDE RID: 11486 RVA: 0x0001F370 File Offset: 0x0001D570
    private static void SerializeNumber(double number, StringBuilder builder)
    {
        builder.Append(number.ToString());
    }

    // Token: 0x04001C90 RID: 7312
    private const int BUILDER_CAPACITY = 2000;

    // Token: 0x04001C91 RID: 7313
    private static int lastErrorIndex = -1;

    // Token: 0x04001C92 RID: 7314
    private static string lastDecode;

    // Token: 0x02000518 RID: 1304
    private enum TOKEN
    {
        // Token: 0x04001C94 RID: 7316
        NONE,
        // Token: 0x04001C95 RID: 7317
        CURLY_OPEN,
        // Token: 0x04001C96 RID: 7318
        CURLY_CLOSE,
        // Token: 0x04001C97 RID: 7319
        SQUARED_OPEN,
        // Token: 0x04001C98 RID: 7320
        SQUARED_CLOSE,
        // Token: 0x04001C99 RID: 7321
        COLON,
        // Token: 0x04001C9A RID: 7322
        COMMA,
        // Token: 0x04001C9B RID: 7323
        STRING,
        // Token: 0x04001C9C RID: 7324
        NUMBER,
        // Token: 0x04001C9D RID: 7325
        TRUE,
        // Token: 0x04001C9E RID: 7326
        FALSE,
        // Token: 0x04001C9F RID: 7327
        NULL
    }
}
