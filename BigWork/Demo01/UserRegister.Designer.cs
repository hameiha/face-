namespace Demo01
{
	partial class UserRegister
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtUserId = new System.Windows.Forms.TextBox();
			this.txtUserInfo = new System.Windows.Forms.TextBox();
			this.cbGroups = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.button2 = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.button1 = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(25, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "用户ID：";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 49);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(65, 12);
			this.label2.TabIndex = 1;
			this.label2.Text = "用户信息：";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(25, 78);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(53, 12);
			this.label3.TabIndex = 2;
			this.label3.Text = "选择组：";
			// 
			// txtUserId
			// 
			this.txtUserId.Location = new System.Drawing.Point(84, 14);
			this.txtUserId.Name = "txtUserId";
			this.txtUserId.Size = new System.Drawing.Size(173, 21);
			this.txtUserId.TabIndex = 3;
			// 
			// txtUserInfo
			// 
			this.txtUserInfo.Location = new System.Drawing.Point(84, 43);
			this.txtUserInfo.Name = "txtUserInfo";
			this.txtUserInfo.Size = new System.Drawing.Size(173, 21);
			this.txtUserInfo.TabIndex = 4;
			// 
			// cbGroups
			// 
			this.cbGroups.FormattingEnabled = true;
			this.cbGroups.Location = new System.Drawing.Point(84, 72);
			this.cbGroups.Name = "cbGroups";
			this.cbGroups.Size = new System.Drawing.Size(173, 20);
			this.cbGroups.TabIndex = 5;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.button2);
			this.groupBox1.Controls.Add(this.pictureBox1);
			this.groupBox1.Controls.Add(this.button1);
			this.groupBox1.Location = new System.Drawing.Point(15, 109);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(244, 235);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "上传人脸信息";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(141, 206);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 2;
			this.button2.Text = "注册";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(7, 21);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(231, 179);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(27, 206);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "选择图片";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// UserRegister
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(268, 356);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.cbGroups);
			this.Controls.Add(this.txtUserInfo);
			this.Controls.Add(this.txtUserId);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "UserRegister";
			this.Text = "用户注册";
			this.Load += new System.EventHandler(this.UserRegister_Load);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtUserId;
		private System.Windows.Forms.TextBox txtUserInfo;
		private System.Windows.Forms.ComboBox cbGroups;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
	}
}