# getMySQLInstance
获取本机MySQL的信息(PID,PORT,SOCKET,EXEC_PATH,DATA_PATH)

```bash
思路：
    linux下一个进程相关的所有信息都在/proc/${pid}/ 路径下
    
    S1 其中 fd目录下存放着文件描述符，链接到该进程打开的文件列表，其中有源文件为socket:[XXXX]
    
        其中XXX为socket文件的inode编号
    
    S2 又知net目录下的tcp文件中存放着TCP服务的连接信息
        其中有一列为innode值，且第一列为IP:PORT信息。
    
    那么用S1中的INNODE和S2中的INNODE进行匹配，获取对应行的第一列的IP:PORT信息。
    当然是使用nest loop的方式进行匹配查找
    
这种方式用于判断MySQL的开放端口是没问题的，使用 .netCore 运行环境，一个Pid的端口所需30毫秒
```


C# 单个进程花了半秒，有点慢了
