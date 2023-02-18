using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace acvm_desktop.Install
{
    public partial class DownloadF : Form
    {
        IForm f1;
        MainWindow mw;
        public DownloadF(IForm main, MainWindow mww)
        {
            f1 = main;
            mw = mww;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            WebClient wc = new WebClient();
            var sP = System.Windows.Forms.Application.StartupPath;  
            System.IO.Directory.CreateDirectory(sP + @"\Download");
            Uri uri = new Uri("https://corretto.aws/downloads/latest/amazon-corretto-" + f1.listBox1.GetItemText(f1.listBox1.SelectedItem) + "-x64-windows-jdk.zip");
            wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
            wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
            if (System.IO.File.Exists(sP + @"\Download\corretto-" + f1.listBox1.GetItemText(f1.listBox1.SelectedItem) + ".zip"))
            {
                System.IO.File.Delete(sP + @"\Download\corretto-" + f1.listBox1.GetItemText(f1.listBox1.SelectedItem) + ".zip");
            }   
            wc.DownloadFileAsync(uri, sP + @"\Download\corretto-" + f1.listBox1.GetItemText(f1.listBox1.SelectedItem) + ".zip");
            wc.Dispose();
        }

        public static string ToReadableSize(double size, int scale = 0, int standard = 1024)
        {
            var unit = new[] { "B", "KB", "MB", "GB" };
            if (scale == unit.Length - 1 || size <= standard)
            {
                return $"{size.ToString(".##")} {unit[scale]}";
            }
            return ToReadableSize(size / standard, scale + 1, standard);
        }

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
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

        private void Wc_DownloadFileCompleted(object? sender, AsyncCompletedEventArgs e)
        {
            var sP = System.Windows.Forms.Application.StartupPath;
            DirectoryInfo di = new DirectoryInfo(sP + "\\Temp\\" + f1.listBox1.GetItemText(f1.listBox1.SelectedItem));
            if (di.Exists)
            {
                di.Delete(true);
            }
            System.IO.Directory.CreateDirectory(sP + "\\Temp");
            System.IO.Directory.CreateDirectory(sP + "\\Temp\\" + f1.listBox1.GetItemText(f1.listBox1.SelectedItem));
            if (System.IO.File.Exists(sP + "\\Download"))
            {
                System.IO.Directory.Delete(sP + "\\Download", true);
            }
            if (!System.IO.File.Exists("C:\\Program Files\\Amazon Corretto"))
            {
                System.IO.Directory.CreateDirectory("C:\\Program Files\\Amazon Corretto");
            }
            ZipFile.ExtractToDirectory(sP + @"\Download\corretto-" + f1.listBox1.GetItemText(f1.listBox1.SelectedItem) + ".zip", sP + "\\Temp\\" + f1.listBox1.GetItemText(f1.listBox1.SelectedItem));
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
            acvm_desktop.Install.Completed form = new acvm_desktop.Install.Completed(   );
            form.TopLevel = false;
            form.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top);

            mw.panel2.Controls.Remove(this);
            mw.panel2.Controls.Add(form);

            form.Show();
        }


        private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            dpce = e;
            timer.Tick += Timer_Tick;
            timer.Interval =  500;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            label3.Text = dpce.ProgressPercentage + "% 完了、" + ToReadableSize(dpce.TotalBytesToReceive) + " 中 " + ToReadableSize(dpce.BytesReceived) + " 取得済み。";
            progressBar1.Value = dpce.ProgressPercentage;
        }

        private void DownloadF_Load(object sender, EventArgs e)
        {

        }
    }
}
