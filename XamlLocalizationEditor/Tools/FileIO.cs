using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xaml;
using XamlLocalizationEditor.Models;
using XamlReader = System.Windows.Markup.XamlReader;

namespace XamlLocalizationEditor.Tools
{
    public static class FileIO
    {
        public static string[] FindAllSupportedXamlFiles(string path)
        {
            var dirs = Directory.GetDirectories(path).ToList();

            dirs = dirs.Where(d => !d.Contains(".git") && !d.Contains(".vscode") && !d.Contains(".vs") && !d.Contains(".idea")
                && !d.Contains("_build") && !d.Contains("ci") && !d.Contains("bin") && !d.Contains("obj")).ToList();

            List<string> filenames = new();

            var dirfiles = Directory.GetFiles(path).Where(s => s.EndsWith(".xaml")).ToList();

            foreach (var f in dirfiles)
            {
                if (TestXamlFile(f))
                    filenames.Add(f);
            }

            foreach (var dir in dirs)
            {
                var files = FindAllSupportedXamlFiles(dir);
                filenames.AddRange(files);
            }

            return filenames.ToArray();
        }

        private static bool TestXamlFile(string filepath)
        {
            using var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            using var xmlReader = System.Xml.XmlReader.Create(stream);
            using var reader = new XamlXmlReader(xmlReader);

            while (reader.Read())
            {
                if (reader.NodeType == XamlNodeType.StartObject)
                {
                    var type = reader.Type;

                    if (type == null)
                        continue;

                    bool allowed = type.UnderlyingType == typeof(string) || type.Name == "ResourceDictionary";
                    if (!allowed)
                        return false;
                }
            }

            return true;
        }


        public static LanguageResources OpenXamlResources(string filepath)
        {
            if (!Path.Exists(filepath))
            {
                throw new Exception("file not found");
            }

            FileStream stream = new(filepath, FileMode.Open);
            var obj = XamlReader.Load(stream);

            if (obj is not ResourceDictionary resources)
            {
                throw new Exception("File is not ResourceDictionary");
            }

            bool ok = resources.Values.Cast<object>().All(v => v is string);
            if (!ok) throw new Exception("File not supported");

            var code = GetLangCodeFromFilename(Path.GetFileNameWithoutExtension(filepath));
            if (code == "")
                code = "ru-RU";

            var languageRes = new LanguageResources(code, Path.GetFileName(filepath));

            foreach (var item in resources.Keys) 
            {
                var valueString = "";
                if (resources?[item] != null)
                    valueString = resources[item].ToString();

                languageRes.Strings.Add(item.ToString(), valueString);
            }

            return languageRes;
        }

        private static string GetLangCodeFromFilename(string filename)
        {
            var langCodeRegex = @"[a-z]{2,3}(-[A-Za-z]{2,8})+";

            var match = Regex.Match(filename, langCodeRegex, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return match.Value;
            }
            else
                return "";
        }

        public static void SaveXaml(LanguageResources lang, string path)
        {
            if (string.IsNullOrEmpty(lang.Filename)) return;

            const string title = @"<ResourceDictionary xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                xmlns:x = ""http://schemas.microsoft.com/winfx/2006/xaml"" xmlns:system = ""clr-namespace:System;assembly=mscorlib"" >";
            const string p1 = @"<system:String x:Key =""";
            const string p2 = "</system:String>";
            const string lastp = "</ResourceDictionary>";

            using StreamWriter file = File.CreateText(Path.Combine(path, lang.Filename));

            file.WriteLine(title);

            foreach (var item in lang.Strings)
            {
                file.WriteLine(p1 + item.Key + @""">" + item.Value + p2);
            }

            file.WriteLine(lastp);
        }

        public static LanguagesCollection LoadFromTxt(string filepath)
        {
            var text = File.ReadAllLines(filepath);
            
            if (text.Length < 2) throw new("Unsupported txt file format");

            List<LanguageResources> resources = new();

            var columns = text[0].Split("\t"); 
            for (int i = 1; i < columns.Length; i++)
            {
                if (!CultureTools.IsValidLanguageCode(columns[i])) throw new("Unsupported txt file format");
                resources.Add(new(columns[i], ""));
            }

            foreach (var line in text.Skip(1))
            {
                var textCols = line.Split("\t");
                var key = textCols[0];

                for(int i = 1; i < textCols.Length; i++)
                {
                    resources[i - 1].Strings.Add(key, textCols[i]);
                }
            }

            LanguagesCollection lang = new(filepath);
            lang.Resources = new(resources);

            return lang;
        }
    }
}
