﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Selenium.Essentials.Utilities.Extensions
{
    public static class StringExtensions
    {

        #region Conversion
        /// <summary>
        /// Converts the string to integer. Returns 0 if the conversion fails and does not throw any exception
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int ToInteger(this string text)
        {
            try
            {
                int.TryParse(text, out int result);
                return result;
            }
            catch (Exception)
            {
                //Log the exception here
                return 0;
            }
        }

        /// <summary>
        /// Converts the string to integer. Returns 0 if the conversion fails and does not throw any exception
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static long ToLong(this string text)
        {
            try
            {
                long.TryParse(text, out long result);
                return result;
            }
            catch (Exception)
            {
                //Log the exception here
                return 0;
            }
        }

        /// <summary>
        /// Converts the string to decimal. Returns 0 if the conversion fails and does not throw any exception
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string text)
        {
            try
            {
                decimal.TryParse(text, out decimal result);
                return result;
            }
            catch (Exception)
            {
                //Log the exception here
                return 0;
            }
        }

        /// <summary>
        /// Returns string to a bool value. Conditions for true values are 'yes', 'true', 'enable', 'enabled'
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool ToBool(this string text)
        {
            if (text.IsEmpty())
                return false;

            return text.EqualsIgnoreCase("yes") ||
                   text.EqualsIgnoreCase("true") ||
                   text.EqualsIgnoreCase("Enable") ||
                   text.EqualsIgnoreCase("Enabled") ||
                   text.Equals("1");
        }

        /// <summary>
        /// Converts the string to double. Returns 0 if the conversion fails and does not throw any exception
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(this string value)
        {
            try
            {
                double.TryParse(value, out double result);
                return result;
            }
            catch (Exception)
            {
                //Log the exception here
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string value)
        {
            try
            {
                if (value.IsEmpty())
                    return DateTime.MinValue;

                DateTime.TryParse(value, out DateTime result);
                return result;
            }
            catch (Exception)
            {
                //Log the exception here
                return DateTime.MinValue;
            }

        }

        /// <summary>
        /// Returns a dynamic object in the JSON format
        /// </summary>
        /// <param name="value">The value which is to be converted to JSON</param>
        /// <param name="exposeError">True if you want to throw the conversion error</param>
        /// <returns></returns>
        public static dynamic ToJson(this string value, bool exposeError = false)
        {
            try
            {
                return JsonConvert.DeserializeObject<dynamic>(value);
            }
            catch (Exception)
            {
                if (exposeError)
                {
                    throw;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion









        /// <summary>
        /// Checks if the string is null or empty or white space
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string text) 
            => string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text);

        /// <summary>
        /// Returns true if the string has any value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HasValue(this string value) => !IsEmpty(value);

        /// <summary>
        /// Returns string.empty if the value is null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EmptyIfNull(this string value) => value ?? string.Empty;

        /// <summary>
        /// Compares two string without considering the culture and case (case insensitive)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="compareValue"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string value, string compareValue) 
            => value.EmptyIfNull().Equals(compareValue, StringComparison.InvariantCultureIgnoreCase);




        public static string SurroundWith(this string str, string surroundString)
        {
            if (!str.StartsWith(surroundString))
            {
                str = $@"{surroundString}{str}";
            }
            if (!str.EndsWith(surroundString))
            {
                str = $@"{str}{surroundString}";
            }

            return str;
        }

        public static IEnumerable<string> ApplyJsonPathExpression(this string value, string jsonFilter) => JObject.Parse(value.EmptyIfNull()).SelectTokens(jsonFilter).Select(s => Convert.ToString(s)).ToArray();
        public static IEnumerable<string> SplitAndTrim(this string value, string splitString) => value.Split(new[] { splitString }, StringSplitOptions.RemoveEmptyEntries)
                .Select(d => d.Trim())
                .Where(d => d.HasValue());
        public static IEnumerable<string> SplitByWithDistinct(this string value, string splitString) => SplitAndTrim(value, splitString)
                 .Distinct();


        public static bool Contains(this string haystack, string pin, StringComparison comparisonOptions) => haystack.IndexOf(pin, comparisonOptions) >= 0;
        public static bool ContainsIgnoreCase(this string data, string value) => data.Contains(value, StringComparison.InvariantCultureIgnoreCase);
        public static bool ContainsIgnoreCase(this string data, string[] value) => value.Any(str => data.ContainsIgnoreCase(str));

        /// <summary>
        /// Will convert a string to standard allowed (acceptable) DB column name format
        /// Removes all non character and prefix with _
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertToDbFormatColumnName(this string value) => "_" + Regex.Replace(value, @"[^\w]+", "").ToLower();

        /// <summary>
        /// Compares the source startWith function against the list of string. For any match found it will trim the match from the source.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="checks"></param>
        /// <returns>Return the source value after compare and trim </returns>
        public static string StartWithCompareThenTrim(this string value, string[] checks)
        {
            return checks.Any(value.StartsWith)
                ? checks.Where(value.StartsWith)
                    .Select(l => value.Substring(l.Length, value.Length - l.Length)).FirstOrDefault()
                : value;
        }
        public static string EndWithCompareThenTrim(this string value, string[] checks)
        {
            return checks.Any(value.EndsWith)
                ? checks.Where(value.EndsWith)
                    .Select(l => value.Substring(0, value.Length - l.Length)).FirstOrDefault()
                : value;
        }

        public static string StripQuotes(this string value)
        {
            return value.StartWithCompareThenTrim(new[] { "\"" }).EndWithCompareThenTrim(new[] { "\"" });
        }

        /// <summary>
        /// to extract numbers from the string
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static string ExtractNumber(this string original)
        {
            return new string(original.Where(c => Char.IsNumber(c)).ToArray());
        }

        /// <summary>
        /// Converts into a string which can be used as file name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ConvertToValidFileName(this string name, int length = 0)
        {
            StringBuilder result = new StringBuilder();
            foreach (var str in name)
            {
                if (Char.IsLetterOrDigit(str))
                    result.Append(str);
            }

            if (length > 0 && result.Length > length)
            {
                return result.ToString().Substring(0, length);
            }
            else
            {
                return result.ToString();
            }
        }

        public static string ReplaceMultiple(this string value, string replaceWith, params string[] replaceContents)
        {
            replaceContents.Iter(r => value = value.Replace(r, replaceWith));
            return value;
        }
    }
}
