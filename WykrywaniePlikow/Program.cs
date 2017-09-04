using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Configuration;
using System.Collections; //Arraylist i StringCollection


namespace WykrywaniePlikow
{
    class Program
    {
        static void Main(string[] args)
        {
            Properties.Settings.Default.Input.CopyTo(Tablice.tablicaInput, 0);
            Properties.Settings.Default.Output.CopyTo(Tablice.tablicaOutput, 0);
            Properties.Settings.Default.Rozszerzenie.CopyTo(Tablice.tablicaRozszerzenia, 0);
            Properties.Settings.Default.Parametry.CopyTo(Tablice.tablicaParametry, 0);
            ArrayList watchers = new ArrayList();
            for(int i = 0; i < Properties.Settings.Default.MaxFolderów; i++)
{
    if (Tablice.tablicaInput[i] == null)
        break;
FileSystemWatcher myWatcher = new System.IO.FileSystemWatcher();
myWatcher.Created += myWatcher_Created;
//if (Tablice.tablicaInput[i] != "") 
myWatcher.Path = Tablice.tablicaInput[i];
myWatcher.EnableRaisingEvents = true;
watchers.Add(myWatcher);
}                
            while (Console.Read() != 'q') ;
        }

        static void myWatcher_Created(object sender, FileSystemEventArgs e)
        {
            var watcher = sender as FileSystemWatcher;
            Konwersja(e, watcher);
        }

      /*  static void watcher_Created(object sender, FileSystemEventArgs e)
        {
            var watcher = sender as FileSystemWatcher;
            Konwersja(e,watcher);
        }*/
        public static void Konwersja(FileSystemEventArgs e,FileSystemWatcher a)
        {
            Console.WriteLine("Dzieje sie!");
            int ktoryWatcher = Properties.Settings.Default.Input.IndexOf(a.Path);
        etykieta:
            try
            {
                string wyjscie = Tablice.tablicaOutput[ktoryWatcher] + "\\" + e.Name;
                string output = Path.ChangeExtension(wyjscie, Tablice.tablicaRozszerzenia[ktoryWatcher]);
                // File.Copy(e.FullPath, Properties.Settings.Default.Output +"\\"+ e.Name);
                for (; ; )
                    if (IsFileClosed(e.FullPath) == true)
                        break;
                Process proc = new Process();
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.FileName = Properties.Settings.Default.FFmpeg;
                proc.StartInfo.Arguments = String.Format(@"-i ""{0}"" {2} ""{1}""", e.FullPath, output, Tablice.tablicaParametry[ktoryWatcher]);
                proc.Start();
                proc.WaitForExit();
                File.Delete(e.FullPath);
            }
            catch (IOException)
            {
                System.Threading.Thread.Sleep(5000);
                goto etykieta;
            }
        }
        public static bool IsFileClosed(string filename)
        {
            try
            {
                using (var inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return true;
                }
            }
            catch (IOException)
            {
                return false;
            }
        }
        public static class Tablice
        {
           public static string[] tablicaInput = new string[Properties.Settings.Default.MaxFolderów];
           public static string[] tablicaOutput = new string[Properties.Settings.Default.MaxFolderów];
           public static string[] tablicaParametry = new string[Properties.Settings.Default.MaxFolderów];
           public static string[] tablicaRozszerzenia = new string[Properties.Settings.Default.MaxFolderów];
        };
    }
}
