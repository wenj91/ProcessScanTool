using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;

namespace 进程线程检测
{
    public partial class 线程进程检测 : Form
    {
        public string processName = "";
        AboutBox1 about = new AboutBox1();
        public 线程进程检测()
        {
            InitializeComponent();
            GetProcesses();
            ThreadInfoInit();
            ModuleInfoInit();
            ThreadOptInit();
        }

        private void ThreadOptInit() {

            ThreadOpt.Items.Add("结束线程");
            ThreadOpt.Items.Add("挂起线程");
            ThreadOpt.Items.Add("恢复线程");
        }

        private void ThreadInfoInit()
        {
            this.ThreadInfo.GridLines = true; //显示表格线
            this.ThreadInfo.View = View.Details;//显示表格细节
            this.ThreadInfo.LabelEdit = false; //是否可编辑,ListView只可编辑第一列。
            this.ThreadInfo.Scrollable = true;//有滚动条
            this.ThreadInfo.HeaderStyle = ColumnHeaderStyle.Clickable;//对表头进行设置
            this.ThreadInfo.FullRowSelect = true;//是否可以选择行

            //this.listView1.HotTracking = true;// 当选择此属性时则HoverSelection自动为true和Activation属性为oneClick
            //this.listView1.HoverSelection = true;
            //this.listView1.Activation = ItemActivation.Standard; //
            ThreadInfo.Columns.Add("线程ID", 50);
            ThreadInfo.Columns.Add("优先级", 50);
            ThreadInfo.Columns.Add("线程入口", 80);
            ThreadInfo.Columns.Add("线程模块", 80);
            ThreadInfo.Columns.Add("线程状态", 80);
        }

        private void ModuleInfoInit()
        {
            this.ModuleInfo.GridLines = true; //显示表格线
            this.ModuleInfo.View = View.Details;//显示表格细节
            this.ModuleInfo.LabelEdit = false; //是否可编辑,ListView只可编辑第一列。
            this.ModuleInfo.Scrollable = true;//有滚动条
            this.ModuleInfo.HeaderStyle = ColumnHeaderStyle.Clickable;//对表头进行设置
            this.ModuleInfo.FullRowSelect = true;//是否可以选择行

            //this.listView1.HotTracking = true;// 当选择此属性时则HoverSelection自动为true和Activation属性为oneClick
            //this.listView1.HoverSelection = true;
            //this.listView1.Activation = ItemActivation.Standard; //
            ModuleInfo.Columns.Add("模块名称", 80);
            ModuleInfo.Columns.Add("基地址", 80);
            ModuleInfo.Columns.Add("入口地址", 80);
            ModuleInfo.Columns.Add("模块大小", 80);
            ModuleInfo.Columns.Add("厂商", 80);
            ModuleInfo.Columns.Add("模块路径", 80);
            
        }

        private void GetProcesses(){
            Process[] prs = Process.GetProcesses();
            string[] str = new string[prs.Length];
            for (int i = 0; i < prs.Length; i++ )
            {
                str[i] = prs[i].ProcessName + " -- " + prs[i].Id;
            }
            Array.Sort(str);
            for (int i = 0; i < str.Length; i++)
            {
                ProcessBox.Items.Add(str[i]);
            }
            ProcessBox.SelectedIndex = 0;
        }

