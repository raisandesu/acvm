using Windows.Networking.Connectivity;

namespace acvm_desktop
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void MainWindow_Load(object sender, EventArgs e)
        {

            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;


            Menubar menubar = new Menubar();
            menubar.TopLevel= false;
            menubar.Anchor= (AnchorStyles.Left | AnchorStyles.Right);
            panel1.Controls.Add(menubar);
            menubar.Show();

            var profile = NetworkInformation.GetInternetConnectionProfile();
            if (profile != null)
            {
                var connectivityLevel = profile.GetNetworkConnectivityLevel();
                if (connectivityLevel != NetworkConnectivityLevel.InternetAccess)
                {
                    NoInternet ni = new NoInternet();
                    ni.TopLevel = false;
                    panel2.Controls.Add(ni);
                    ni.Show();
                }
            }
 

                IForm install = new IForm(this);
                install.TopLevel = false;
                install.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top);
                panel2.Controls.Add(install);
                install.Show();
            }

        private void NetworkInformation_NetworkStatusChanged(object sender)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(this.ChangeInternetStatus));
                return;
            }

           

        }

        private void ChangeInternetStatus()
        {
            var profile = NetworkInformation.GetInternetConnectionProfile();
            if (profile != null)
            {
                var connectivityLevel = profile.GetNetworkConnectivityLevel();
                if (connectivityLevel != NetworkConnectivityLevel.InternetAccess)
                {
                    panel2.Controls.Clear();
                    NoInternet ni = new NoInternet();
                    ni.TopLevel = false;
                    this.panel2.Controls.Add(ni);
                    ni.Show();
                }
                else if (connectivityLevel == NetworkConnectivityLevel.InternetAccess)
                {
                    {
                        panel2.Controls.Clear();
                        IForm install = new IForm(this);
                        install.TopLevel = false;
                        install.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top);
                        this.panel2.Controls.Add(install);
                        install.Show();
                    }
                }
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}