using System;
using System.Collections.Generic;
using System.Text;

namespace TcpChatServer.Model
{
    public class DeviceRawData
    {
        public string RawData { get; set; }
        public string DeviceLogin { get; set; }
        public bool IsSync { get; set; } = false;
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