        private void GetProcessThreads(){
            string[] str = Regex.Split(ProcessBox.Text,"--");
            int pid = int.Parse(str[1].Trim());
            Process pr = Process.GetProcessById(pid);
            label3.Text = pr.Threads.Count + "";

            //获取线程模块
            CSTools.EnableDebugPrivilege(true);
            ProcessModuleCollection pm = pr.Modules;

            for (int i = 0; i < pr.Threads.Count; i++ )
            {
                CSTools.EnableDebugPrivilege(true);
                IntPtr handle = CSTools.OpenThread(CSTools.ThreadAccess.THREAD_ALL_ACCESS, false, pr.Threads[i].Id);
                CSTools.EnableDebugPrivilege(true);
                int addr = 0;
                CSTools.NtQueryInformationThread(handle, CSTools.ThreadInfoClass.ThreadQuerySetWin32StartAddress, out addr, sizeof(int), 0);
                string name = "";
                for (int j = 0; j < pr.Modules.Count; j++)
                {
                    if (addr >= pr.Modules[j].BaseAddress.ToInt32() && addr <= (pr.Modules[j].BaseAddress.ToInt32() + pr.Modules[j].ModuleMemorySize))
                    {
                        name = pr.Modules[j].ModuleName.PadRight(40, ' ');
                    }
                }
                string status = CSTools.GetThreadStatus(pr.Threads[i]);

                ListViewItem li = new ListViewItem();
                li.Text = pr.Threads[i].Id.ToString().PadLeft(4, '0').PadRight(2, ' ');
                li.SubItems.Add(pr.Threads[i].BasePriority .ToString().PadLeft(2, '0').PadRight(1, ' '));
                li.SubItems.Add("0x" + addr.ToString("X8"));
                li.SubItems.Add(name);
                li.SubItems.Add(status.PadLeft(4, ' '));
                ThreadInfo.Items.Add(li);
                CSTools.CloseHandle(handle);
            }
        }

        private void GetProcessModules() {
            string[] str = Regex.Split(ProcessBox.Text, "--");
            int pid = int.Parse(str[1].Trim());
            Process pr = Process.GetProcessById(pid);
            label5.Text = pr.Modules.Count + "";

            //获取线程模块
            CSTools.EnableDebugPrivilege(true);
            ProcessModuleCollection pm = pr.Modules;

            for (int i = 0; i < pm.Count; i++)
            {
                ListViewItem li = new ListViewItem();
                li.Text = pm[i].ModuleName.PadRight(35, ' ');
                li.SubItems.Add("0x" + pm[i].BaseAddress.ToString("X8"));
                li.SubItems.Add("0x" + pm[i].EntryPointAddress.ToString("X8"));
                li.SubItems.Add("0x" + pm[i].ModuleMemorySize.ToString("X8"));
                li.SubItems.Add(pm[i].FileVersionInfo.CompanyName == null ? " ":pm[i].FileVersionInfo.CompanyName.PadRight(21, ' '));
                li.SubItems.Add(pm[i].FileName);
                ModuleInfo.Items.Add(li);
            }
        }

        private void checkBtn(object sender, EventArgs e)
        {
            ThreadInfo.Items.Clear();
            GetProcessThreads();
            ModuleInfo.Items.Clear();
            GetProcessModules();
        }

        //自定义变量
        //实现ListView排序
        #region 
        int currentCol = -1;
        bool sort;
        #endregion
        private void ThreadInfo_ColumnClick(object sender, ColumnClickEventArgs e)
        {
 
            if (sort == false)
            {
                sort = true;
                string oldStr = this.ThreadInfo.Columns[e.Column].Text;
                this.ThreadInfo.Columns[e.Column].Text = oldStr;
            }
            else if (sort == true)
            {
                sort = false;
                string oldStr = this.ThreadInfo.Columns[e.Column].Text;
                this.ThreadInfo.Columns[e.Column].Text = oldStr;
            }

            ThreadInfo.ListViewItemSorter = (System.Collections.IComparer)new ListViewItemComparer(e.Column, sort);
            this.ThreadInfo.Sort();
            int rowCount = this.ThreadInfo.Items.Count;
            if (currentCol != -1)
            {
                for (int i = 0; i < rowCount; i++)
                {
                    this.ThreadInfo.Items[i].UseItemStyleForSubItems = false;
                    this.ThreadInfo.Items[i].SubItems[currentCol].BackColor = Color.White;

                    if (e.Column != currentCol)
                        this.ThreadInfo.Columns[currentCol].Text = this.ThreadInfo.Columns[currentCol].Text;
                }
            }

            for (int i = 0; i < rowCount; i++)
            {
                if (this.ThreadInfo.Items[i].Text != null){
                    this.ThreadInfo.Items[i].UseItemStyleForSubItems = false;
                    this.ThreadInfo.Items[i].SubItems[e.Column].BackColor = Color.WhiteSmoke;
                    currentCol = e.Column;
                }
            }
        }

