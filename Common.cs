using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace getMySQLSocket
{
    public static class Common
    {
        /// <summary>
        /// 获取本机IP
        /// </summary>
        /// <returns>string 本机外联指定地址出口IP</returns>
        public static string GetLocalIP()
        {
            var ip = new string("250.250.250.250");
            try
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    // socket.Connect("tmfast-proxy.163.internal", 80);
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    ip = endPoint.Address.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fuck, get local ip failed \n {0}", ex);
            }
            return ip;
        }
        /// <summary>
        /// 返回链接文件的源地址
        /// </summary>
        /// <returns>string origin path.</returns>
        /// <param name="symbolic_path">Symbolic path.</param>
        public static string ReadSymbolicLink(string symbolic_path)
        {
            //var timer = Stopwatch.StartNew();
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/usr/bin/readlink",
                    Arguments = symbolic_path,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            string full_path = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            //Console.WriteLine("Hello World!");
            // full_path = System.IO.Path.GetRelativePath("/User/psyduck/ddb43");
            //Console.WriteLine(full_path);
            //timer.Stop();
            //TimeSpan elapsed = timer.Elapsed;
            //Console.WriteLine(String.Format("\n\n{0:00}:{1:00}:{2:00}\n\n", elapsed.Minutes, elapsed.Seconds, elapsed.Milliseconds / 10));

            return full_path;
        }
    }
    public class ProcessInfo
    {
        private readonly int pid;
        /// <summary>
        /// 构造函数，传入Pid
        /// </summary>
        /// <param name="process_id">Process identifier.</param>
        public ProcessInfo(int process_id)
         { 
            this.pid = process_id; 
         } 
        public string[] CmdLine
        {
            get {
                var raw = System.IO.File.ReadAllText(string.Format("/proc/{0}/cmdline", pid));
                return raw.Split("\0");
                }
        }
        public System.Collections.Generic.List<int> GetProcessPort()
        {
            var port_list = new System.Collections.Generic.List<int> { };
            var fd_path = System.IO.Directory.EnumerateFileSystemEntries(string.Format("/proc/{0}/fd", pid));
            // 对该进程打开的每一个文件描述符进行遍历，判断其链接对象是否为socket
            foreach (string fd in fd_path)
            {
                // origin_path是被fd链接的对象
                var origin_path = Common.ReadSymbolicLink(fd);
                // 如果打开的文件描述符中的链接对象是一个socket文件，那么进入获取这个文件的innode编号，然后和
                if (origin_path.StartsWith("socket:[",StringComparison.Ordinal)) //二进制方式比较字符串前缀是否相同
                {
                    //Console.WriteLine(origin_path+"l0ve");
                    // 该进程的/
                    var socket_innode = origin_path.Substring(origin_path.IndexOf('[') + 1, origin_path.IndexOf(']') - origin_path.IndexOf('[') - 1);
                    // var task =  System.IO.File.ReadAllLinesAsync(string.Format("/proc/{0}/net/tcp", pid));
                    var net_tcp = System.IO.File.ReadLines(string.Format("/proc/{0}/net/tcp", pid));
                    //System.IO.File.ReadAllLines
                    foreach (string line in net_tcp.Skip(1))  // 跳过第一行列名 use linq
                    {
                        string net_tcp_inode = (line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries))[9];
                        if (socket_innode == net_tcp_inode)
                        {
                            // 16位的IP:PORT
                            var hex_port_str = (((line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries))[1]).Split(':'))[1];
                            Console.WriteLine((line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries))[1]);
                            int process_port = Convert.ToInt32(hex_port_str, 16);
                            Console.WriteLine(Convert.ToInt32(hex_port_str,16));
                            port_list.Add(process_port);
                        }
                        // https://stackoverflow.com/questions/6111298/best-way-to-specify-whitespace-in-a-string-split-operation
                        //Console.WriteLine((line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries))[9]);
                    }
                }
            }
            return port_list;
        }
    }
}
