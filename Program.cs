using System;
using Newtonsoft.Json;
using System.Diagnostics;
namespace getMySQLSocket
{
    class Miscellanoues
    {
        readonly string ip = Common.GetLocalIP();
        public static System.Collections.Generic.List<int> GetMySQLPid()
        {
            var pid_list = new System.Collections.Generic.List<int> {};

            var mysql_process_list = Process.GetProcessesByName("mysqld");

            foreach (Process p in mysql_process_list)
            {
                pid_list.Add(p.Id);
            }
            return pid_list;
        }
        //public MySQLInstance[] GatherInstanceInfo()
        //{
        //    foreach (int pid in GetMySQLPid())
        //    {
        //        MySQLInstance new_instance = new MySQLInstance
        //        {
        //            pid = pid,
        //            ip= ip,
        //        };
                
        //    }
        //}

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
        }
    }
}
