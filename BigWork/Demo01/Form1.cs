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
using System.Threading;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace Demo01
{
    public partial class Form1 : Form
	{
		#region  
		private string appKey = "v3l0duXNp0D6zsvRAaEEGjiR";
		private string sKey = "MLQ7eocOmdGgzTw32tiaQqN1na7fF9K4";
		private string strSqlPath = "Data source=userInfo.db";
        
        private string voiceAppKey = "GRsvpI62QCUUD88RVglKoMmX";
        private string voiceSshKey = "Et1ydT3YQf4iQCbhCcLUZGSOkyVblm4x";
		[DllImport("winmm.dll", SetLastError = true)]
		static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallBack);
		#endregion

		//抓拍线程，每个3秒进行一次抓拍
		private Thread tShot;

		//用来缓存抓拍图片的文件名
		private string strCacheImgName = "test.jpg";
        
		public Form1()
        {
            InitializeComponent();

			//启动窗口时居中显示
			this.StartPosition = FormStartPosition.CenterScreen;
		}

        private FilterInfoCollection videoDevices;//所有摄像设备
        private VideoCaptureDevice videoDevice;//摄像设备

		/// <summary>
		/// 启动摄像头进行识别
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if(this.button1.Text == "摄像头识别")
            {
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);//得到机器所有接入的摄像设备
                if (videoDevices.Count != 0)
                {
                    foreach (FilterInfo device in videoDevices)
                    {
                        videoDevice = new VideoCaptureDevice(device.MonikerString);
                        vispShoot.VideoSource = videoDevice;//把摄像头赋给控件
                        foreach (var capability in videoDevice.VideoCapabilities)
                        {
                            if (capability.FrameSize.Width == 640 && capability.FrameSize.Height == 480)
                            {
                                videoDevice.VideoResolution = capability;
                            }
                        }
                        vispShoot.Start();//开启摄像头

                        camreaOn = true;
                        tShot = new Thread(ShotThread);
                        tShot.Start();

                        this.button1.Text = "关闭摄像头";
                    }
                }
            }
            else
            {
                camreaOn = false;
                DisConnect();
                this.button1.Text = "摄像头识别";
            }           
        }

        /// <summary>
        /// 调用摄像头的实现方式就是不断地抓拍图片进行识别，每隔3秒一次抓拍
        /// </summary>
        private bool camreaOn = false;
        private void ShotThread()
        {
            while(camreaOn)
            {
                this.BeginInvoke(new Action(() =>
                {
                    Bitmap img = vispShoot.GetCurrentVideoFrame();//拍照
                    try
                    {
                        if (img != null)
                        {
                            //拍照之后写入磁盘，并触发识别线程进行人脸检测
                            img.Save(strCacheImgName);
                            Thread thFaceCheck = new Thread(FaceCheck);
                            thFaceCheck.Start(strCacheImgName);
                        }
                    }
                    finally
                    {
                        if(img != null)
                        {
                            img.Dispose();
                            img = null;
                        }
                    }
                   
                }));
                Thread.Sleep(3000);
            }
        }

        //关闭并释放摄像头
        private void DisConnect()
        {
            if (vispShoot.VideoSource != null)
            {
                vispShoot.SignalToStop();
                vispShoot.WaitForStop();
                vispShoot.VideoSource = null;
            }
        }

        /// <summary>
        /// 人脸检测，进行红框绘制
        /// </summary>
        /// <returns></returns>
        private void FaceCheck(object objImagePath)
        {
            try
            {
				string strImagePath = (string)objImagePath;
                var client = new Baidu.Aip.Face.Face(appKey, sKey);

                var image = Convert.ToBase64String(File.ReadAllBytes(strImagePath));

                var imageType = "BASE64";
				
                var options = new Dictionary<string, object>{
                    {
                        "max_face_num", 5
                    }
                };
                // 带参数调用人脸检测
                var result = client.Detect(image, imageType, options);
                Console.WriteLine(result);

				//如果有人脸，则将图片加载到识别结果中
                if(int.Parse(result["result"]["face_num"].ToString()) > 0)
                {
                    this.BeginInvoke(new Action(() =>
                    {
						//清空之前的查询结果
						this.listView1.Items.Clear();
						//直接从内存中拿数据，不用再读写磁盘数据，即将base64转为stream
                        byte[] imageBytes = Convert.FromBase64String(image);
                        using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                        {
                            ms.Write(imageBytes, 0, imageBytes.Length);
                            this.pictureBox2.Image = Image.FromStream(ms, true);

							//根据返回的人脸数据进行红框的绘制
                            Image img = pictureBox2.Image;

                            var faceArr = JsonConvert.DeserializeObject<List<FaceCheckResult>>(result["result"]["face_list"].ToString());
                            string strAllUsers = "";
                            foreach (var item in faceArr)
							{
								Graphics g = Graphics.FromImage(img);
								Pen pen = new Pen(Color.Red, 3);
                                pen.DashStyle = DashStyle.Solid;
                                g.RotateTransform(item.location.rotation);
                                g.TranslateTransform((float)item.location.left, (float)item.location.top, MatrixOrder.Append);
                                g.DrawRectangle(pen, new Rectangle(0, 0, (int)item.location.width, (int)item.location.height));

                                g.Dispose();
								strAllUsers += FaceSearch(item);
                            }
                            if(!string.IsNullOrEmpty(strAllUsers))
                            {
                                Thread tVoice = new Thread(VoiceBroadcast);
                                tVoice.Start("你好，" + strAllUsers);
                            }
                        };
                    }));
                }
            }
            catch(Exception exp)
            {
                return;
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
                camreaOn = false;

                DisConnect();

				FaceCheck(dlg.FileName);
			}
		}

		/// <summary>
		/// 进行人脸库的搜索，根据人脸检测的结果中的face
		/// </summary>
		/// <returns></returns>
		private string FaceSearch(FaceCheckResult faceResult)
		{	

			var client = new Baidu.Aip.Face.Face(appKey, sKey);
			//这里根据face_token搜索，其实没必要使用 m:n 方式，因为token只对应一个人脸
			var result = client.Search(faceResult.face_token, "FACE_TOKEN", "group1");

			if(result != null && JsonConvert.DeserializeObject<int>(result["error_code"].ToString()) == 0)
			{
				var userList = JsonConvert.DeserializeObject<List<SearchResult>>(result["result"]["user_list"].ToString());
				int index = 1;
                string strAllUser = "";
				foreach (var item in userList)
				{
					if(item.Score > 80.0)
					{
						this.listView1.Items.Add(new ListViewItem(new string[]
							{
							index++.ToString(),
							item.Group_id,
							item.User_id,
							item.User_info
							}));
                        strAllUser += item.User_info + ",";
                        InsertSignTime(item.Group_id, item.User_id, item.User_info, DateTime.Now);

                    }
				}
				return strAllUser;
			}
			else
			{
				return "";
			}
		}

        /// <summary>
        /// 语音播放接口
        /// </summary>
        /// <param name="obj"></param>
		private void VoiceBroadcast(object obj)
		{
			try
			{
                string strAllUserInfo = obj as string;

                var client = new Baidu.Aip.Speech.Tts(voiceAppKey, voiceSshKey);

				var options = new Dictionary<string, object>()
				{
					{ "spd", 5},    //语速
					{ "vol", 7},     //音量
					{ "per", 4}     //语音人
				};

				var ret = client.Synthesis(strAllUserInfo, options);
                if (ret.ErrorCode == 0)
                {
                    this.BeginInvoke(new Action(() =>
                {
                   
                        File.WriteAllBytes("temp.mp3", ret.Data);
                        mciSendString("open temp.mp3" + " alias temp_alias", null, 0, IntPtr.Zero);
                        mciSendString("play temp_alias", null, 0, IntPtr.Zero);

                        StringBuilder strRet = new StringBuilder(64);

                        do
                        {
                            mciSendString("status temp_alias mode", strRet, 64, IntPtr.Zero);
                        } while (!strRet.ToString().Contains("stopped"));

                        mciSendString("close temp_alias", null, 0, IntPtr.Zero);

                }));
                }
                else
                {
                    MessageBox.Show(ret.ErrorCode.ToString() + ret.ErrorMsg);
                }

            }
			catch (Exception exp)
			{
				MessageBox.Show("合成失败，异常信息：" + exp.Message);
			}

		}

        private void InsertSignTime(string groupId, string userId, string userInfo, DateTime signTime)
        {
            using (SQLiteConnection conn = new SQLiteConnection(strSqlPath))
            {
                string strSQL = $"INSERT INTO signform(groupid, userid, userinfo, signtime)VALUES('{groupId}', '{userId}', '{userInfo}', '{signTime.ToString("yyyy-MM-dd HH-mm-ss")}')";

                using (SQLiteCommand cmd = new SQLiteCommand(strSQL, conn))
                {
                    conn.Open();
                    using (SQLiteDataReader dbReader = cmd.ExecuteReader())
                    {
                        if (dbReader.HasRows)
                        {
                            while (dbReader.Read())
                            {
                                string strUserName = dbReader.GetString(1);
                                Console.WriteLine(dbReader.GetString(0) + dbReader.GetString(1));
                            }
                        }
                    }
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
		public double Score { get; set; }

		public string Name { get; set; }
	}

    public class FaceCheckResult
    {
        [JsonProperty("face_token")]
        public string face_token { get; set; }

        [JsonProperty("location")]
        public Location location { get; set; }
    }

    public class Location
    {
        [JsonProperty("left")]
        public double left { get; set; }
        [JsonProperty("top")]
        public double top { get; set; }
        [JsonProperty("width")]
        public double width { get; set; }
        [JsonProperty("height")]
        public double height { get; set; }
        [JsonProperty("rotation")]
        public int rotation { get; set; }
    }
}