        private void ProcessBox_DropDown(object sender, EventArgs e)
        {
            GetProcesses();
        }

        //全局变量,文件全路径
        private string stFilePath = string.Empty;

        /// 导出数据到txt文件.
        private void ExportBtn_Click(object sender, EventArgs e)
        {
            //此处的文本文件在工程下Bin的程序集目录下
            stFilePath = Application.StartupPath.Trim() + "//" + ProcessBox.Text 
                + "  " + DateTime.Now.ToString("yyyy年MM月dd日hh时mm分ss秒") + ".txt";
            StreamWriter swStream;
            if (File.Exists(stFilePath))
            {
                swStream = new StreamWriter(stFilePath);
            }
            else
            {
                swStream = File.CreateText(stFilePath);
            }
            swStream.WriteLine("进程: " + ProcessBox.Text);
            swStream.WriteLine("线程数: " + label3.Text);
            swStream.WriteLine();
            swStream.WriteLine("线程Id     优先级       线程入口           线程模块                                         线程状态");
            for (int i = 0; i < ThreadInfo.Items.Count; i++)
            {
                for (int j = 0; j < ThreadInfo.Items[i].SubItems.Count; j++)
                {
                    string _strTemp = ThreadInfo.Items[i].SubItems[j].Text;
                    swStream.Write(_strTemp);
                    //插入"<----->"作为分隔符,可以任取
                    swStream.Write("         ");
                }
                swStream.WriteLine();
            }

            //关闭流,释放资源
            swStream.Flush();
            swStream.Close();
            //导入Txt文件后,自动打开文件
            Process.Start("notepad.exe", stFilePath);
        }

        //自定义变量
        //实现ListView排序
        #region
        int currentCol2 = -1;
        bool sort2;
        #endregion
        private void ModuleInfo_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (sort2 == false)
            {
                sort2 = true;
                string oldStr = this.ModuleInfo.Columns[e.Column].Text;
                this.ModuleInfo.Columns[e.Column].Text = oldStr;
            }
            else if (sort2 == true)
            {
                sort2 = false;
                string oldStr = this.ModuleInfo.Columns[e.Column].Text;
                this.ModuleInfo.Columns[e.Column].Text = oldStr;
            }

            ModuleInfo.ListViewItemSorter = (System.Collections.IComparer)new ListViewItemComparer(e.Column, sort2);
            this.ModuleInfo.Sort();
            int rowCount = this.ModuleInfo.Items.Count;
            if (currentCol2 != -1)
            {
                for (int i = 0; i < rowCount; i++)
                {
                    this.ModuleInfo.Items[i].UseItemStyleForSubItems = false;
                    this.ModuleInfo.Items[i].SubItems[currentCol2].BackColor = Color.White;

                    if (e.Column != currentCol2)
                        this.ModuleInfo.Columns[currentCol2].Text = this.ModuleInfo.Columns[currentCol2].Text;
                }
            }

            for (int i = 0; i < rowCount; i++)
            {
                if (this.ModuleInfo.Items[i].Text != null)
                {
                    this.ModuleInfo.Items[i].UseItemStyleForSubItems = false;
                    this.ModuleInfo.Items[i].SubItems[e.Column].BackColor = Color.WhiteSmoke;
                    currentCol2 = e.Column;
                }
            }
        }

