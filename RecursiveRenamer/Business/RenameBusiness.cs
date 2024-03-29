using System.Collections.Generic;
using System.IO;
using RecursiveRenamer.Models;
using Newtonsoft.Json;
using System.Linq;
using System.Text.RegularExpressions;
using System;

namespace RecursiveRenamer.Business
{
    public class RenameBusiness
    {
        public string WorkingDir { get; private set; }
        public string CurrentDir { get; private set; }

        public RenameBusiness(string workingDir, string currentDir)
        {
            WorkingDir = workingDir;
            CurrentDir = currentDir;
        }

        public List<string> Execute(RenamePatern pattern, string path, bool simulate, bool complete)
        {
            List<string> result = new List<string>();
            string sourceName;
            string targetName;
            int index = 1;
            string reportPrefix = simulate ? "Simulated" : "Renamed";

            if (Directory.Exists(Path.GetFullPath(path)))
            {

                string[] folders = Directory.GetDirectories(Path.GetFullPath(path));
                string[] files = Directory.GetFiles(Path.GetFullPath(path));

                foreach (string directory in folders)
                {
                    result.AddRange(Execute(pattern, directory, simulate, complete));
                }

                if (string.IsNullOrEmpty(pattern.FolderFilter) || Regex.IsMatch(Path.GetDirectoryName(path), pattern.FolderFilter))
                {
                    foreach (string file in files)
                    {
                        sourceName = Path.GetFileName(file);
                        if (string.IsNullOrEmpty(pattern.FileFilter) || Regex.IsMatch(sourceName, pattern.FileFilter))
                        {
                            if (pattern.FindPatern.StartsWith("<B>"))
                            {
                                targetName = pattern.ReplacePatern.Replace("<B>", "")
                                                                  .Replace("<S>", index.ToString())
                                                                  .Replace("<s>", (index - 1).ToString())
                                                                  + sourceName;
                            }
                            else if (pattern.FindPatern.StartsWith("<E>"))
                            {
                                targetName = sourceName + pattern.ReplacePatern.Replace("<E>", "")
                                                                               .Replace("<S>", index.ToString())
                                                                               .Replace("<s>", (index - 1).ToString());
                            }
                            else if (pattern.FindPatern.StartsWith("<e>"))
                            {
                                targetName = Path.GetFileNameWithoutExtension(sourceName)
                                                        + pattern.ReplacePatern.Replace("<e>", "")
                                                                                .Replace("<S>", index.ToString())
                                                                                .Replace("<s>", (index - 1).ToString())
                                                        + Path.GetExtension(sourceName);

                            }
                            else
                            {
                                targetName = Regex.Replace(sourceName, pattern.FindPatern, pattern.ReplacePatern
                                                                                                .Replace("<S>", index.ToString())
                                                                                                .Replace("<s>", (index - 1).ToString()));
                            }
                            if (!simulate)
                            {
                                try
                                {
                                    File.Move(file, Path.Combine(Path.GetDirectoryName(file), targetName));                                    
                                }
                                catch (Exception e)
                                {                                    
                                    result.Add($"Error: {e.Message} on {sourceName} => {targetName}");
                                }
                            }
                            if (!complete)
                                result.Add($"{reportPrefix}: {sourceName} => {targetName}");
                            else
                                result.Add($"{reportPrefix}: {file} => {Path.Combine(Path.GetDirectoryName(file), targetName)}");
                            index++;
                        }
                    }
                }
            }
            else
            {
                result.Add($"Path not found {path}");
            }
            return result;
        }

        public RenamePatern Open(string name)
        {
            RenamePatern result = null;
            StreamReader file;
            string filePath = Path.Combine(WorkingDir, $"{name}.json");


            if (File.Exists(filePath))
            {
                file = new StreamReader(filePath);
                result = JsonConvert.DeserializeObject<RenamePatern>(file.ReadToEnd());
                file.Close();
                file.Dispose();
                file = null;
            }


            return result;
        }

        public List<string> Save(RenamePatern patern)
        {
            List<string> result = new List<string>();
            StreamWriter file;
            string filePath = Path.Combine(WorkingDir, $"{patern.Name}.json");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                result.Add("Patern Replaced.");
            }

            file = new StreamWriter(filePath);
            file.WriteLine(JsonConvert.SerializeObject(patern, Formatting.Indented));
            file.Close();
            file.Dispose();
            file = null;
            result.Add($"Patern Saved: {patern.Name}");

            return result;
        }

        public List<string> List()
        {
            List<string> result = new List<string>();
            string[] files = Directory.GetFiles(WorkingDir);
            result = files.Select(file => Path.GetFileNameWithoutExtension(file)).ToList();
            return result;
        }

    }
}