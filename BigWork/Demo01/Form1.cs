using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Baidu;
using Newtonsoft;
using Baidu.Aip.Face;
using System.Data.SQLite;

namespace Demo01
{
    public partial class Form1 : Form
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
		public Form1()
        {
            InitializeComponent();
			getAccessToken();
        }

        private FilterInfoCollection videoDevices;//所有摄像设备
        private VideoCaptureDevice videoDevice;//摄像设备
        private VideoCapabilities[] videoCapabilities;//摄像头分辨率


        private void button1_Click(object sender, EventArgs e)
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);//得到机器所有接入的摄像设备
            if (videoDevices.Count != 0)
            {
                foreach (FilterInfo device in videoDevices)
                {
                    videoDevice = new VideoCaptureDevice(device.MonikerString);

                }
            }
        }

        /// <summary>
        /// 管理人脸库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            DataManage dlgData = new DataManage();
            dlgData.ShowDialog();
        }

		/// <summary>
		/// 图片识别
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button3_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.ShowDialog();

			if (!string.IsNullOrEmpty(dlg.FileName))
			{
				this.pictureBox1.Image = Image.FromFile(dlg.FileName);
				string strImg = Convert.ToBase64String(File.ReadAllBytes(dlg.FileName));
				SearchFaceInfo faceInfo = new SearchFaceInfo()
				{
					Image = strImg,
					Image_type = "BASE64",
					Group_id_list = "group1",
					Quality_control = "NORMAL",
					Liveness_control = "NONE"
				};
				var jString = JsonConvert.SerializeObject(faceInfo);
				string host = "https://aip.baidubce.com/rest/2.0/face/v3/search?access_token=" + TOKEN;
				Encoding encoding = Encoding.Default;
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
				request.Method = "post";
				request.KeepAlive = true;
				byte[] buffer = encoding.GetBytes(jString);
				request.ContentLength = buffer.Length;
				request.GetRequestStream().Write(buffer, 0, buffer.Length);
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
				string result = reader.ReadToEnd();
				Console.WriteLine("人脸搜索:");
				Console.WriteLine(result);
				var json = (JObject)JsonConvert.DeserializeObject(result);
				var userList = JsonConvert.DeserializeObject<List<SearchResult>>(json["result"]["user_list"].ToString());
				int index = 1;
				foreach (var item in userList)
				{
					this.listView1.Items.Add(new ListViewItem(new string[]
						{
							index++.ToString(),
							item.Group_id,
							item.User_id,
							item.User_info
						}));
				}

			}
		}
	}

	public class FaceInfo
	{
		//人脸图像信息  不超过10M
		[JsonProperty("image")]
		public string Image { get; set; }

		//图像信息，可选  base64    URL     FACE_TOKEN（就是每次使用人脸检测的时候返回的那个TOKEN，可直接使用）
		[JsonProperty("image_type")]
		public string Image_type { get; set; }

		//用户组
		[JsonProperty("group_id")]
		public string Group_id { get; set; }

		//用户ID
		[JsonProperty("user_id")]
		public string User_id { get; set; }

		//用户信息 256B大小
		[JsonProperty("user_info")]
		public string User_info { get; set; }

		//图片质量
		[JsonProperty("quality_control")]
		public string Quality_control { get; set; }

		//活体检测控制
		[JsonProperty("liveness_control")]
		public string Liveness_control { get; set; }

	}

	public class SearchFaceInfo
	{
		[JsonProperty("image")]
		public string Image { get; set; }

		[JsonProperty("image_type")]
		public string Image_type { get; set; }

		[JsonProperty("group_id_list")]
		public string Group_id_list { get; set; }

		[JsonProperty("quality_control")]
		public string Quality_control { get; set; }

		[JsonProperty("liveness_control")]
		public string Liveness_control { get; set; }

	}

	public class SearchResult
	{

		//用户组
		[JsonProperty("group_id")]
		public string Group_id { get; set; }

		//用户ID
		[JsonProperty("user_id")]
		public string User_id { get; set; }

		//用户信息 256B大小
		[JsonProperty("user_info")]
		public string User_info { get; set; }
		//用户组
		[JsonProperty("score")]
		public string Score { get; set; }

		public string Name { get; set; }
	}
}
