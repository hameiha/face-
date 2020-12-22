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
using System.IO;

namespace Demo01
{
	public partial class UserRegister : Form
	{

		private string appKey = "v3l0duXNp0D6zsvRAaEEGjiR";
		private string sKey = "MLQ7eocOmdGgzTw32tiaQqN1na7fF9K4";

		private string strFilePath = "";
		
		public UserRegister()
		{
			InitializeComponent();
			GroupGetlistDemo();

			//启动窗口时居中显示
			this.StartPosition = FormStartPosition.CenterScreen;
		}

		/// <summary>
		/// 查询所有组信息
		/// </summary>
		public void GroupGetlistDemo()
		{
			try
			{
				var client = new Face(appKey, sKey);

				var json = client.GroupGetlist();
				Console.WriteLine(json);

				var groupArr = JsonConvert.DeserializeObject<List<string>>(json["result"]["group_id_list"].ToString());
				if (groupArr.Count > 0 && this.cbGroups.Items.Count > 0)
					this.cbGroups.Items.Clear();
				foreach (var item in groupArr)
				{
					this.cbGroups.Items.Add(item);
				}
			}
			catch(Exception exp)
			{
				MessageBox.Show("获取组列表信息失败，错误信息：" + exp.Message);
			}
		}

		/// <summary>
		/// 选择图片按钮
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.ShowDialog();

			if(string.IsNullOrEmpty(dlg.FileName))
			{
				MessageBox.Show("请选择图片");
				return;
			}

			strFilePath = dlg.FileName;

			try
			{
				//进行人脸检测，确定符合注册条件
				var image = Convert.ToBase64String(File.ReadAllBytes(strFilePath));
				var imageType = "BASE64";

				// 调用人脸检测
				var client = new Face(appKey, sKey);
				var options = new Dictionary<string, object>{
					{ "face_field", "quality"},  //检测人脸质量，太低不予通过
					{ "max_face_num", 2}
				};
				// 带参数调用人脸检测
				var result = client.Detect(image, imageType, options);
				Console.WriteLine(result);

				if(int.Parse(result["error_code"].ToString()) == 0)
				{
					if(int.Parse(result["result"]["face_num"].ToString()) > 1)
					{
						MessageBox.Show("请选择只有一张人脸信息的图片");
						return;
					}
					if(double.Parse(result["result"]["face_list"][0]["face_probability"].ToString()) > 0.8)
					{
						this.pictureBox1.Image = Image.FromFile(dlg.FileName);
					}else
					{
						MessageBox.Show("人脸置信度太低，请重新选择");
						return;
					}
				}
				else
				{
					MessageBox.Show("人脸检测失败，错误信息：" + result["error_msg"].ToString());
				}
			}
			catch(Exception exp)
			{
				MessageBox.Show("人脸检测失败，错误信息：" + exp.Message);
			}			
		}
		
		/// <summary>
		/// 注册按钮
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button2_Click(object sender, EventArgs e)
		{
			if(this.pictureBox1.Image == null)
			{
				MessageBox.Show("请选择人脸图片");
				return;
			}
			try
			{
				var image = Convert.ToBase64String(File.ReadAllBytes(strFilePath));

				var imageType = "BASE64";

				var groupId = this.cbGroups.SelectedItem.ToString();

				var userId = this.txtUserId.Text;
				
				var options = new Dictionary<string, object>{
					{ "user_info", this.txtUserInfo.Text},
					{ "quality_control", "NORMAL"},
					{ "liveness_control", "NONE"},
					{ "action_type", "APPEND"}
				};

				// 带参数调用人脸注册
				var client = new Face(appKey, sKey);
				var result = client.UserAdd(image, imageType, groupId, userId, options);
				if(int.Parse(result["error_code"].ToString()) == 0)
				{
					MessageBox.Show("注册成功！");
					return;
				}
				else
				{
					MessageBox.Show("注册失败，错误信息：" + result["error_msg"].ToString());
					return;
				}
				Console.WriteLine(result);
			}
			catch(Exception exp)
			{
				MessageBox.Show("注册失败，错误信息：" + exp.Message);
			}
		}

		/// <summary>
		/// 窗口加载时设置默认组信息
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UserRegister_Load(object sender, EventArgs e)
		{
			if (this.cbGroups != null && this.cbGroups.Items.Count > 0)
				this.cbGroups.SelectedIndex = 0;
		}
	}
}
