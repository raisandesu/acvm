using System.ComponentModel;
using System.IO.Compression;
using System.Net;

namespace acvm
{
    class Program
    {
        public string[] Aargs;
        public int Left;
        public int Top;
        DownloadProgressChangedEventArgs dpce = null;

        private static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }

        private static void Wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            var classe = new Program();
            string[] args = null;
            args = classe.Aargs;

            Console.WriteLine("Amazon Corretto を展開しています...");

            for (int i = 0; i < classe.Aargs.Length; i++)
            {
                var sP = AppDomain.CurrentDomain.BaseDirectory;
                DirectoryInfo di = new DirectoryInfo(sP + "\\Temp\\" + args[1]);
                if (di.Exists)
                {
                    di.Delete(true);
                }
                System.IO.Directory.CreateDirectory(sP + "\\Temp");
                System.IO.Directory.CreateDirectory(sP + "\\Temp\\" + args[1]);
                if (System.IO.File.Exists(sP + "\\Download"))
                {
                    System.IO.Directory.Delete(sP + "\\Download", true);
                }
                if (!System.IO.File.Exists("C:\\Program Files\\Amazon Corretto"))
                {
                    System.IO.Directory.CreateDirectory("C:\\Program Files\\Amazon Corretto");
                }
                ZipFile.ExtractToDirectory(sP + @"\Download\corretto-" + args[1] + ".zip", sP + "\\Temp\\" + args[1]);
                DirectoryInfo[] files = di.GetDirectories();
                foreach (DirectoryInfo dir in files)
                {
                    if (System.IO.File.Exists("C:\\Program Files\\Amazon Corretto\\acvm"))
                    {
                        Directory.Delete("C:\\Program Files\\Amazon Corretto\\acvm");
                        CopyFilesRecursively(dir.FullName, "C:\\Program Files\\Amazon Corretto\\acvm");
                    }
                    else
                    {
                        CopyFilesRecursively(dir.FullName, "C:\\Program Files\\Amazon Corretto\\acvm");
                    }
                }
                Console.WriteLine("Amazon Corretto のインストールが完了しました");
            }
        }


        static void Main(string[] args)
        {
            var classe = new Program();
            classe.Aargs = args;
            for (int i = 0; i < args.Length; i++)
            {
                if ((i == 0 & args[i] == "--install") & (i == 1 & args[1] == "8" | args[1] == "11" | args[1] == "15" | args[1] == "16" | args[1] == "17" | args[1] == "19"))
                {
                    Console.WriteLine("Amazon Corretto バージョン  " + args[1] + " をダウンロードしています...");

                    classe.Left = Console.CursorLeft;
                    classe.Top = Console.CursorTop;

                    WebClient wc = new WebClient();
                    var sP = AppDomain.CurrentDomain.BaseDirectory;
                    System.IO.Directory.CreateDirectory(sP + @"\Download");
                    Uri uri = new Uri("https://corretto.aws/downloads/latest/amazon-corretto-" + args[1] + "-x64-windows-jdk.zip");
                    wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
                    if (System.IO.File.Exists(sP + @"\Download\corretto-" + args[1] + ".zip"))
                    {
                        System.IO.File.Delete(sP + @"\Download\corretto-" + args[1] + ".zip");
                    }
                    wc.DownloadFileAsync(uri, sP + @"\Download\corretto-" + args[1] + ".zip");

                    wc.Dispose();
                }
                else
                {
                }
            }
            Console.ReadLine();
        }

    }
}