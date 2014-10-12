using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteXMLFiles
{
    class Program
    {
        static void Main(string[] args)
        {


            // the path we will be saving the files
            string path = @"C:\temp\";
            // if c:\temp not there, dreate
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            /**** 
             * Get the preloaded XML files
             * read them locally
             * write them to a temp directory
             * for MVCApp to use
             * *****/
            string[] dirs = Directory.GetFiles(@".\LoadXML\", "temp?.xml");
            foreach (var filename in dirs)
            {
                string data = File.ReadAllText(filename);
                string name = Path.GetFileName(filename);

                File.WriteAllText(path + name, data);
            }



        }
    }
}
