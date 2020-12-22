namespace Demo01
{
    partial class DataManage
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lbGroups = new System.Windows.Forms.ListBox();
			this.button2 = new System.Windows.Forms.Button();
			this.btnSelectGroup = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.button4 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.listView1 = new System.Windows.Forms.ListView();
			this.userID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.userInfo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lbGroups);
			this.groupBox1.Controls.Add(this.button2);
			this.groupBox1.Controls.Add(this.btnSelectGroup);
			this.groupBox1.Location = new System.Drawing.Point(10, 10);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.groupBox1.Size = new System.Drawing.Size(140, 332);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "组管理";
			// 
			// lbGroups
			// 
			this.lbGroups.FormattingEnabled = true;
			this.lbGroups.ItemHeight = 12;
			this.lbGroups.Location = new System.Drawing.Point(5, 67);
			this.lbGroups.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.lbGroups.Name = "lbGroups";
			this.lbGroups.Size = new System.Drawing.Size(132, 256);
			this.lbGroups.TabIndex = 2;
			this.lbGroups.SelectedIndexChanged += new System.EventHandler(this.lbGroups_SelectedIndexChanged);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(80, 20);
			this.button2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(56, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "新建";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// btnSelectGroup
			// 
			this.btnSelectGroup.Location = new System.Drawing.Point(5, 20);
			this.btnSelectGroup.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.btnSelectGroup.Name = "btnSelectGroup";
			this.btnSelectGroup.Size = new System.Drawing.Size(56, 23);
			this.btnSelectGroup.TabIndex = 0;
			this.btnSelectGroup.Text = "查询";
			this.btnSelectGroup.UseVisualStyleBackColor = true;
			this.btnSelectGroup.Click += new System.EventHandler(this.btnSelectGroup_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.button4);
			this.groupBox2.Controls.Add(this.button3);
			this.groupBox2.Controls.Add(this.listView1);
			this.groupBox2.Location = new System.Drawing.Point(154, 10);
			this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.groupBox2.Size = new System.Drawing.Size(290, 332);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "用户管理";
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(5, 20);
			this.button4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(56, 23);
			this.button4.TabIndex = 2;
			this.button4.Text = "查询";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(80, 19);
			this.button3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(56, 23);
			this.button3.TabIndex = 1;
			this.button3.Text = "注册";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// listView1
			// 
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.userID,
            this.userInfo});
			this.listView1.Location = new System.Drawing.Point(4, 67);
			this.listView1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(282, 261);
			this.listView1.TabIndex = 0;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// userID
			// 
			this.userID.Text = "用户ID";
			this.userID.Width = 113;
			// 
			// userInfo
			// 
			this.userInfo.Text = "用户信息";
			this.userInfo.Width = 238;
			// 
			// DataManage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(454, 352);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.Name = "DataManage";
			this.Text = "数据管理";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lbGroups;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnSelectGroup;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader userID;
        private System.Windows.Forms.ColumnHeader userInfo;
    }
}