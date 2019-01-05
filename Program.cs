using System;
using System.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections.Generic;

namespace MonitorMySQLOpenFaclon
{
    class Miscellaneous
    {
        /// <summary>
        /// 获取正在运行的MySQL进程编号
        /// </summary>
        /// <value>return int List</value>
        private static List<int> _GetRunningMySQLPid
        {
            get
            {
                var pid_list = new System.Collections.Generic.List<int> { };
                var mysql_process_list = Process.GetProcessesByName("mysqld");
                foreach (Process p in mysql_process_list)
                {
                    pid_list.Add(p.Id);
                }
                return pid_list;
            }
        }
        /// <summary>
        /// 收集正在运行的MySQL实例基本信息,pid,ip,user,password,port,socket_path,exec_path
        /// </summary>
        /// <returns>The instance info struct list.</returns>
        public static List<MySQLInstance> GatherInstanceInfo
        {
            get
            {
                var mysql_list = new List<MySQLInstance> { };
                foreach (int pid in _GetRunningMySQLPid)
                {
                    var MySQLUtil = new MySQL.MySQLCommon(pid);
                    var ProcessUtil = new ProcessInfo(pid);
                    var user = "django";
                    var password = "123456";
                    var ip = Common.GetLocalIP();
                    var port_list = ProcessUtil.GetProcessPort();
                    // TODO:对于MGR来说，有两个端口,需要进行判断
                    // 三元表达式 https://docs.microsoft.com/zh-cn/dotnet/csharp/language-reference/operators/conditional-operator
                    var port = (port_list.Count >= 1) ? port_list[0] : -1;
                    MySQLInstance new_instance = new MySQLInstance
                    {
                        pid = pid,
                        ip = ip,
                        user = user,
                        password = password,
                        port = port,
                        socket_path = MySQLUtil.GetMySQLSocket,
                        exec_path = ProcessUtil.ExecPath
                    };
                    mysql_list.Add(new_instance);
                };
                return mysql_list;
            }
        }
    }
    class ConnMySQL
    {
        private MySQLInstance mysql_instance;

    }
    class Program
    {
        static void Main(string[] args)
        {
            var timer = Stopwatch.StartNew();
            foreach (var mysql in Miscellaneous.GatherInstanceInfo)
            {
                Console.WriteLine(string.Format("IP:{0}", mysql.ip));
                Console.WriteLine(string.Format("PORT:{0}", mysql.port));
                Console.WriteLine(string.Format("USER:{0}", mysql.user));
                Console.WriteLine(string.Format("PASSWORD:{0}", mysql.password));
                Console.WriteLine(string.Format("EXEC_FILE:{0}", mysql.exec_path));
                Console.WriteLine(string.Format("SOCKET_FILE:{0}", mysql.socket_path));
                Console.WriteLine("\n");
            }
            timer.Stop();
            var elapsed = timer.Elapsed;
            Console.WriteLine(String.Format("\n\n{0:00}:{1:00}:{2:00}\n\n", elapsed.Minutes, elapsed.Seconds, elapsed.Milliseconds / 10));
        }
    }
}
