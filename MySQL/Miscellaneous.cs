using System;

namespace MonitorMySQLOpenFaclon.MySQL
{
    public class MySQLCommon
    {
        private readonly int pid;
        public MySQLCommon(int pid)
        {
            this.pid = pid;
        }
        public string GetMySQLSocket
        {
            get
            {
                var socket_file = "";
                var MySQLProcess = new ProcessInfo(pid);
                var cmdlines = MySQLProcess.CmdLine;
                foreach (string cmdline in cmdlines)
                {
                    if (cmdline.Contains("--socket="))
                    {
                        socket_file = cmdline.Split("--socket=")[1];
                    }
                }
                if (socket_file == "")
                {
                    Console.WriteLine("进程编号:{0}启动参数行没有使用--socket参数");
                }
                return socket_file;
            }
        }
    }
}
