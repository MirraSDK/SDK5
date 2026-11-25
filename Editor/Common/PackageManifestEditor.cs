using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MirraGames.SDK.Editor
{
    /// <summary>
    /// Edits the text of a Unity project manifest (Packages/manifest.json): scoped registries and
    /// registry package versions. Text in, text out, so nothing here touches the project on disk.
    /// </summary>
    public static class PackageManifestEditor
    {
        private const string DependenciesKey = "dependencies";
        private const string ScopedRegistriesKey = "scopedRegistries";
        private const string NameKey = "name";
        private const string UrlKey = "url";
        private const string ScopesKey = "scopes";

        private static readonly Regex PlainVersionPattern = new(@"^\d+(\.\d+){0,2}(-[0-9A-Za-z.\-]+)?(\+[0-9A-Za-z.\-]+)?$");

        /// <summary>
        /// Adds the registries and raises the packages to the given versions.
        /// A package is written when it is absent, when it is not a plain version (file or git reference)
        /// or when its version is lower. A higher version is kept. Returns the input text unchanged
        /// when nothing had to change.
        /// </summary>
        public static string Apply(string manifestJson, ScopedRegistry[] registries, RegistryPackage[] packages, out bool changed)
        {
            changed = false;
            JsonObject root = ParseRoot(manifestJson);
            if (registries != null)
            {
                foreach (ScopedRegistry registry in registries)
                {
                    changed |= MergeRegistry(root, registry);
                }
            }
            if (packages != null)
            {
                foreach (RegistryPackage package in packages)
                {
                    changed |= PinPackage(root, package);
                }
            }
            return changed ? JsonWriter.Write(root) : manifestJson;
        }

        /// <summary>
        /// Removes a package from the dependencies. Returns the input text unchanged when it was not listed.
        /// </summary>
        public static string Remove(string manifestJson, string packageName, out bool changed)
        {
            changed = false;
            JsonObject root = ParseRoot(manifestJson);
            if (root.TryGet(DependenciesKey, out object value) && value is JsonObject dependencies)
            {
                changed = dependencies.Remove(packageName);
            }
            return changed ? JsonWriter.Write(root) : manifestJson;
        }

        /// <summary>
        /// Returns the manifest entry of a package (a version, a file path or a git URL), or null when it is not listed.
        /// </summary>
        public static string GetDependency(string manifestJson, string packageName)
        {
            JsonObject root = ParseRoot(manifestJson);
            if (root.TryGet(DependenciesKey, out object value) && value is JsonObject dependencies
                && dependencies.TryGet(packageName, out object entry))
            {
                return entry as string;
            }
            return null;
        }

        /// <summary>
        /// Lists scopes that another registry of the manifest already serves under a different URL.
        /// Unity refuses to resolve packages while one scope belongs to two registries.
        /// </summary>
        public static string[] FindScopeConflicts(string manifestJson, ScopedRegistry[] registries)
        {
            List<string> conflicts = new();
            if (registries == null)
            {
                return conflicts.ToArray();
            }
            JsonObject root = ParseRoot(manifestJson);
            if (!root.TryGet(ScopedRegistriesKey, out object value) || value is not List<object> registryList)
            {
                return conflicts.ToArray();
            }
            foreach (ScopedRegistry registry in registries)
            {
                if (registry?.Scopes == null || string.IsNullOrWhiteSpace(registry.Url))
                {
                    continue;
                }
                foreach (object item in registryList)
                {
                    if (item is not JsonObject existing || IsSameUrl(GetString(existing, UrlKey), registry.Url))
                    {
                        continue;
                    }
                    foreach (string scope in registry.Scopes)
                    {
                        if (ContainsText(GetArray(existing, ScopesKey), scope))
                        {
                            conflicts.Add($"Scope '{scope}' is already served by registry '{GetString(existing, NameKey)}' ({GetString(existing, UrlKey)})");
                        }
                    }
                }
            }
            return conflicts.ToArray();
        }

        /// <summary>
        /// True for a plain semantic version such as 11.2.0 or 1.0.0-preview.2; false for file paths and URLs.
        /// </summary>
        public static bool IsPlainVersion(string value)
        {
            return !string.IsNullOrEmpty(value) && PlainVersionPattern.IsMatch(value);
        }

        /// <summary>
        /// Compares two semantic versions. Negative when left is lower, zero when equal, positive when higher.
        /// A prerelease is lower than the same version without one; build metadata is ignored.
        /// </summary>
        public static int CompareVersions(string left, string right)
        {
            ParsedVersion leftVersion = ParsedVersion.Parse(left);
            ParsedVersion rightVersion = ParsedVersion.Parse(right);
            for (int index = 0; index < 3; index++)
            {
                int comparison = leftVersion.Numbers[index].CompareTo(rightVersion.Numbers[index]);
                if (comparison != 0)
                {
                    return comparison;
                }
            }
            return ComparePrerelease(leftVersion.Prerelease, rightVersion.Prerelease);
        }

        private static JsonObject ParseRoot(string manifestJson)
        {
            if (JsonReader.Parse(manifestJson) is JsonObject root)
            {
                return root;
            }
            throw new FormatException("The package manifest is not a JSON object");
        }

        private static bool MergeRegistry(JsonObject root, ScopedRegistry registry)
        {
            if (registry == null || string.IsNullOrWhiteSpace(registry.Url))
            {
                return false;
            }
            string[] scopes = registry.Scopes ?? Array.Empty<string>();
            List<object> registryList = null;
            if (root.TryGet(ScopedRegistriesKey, out object value))
            {
                registryList = value as List<object> ?? throw new FormatException($"'{ScopedRegistriesKey}' is not a JSON array");
            }
            JsonObject existing = FindRegistry(registryList, registry.Url);
            if (existing == null)
            {
                JsonObject created = new();
                created.Set(NameKey, string.IsNullOrWhiteSpace(registry.Name) ? registry.Url : registry.Name);
                created.Set(UrlKey, registry.Url);
                created.Set(ScopesKey, new List<object>(scopes));
                if (registryList == null)
                {
                    registryList = new List<object>();
                    root.Set(ScopedRegistriesKey, registryList);
                }
                registryList.Add(created);
                return true;
            }
            List<object> existingScopes = GetArray(existing, ScopesKey);
            if (existingScopes == null)
            {
                existingScopes = new List<object>();
                existing.Set(ScopesKey, existingScopes);
            }
            bool changed = false;
            foreach (string scope in scopes)
            {
                if (!ContainsText(existingScopes, scope))
                {
                    existingScopes.Add(scope);
                    changed = true;
                }
            }
            return changed;
        }

        private static bool PinPackage(JsonObject root, RegistryPackage package)
        {
            if (package == null || string.IsNullOrWhiteSpace(package.Name) || string.IsNullOrWhiteSpace(package.Version))
            {
                return false;
            }
            JsonObject dependencies;
            if (root.TryGet(DependenciesKey, out object value))
            {
                dependencies = value as JsonObject ?? throw new FormatException($"'{DependenciesKey}' is not a JSON object");
            }
            else
            {
                dependencies = new JsonObject();
                root.Insert(0, DependenciesKey, dependencies);
            }
            if (dependencies.TryGet(package.Name, out object current) && current is string currentVersion
                && IsPlainVersion(currentVersion) && CompareVersions(currentVersion, package.Version) >= 0)
            {
                return false;
            }
            dependencies.Set(package.Name, package.Version);
            return true;
        }

        private static JsonObject FindRegistry(List<object> registryList, string url)
        {
            if (registryList == null)
            {
                return null;
            }
            foreach (object item in registryList)
            {
                if (item is JsonObject registry && IsSameUrl(GetString(registry, UrlKey), url))
                {
                    return registry;
                }
            }
            return null;
        }

        private static bool IsSameUrl(string left, string right)
        {
            return string.Equals(NormalizeUrl(left), NormalizeUrl(right), StringComparison.OrdinalIgnoreCase);
        }

        private static string NormalizeUrl(string url)
        {
            return (url ?? string.Empty).Trim().TrimEnd('/');
        }

        private static string GetString(JsonObject jsonObject, string key)
        {
            return jsonObject.TryGet(key, out object value) ? value as string : null;
        }

        private static List<object> GetArray(JsonObject jsonObject, string key)
        {
            return jsonObject.TryGet(key, out object value) ? value as List<object> : null;
        }

        private static bool ContainsText(List<object> items, string text)
        {
            if (items == null)
            {
                return false;
            }
            foreach (object item in items)
            {
                if (item is string itemText && string.Equals(itemText, text, StringComparison.Ordinal))
                {
                    return true;
                }
            }
            return false;
        }

        private static int ComparePrerelease(string left, string right)
        {
            bool isLeftRelease = string.IsNullOrEmpty(left);
            bool isRightRelease = string.IsNullOrEmpty(right);
            if (isLeftRelease || isRightRelease)
            {
                return isLeftRelease == isRightRelease ? 0 : (isLeftRelease ? 1 : -1);
            }
            string[] leftParts = left.Split('.');
            string[] rightParts = right.Split('.');
            int sharedLength = Math.Min(leftParts.Length, rightParts.Length);
            for (int index = 0; index < sharedLength; index++)
            {
                bool isLeftNumber = long.TryParse(leftParts[index], NumberStyles.None, CultureInfo.InvariantCulture, out long leftNumber);
                bool isRightNumber = long.TryParse(rightParts[index], NumberStyles.None, CultureInfo.InvariantCulture, out long rightNumber);
                int comparison;
                if (isLeftNumber && isRightNumber)
                {
                    comparison = leftNumber.CompareTo(rightNumber);
                }
                else if (isLeftNumber != isRightNumber)
                {
                    comparison = isLeftNumber ? -1 : 1;
                }
                else
                {
                    comparison = string.CompareOrdinal(leftParts[index], rightParts[index]);
                }
                if (comparison != 0)
                {
                    return comparison;
                }
            }
            return leftParts.Length.CompareTo(rightParts.Length);
        }

        private readonly struct ParsedVersion
        {
            public readonly long[] Numbers;
            public readonly string Prerelease;

            private ParsedVersion(long[] numbers, string prerelease)
            {
                Numbers = numbers;
                Prerelease = prerelease;
            }

            public static ParsedVersion Parse(string text)
            {
                string version = (text ?? string.Empty).Trim();
                int metadataIndex = version.IndexOf('+');
                if (metadataIndex >= 0)
                {
                    version = version[..metadataIndex];
                }
                string prerelease = string.Empty;
                int prereleaseIndex = version.IndexOf('-');
                if (prereleaseIndex >= 0)
                {
                    prerelease = version[(prereleaseIndex + 1)..];
                    version = version[..prereleaseIndex];
                }
                long[] numbers = new long[3];
                string[] parts = version.Split('.');
                for (int index = 0; index < numbers.Length && index < parts.Length; index++)
                {
                    long.TryParse(parts[index], NumberStyles.None, CultureInfo.InvariantCulture, out numbers[index]);
                }
                return new ParsedVersion(numbers, prerelease);
            }
        }

        /// <summary>
        /// JSON object that keeps its members in file order.
        /// </summary>
        private sealed class JsonObject
        {
            public readonly List<KeyValuePair<string, object>> Members = new();

            public bool TryGet(string key, out object value)
            {
                int index = IndexOf(key);
                value = index >= 0 ? Members[index].Value : null;
                return index >= 0;
            }

            public void Set(string key, object value)
            {
                int index = IndexOf(key);
                if (index >= 0)
                {
                    Members[index] = new KeyValuePair<string, object>(key, value);
                }
                else
                {
                    Members.Add(new KeyValuePair<string, object>(key, value));
                }
            }

            public void Insert(int position, string key, object value)
            {
                Members.Insert(position, new KeyValuePair<string, object>(key, value));
            }

            public bool Remove(string key)
            {
                int index = IndexOf(key);
                if (index < 0)
                {
                    return false;
                }
                Members.RemoveAt(index);
                return true;
            }

            private int IndexOf(string key)
            {
                for (int index = 0; index < Members.Count; index++)
                {
                    if (string.Equals(Members[index].Key, key, StringComparison.Ordinal))
                    {
                        return index;
                    }
                }
                return -1;
            }
        }

        /// <summary>
        /// JSON number kept as written, so values this editor does not own are never reformatted.
        /// </summary>
        private sealed class JsonNumber
        {
            public readonly string Text;

            public JsonNumber(string text)
            {
                Text = text;
            }
        }

        private sealed class JsonReader
        {
            private readonly string text;
            private int position;

            private JsonReader(string text)
            {
                this.text = text;
            }

            public static object Parse(string json)
            {
                if (json == null)
                {
                    throw new ArgumentNullException(nameof(json));
                }
                JsonReader reader = new(json);
                if (reader.text.Length > 0 && reader.text[0] == '﻿')
                {
                    reader.position = 1;
                }
                reader.SkipWhitespace();
                object value = reader.ReadValue();
                reader.SkipWhitespace();
                if (reader.position != reader.text.Length)
                {
                    throw reader.Error("Unexpected content after the JSON value");
                }
                return value;
            }

            private object ReadValue()
            {
                if (position >= text.Length)
                {
                    throw Error("Unexpected end of JSON");
                }
                char current = text[position];
                switch (current)
                {
                    case '{':
                        return ReadObject();
                    case '[':
                        return ReadArray();
                    case '"':
                        return ReadString();
                    case 't':
                        ReadLiteral("true");
                        return true;
                    case 'f':
                        ReadLiteral("false");
                        return false;
                    case 'n':
                        ReadLiteral("null");
                        return null;
                    default:
                        return ReadNumber();
                }
            }

            private JsonObject ReadObject()
            {
                JsonObject result = new();
                position++;
                SkipWhitespace();
                if (TryConsume('}'))
                {
                    return result;
                }
                while (true)
                {
                    SkipWhitespace();
                    if (position >= text.Length || text[position] != '"')
                    {
                        throw Error("Expected a member name");
                    }
                    string key = ReadString();
                    SkipWhitespace();
                    if (!TryConsume(':'))
                    {
                        throw Error("Expected ':' after a member name");
                    }
                    SkipWhitespace();
                    result.Set(key, ReadValue());
                    SkipWhitespace();
                    if (TryConsume(','))
                    {
                        continue;
                    }
                    if (TryConsume('}'))
                    {
                        return result;
                    }
                    throw Error("Expected ',' or '}' in an object");
                }
            }

            private List<object> ReadArray()
            {
                List<object> result = new();
                position++;
                SkipWhitespace();
                if (TryConsume(']'))
                {
                    return result;
                }
                while (true)
                {
                    SkipWhitespace();
                    result.Add(ReadValue());
                    SkipWhitespace();
                    if (TryConsume(','))
                    {
                        continue;
                    }
                    if (TryConsume(']'))
                    {
                        return result;
                    }
                    throw Error("Expected ',' or ']' in an array");
                }
            }

            private string ReadString()
            {
                StringBuilder builder = new();
                position++;
                while (true)
                {
                    if (position >= text.Length)
                    {
                        throw Error("Unterminated string");
                    }
                    char current = text[position++];
                    if (current == '"')
                    {
                        return builder.ToString();
                    }
                    if (current != '\\')
                    {
                        builder.Append(current);
                        continue;
                    }
                    if (position >= text.Length)
                    {
                        throw Error("Unterminated escape sequence");
                    }
                    char escape = text[position++];
                    switch (escape)
                    {
                        case '"':
                        case '\\':
                        case '/':
                            builder.Append(escape);
                            break;
                        case 'b':
                            builder.Append('\b');
                            break;
                        case 'f':
                            builder.Append('\f');
                            break;
                        case 'n':
                            builder.Append('\n');
                            break;
                        case 'r':
                            builder.Append('\r');
                            break;
                        case 't':
                            builder.Append('\t');
                            break;
                        case 'u':
                            if (position + 4 > text.Length
                                || !ushort.TryParse(text.Substring(position, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ushort code))
                            {
                                throw Error("Invalid unicode escape");
                            }
                            builder.Append((char)code);
                            position += 4;
                            break;
                        default:
                            throw Error($"Unknown escape sequence '\\{escape}'");
                    }
                }
            }

            private JsonNumber ReadNumber()
            {
                int start = position;
                while (position < text.Length && "+-0123456789.eE".IndexOf(text[position]) >= 0)
                {
                    position++;
                }
                if (position == start)
                {
                    throw Error($"Unexpected character '{text[position]}'");
                }
                string token = text[start..position];
                if (!double.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out _))
                {
                    throw Error($"Invalid number '{token}'");
                }
                return new JsonNumber(token);
            }

            private void ReadLiteral(string literal)
            {
                if (string.CompareOrdinal(text, position, literal, 0, literal.Length) != 0)
                {
                    throw Error($"Expected '{literal}'");
                }
                position += literal.Length;
            }

            private bool TryConsume(char expected)
            {
                if (position < text.Length && text[position] == expected)
                {
                    position++;
                    return true;
                }
                return false;
            }

            private void SkipWhitespace()
            {
                while (position < text.Length && char.IsWhiteSpace(text[position]))
                {
                    position++;
                }
            }

            private FormatException Error(string message)
            {
                return new FormatException($"{message} at character {position} of the package manifest");
            }
        }

        private static class JsonWriter
        {
            private const string Indent = "  ";

            public static string Write(JsonObject root)
            {
                StringBuilder builder = new();
                WriteValue(builder, root, 0);
                builder.Append('\n');
                return builder.ToString();
            }

            private static void WriteValue(StringBuilder builder, object value, int depth)
            {
                switch (value)
                {
                    case null:
                        builder.Append("null");
                        break;
                    case string text:
                        WriteString(builder, text);
                        break;
                    case bool flag:
                        builder.Append(flag ? "true" : "false");
                        break;
                    case JsonNumber number:
                        builder.Append(number.Text);
                        break;
                    case JsonObject jsonObject:
                        WriteObject(builder, jsonObject, depth);
                        break;
                    case List<object> array:
                        WriteArray(builder, array, depth);
                        break;
                    default:
                        throw new InvalidOperationException($"Unsupported JSON value of type {value.GetType().Name}");
                }
            }

            private static void WriteObject(StringBuilder builder, JsonObject jsonObject, int depth)
            {
                if (jsonObject.Members.Count == 0)
                {
                    builder.Append("{}");
                    return;
                }
                builder.Append("{\n");
                for (int index = 0; index < jsonObject.Members.Count; index++)
                {
                    AppendIndent(builder, depth + 1);
                    WriteString(builder, jsonObject.Members[index].Key);
                    builder.Append(": ");
                    WriteValue(builder, jsonObject.Members[index].Value, depth + 1);
                    builder.Append(index < jsonObject.Members.Count - 1 ? ",\n" : "\n");
                }
                AppendIndent(builder, depth);
                builder.Append('}');
            }

            private static void WriteArray(StringBuilder builder, List<object> array, int depth)
            {
                if (array.Count == 0)
                {
                    builder.Append("[]");
                    return;
                }
                builder.Append("[\n");
                for (int index = 0; index < array.Count; index++)
                {
                    AppendIndent(builder, depth + 1);
                    WriteValue(builder, array[index], depth + 1);
                    builder.Append(index < array.Count - 1 ? ",\n" : "\n");
                }
                AppendIndent(builder, depth);
                builder.Append(']');
            }

            private static void WriteString(StringBuilder builder, string text)
            {
                builder.Append('"');
                foreach (char current in text)
                {
                    switch (current)
                    {
                        case '"':
                            builder.Append("\\\"");
                            break;
                        case '\\':
                            builder.Append("\\\\");
                            break;
                        case '\b':
                            builder.Append("\\b");
                            break;
                        case '\f':
                            builder.Append("\\f");
                            break;
                        case '\n':
                            builder.Append("\\n");
                            break;
                        case '\r':
                            builder.Append("\\r");
                            break;
                        case '\t':
                            builder.Append("\\t");
                            break;
                        default:
                            if (current < ' ')
                            {
                                builder.Append("\\u").Append(((int)current).ToString("x4", CultureInfo.InvariantCulture));
                            }
                            else
                            {
                                builder.Append(current);
                            }
                            break;
                    }
                }
                builder.Append('"');
            }

            private static void AppendIndent(StringBuilder builder, int depth)
            {
                for (int index = 0; index < depth; index++)
                {
                    builder.Append(Indent);
                }
            }
        }
    }
}
