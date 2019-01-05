using System;
namespace MonitorMySQLOpenFaclon
{
    public struct MySQLInstance
    {
        public int pid;
        public string ip;
        public int port;
        public string user;
        public string password;
        public string socket_path;
        public string exec_path;
        // public string data_path;
    }
}
