using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace acvm_desktop
{
    public partial class IForm : Form
    {
        MainWindow f1;
        public IForm(MainWindow main)
        {
            f1 = main;
            InitializeComponent();
        }

        System.Net.WebClient wc = new System.Net.WebClient();
        private void button1_Click(object sender, EventArgs e)
        {
            acvm_desktop.Install.DownloadF form = new acvm_desktop.Install.DownloadF(this, f1);
            form.TopLevel = false;
            form.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top);

            f1.panel2.Controls.Remove(this);
            f1.panel2.Controls.Add(form);

            form.Show();
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }
    }
}
