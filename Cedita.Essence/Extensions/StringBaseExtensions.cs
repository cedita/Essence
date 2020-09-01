// Copyright (c) Cedita Ltd. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cedita.Essence.Extensions
{
    public static class StringBaseExtensions
    {
        /// <summary>
        /// A string extension method that query if <paramref name="str"/> contains all values.
        /// </summary>
        /// <param name="str">String to work with.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>true if it contains all values, otherwise false.</returns>
        public static bool ContainsAll(this string str, params string[] values)
        {
            foreach (string value in values)
            {
                if (str.IndexOf(value) == -1)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// A string extension method that query if this object contains the given <paramref name="str"/>.
        /// </summary>
        /// <param name="str">String to work with.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>true if it contains all values, otherwise false.</returns>
        public static bool ContainsAll(this string str, StringComparison comparisonType, params string[] values)
        {
            foreach (string value in values)
            {
                if (str.IndexOf(value, comparisonType) == -1)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// A string extension method that query if <paramref name="str"/> contains any values.
        /// </summary>
        /// <param name="str">String to work with.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>true if it contains any values, otherwise false.</returns>
        public static bool ContainsAny(this string str, params string[] values)
        {
            foreach (string value in values)
            {
                if (str.IndexOf(value) != -1)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// A string extension method that query if <paramref name="str"/> contains any values.
        /// </summary>
        /// <param name="str">String to work with.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>true if it contains any values, otherwise false.</returns>
        public static bool ContainsAny(this string str, StringComparison comparisonType, params string[] values)
        {
            foreach (string value in values)
            {
                if (str.IndexOf(value, comparisonType) != -1)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Retrieves the system's reference to the specified <paramref name="str"/>.
        /// </summary>
        /// <param name="str">A string to search for in the intern pool.</param>
        /// <returns>
        /// The system's reference to <paramref name="str"/>, if it is interned; otherwise, a new reference to a string with the value of <paramref name="str"/>.
        /// </returns>
        public static string Intern(this string str)
        {
            return string.Intern(str);
        }

        /// <summary>
        /// A string extension method that query if <paramref name="str"/> is Alpha-only.
        /// </summary>
        /// <param name="str">String to work with.</param>
        /// <returns>true if Alpha-only, false if not.</returns>
        public static bool IsAlpha(this string str)
        {
            return !Regex.IsMatch(str, "[^a-zA-Z]");
        }

        /// <summary>
        /// A string extension method that query if <paramref name="str"/> is Alphanumeric.
        /// </summary>
        /// <param name="str">String to work with.</param>
        /// <returns>true if Alphanumeric, false if not.</returns>
        public static bool IsAlphaNumeric(this string str)
        {
            return !Regex.IsMatch(str, "[^a-zA-Z0-9]");
        }

        /// <summary>
        /// A string extension method that query if <paramref name="str"/> is anagram of <paramref name="otherString"/>.
        /// </summary>
        /// <param name="str">String to work with.</param>
        /// <param name="otherString">The other string.</param>
        /// <returns>true if the @this is anagram of the otherString, false if not.</returns>
        public static bool IsAnagram(this string str, string otherString)
        {
            return str
                .OrderBy(c => c)
                .SequenceEqual(otherString.OrderBy(c => c));
        }

        /// <summary>
        /// Retrieves a reference to a specified.
        /// </summary>
        /// <param name="str">The string to search for in the intern pool.</param>
        /// <returns>A reference to if it is in the common language runtime intern pool; otherwise, null.</returns>
        public static string IsInterned(this string str)
        {
            return string.IsInterned(str);
        }

        /// <summary>
        /// Returns true if <paramref name="str"/>  is null or whitespace.
        /// </summary>
        /// <param name="str">String to work with.</param>
        /// <returns>True if the string is null or whitespace.</returns>
        public static bool IsNullOrWhitespace(this string str) => string.IsNullOrWhiteSpace(str);

        /// <summary>
        /// Returns true if <paramref name="str"/>  is null or empty.
        /// </summary>
        /// <param name="str">String to work with.</param>
        /// <returns>True if the string is null or empty.</returns>
        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);

        /// <summary>
        /// A string extension method that query if <paramref name="str"/> is Numeric.
        /// </summary>
        /// <param name="str">String to work with.</param>
        /// <returns>true if Numeric, false if not.</returns>
        public static bool IsNumeric(this string str)
        {
            return !Regex.IsMatch(str, "[^0-9]");
        }

        /// <summary>A string extension method that query if <paramref name="str"/> is a palindrome.</summary>
        /// <param name="str">String to work with.</param>
        /// <returns>true if palindrome, false if not.</returns>
        public static bool IsPalindrome(this string str)
        {
            var rgx = new Regex("[^a-zA-Z0-9]");
            str = rgx.Replace(str, string.Empty);
            return str.SequenceEqual(str.Reverse());
        }

        /// <summary>
        /// Return <paramref name="str"/> truncated down to <paramref name="maxLength"/> characters, with optional trailing content from <paramref name="trailer"/>.
        /// </summary>
        /// <param name="str">String to work with.</param>
        /// <param name="maxLength">Maximum length of string.</param>
        /// <param name="trailer">Optional Trailer.</param>
        /// <returns>Truncated string.</returns>
        /// <remarks><paramref name="maxLength"/> is inclusive of the trailer if provided.</remarks>
        public static string MaxLength(this string str, int maxLength, string trailer = null)
        {
            if (str.Length <= maxLength)
            {
                return str;
            }

            if (trailer != null)
            {
                return str.Substring(0, maxLength - trailer.Length) + trailer;
            }

            return str.Substring(0, maxLength);
        }

        /// <summary>
        /// Count the occurences of a value within a string.
        /// </summary>
        /// <param name="str">String to work with.</param>
        /// <param name="value">Value to find.</param>
        /// <returns>Count of occurences.</returns>
        public static int OccurencesOf(this string str, string value)
        {
            int count = 0, idx = 0;

            while ((idx = str.IndexOf(value, idx)) != -1)
            {
                idx += value.Length;
                count++;
            }

            return count;
        }

        /// <summary>
        /// Returns the string, or <paramref name="defaultValue"/> if null or whitespace.
        /// </summary>
        /// <param name="str">String to work with.</param>
        /// <param name="defaultValue">Default value to return.</param>
        /// <returns><paramref name="str"/>, or <paramref name="defaultValue"/> if null or empty.</returns>
        public static string OrDefault(this string str, string defaultValue)
        {
            if (str.IsNullOrWhitespace())
            {
                return defaultValue;
            }

            return str;
        }
    }
}
