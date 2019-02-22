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
        private DBConn conn = new DBConn();
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
            if(txtPasscode.Text.Trim() != passcode)
            {
                MessageBox.Show("Passcode did not match");
            }
            else
            {
                if (txtCode.Text.Trim() == "")
                {
                    MessageBox.Show("Hospital code is required.");
                    return;
                }
                if(txtHospitalName.Text.Trim() == "")
                {
                    MessageBox.Show("Hospital name is required.");
                    return;
                }
                if(txtHospitalAddress.Text.Trim() == "")
                {
                    MessageBox.Show("Hospital address is required");
                    return;
                }

                setConfig("hospitalcode",txtCode.Text);
                setConfig("hospitalname", txtHospitalName.Text);
                setConfig("hospitaladdress", txtHospitalAddress.Text);
                conn.SubmitHospitalInfo(txtCode.Text,txtHospitalName.Text,txtHospitalAddress.Text);
                form1.SetCode(txtCode.Text);
                this.Close();
            }
        }


        public void setConfig(String key, String value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            String configValue = "";

            configValue = ConfigurationManager.AppSettings[key];
            if (configValue == null)
            {
                configuration.AppSettings.Settings.Add(key, value);
                configuration.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            else
            {
                configuration.AppSettings.Settings[key].Value = value;
                configuration.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
        public String getConfigValue(String key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        private void formCode_Load(object sender, EventArgs e)
        {

        }
        
        public void SetInfo()
        {
            txtCode.Text = getConfigValue("hospitalcode");
            txtHospitalName.Text = getConfigValue("hospitalname");
            txtHospitalAddress.Text =  getConfigValue("hospitaladdress");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPasscode_TextChanged(object sender, EventArgs e)
        {
            if (txtPasscode.Text == passcode)
            {
                SetInfo();
            }
            else
            {
                txtCode.Text = "";
                txtHospitalName.Text = "";
                txtHospitalAddress.Text = "";
            }
        }
    }
}
