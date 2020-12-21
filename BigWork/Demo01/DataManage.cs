using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Baidu;
using Newtonsoft;
using Baidu.Aip.Face;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SQLite;

namespace Demo01
{
    public partial class DataManage : Form
    {
        #region  
        private string appKey = "v3l0duXNp0D6zsvRAaEEGjiR";
        private string sKey = "MLQ7eocOmdGgzTw32tiaQqN1na7fF9K4";
        private string TOKEN = "";
        private string strSqlPath = "Data source=userInfo.db";
        #endregion

        public void getAccessToken()
        {
            string httpRequestToken = $"https://aip.baidubce.com/oauth/2.0/token?grant_type=client_credentials&client_id={appKey}&client_secret={sKey}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(httpRequestToken);
            request.Method = "Get";
            WebResponse response = request.GetResponse();

            var test = new StreamReader(response.GetResponseStream());
            var result = test.ReadToEnd();
            Console.WriteLine(result);
            var json = (JObject)JsonConvert.DeserializeObject(result);

            TOKEN = json["access_token"].ToString();
        }

        public DataManage()
        {
            InitializeComponent();
            getAccessToken();
        }

        /// <summary>
        /// 查询组信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectGroup_Click(object sender, EventArgs e)
        {
            groupGetlist();
        }   
        
        // 组列表查询
        public void groupGetlist()
        {
            string host = "https://aip.baidubce.com/rest/2.0/face/v3/faceset/group/getlist?access_token=" + TOKEN;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;
            String str = "{\"start\":0,\"length\":100}";
            byte[] buffer = encoding.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string result = reader.ReadToEnd();
            Console.WriteLine("组列表查询:");
            Console.WriteLine(result);
            var json = (JObject)JsonConvert.DeserializeObject(result);
            var groupArr = JsonConvert.DeserializeObject<List<string>>(json["result"]["group_id_list"].ToString());
            if (groupArr.Count > 0 && this.lbGroups.Items.Count > 0)
                this.lbGroups.Items.Clear();
            foreach (var item in groupArr)
            {
                this.lbGroups.Items.Add(item);
            }
        }

        /// <summary>
        /// 新建组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            AddGroup dlgAddGroup = new AddGroup();
            dlgAddGroup.ShowDialog();
            if(dlgAddGroup.bOK)
            {
                groupAdd(dlgAddGroup.strGroupName);
            }
        }

        // 创建用户组
        public void groupAdd(string strGroupName)
        {
            string host = $"https://aip.baidubce.com/rest/2.0/face/v3/faceset/group/add?access_token={TOKEN}";

            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;
            String str = "{\"group_id\":\"" + strGroupName + "\"}";
            byte[] buffer = encoding.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string result = reader.ReadToEnd();
            Console.WriteLine("创建用户组:");
            Console.WriteLine(result);
            var json = (JObject)JsonConvert.DeserializeObject(result);

            if(json["error_code"].ToString().Equals("0"))
            {
                MessageBox.Show("创建成功！");
            }
            else
            {
                MessageBox.Show("创建失败，错误信息：" + json["error_code"].ToString() + json["error_msg"].ToString());
            }
        }

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {

        }

        // 获取用户列表
        public  void groupGetusers(string strGroupID)
        {
            string host = "https://aip.baidubce.com/rest/2.0/face/v3/faceset/group/getusers?access_token=" + TOKEN;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;
            String str = "{\"group_id\":\"" + strGroupID + "\"}";
            byte[] buffer = encoding.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string result = reader.ReadToEnd();
            Console.WriteLine("获取用户列表:");
            Console.WriteLine(result);

            var json = (JObject)JsonConvert.DeserializeObject(result);
            var users = JsonConvert.DeserializeObject<List<string>>(json["result"]["user_id_list"].ToString());
            if (users.Count > 0 && this.lbGroups.Items.Count > 0)
                this.listView1.Items.Clear();
            foreach (var item in users)
            {
                this.listView1.Items.Add(new ListViewItem(new string[]
                    {
                        item,
                        GetUserInfoFromSQL(item)
                    }));
                ;
            }
        }

        // 用户信息查询
        public string userGet(string strGroupID, string strUserID)
        {
            string host = "https://aip.baidubce.com/rest/2.0/face/v3/faceset/user/get?access_token=" + TOKEN;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;
            String str = "{\"user_id\":\"" + strUserID + "\",\"group_id\":\"" + strGroupID+"\"}";
            byte[] buffer = encoding.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string result = reader.ReadToEnd();
            Console.WriteLine("用户信息查询:");
            Console.WriteLine(result);
            return result;
        }

        private string GetUserInfoFromSQL(string strUserId)
        {
            using (SQLiteConnection conn = new SQLiteConnection(strSqlPath))
            {
                string strSQL = "select * from userInfo where userId = \"" + strUserId + "\"";

                using (SQLiteCommand cmd = new SQLiteCommand(strSQL, conn))
                {
                    conn.Open();
                    using (SQLiteDataReader dbReader = cmd.ExecuteReader())
                    {
                        if(dbReader.HasRows)
                        {
                            while(dbReader.Read())
                            {
                                string strUserName = dbReader.GetString(1);
                                Console.WriteLine(dbReader.GetString(0) + dbReader.GetString(1));
                                return strUserName;
                            }
                        }
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 用户组选择改变的时候更新对应的用户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.lbGroups.Items.Count > 0 && this.lbGroups.SelectedItem != null)
            groupGetusers(this.lbGroups.SelectedItem.ToString());
        }

        /// <summary>
        /// 注册用户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
