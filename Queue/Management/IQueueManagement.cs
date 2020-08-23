using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Queue.Management
{
    public interface IQueueManagement
    {
        public void CreateQueue(string Queue);
        public void Produce<T>(T Body, string Queue);
        void SetQueueConsumer<T>(string Queue, Action<T> Callback);
    }

    public class ConnectArgs 
    {
        public string Host { get; set; } = "localhost";
    }
}
