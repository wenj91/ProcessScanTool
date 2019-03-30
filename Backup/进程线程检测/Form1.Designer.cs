namespace 进程线程检测
{
    partial class 线程进程检测
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.检测按钮 = new System.Windows.Forms.Button();
            this.ThreadInfo = new System.Windows.Forms.ListView();
            this.ThreadOpt = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ProcessBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ExportBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ModuleExportBtn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ModuleInfo = new System.Windows.Forms.ListView();
            this.About = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "进程:";
            // 
            // 检测按钮
            // 
            this.检测按钮.Location = new System.Drawing.Point(659, 12);
            this.检测按钮.Name = "检测按钮";
            this.检测按钮.Size = new System.Drawing.Size(67, 21);
            this.检测按钮.TabIndex = 2;
            this.检测按钮.Text = "开始检测";
            this.检测按钮.UseVisualStyleBackColor = true;
            this.检测按钮.Click += new System.EventHandler(this.checkBtn);
            // 
            // ThreadInfo
            // 
            this.ThreadInfo.FullRowSelect = true;
            this.ThreadInfo.GridLines = true;
            this.ThreadInfo.Location = new System.Drawing.Point(8, 32);
            this.ThreadInfo.Name = "ThreadInfo";
            this.ThreadInfo.Size = new System.Drawing.Size(348, 460);
            this.ThreadInfo.TabIndex = 3;
            this.ThreadInfo.UseCompatibleStateImageBehavior = false;
            this.ThreadInfo.View = System.Windows.Forms.View.Details;
            this.ThreadInfo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ThreadInfo_MouseClick);
            this.ThreadInfo.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ThreadInfo_ColumnClick);
            // 
            // ThreadOpt
            // 
            this.ThreadOpt.Name = "ThreadOpt";
            this.ThreadOpt.Size = new System.Drawing.Size(61, 4);
            this.ThreadOpt.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ThreadOpt_ItemClicked);
            // 
            // ProcessBox
            // 
            this.ProcessBox.FormattingEnabled = true;
            this.ProcessBox.Location = new System.Drawing.Point(53, 9);
            this.ProcessBox.Name = "ProcessBox";
            this.ProcessBox.Size = new System.Drawing.Size(600, 20);
            this.ProcessBox.TabIndex = 4;
            this.ProcessBox.DropDown += new System.EventHandler(this.ProcessBox_DropDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "线程数量:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(71, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "0";
            // 
            // ExportBtn
            // 
            this.ExportBtn.Location = new System.Drawing.Point(8, 502);
            this.ExportBtn.Name = "ExportBtn";
            this.ExportBtn.Size = new System.Drawing.Size(137, 25);
            this.ExportBtn.TabIndex = 7;
            this.ExportBtn.Text = "将线程信息导出到TxT";
            this.ExportBtn.UseVisualStyleBackColor = true;
            this.ExportBtn.Click += new System.EventHandler(this.ExportBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.About);
            this.groupBox1.Controls.Add(this.ModuleExportBtn);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.ModuleInfo);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.ExportBtn);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.ThreadInfo);
            this.groupBox1.Location = new System.Drawing.Point(14, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(719, 534);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "进程信息";
            // 
            // ModuleExportBtn
            // 
            this.ModuleExportBtn.Location = new System.Drawing.Point(362, 502);
            this.ModuleExportBtn.Name = "ModuleExportBtn";
            this.ModuleExportBtn.Size = new System.Drawing.Size(112, 26);
            this.ModuleExportBtn.TabIndex = 11;
            this.ModuleExportBtn.Text = "将模块信息导出";
            this.ModuleExportBtn.UseVisualStyleBackColor = true;
            this.ModuleExportBtn.Click += new System.EventHandler(this.ModuleExportBtn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(425, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(11, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(360, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "进程模块: ";
            // 
            // ModuleInfo
            // 
            this.ModuleInfo.Location = new System.Drawing.Point(362, 32);
            this.ModuleInfo.Name = "ModuleInfo";
            this.ModuleInfo.Size = new System.Drawing.Size(341, 460);
            this.ModuleInfo.TabIndex = 8;
            this.ModuleInfo.UseCompatibleStateImageBehavior = false;
            this.ModuleInfo.View = System.Windows.Forms.View.Details;
            this.ModuleInfo.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ModuleInfo_ColumnClick);
            // 
            // About
            // 
            this.About.Location = new System.Drawing.Point(617, 503);
            this.About.Name = "About";
            this.About.Size = new System.Drawing.Size(86, 24);
            this.About.TabIndex = 12;
            this.About.Text = "关于本软件";
            this.About.UseVisualStyleBackColor = true;
            this.About.Click += new System.EventHandler(this.About_Click);
            // 
            // 线程进程检测
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 583);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ProcessBox);
            this.Controls.Add(this.检测按钮);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "线程进程检测";
            this.Text = "进程线程检测V1.0";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button 检测按钮;
        private System.Windows.Forms.ListView ThreadInfo;
        private System.Windows.Forms.ComboBox ProcessBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ExportBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView ModuleInfo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button ModuleExportBtn;
        private System.Windows.Forms.ContextMenuStrip ThreadOpt;
        private System.Windows.Forms.Button About;
    }
}

