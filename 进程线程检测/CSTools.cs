using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ProcessThreadScanTools
{
    class CSTools
    {
        public enum ThreadInfoClass
        {
            ThreadBasicInformation,
            ThreadTimes,
            ThreadPriority,
            ThreadBasePriority,
            ThreadAffinityMask,
            ThreadImpersonationToken,
            ThreadDescriptorTableEntry,
            ThreadEnableAlignmentFaultFixup,
            ThreadEventPair_Reusable,
            ThreadQuerySetWin32StartAddress,
            ThreadZeroTlsCell,
            ThreadPerformanceCount,
            ThreadAmILastThread,
            ThreadIdealProcessor,
            ThreadPriorityBoost,
            ThreadSetTlsArrayAddress,   // Obsolete  
            ThreadIsIoPending,
            ThreadHideFromDebugger,
            ThreadBreakOnTermination,
            ThreadSwitchLegacyState,
            ThreadIsTerminated,
            ThreadLastSystemCall,
            ThreadIoPriority,
            ThreadCycleTime,
            ThreadPagePriority,
            ThreadActualBasePriority,
            ThreadTebInformation,
            ThreadCSwitchMon,          // Obsolete  
            ThreadCSwitchPmu,
            ThreadWow64Context,
            ThreadGroupInformation,
            ThreadUmsInformation,      // UMS  
            ThreadCounterProfiling,
            ThreadIdealProcessorEx,
            MaxThreadInfoClass
        }

        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200),
            THREAD_ALL_ACCESS = (0x001F03FFF),
            DELETE = 0x10000,        
            READ_CONTROL = 0x20000,        
            WRITE_DAC = 0x40000,    
            WRITE_OWNER = 0x80000,    
            SYNCHRONIZE = 0x100000,    
            THREAD_DIRECT_IMPERSONATION = 0x200,    
            THREAD_GET_CONTEXT = 0x8,    
            THREAD_IMPERSONATE = 0x100,    
            THREAD_QUERY_INFORMATION = 0x40,    
            THREAD_QUERY_LIMITED_INFORMATION = 0x800,    
            THREAD_SET_CONTEXT = 0x10,    
            THREAD_SET_INFORMATION = 0x20,    
            THREAD_SET_LIMITED_INFORMATION = 0x400,    
            THREAD_SET_THREAD_TOKEN = 0x80,    
            THREAD_SUSPEND_RESUME = 0x2,    
            THREAD_TERMINATE = 0x1,    
            STANDARD_RIGHTS_REQUIRED=0x000F0000,    
            PROCESS_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0xFFF)
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, int dwThreadId);
        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll")]
        public static extern int TerminateThread(IntPtr hThread, int exitCode);
        [DllImport("kernel32.dll")]
        public static extern int SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        public static extern int ResumeThread(IntPtr hThread);
        [DllImport("Psapi.dll")]
        public static extern int GetMappedFileName(
                          IntPtr hProcess,
                          int addr,
                          out string lpFilename,
                          int nSize
        );
        [DllImport("ntdll.dll")]
        public static extern int NtQueryInformationThread(
                          IntPtr ThreadHandle,
                          ThreadInfoClass ThreadInformationClass,
                          out int ThreadInformation,
                          int ThreadInformationLength,
                          int ReturnLength
        );

        private static void SuspendProcess(int pid)
        {
            Process process = Process.GetProcessById(pid);

            if (process.ProcessName == string.Empty)
                return;

            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (int)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }

                SuspendThread(pOpenThread);

                CloseHandle(pOpenThread);
            }
        }

        public static void ResumeProcess(int pid)
        {
            Process process = Process.GetProcessById(pid);

            if (process.ProcessName == string.Empty)
                return;

            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (int)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }

                int suspendCount = 0;
                do
                {
                    suspendCount = ResumeThread(pOpenThread);
                } while (suspendCount > 0);

                CloseHandle(pOpenThread);
            }
        }

        public static string GetModuleName(Process pr, int entryAddr){
            string name = "";
            for (int i = 0; i < pr.Modules.Count; i++)
            {
                if (entryAddr >= pr.Modules[i].BaseAddress.ToInt32() && entryAddr <= (pr.Modules[i].BaseAddress.ToInt32() + pr.Modules[i].ModuleMemorySize))
                {
                    name = pr.Modules[i].ModuleName;
                }
            }
            Console.WriteLine(name);
            return name;
        }

        public static string GetThreadStatus(ProcessThread pt){
            switch(pt.ThreadState){
                case ThreadState.Initialized:
                    return "初始化";
                case ThreadState.Ready:
                    return "预备";
                case ThreadState.Running:
                    return "正在运行";
                case ThreadState.Standby:
                    return "准备运行";
                case ThreadState.Terminated:
                    return "终止";
                case ThreadState.Transition:
                    return "等待资源";
                case ThreadState.Unknown:
                    return "未知";
                case ThreadState.Wait:
                    return "等待";
                default:
                    return "未知"; 
            }
        }

        public static string GetThreadWaitReason(ProcessThread pt)
        {
            switch (pt.WaitReason)
            {
                case ThreadWaitReason.EventPairHigh:
                    Console.WriteLine("线程正在等待事件对高");
                    return "线程正在等待事件对高";
                case ThreadWaitReason.EventPairLow:
                    Console.WriteLine("线程正在等待事件对低");
                    return "线程正在等待事件对低";
                case ThreadWaitReason.ExecutionDelay:
                    Console.WriteLine("线程执行延迟");
                    return "线程执行延迟";
                case ThreadWaitReason.Executive:
                    Console.WriteLine("线程正在等待计划程序");
                    return "线程正在等待计划程序";
                case ThreadWaitReason.FreePage:
                    Console.WriteLine("线程正在等待可用的虚拟内存页");
                    return "线程正在等待可用的虚拟内存页";
                case ThreadWaitReason.LpcReceive:
                    Console.WriteLine("线程正在等待本地过程调用到达");
                    return "线程正在等待本地过程调用到达";
                case ThreadWaitReason.LpcReply:
                    Console.WriteLine("线程正在等待对本地过程调用的回复到达");
                    return "线程正在等待对本地过程调用的回复到达";
                case ThreadWaitReason.PageIn:
                    Console.WriteLine("线程正在等待虚拟内存页到达内存");
                    return "线程正在等待虚拟内存页到达内存";
                case ThreadWaitReason.PageOut:
                    Console.WriteLine("线程正在等待虚拟内存页写入磁盘");
                    return "线程正在等待虚拟内存页写入磁盘";
                case ThreadWaitReason.Suspended:
                    Console.WriteLine("线程执行暂停");
                    return "线程执行暂停";
                case ThreadWaitReason.SystemAllocation:
                    Console.WriteLine("线程正在等待系统分配");
                    return "线程正在等待系统分配";
                case ThreadWaitReason.Unknown:
                    Console.WriteLine("线程因位置原因而等待");
                    return "线程因位置原因而等待";
                case ThreadWaitReason.UserRequest:
                    Console.WriteLine("线程正在等待用户请求");
                    return "线程正在等待用户请求";
                case ThreadWaitReason.VirtualMemory:
                    Console.WriteLine("线程正在等待系统分配虚拟内存");
                    return "线程正在等待系统分配虚拟内存";
                default:
                    return "未知";
            }
        }

        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr phtok);

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool LookupPrivilegeValue(string host, string name, ref long pluid);

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool AdjustTokenPrivileges(IntPtr htok, bool disall,
        ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool ExitWindowsEx(int flg, int rea);  
  

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct TokPriv1Luid
        {
            public int Count;
            public long Luid;
            public int Attr;
        }   
        const int TOKEN_ADJUST_PRIVILEGES = 0x20;
        const string SE_BACKUP_NAME = ("SeBackupPrivilege");
        const string SE_RESTORE_NAME = ("SeRestorePrivilege");
        const string SE_SHUTDOWN_NAME = ("SeShutdownPrivilege");
        const string SE_DEBUG_NAME = ("SeDebugPrivilege");
        internal const int SE_PRIVILEGE_ENABLED = 0x00000002;
        internal const int TOKEN_QUERY = 0x00000008;
        internal const int EWX_SHUTDOWN = 0x00000001;
        //internal const int EWX_LOGOFF = 0x00000000;  
        //internal const int EWX_REBOOT = 0x00000002;  
        //internal const int EWX_FORCE = 0x00000004;  
        //internal const int EWX_POWEROFF = 0x00000008;  
        //internal const int EWX_FORCEIFHUNG = 0x00000010;   

        public static bool EnableDebugPrivilege(bool bEnable) 
        {
            // 附给本进程特权，以便访问系统进程
            bool bOk = false;;
            TokPriv1Luid tp;
            IntPtr htok = IntPtr.Zero;

            // 打开一个进程的访问令牌
            if (OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES, ref htok)) 
            {
                // 调整特权级别

                bOk = OpenProcessToken(htok, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref htok);
                tp.Count = 1;
                tp.Luid = 0;
                tp.Attr = SE_PRIVILEGE_ENABLED;
                // 取得特权名称为“SetDebugPrivilege”的LUID
                LookupPrivilegeValue(null, SE_DEBUG_NAME, ref tp.Luid);
                bOk = AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);   

                // 关闭访问令牌句柄
                CloseHandle(htok);
            }
            return bOk;
        }
    }
}
