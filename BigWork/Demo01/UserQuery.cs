using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Baidu.Aip.Face;
using Newtonsoft;
using Newtonsoft.Json;

namespace Demo01
{
	public partial class UserQuery : Form
	{
		#region  
		private string appKey = "v3l0duXNp0D6zsvRAaEEGjiR";
		private string sKey = "MLQ7eocOmdGgzTw32tiaQqN1na7fF9K4";
		#endregion

		public bool bOk = false;
		public string strUserId = "";
		public string strGroupId = "";

		public UserQuery()
		{
			InitializeComponent();

			GetGroupInfo();
			this.cbGroupId.SelectedIndex = 0;

			this.StartPosition = FormStartPosition.CenterScreen;
		}

		private void GetGroupInfo()
		{
			try
			{
				var client = new Face(appKey, sKey);
				var result = client.GroupGetlist();
				var groupArr = JsonConvert.DeserializeObject<List<string>>(result["result"]["group_id_list"].ToString());
				foreach (var item in groupArr)
				{
					this.cbGroupId.Items.Add(item);
				}
			}
			catch (Exception exp)
			{
				MessageBox.Show("查询组信息失败，错误信息：" + exp.Message);
			}
		}

		/// <summary>
		/// 确定按钮
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e)
		{
			if(string.IsNullOrEmpty(this.txtUserInfo.Text))
			{
				MessageBox.Show("请输入用户ID");
				return;
			}

			if(this.cbGroupId.SelectedItem == null)
			{
				MessageBox.Show("请选择组ID");
				return;
			}

			strUserId = this.txtUserInfo.Text;
			strGroupId = this.cbGroupId.SelectedItem.ToString();
			bOk = true;
			this.Close();
		}

		/// <summary>
		/// 取消按钮
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button2_Click(object sender, EventArgs e)
		{
			bOk = false;
			this.Close();
		}
	}
}
