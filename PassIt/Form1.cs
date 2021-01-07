using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Principal;
using System.Security.Claims;

namespace PassIt
{
    public partial class PassItVPN : Form
    {
        private bool IsAdmin
        {
            get
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                if (identity != null)
                {
                    WindowsPrincipal principal = new WindowsPrincipal(identity);
                    List<Claim> list = new List<Claim>(principal.UserClaims);
                    Claim c = list.Find(p => p.Value.Contains("S-1-5-32-544"));
                    if (c != null)
                        return true;
                }
                return false;
            }
        }

        public PassItVPN()
        {
            if (IsAdmin == true)
            {
                InitializeComponent();
            }
            else if(IsAdmin == false)
            {
                MessageBox.Show("Start PassItVPN as an administrator!");
                Application.Exit();
            }
        }

        private void connectButton(object sender, EventArgs e)
        {
            if (ConnectButton.Text == "CONNECT")
            {
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = true; //Hide openVPN Console
                startInfo.FileName = @"C:\Program Files\OpenVPN\bin\openvpn.exe"; //OPENVPN Path
                startInfo.Arguments = "--config serveropenvpn.ovpn"; //OpenVPN cert
                //startInfo.Verb = "runas"; //Run As Admin
                process.StartInfo = startInfo;
                process.Start();
                MessageBox.Show("Succesfully connected to your own PassItVPN Server!.");
                progressBar1.Value = 100;

                ConnectButton.Text = "DISCONNECT";
            }
            else
            {
                Disconnect();
                ConnectButton.Text = "CONNECT";
            }
        }

        private void Disconnect()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "taskkill",
                Arguments = $"/f /im openvpn.exe",
                CreateNoWindow = true,
                Verb = "runas",
                UseShellExecute = false
            }).WaitForExit();
            progressBar1.Value = 0;
        }

        private void LogoClick(object sender, EventArgs e)
        {

        }
    }
}
