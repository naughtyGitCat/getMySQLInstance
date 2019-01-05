using System;
using Newtonsoft.Json;
using System.Diagnostics;
namespace MonitorMySQLOpenFaclon
{
    class Miscellaneous
    {

        public static System.Collections.Generic.List<int> GetMySQLPid()
        {
            Console.WriteLine("Now in Miscellaneous.GetMySQLPid()");
            var pid_list = new System.Collections.Generic.List<int> {};

            var mysql_process_list = Process.GetProcessesByName("mysqld");

            foreach (Process p in mysql_process_list)
            {
                Console.WriteLine("line16:{0}", p.Id);
                pid_list.Add(p.Id);
            }
            return pid_list;
        }
        public static System.Collections.Generic.List<MySQLInstance> GatherInstanceInfo()
        {
            var mysql_list = new System.Collections.Generic.List<MySQLInstance> { };
            foreach (int pid in GetMySQLPid())
            {
                var MySQLUtil = new MySQL.MySQLCommon(pid);
                var ProcessUtil = new ProcessInfo(pid);
                var user = "django";
                var password = "123456";
                string ip = Common.GetLocalIP();
                MySQLInstance new_instance = new MySQLInstance
                {
                    pid = pid,
                    ip= ip,
                    user = user,
                    password = password,
                    socket_path = MySQLUtil.GetMySQLSocket,
                    port = ProcessUtil.GetProcessPort()[0]
                };
                mysql_list.Add(new_instance);
            };
            return mysql_list;
        }

    }
    class Program
    {
        //static void Main(string[] args)
        //{
        //    var timer = Stopwatch.StartNew();
        //    var mysqld = new ProcessInfo(7249);
        //    //Console.WriteLine()
        //    Console.WriteLine(string.Join("\n",mysqld.CmdLine));
        //    Console.WriteLine(JsonConvert.SerializeObject(Miscellanoues.GetMySQLPid()));
        //    timer.Stop();
        //    TimeSpan elapsed = timer.Elapsed;
        //    Console.WriteLine(String.Format("\n\n{0:00}:{1:00}:{2:00}\n\n", elapsed.Minutes, elapsed.Seconds, elapsed.Milliseconds / 10));
        //}
        static void Main(string[] args)
        {
            var timer = Stopwatch.StartNew();
            var mysqld = new ProcessInfo(12816);
            mysqld.GetProcessPort();
            timer.Stop();
            TimeSpan elapsed = timer.Elapsed;
            Console.WriteLine(String.Format("\n\n{0:00}:{1:00}:{2:00}\n\n", elapsed.Minutes, elapsed.Seconds, elapsed.Milliseconds / 10));
            Console.WriteLine("fuck");
            timer = Stopwatch.StartNew();
            foreach (var mysql in Miscellaneous.GatherInstanceInfo())
            {
                Console.WriteLine(mysql.port);
            }
            timer.Stop();
            elapsed = timer.Elapsed;
            Console.WriteLine(String.Format("\n\n{0:00}:{1:00}:{2:00}\n\n", elapsed.Minutes, elapsed.Seconds, elapsed.Milliseconds / 10));
        }
    }
}
