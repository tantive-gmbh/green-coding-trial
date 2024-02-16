using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace HighLoad;

public static class ExtensionMethods
{
    #region --- conversions ---
    /// <summary>
    /// If <paramref name="firstChoice"/> is not <c>null</c> or empty, the value is returned; otherwise, <paramref name="alternativeValue"/> is returned.
    /// </summary>
    /// <param name="firstChoice">The first choice.</param>
    /// <param name="alternativeValue">The alternative value.</param>
    public static string Otherwise(this string firstChoice, string alternativeValue = "")
    {
        return firstChoice.IsEmpty() ? alternativeValue : firstChoice;
    }

    /// <summary>
    /// Tries to cast the specified object to string and return the value. In case the object is <c>null</c> or cannot be cast, then <c>string.Empty</c> is returned.
    /// </summary>
    /// <param name="toString">The object to be cast to string.</param>
    public static string AsString(this object toString)
    {
        return toString.AsString(string.Empty);
    }

    /// <summary>
    /// Tries to cast the specified object to string and return the value. In case the object is <c>null</c> or cannot be cast, then the <paramref name="alternativeValue"/> is returned.
    /// </summary>
    /// <param name="toString">The object to be cast to string.</param>
    /// <param name="alternativeValue">The return value, in case the object cannot be cast to string.</param>
    public static string AsString(this object toString, string alternativeValue)
    {
        string firstChoice = toString == null ? string.Empty : toString.ToString() ?? string.Empty;
        return firstChoice.Otherwise(alternativeValue);
    }

    /// <summary>
    /// Tries to cast the specified object to DateTime and return the value. In case the object is <c>null</c> or cannot be cast, then <c>null</c> is returned.
    /// </summary>
    /// <param name="toDate">The object to be cast to DateTime.</param>
    public static DateTime? AsDateN(this object toDate)
    {
        if (toDate.AsString().IsEmpty())
            return null;
        if (toDate is DateTime)
            return (DateTime?)toDate;
        var formats = new List<string>
            {
                "yyyy-MM-dd",
                "dd.MM.yyyy",
                "MM/dd/yyyy",
                CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern
            };
        var result = DateTime.TryParseExact(toDate.AsString(), formats.ToArray(), CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dummy) ? (DateTime?)dummy : null;
        return result;
    }

    /// <summary>
    /// Tries to cast the specified object to DateTime and return the value. In case the object is <c>null</c> or cannot be cast, then <c>DateTime.MinValue</c> is returned.
    /// </summary>
    /// <param name="toDate">The object to be cast to DateTime.</param>
    public static DateTime AsDate(this object toDate)
    {
        return toDate.AsDateN() ?? DateTime.MinValue;
    }

    /// <summary>
    /// Tries to cast the specified object to integer and return the value. In case the object is <c>null</c> or cannot be cast, then <c>null</c> is returned.
    /// </summary>
    /// <param name="toInt">The object to be cast to integer.</param>
    public static int? AsIntN(this object toInt)
    {
        return int.TryParse(toInt.AsString(), out int dummy) ? dummy : null;
    }

    /// <summary>
    /// Tries to cast the specified object to integer and return the value. In case the object is <c>null</c> or cannot be cast, then the default value is returned.
    /// </summary>
    /// <param name="toInt">The object to be cast to integer.</param>
    public static int AsInt(this object toInt)
    {
        return toInt.AsInt(default(int));
    }

    /// <summary>
    /// Tries to cast the specified object to integer and return the value. In case the object is <c>null</c> or cannot be cast, then the <paramref name="alternativeValue"/> is returned.
    /// </summary>
    /// <param name="toInt">The object to be cast to integer.</param>
    /// <param name="alternativeValue">The return value, in case the object cannot be cast to integer.</param>
    public static int AsInt(this object toInt, int alternativeValue)
    {
        return toInt.AsIntN() ?? alternativeValue;
    }

    /// <summary>
    /// Tries to cast the specified object to float and return the value. In case the object is <c>null</c> or cannot be cast, then the default value is returned.
    /// </summary>
    /// <param name="toFloat">The object to be cast to float.</param>
    public static float AsFloat(this object toFloat)
    {
        return toFloat.AsFloat(default(float));
    }

    /// <summary>
    /// Tries to cast the specified object to float and return the value. In case the object is <c>null</c> or cannot be cast, then the <paramref name="alternativeValue"/> is returned.
    /// </summary>
    /// <param name="toFloat">The object to be cast to float.</param>
    /// <param name="alternativeValue">The return value, in case the object cannot be cast to integer.</param>
    public static float AsFloat(this object toFloat, float alternativeValue)
    {
        try
        {
            if (toFloat == null)
                return alternativeValue;
            return float.TryParse(toFloat.AsString(), out float output) ? output : alternativeValue;
        }
        catch
        {
            return alternativeValue;
        }
    }

