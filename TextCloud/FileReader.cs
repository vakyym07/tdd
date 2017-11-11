using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextCloud
{
    public class FileReader: IReader
    {
        private readonly string pathToFile;

        public FileReader(string path)
        {
            pathToFile = path;
        }

        public string GetData()
        {
            return File.ReadAllText(pathToFile);
        }
    }
}