        private void ModuleExportBtn_Click(object sender, EventArgs e)
        {
            //此处的文本文件在工程下Bin的程序集目录下
            stFilePath = Application.StartupPath.Trim() + "//" + ProcessBox.Text + "  " + DateTime.Now.ToString("yyyy年MM月dd日hh时mm分ss秒") + ".txt";
            StreamWriter swStream;
            if (File.Exists(stFilePath))
            {
                swStream = new StreamWriter(stFilePath);
            }
            else
            {
                swStream = File.CreateText(stFilePath);
            }
            swStream.WriteLine("进程: " + ProcessBox.Text);
            swStream.WriteLine("模块数: " + label3.Text);
            swStream.WriteLine();
            swStream.WriteLine("模块名称                                    基地址" +
                "             入口地址           内存大小           厂商                          模块路径");
            for (int i = 0; i < ModuleInfo.Items.Count; i++)
            {
                for (int j = 0; j < ModuleInfo.Items[i].SubItems.Count; j++)
                {
                    string _strTemp = ModuleInfo.Items[i].SubItems[j].Text;
                    swStream.Write(_strTemp);
                    //插入"<----->"作为分隔符,可以任取
                    swStream.Write("         ");
                }
                swStream.WriteLine();
            }

            //关闭流,释放资源
            swStream.Flush();
            swStream.Close();
            //导入Txt文件后,自动打开文件
            Process.Start("notepad.exe", stFilePath);
        }

        private void ThreadInfo_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && this.ThreadInfo.SelectedItems.Count > 0)
            {
                string[] str = Regex.Split(ProcessBox.Text, "--");
                int pid = int.Parse(str[1].Trim());
                Process pr = Process.GetProcessById(pid);

                this.ThreadInfo.ContextMenuStrip = this.ThreadOpt;
                ListViewItem li = this.ThreadInfo.SelectedItems[0];
                int tid = int.Parse(li.Text);
                

                //获取线程模块
                CSTools.EnableDebugPrivilege(true);

                for (int i = 0; i < pr.Threads.Count; i++)
                {
                    if(tid == pr.Threads[i].Id){
                       //MessageBox.Show(pr.Threads[i].WaitReason.ToString());
                       switch(pr.Threads[i].WaitReason){
                           case ThreadWaitReason.EventPairLow : 
                           case ThreadWaitReason.EventPairHigh:
                           case ThreadWaitReason.UserRequest:
                           case ThreadWaitReason.ExecutionDelay:
                           case ThreadWaitReason.Executive:
                           case ThreadWaitReason.FreePage:
                               this.ThreadOpt.Items[1].Enabled = true;
                               this.ThreadOpt.Items[2].Enabled = false;
                               break;                          
                           case ThreadWaitReason.Suspended:
                               this.ThreadOpt.Items[1].Enabled = false;
                               this.ThreadOpt.Items[2].Enabled = true;
                               break;
                           default:
                               this.ThreadOpt.Items[2].Enabled = false;
                               break;
                       }
                    }
                }
            }else{
                this.ThreadInfo.ContextMenuStrip = null;
            }
        }

        private void ThreadOpt_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ListViewItem li = this.ThreadInfo.SelectedItems[0];
            int tid = int.Parse(li.Text);
            IntPtr handle = CSTools.OpenThread(CSTools.ThreadAccess.THREAD_ALL_ACCESS, false, tid);
            switch(e.ClickedItem.Text){
                case "结束线程":                  
                    CSTools.TerminateThread(handle, 0);
                    this.ThreadInfo.Items.Remove(li);
                    this.label3.Text = int.Parse(this.label3.Text) - 1 + "";
                    break;
                case "挂起线程":
                    CSTools.SuspendThread(handle);
                    li.ForeColor = Color.Red;
                    break;
                case "恢复线程":
                    CSTools.ResumeThread(handle);
                    li.ForeColor = Color.Black; 
                    break;
            }
            CSTools.CloseHandle(handle);
        }

        private void About_Click(object sender, EventArgs e)
        {
            if(about.IsDisposed || about == null){
                about = new AboutBox1();
            }
            about.Show();
        }


       
        
    }

    public class ListViewItemComparer : IComparer
    {
        public bool sort_b;
        public SortOrder order = SortOrder.Ascending;

        private int col;

        public ListViewItemComparer()
        {
            col = 0;
        }

        public ListViewItemComparer(int column, bool sort)
        {
            col = column;
            sort_b = sort;
        }

        public int Compare(object x, object y)
        {
            if (sort_b)
            {
                return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
            }
            else
            {
                
                return String.Compare(((ListViewItem)y).SubItems[col].Text, ((ListViewItem)x).SubItems[col].Text);
            }
        }

    }


}