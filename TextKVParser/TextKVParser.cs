using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TextKVParser
{
    public static class TextKVParser
    {
        private static readonly char[] TrimChars = {' ', '\n', '\t', '\r'};
        private static readonly char Separator = '=';

        public static T Deserialize<T>(string text) where T : IDictionary<string, string>, new()
        {
            T dic = default(T);
            string[] list;
            if (text != null)
            {
                list = text.Split('\n');
                DeserializeString(out dic, list);
            }

            return dic;
        }

        public static T DeserializeFromFile<T>(string path) where T : IDictionary<string, string>, new()
        {
            T dic = default(T);
            string[] list = File.ReadAllLines(path);
            DeserializeString(out dic, list);
            return dic;
        }

        public static void DeserializeFromFile<T>(string path, out T dic) where T : IDictionary<string, string>, new()
        {
            string[] list = File.ReadAllLines(path);
            DeserializeString(out dic, list);
        }

        private static void DeserializeString<T>(out T dic, string[] list) where T : IDictionary<string, string>, new()
        {
            dic = new T();
            Exception lastException = null;
            for (int i = 0; i < list.Length; i++)
            {
                try
                {
                    string line = list[i];
                    if (line.StartsWith("#")) continue;
                    if (line.StartsWith(";")) continue;
                    int offset = line.IndexOf(Separator);
                    if (offset > 0 && line.Length > offset)
                    {
                        string key = line.Substring(0, offset).Trim(TrimChars);
                        if (string.IsNullOrEmpty(key)) continue;

                        string value = line.Substring(offset + 1).Trim(TrimChars);
                        dic[key] = value;
                    }
                }
                catch (Exception ex)
                {
                    lastException = ex;
                }
            }

            if (lastException != null)
            {
                throw lastException;
            }
        }

        public static void SerializeToFile<T>(T obj, string path) where T : IDictionary<string, string>, new()
        {
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                if (obj == null) return;
                // StringBuilder sb = new StringBuilder();
                foreach (KeyValuePair<string, string> each in obj)
                {
                    string text = string.Format("{0}={1}", each.Key, each.Value);
                    sw.WriteLine(text);
                }
            }
        }

        public static string Serialize<T>(T obj) where T : IDictionary<string, string>
        {
            if (obj == null) return string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> each in obj)
            {
                sb.Append(each.Key);
                sb.Append('=');
                sb.Append(each.Value);
                sb.Append('\n');
            }

            return sb.ToString();
        }
    }
}