using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadValidateSecurityLogs
{
    public class SecurityLogLoadResult
    {

        public List<HttpLog> ValidLogs { get; set; } = new();

        public List<string> ValidationErrors { get; set; } = new();
    }
}
