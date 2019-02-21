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
    public partial class Form1 : Form
    {
        private DBConn conn;
        public Form1()
        {
            conn = new DBConn();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(txt_code.Text == null)
            {
                if (dtp_datefrom.Value == null)
                {
                    conn.connect();
                    String hospitalCode = txt_code.Text;
                    String fromMonth = "";
                    if (dtp_datefrom.Value.Month < 10)
                    {
                        fromMonth = "0" + dtp_datefrom.Value.Day.ToString();
                    }
                    else
                    {
                        fromMonth = dtp_datefrom.Value.Month.ToString();
                    }
                    String month = dtp_datefrom.Value.ToString("MMM");
                    String dateFrom = dtp_datefrom.Value.Year.ToString() + "-" + fromMonth + "-01";
                    String dateTo = dtp_datefrom.Value.ToString("yyyy-MM-dd");
                    String year = dtp_datefrom.Value.ToString("yyyy");
                    conn.SubmitData(dateFrom, dateTo, hospitalCode, month, year);
                }
                else
                    MessageBox.Show("Date is not set.");
            }
            else
            {
                MessageBox.Show("You must set a hospital code first.");
                formCode code = new formCode(this);
                code.ShowDialog();
            }
            

            
            conn.close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txt_code.Text = ConfigurationManager.AppSettings["hospitalcode"];
            txt_code.Enabled = false;
            txt_query.Visible = false;
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            formCode code = new formCode(this);
            code.ShowDialog();
        }
        public void SetCode(String code)
        {
            txt_code.Text = code;
        }
    }
}
