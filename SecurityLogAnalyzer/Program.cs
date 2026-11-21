using System.Collections.Generic;



public class Log
{
    public string IpAdress {  get; set; } = string.Empty;

    public int StatusCode { get; set; }

    public int ResponseTime { get; set; }
}

public class SecurityLogReport
{
    public int TotalRequests { get; set; }

    public int TotalUniqueIp { get; set; }

    public int CountFailedRequests { get; set; }

    public int CountOfSlowRequests { get; set; }

    public double AvgResponseTime { get; set; }

    public Dictionary<string, int> NumberOfRequestsByIp { get; set; } = new();

    public List<string> IpAddressesAtLeastTwoFailedReq { get; set; } = new();

}

class Program
{

    static void Main(string[] args)
    {

        List<Log> logs = new List<Log>
        {

            new Log{ IpAdress = "192.168.1.10" , StatusCode = 200 , ResponseTime = 120 },
             new Log{ IpAdress = "10.0.0.5" , StatusCode = 404 , ResponseTime = 450 },
              new Log{ IpAdress = "192.168.1.10" , StatusCode = 500 , ResponseTime = 600 },
               new Log{ IpAdress = "172.16.0.2" , StatusCode = 200 , ResponseTime = 80 },
                new Log{ IpAdress = "10.0.0.5" , StatusCode = 200 , ResponseTime = 230 },
                 new Log{ IpAdress = "192.168.1.10" , StatusCode = 403 , ResponseTime = 900 },
                 new Log{ IpAdress = "10.0.0.5" , StatusCode = 500 , ResponseTime = 320 },
                  new Log{ IpAdress = "172.16.0.2" , StatusCode = 201 , ResponseTime = 700 },
                   new Log{ IpAdress = "192.168.1.10" , StatusCode = 200 , ResponseTime = 110 },
                    new Log{ IpAdress = "172.16.0.2" , StatusCode = 404 , ResponseTime = 310 }

        };

    }

    static SecurityLogReport AnalyzeSecurityLogs(List<Log> logs)
    {

        ArgumentNullException.ThrowIfNull(logs);

        var TotalLogs = logs.Count;

        var CountUniqueIpAddresses = new HashSet<Log>(logs).Count;

        var CountFailedReq = logs.Count(l => l.StatusCode >= 400 && l.StatusCode <= 599);

        var CountSlowReq = logs.Count(l => l.ResponseTime > 300);

        var AvgReqTime = logs.Average(l => l.ResponseTime);

        var ReqByIp = logs.GroupBy(l => l.IpAdress).ToDictionary(g => g.Key, g => g.Count());

        var IpWithTwoFailedReq = logs.Where(l => l.StatusCode >= 400 && l.StatusCode <= 599).GroupBy(l => l.IpAdress).Where(g => g.Count() >= 2).Select(g => g.Key).ToList();

        return new SecurityLogReport
        {

            TotalRequests = TotalLogs,
            TotalUniqueIp = TotalLogs,
            CountFailedRequests = CountFailedReq,
            CountOfSlowRequests = CountSlowReq,
            AvgResponseTime = AvgReqTime,
            NumberOfRequestsByIp = ReqByIp,
            IpAddressesAtLeastTwoFailedReq = IpWithTwoFailedReq

        };

    }



}