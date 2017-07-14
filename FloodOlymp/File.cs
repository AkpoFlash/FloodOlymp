using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloodOlymp
{
    class File
    {
        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public File(string fileName)
        {
            this.FileName = fileName;
        }


        public string ReadFromFile()
        {
            string fileContent = "";

            try
            {
                using (StreamReader stream = new StreamReader(this.FileName))
                {
                    fileContent = stream.ReadToEnd();
                    stream.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return fileContent;
        }


        public string[] WriteToFile(string[] fileContent)
        {
            try
            {
                using (StreamWriter stream = new StreamWriter(this.FileName))
                {
                    for (int i = 0; i < fileContent.Length; i++)
                    {
                        stream.WriteLine(fileContent[i]);
                    }
                    stream.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return fileContent;
        }


    }
}
