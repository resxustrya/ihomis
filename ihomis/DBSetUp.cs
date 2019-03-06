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
    public partial class DBSetUp : Form
    {
        private Form1 form1;
        public DBSetUp()
        {
            InitializeComponent();
        }
        public DBSetUp(Form1 form1)
        {
            this.form1 = form1;
            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            setConfig("ip", txtIP.Text);
            setConfig("username", txtUsername.Text);
            setConfig("password", txtPassword.Text);
            setConfig("port", txtPort.Text);
            setConfig("dbname", txtDbName.Text);
            MessageBox.Show("Successfully Saved");
            MessageBox.Show("Program needs to close to affect configuration.");
            this.Close();
            this.form1.Close();
            
            
        }
        public void setConfig(String key, String value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            String configValue = "";

            if(key == "port")
            {
                if (value == null)
                    value = "3306";
            }
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
        private void DBSetUp_Load(object sender, EventArgs e)
        {
            txtIP.Text = getConfigValue("ip");
            txtUsername.Text = getConfigValue("username");
            txtPassword.Text = getConfigValue("password");
            txtPort.Text = getConfigValue("port");
            txtDbName.Text = getConfigValue("dbname");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
