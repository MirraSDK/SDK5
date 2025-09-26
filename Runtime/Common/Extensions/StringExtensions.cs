using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MirraGames.SDK.Common {

    public static class StringExtensions {

        public static string TrimInterfacePrefix(this string value) {
            if (string.IsNullOrEmpty(value)) {
                return string.Empty;
            }
            // Make sure that the value starts with 'I' and has at least one more upper case letter after it
            if (value.Length > 1 && value[0] == 'I' && char.IsUpper(value[1])) {
                return value.Substring(1);
            }
            return value;
        }

        public static string ToSafeFileName(this string value, string fallbackValue = "") {
            if (string.IsNullOrEmpty(value)) {
                return string.Empty;
            }
            HashSet<char> allowedCharacters = new(
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-.[]()");
            StringBuilder sanitized = new(value.Length);
            foreach (char c in value) {
                if (allowedCharacters.Contains(c)) {
                    sanitized.Append(c);
                }
            }
            if (sanitized.Length == 0) {
                return fallbackValue;
            }
            return sanitized.ToString();
        }

        public static Enum ToEnumOrDefault(this string text, Type enumType, Enum defaultValue = default) {
            if (string.IsNullOrEmpty(text)) {
                return defaultValue;
            }
            return Enum.Parse(enumType, text) as Enum;
        }

        public static T ToEnumOrDefault<T>(this string text, T defaultValue = default) where T : struct, Enum {
            if (string.IsNullOrEmpty(text)) {
                return defaultValue;
            }
            if (Enum.TryParse(text, out T result)) {
                return result;
            }
            return defaultValue;
        }

        public static string InsertSpacing(this string caseText) {
            if (string.IsNullOrEmpty(caseText)) {
                return caseText;
            }
            // If the first character is lower case, return the original string.
            if (char.IsLower(caseText[0])) {
                return caseText;
            }
            StringBuilder stringBuilder = new();
            // Append characters from enum with whitespaces through loop.
            for (int x = 0; x < caseText.Length - 1; x++) {
                // Ignore any casing for the first two characters, in cases when enum
                // starts with lower case but means abbreviation, for example 'iOS'.
                if (x < 2) {
                    stringBuilder.Append(caseText[x]);
                    continue;
                }
                // Append whitespace in cases 'aB' to make 'a B'.
                if (char.IsLower(caseText[x - 1]) && char.IsUpper(caseText[x])) {
                    stringBuilder.Append(' ');
                }
                // Append whitespace in cases 'ABb' to make 'A Bb'.
                if (char.IsUpper(caseText[x - 1]) && char.IsUpper(caseText[x]) && char.IsLower(caseText[x + 1])) {
                    stringBuilder.Append(' ');
                }
                // Append source characters.
                stringBuilder.Append(caseText[x]);
            }
            // Append last character from loop offset.
            if (caseText.Length > 1) {
                if (char.IsLower(caseText[^2]) && char.IsUpper(caseText[^1])) {
                    // Append whitespace in case of 'aB' to make 'a B'.
                    stringBuilder.Append(' ');
                }
                // Append last character.
                stringBuilder.Append(caseText[^1]);
            }
            return stringBuilder.ToString();
        }

    }

}