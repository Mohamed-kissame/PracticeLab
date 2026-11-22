using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadValidateSecurityLogs
{
    public class HttpLog
    {

        public string IpAddress { get; set; } = string.Empty;

        public int StatusCode { get; set; }

        public int ResponseTime { get; set; }
    }
}
