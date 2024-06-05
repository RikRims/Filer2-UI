using System.IO;

namespace Filer2_UI.Services;
public class Log
{
    public static void LogCreate(string path)
    {
        using(StreamWriter fs = new StreamWriter(path + "\\Log.log", true))
        {
            fs.WriteLine(DateTime.Now.ToString() + " => ЗАПУСК ПРОГРАММЫ");
        }
    }

    public static void LogAdd(string path, string textAdd)
    {
        using(StreamWriter fs = new StreamWriter(path + "\\Log.log", true))
        {
            fs.WriteLine(textAdd);
        }
    }

    public static string GetLog(string path) 
    {
        using(StreamReader fs = new StreamReader(path + "\\Log.log"))
        {
           return fs.ReadToEnd();
        }
    }
}