    /// <summary>
    /// Tries to cast the specified object to boolean and return the value. In case the object is <c>null</c> or cannot be cast, then <c>false</c> is returned.
    /// </summary>
    /// <param name="toBool">The object to be cast to boolean.</param>
    public static bool AsBool(this object toBool)
    {
        return toBool.AsBool(false);
    }

    /// <summary>
    /// Tries to cast the specified object to boolean and return the value. In case the object is <c>null</c> or cannot be cast, then the <paramref name="alternativeValue"/> is returned.
    /// </summary>
    /// <param name="toBool">The object to be cast to boolean.</param>
    /// <param name="alternativeValue">The return value, in case the object cannot be cast to boolean.</param>
    public static bool AsBool(this object toBool, bool alternativeValue)
    {
        try
        {
            if (toBool == null)
                return alternativeValue;
            string asString = toBool.AsString();
            if (asString.IsEmpty())
                return false;
            if (bool.TryParse(asString, out bool result))
                return result;
            switch (asString.AsInt(int.MinValue))
            {
                case 0:
                    return false;
                default:
                    return true;
            }
        }
        catch
        {
            return alternativeValue;
        }
    }
    #endregion

    #region --- string extensions ---
    public static bool IsEmpty(this string value)
    {
        return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
    }

    public static bool IsNotEmpty(this string value)
    {
        return !value.IsEmpty();
    }

    public static string MaxLen(this string message, int maxLen)
    {
        return message.MaxLen(maxLen, "...");
    }

    public static string MaxLen(this string message, int maxLen, string postfix)
    {
        string result = message;
        if (!message.IsEmpty())
        {
            if (message.Length > maxLen)
            {
                int length = maxLen - postfix.Length;
                result = $"{message.Substring(0, length)}{postfix}";
            }
        }
        return result;
    }

    public static string Plural(this int count, string unit)
    {
        return count.Plural(unit, unit + "s");
    }

    public static string Plural(this int count, string singular, string plural)
    {
        return $"{count} {(count == 1 ? singular : plural)}";
    }

    public static string Reverse(this string input)
    {
        if (input.IsEmpty())
            return string.Empty;
        var result = new StringBuilder();
        for (int i = input.Length - 1; i >= 0; i--)
        {
            result.Append(input[i]);
        }
        return result.ToString();
    }

    public static bool IsGuid(this string value)
    {
        return Regex.IsMatch(value.Otherwise(), @"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");
    }

    public static string ToMD5(this string input)
    {
        var hash = new StringBuilder();
        var md5Provider = MD5.Create();
        var bytes = md5Provider.ComputeHash(new UTF8Encoding().GetBytes(input.AsString()));

        foreach (byte t in bytes)
        {
            hash.Append(t.ToString("x2"));
        }
        return hash.ToString();
    }

    public static string Checksum(this string input)
    {
        var hash = input.ToMD5();
        hash = $"{hash}{hash.Reverse()}".ToMD5();
        hash = $"{hash.Reverse()}{hash}".ToMD5();
        hash = $"{hash}{hash.Reverse()}".ToMD5();
        hash = $"{hash.Reverse()}{hash}".ToMD5();
        hash = $"{hash}{hash.Reverse()}".ToMD5();
        hash = $"{hash.Reverse()}{hash}".ToMD5();
        return hash;
    }

    public static string Multiply(this string input, int count)
    {
        if (input.IsEmpty()) return string.Empty;
        var sb = new List<string>();
        for (int i = 0; i < count; i++)
        {
            sb.Add(input);
        }
        return string.Join("", sb);
    }
    #endregion

    #region --- path stuff ---
    private static List<string> GetAllFileNames(string path)
    {
        var files = new List<string>();
        var di = new DirectoryInfo(path);
        if (di.Exists)
        {
            foreach (var directory in di.GetDirectories())
            {
                files.AddRange(GetAllFiles(directory.FullName));
            }
            foreach (var file in di.GetFiles())
            {
                files.Add(file.FullName);
            }
        }
        return files;
    }

    public static string[] GetAllFiles(this string rootPath)
    {
        var allFiles = GetAllFileNames(rootPath).Distinct().ToList();
        allFiles.Sort();
        return allFiles.ToArray();
    }

    public static string FindFilePath(this string rootPath, string fileName)
    {
        string result = Path.Combine(rootPath, fileName);
        return File.Exists(result) ? result : rootPath.GetAllFiles().FirstOrDefault(f => f.EndsWith(fileName, StringComparison.CurrentCultureIgnoreCase)) ?? string.Empty;
    }
    #endregion
}