using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public static class MD5Sign
{
    // 密钥，通常应该放到服务端，此处为了演示先放客户端
    static string signKey = "abcdefg";

    /// <summary>
    /// 计算参数签名
    /// </summary>
    /// <param name="args">参数</param>
    public static string ComputeSign(Dictionary<string, object> args)
    {
        // 按照 ASCII 码从小到大排序并拼接
        string result = AsciiDicToStr(args);
        // 拼接密钥
        result += $"&key={signKey}";
        // 计算拼接后的字符串的 MD5 值，结果就是参数签名
        string md5up = ComputeStringMD5(result, true);
        return md5up;
    }

    /// <summary>
    /// 以参数名的 ASCII 码从小到大排序并拼接
    /// </summary>
    /// <param name="args">参数</param>
    static string AsciiDicToStr(Dictionary<string, object> args)
    {
        // 取出 key 集合
        string[] arrKeys = args.Keys.ToArray();
        // 对 key 集合进行排序（ASCII 码从小到大）
        Array.Sort(arrKeys, string.CompareOrdinal);
        // 按照排序后的 key 集合依次从字典中取出值进行拼接
        var sb = new StringBuilder();
        foreach (var key in arrKeys)
        {
            string value = args[key]?.ToString();
            // 参数值为 null 或者 "" 不参与签名
            if (value != null && !string.Empty.Equals(value))
            {
                sb.Append(key + "=" + value + "&");
            }
        }
        // 去掉末尾的 & 符号并返回
        return sb.ToString().Substring(0, sb.ToString().Length - 1);
    }

    /// <summary>
    /// 计算字符串的 MD5 值
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="isUpperCase"> MD5 是否大写</param>
    static string ComputeStringMD5(string str, bool isUpperCase)
    {
        StringBuilder sbMd5 = new StringBuilder();
        // 创建 MD5 对象
        using (MD5 md5 = MD5.Create())
        {
            // 字符串转 byte 数组
            byte[] buffer = System.Text.Encoding.Default.GetBytes(str);
            // 计算 MD5 值
            byte[] mdBytes = md5.ComputeHash(buffer);
            md5.Clear();

            // 将 byte 数组中的每个元素以 16 进制字符串的方式拼接
            for (int i = 0; i < mdBytes.Length; i++)
            {
                sbMd5.Append(mdBytes[i].ToString("x2"));
            }
            // MD5 大写或小写结果
            string result = isUpperCase ? sbMd5.ToString().ToUpper() : sbMd5.ToString();
            return result;
        }
    }
}
