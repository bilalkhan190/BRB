using System;
using System.IO;
using System.Reflection;

namespace MatechEssential
{

    public class LogHelper
    {
        string FilePath = "";
        string FileName = "";
        public bool DailyFile { get; set; }

        public LogHelper(bool dailyFile = true)
        {
            FilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            FileName = "log.txt";
            DailyFile = dailyFile;
        }

        public void Add(string message)
        {
            try
            {
                string fName = Path.GetFileNameWithoutExtension(FileName) + (DailyFile == true ? DateTime.Today.ToString("-MM-dd-yyyy") : "") + Path.GetExtension(FileName);
                if (!Directory.Exists(FilePath))
                    Directory.CreateDirectory(FilePath);
                StreamWriter sw = new StreamWriter(new FileStream(FilePath + "\\" + fName, FileMode.Append));
                sw.WriteLine(DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss tt") + ": " + message);
                sw.Close();
            }
            catch (Exception ex)
            {

            }
        }
    }
}