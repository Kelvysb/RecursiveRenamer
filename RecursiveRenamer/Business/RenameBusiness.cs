using System.Collections.Generic;
using RecursiveRenamer.Models;

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

        public List<string> Execute(RenamePatern patern, string path)
        {
            List<string> result = new List<string>();

            
            

            return result;
        }        

        public RenamePatern Open(string name)
        {
            RenamePatern result = null;




            return result;
        }        

        public List<string> Save(RenamePatern patern)
        {
            List<string> result = new List<string>();




            return result;
        } 

        public List<string> List()
        {
            List<string> result = new List<string>();




            return result;
        }        

    }
}