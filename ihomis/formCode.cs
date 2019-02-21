using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ihomis
{
    public partial class formCode : Form
    {
        private Form1 form1;
        private String passcode = "@doh7systems";
        public formCode()
        {
            InitializeComponent();
        }

        public formCode(Form1 form)
        {
            this.form1 = form;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(txtPasscode.Text != passcode)
            {
                MessageBox.Show("Passcode did not match");
            }
            else
            {
                Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                string key = ConfigurationManager.AppSettings["hospitalcode"];

                if(key == null)
                {
                    configuration.AppSettings.Settings.Add("hospitalcode", txtCode.Text);
                    configuration.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                    this.form1.SetCode(ConfigurationManager.AppSettings["hospitalcode"]);
                }
                else
                {
                    configuration.AppSettings.Settings["hospitalcode"].Value = txtCode.Text;
                    configuration.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");
                    this.form1.SetCode(ConfigurationManager.AppSettings["hospitalcode"]);
                }
                
                this.Close();
            }
        }

        private void formCode_Load(object sender, EventArgs e)
        {

        }
    }
}
