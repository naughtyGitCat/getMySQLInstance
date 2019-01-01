using System;
namespace getMySQLSocket
{
    public struct MySQLInstance
    {
        public int pid;
        public string ip;
        public int port;
        public string socket_path;
        public string exec_path;
        public string data_path;
    }
}
