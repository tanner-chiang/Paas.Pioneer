﻿using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;

namespace Paas.Pioneer.AutoWrapper.Extensions
{
    internal static class StringExtension
    {
        public static bool IsValidJson(this string text)
        {
            text = text.Trim();
            if ((text.StartsWith("{") && text.EndsWith("}")) || //For object
                (text.StartsWith("[") && text.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(text);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static (bool IsEncoded, string ParsedText) VerifyBodyContent(this string text)
        {
            try
            {
                var obj = JToken.Parse(text);
                return (true, obj.ToString());
            }
            catch (Exception)
            {
                return (false, text);
            }
        }

        public static bool IsHtml(this string text)
        {
            Regex tagRegex = new Regex(@"<\s*([^ >]+)[^>]*>.*?<\s*/\s*\1\s*>");

            return tagRegex.IsMatch(text);
        }
    }
}
