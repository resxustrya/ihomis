using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Data;
using System.Configuration;

namespace ihomis
{
    public class DBConn
    {
        private MySqlConnection conn;
        private String connstring;

        public DBConn()
        {
            connstring = "SERVER=" + getConfigValue("ip") + ";PORT=" + getConfigValue("port") + ";DATABASE=" + getConfigValue("dbname")  +";"  + "UID=" + getConfigValue("username") + ";" + "PASSWORD=" + getConfigValue("password") + ";";
            conn = new MySqlConnection(connstring);
        }

        public String getConfigValue(String key)
        {
            return ConfigurationManager.AppSettings[key];
        }
        public void connect()
        {
            try
            {
                this.conn.Open();
            }catch(MySqlException ex)
            {
                MessageBox.Show("Database connection failed.\n" + ex.Message);
            }
        }
        public void close()
        {

            try { this.conn.Close(); } catch { }
        }

        public DataTable GetData(String dateFrom, String dateTo)
        {
            String query = "";
            DataTable result = new DataTable();
            query += "SELECT DISTINCT ";
            query += "(SELECT COUNT(*) FROM henctr where (toecode = 'ER' or toecode = 'ERADM') AND encdate BETWEEN '"+ dateFrom +"' AND '"+ dateTo + "') as ER_COUNT,";
            query += "(SELECT COUNT(*) FROM henctr where toecode = 'OPD' AND encdate BETWEEN '" + dateFrom + "' AND '" + dateTo + "') as OPD_COUNT,";
            query += "(SELECT COUNT(*) FROM henctr where toecode = 'ADM' AND encdate BETWEEN '" + dateFrom + "' AND '" + dateTo + "') as ADM_COUNT ";
            query += "FROM henctr";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            sda.Fill(result);
            this.close();
            return result;
        }

        public bool SubmitData(String dateFrom, String dateto,String hospitalCode,String Month,String year)
        {
            bool ok = true;
            try
            {
                WebRequest request = WebRequest.Create("http://192.168.101.12:3000/API/ihomis/save_encounter");
                ((HttpWebRequest)request).UserAgent = "iHomisApplication";
                request.Method = "POST";

                DataTable result = this.GetData(dateFrom, dateto);
                String uploadate = DateTime.Now.ToString("yyyy/MM/dd");
                String postData = "hospitalCode=" + hospitalCode + "&datefrom=" + dateFrom + "&dateto=" + dateto + "&ER_COUNT=" + result.Rows[0]["ER_COUNT"].ToString() + "&OPD_COUNT=" + result.Rows[0]["OPD_COUNT"].ToString() + "&ADM_COUNT=" + result.Rows[0]["ADM_COUNT"].ToString() + "&month=" + Month + "&year="+ year + "&uploadDate="+ uploadate;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);


                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();

                dataStream = response.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();

                MessageBox.Show(((HttpWebResponse)response).StatusDescription + "\n" + responseFromServer);

                reader.Close();
                dataStream.Close();
                response.Close();


            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return ok;
        }

        public bool SubmitHospitalInfo(String hospitalCode,String hospitalName, String hospitalAddress)
        {
            bool ok = true;
            try
            {
                WebRequest request = WebRequest.Create("http://192.168.101.12:3000/API/ihomis/save_hospital");
                ((HttpWebRequest)request).UserAgent = "iHomisApplication";
                request.Method = "POST";

                String postData = "hospitalCode=" + hospitalCode + "&hospitalName=" + hospitalName + "&hospitalAddress=" + hospitalAddress;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();

                dataStream = response.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();

                MessageBox.Show(((HttpWebResponse)response).StatusDescription + "\n" + responseFromServer);

                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return ok;
        }
    }
}
