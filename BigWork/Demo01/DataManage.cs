﻿using System;
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
        #endregion
        private string strSqlPath = "Data source=userInfo.db";

        private ContextMenu groupMenu = new ContextMenu();
		private ContextMenu userMenu = new ContextMenu();

        public DataManage()
        {
            InitializeComponent();

			//启动窗口时居中显示
			this.StartPosition = FormStartPosition.CenterScreen;

			MenuItem groupDelete = new MenuItem();
			groupDelete.Text = "删除";
			groupDelete.Click += GroupDelete_Click;
			groupMenu.MenuItems.Add(groupDelete);

			MenuItem userDelete = new MenuItem();
			userDelete.Text = "删除";
			userDelete.Click += UserDelete_Click;
			userMenu.MenuItems.Add(userDelete);
		}

		/// <summary>
		/// 用户信息删除
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UserDelete_Click(object sender, EventArgs e)
		{
			if (this.listView1.SelectedItems != null)
			{
				string strUserId = this.listView1.SelectedItems[0].Text;
				try
				{
					var client = new Face(appKey, sKey);
					var result = client.UserDelete(lbGroups.SelectedItem.ToString(), strUserId);
					Console.WriteLine(result);
					if (result["error_code"].ToString().Equals("0"))
					{
						MessageBox.Show("删除成功");
						this.listView1.Items.Remove(this.listView1.SelectedItems[0]);
					}
					else
					{
						MessageBox.Show("删除失败，错误码：" + result["error_code"].ToString() + "\n错误信息：" + result["error_msg"].ToString());
					}

				}
				catch (Exception exp)
				{
					MessageBox.Show("删除失败，错误信息：" + exp.Message);
				}
			}
			else
			{
				MessageBox.Show("请选中需要删除的数据");
			}
		}

		private void GroupDelete_Click(object sender, EventArgs e)
		{
			if(this.lbGroups.SelectedItem != null)
			{
				try
				{
					var client = new Face(appKey, sKey);
					var result = client.GroupDelete(lbGroups.SelectedItem.ToString());
					Console.WriteLine(result);
					if(result["error_code"].ToString().Equals("0"))
					{
						MessageBox.Show("删除成功");
						this.lbGroups.Items.Remove(this.lbGroups.SelectedItem);
					}
					else
					{
						MessageBox.Show("删除失败，错误码：" + result["error_code"].ToString() + "\n错误信息：" + result["error_msg"].ToString());
					}

				}
				catch(Exception exp)
				{
					MessageBox.Show("删除失败，错误信息：" + exp.Message);
				}
			}
			else
			{
				MessageBox.Show("请选中需要删除的数据");
			}
		}

		/// <summary>
		/// 查询组信息
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSelectGroup_Click(object sender, EventArgs e)
        {
			try
			{
				var client = new Face(appKey, sKey);
				var result = client.GroupGetlist();
				var groupArr = JsonConvert.DeserializeObject<List<string>>(result["result"]["group_id_list"].ToString());
				if (groupArr.Count > 0 && this.lbGroups.Items.Count > 0)
					this.lbGroups.Items.Clear();
				foreach (var item in groupArr)
				{
					this.lbGroups.Items.Add(item);
				}
			}
			catch(Exception exp)
			{
				MessageBox.Show("查询组信息失败，错误信息：" + exp.Message);
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
                if(groupAdd(dlgAddGroup.strGroupName))
                {
                    btnSelectGroup_Click(null, null);
                }
            }
        }

        // 创建用户组
        public bool groupAdd(string strGroupName)
        {
            try
            {
                var client = new Face(appKey, sKey);
                var result = client.GroupAdd(strGroupName);
                Console.WriteLine(result);
                
                if(result["error_code"].ToString().Equals("0"))
                {
                    MessageBox.Show("创建成功！");
                    return true;
                }
                else
                {
                    MessageBox.Show("创建失败，错误码：" + result["error_code"].ToString() +"\n错误信息：" + result["error_msg"].ToString());
                }

            }
            catch (Exception exp)
            {
                MessageBox.Show("创建失败，错误信息：" + exp.Message);
            }
            return false;
        }

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
			UserQuery dlg = new UserQuery();
			dlg.ShowDialog();

			if(dlg.bOk)
			{
				try
				{
					this.listView1.Items.Clear();
					var client = new Face(appKey, sKey);
					var result = client.UserGet(dlg.strUserId, dlg.strGroupId);
					Console.WriteLine(result);
					if (int.Parse(result["error_code"].ToString()) == 0)
					{
						this.listView1.Items.Add(new ListViewItem(new string[]
							 {
							dlg.strUserId,
							result["result"]["user_list"][0]["user_info"].ToString()
							 }));
					}
					else
					{
						MessageBox.Show("获取用户信息失败，错误码：" + result["error_code"].ToString() + "\n错误信息" + result["error_msg"].ToString());
					}
				}
				catch (Exception exp)
				{
					MessageBox.Show("获取用户信息失败，错误信息：" + exp.Message);
				}	
			}
        }

        // 获取用户列表
        public  void groupGetusers(string strGroupID)
        {
            try
            {
                var client = new Face(appKey, sKey);
                var result = client.GroupGetusers(strGroupID);
                Console.WriteLine(result);

                var users = JsonConvert.DeserializeObject<List<string>>(result["result"]["user_id_list"].ToString());
                if (users.Count > 0 && this.lbGroups.Items.Count > 0)
                    this.listView1.Items.Clear();
                foreach (var item in users)
                {
                    this.listView1.Items.Add(new ListViewItem(new string[]
                        {
                            item,
                            GetUserInfo(client, item, strGroupID)
                        }));
                    ;
                }

            }
            catch (Exception exp)
            {

            }
        }

        private string GetUserInfo(Face client, string userId, string groupId)
        {
            try
            {
                var result = client.UserGet(userId, groupId);
                Console.WriteLine(result);
                if(int.Parse(result["error_code"].ToString()) == 0)
                {
                    return result["result"]["user_list"][0]["user_info"].ToString();
                }
                else
                {
                    MessageBox.Show("获取用户信息失败，错误信息：" + result["error_msg"].ToString());
                    return "";
                }
            }catch(Exception exp)
            {
                MessageBox.Show("获取用户信息失败，错误信息：" + exp.Message);
            }
            return "";
        }

        // 用户信息查询
        public void userGet(string strGroupID, string strUserID)
        {
            //string host = "https://aip.baidubce.com/rest/2.0/face/v3/faceset/user/get?access_token=" + TOKEN;
            //Encoding encoding = Encoding.Default;
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            //request.Method = "post";
            //request.KeepAlive = true;
            //String str = "{\"user_id\":\"" + strUserID + "\",\"group_id\":\"" + strGroupID+"\"}";
            //byte[] buffer = encoding.GetBytes(str);
            //request.ContentLength = buffer.Length;
            //request.GetRequestStream().Write(buffer, 0, buffer.Length);
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            //string result = reader.ReadToEnd();
            //Console.WriteLine("用户信息查询:");
            //Console.WriteLine(result);
            //return result;
        }

        private void GetUserInfoFromSQL(string strUserId)
        {
            //using (SQLiteConnection conn = new SQLiteConnection(strSqlPath))
            //{
            //    string strSQL = "select * from userInfo where userId = \"" + strUserId + "\"";

            //    using (SQLiteCommand cmd = new SQLiteCommand(strSQL, conn))
            //    {
            //        conn.Open();
            //        using (SQLiteDataReader dbReader = cmd.ExecuteReader())
            //        {
            //            if(dbReader.HasRows)
            //            {
            //                while(dbReader.Read())
            //                {
            //                    string strUserName = dbReader.GetString(1);
            //                    Console.WriteLine(dbReader.GetString(0) + dbReader.GetString(1));
            //                    return strUserName;
            //                }
            //            }
            //        }
            //    }
            //}
            //return "";
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
			UserRegister userRegister = new UserRegister();
			userRegister.ShowDialog();
        }

		/// <summary>
		/// 鼠标按下的时候需要判断并添加菜单
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lbGroups_MouseDown(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right)
			{
				//设置右键选中功能
				int height = 0;
				for (int i = 0; i < lbGroups.Items.Count; i++)
				{
					height += lbGroups.GetItemHeight(i);
					if (e.Y <= height)
					{
						this.lbGroups.SelectedIndex = i;
						break;
					}
				}
                if(this.lbGroups.SelectedItems.Count > 0)
                    lbGroups.ContextMenu = groupMenu;
			}
		}

		/// <summary>
		/// 用户信息鼠标右键删除
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listView1_MouseDown(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right && this.listView1.SelectedItems.Count > 0)
			{
				this.listView1.ContextMenu = userMenu;
			}
		}

        /// <summary>
        /// 查询签到记录，根据起止时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSignFormSelect_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(strSqlPath))
            {
                string strSQL = $"select * from signform where signtime >= '{this.dateStart.Value.ToString("yyyy-MM-dd")}' and signtime <= '{this.dateEnd.Value.AddDays(1).ToString("yyyy-MM-dd")}'";

                using (SQLiteCommand cmd = new SQLiteCommand(strSQL, conn))
                {
                    conn.Open();
                    using (SQLiteDataReader dbReader = cmd.ExecuteReader())
                    {
                        if (dbReader.HasRows)
                        {
                            while (dbReader.Read())
                            {
                                lvSignForm.Items.Add(new ListViewItem(new string[]
                                {
                                    dbReader.GetInt32(0).ToString(),
                                    dbReader.GetString(1),
                                    dbReader.GetString(2),
                                    dbReader.GetString(3),
                                    dbReader.GetString(4)
                            }));
                            }
                        }
                    }
                }
            }
        }
    }
}
