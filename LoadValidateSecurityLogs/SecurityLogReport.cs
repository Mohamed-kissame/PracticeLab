
using System.Collections.Generic;

namespace LoadValidateSecurityLogs
{
    public class SecurityLogReport
    {
        public int TotalRequests { get; set; }

        public int TotalUniqueIp { get; set; }

        public int CountFailedRequests { get; set; }

        public int CountOfSlowRequests { get; set; }

        public double AvgResponseTime { get; set; }

        public Dictionary<string, int> NumberOfRequestsByIp
        {
            get;
            set;
        } = new();

        public List<string> IpAddressesAtLeastTwoFailedReq
        {
            get;
            set;
        } = new();
    }
}
