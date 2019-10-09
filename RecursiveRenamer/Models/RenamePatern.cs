namespace RecursiveRenamer.Models
{
    public class RenamePatern
    {
        public RenamePatern(string name, string folderFilter, string fileFilter, string findPatern, string replacePatern)
        {
            Name = name;
            FolderFilter = folderFilter;
            FileFilter = fileFilter;
            FindPatern = findPatern;
            ReplacePatern = replacePatern;
        }

        public string Name { get; private set; }
        public string FolderFilter { get; private set; }
        public string FileFilter { get; private set; }
        public string FindPatern { get; private set; }
        public string ReplacePatern { get; private set; }
    }
}