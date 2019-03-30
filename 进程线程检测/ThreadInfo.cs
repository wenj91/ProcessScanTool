using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessThreadScanTools
{

    class ThreadInfo
    {
        private String tid { set; get; }
        private String priority { set; get; }
        private String tEnter { set; get; }
        private String tModule { set; get; }
        private String tStatus { set; get; }
        private String tWaitReason { set; get; }
        public ThreadInfo() { }
    }
}
