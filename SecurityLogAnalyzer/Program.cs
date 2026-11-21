using System.Collections.Generic;

public class HttpRequestLog
{
    public string IpAddress {  get; set; } = string.Empty;

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

        List<HttpRequestLog> logs = new List<HttpRequestLog>
        {

            new HttpRequestLog{ IpAddress = "192.168.1.10" , StatusCode = 200 , ResponseTime = 120 },
             new HttpRequestLog{ IpAddress = "10.0.0.5" , StatusCode = 404 , ResponseTime = 450 },
              new HttpRequestLog{ IpAddress = "192.168.1.10" , StatusCode = 500 , ResponseTime = 600 },
               new HttpRequestLog{ IpAddress = "172.16.0.2" , StatusCode = 200 , ResponseTime = 80 },
                new HttpRequestLog{ IpAddress = "10.0.0.5" , StatusCode = 200 , ResponseTime = 230 },
                 new HttpRequestLog{ IpAddress = "192.168.1.10" , StatusCode = 403 , ResponseTime = 900 },
                 new HttpRequestLog{ IpAddress = "10.0.0.5" , StatusCode = 500 , ResponseTime = 320 },
                  new HttpRequestLog{ IpAddress = "172.16.0.2" , StatusCode = 201 , ResponseTime = 700 },
                   new HttpRequestLog{ IpAddress = "192.168.1.10" , StatusCode = 200 , ResponseTime = 110 },
                    new HttpRequestLog{ IpAddress = "172.16.0.2" , StatusCode = 404 , ResponseTime = 310 }

        };

        DisplayResults(AnalyzeSecurityLogs(logs));

    }

    static SecurityLogReport AnalyzeSecurityLogs(List<HttpRequestLog> logs)
    {

        ArgumentNullException.ThrowIfNull(logs);

        var totalLogs = logs.Count;

        var CountUniqueIpAddresses = logs.Select(l => l.IpAddress).Distinct().Count();

        var failedRequestCount = logs.Count(l => l.StatusCode >= 400 && l.StatusCode <= 599);

        var CountSlowReq = logs.Count(l => l.ResponseTime > 300);

        var averageResponseTime = logs.Count != 0 ? logs.Average(l => l.ResponseTime) : 0;

        var requestsByIp = logs.GroupBy(l => l.IpAddress).ToDictionary(g => g.Key, g => g.Count());

        var IpWithTwoFailedReq = logs.Where(l => l.StatusCode >= 400 && l.StatusCode <= 599).GroupBy(l => l.IpAddress).Where(g => g.Count() >= 2).Select(g => g.Key).ToList();

        return new SecurityLogReport
        {

            TotalRequests = totalLogs,
            TotalUniqueIp = CountUniqueIpAddresses,
            CountFailedRequests = failedRequestCount,
            CountOfSlowRequests = CountSlowReq,
            AvgResponseTime = averageResponseTime,
            NumberOfRequestsByIp = requestsByIp,
            IpAddressesAtLeastTwoFailedReq = IpWithTwoFailedReq

        };

    }

    static void DisplayResults(SecurityLogReport report)
    {

        Console.WriteLine($"Total requests   : {report.TotalRequests}");
        Console.WriteLine($"Unique IPs       : {report.TotalUniqueIp}");
        Console.WriteLine($"Failed requests  : {report.CountFailedRequests}");
        Console.WriteLine($"Slow requests    : {report.CountOfSlowRequests}");
        Console.WriteLine($"Avg res Time     : {report.AvgResponseTime} ms");
        Console.WriteLine("---------------------------------------------");
        foreach(var entry in report.NumberOfRequestsByIp)
        {
            Console.WriteLine($"{entry.Key}        ->   {entry.Value}");
        }
        Console.WriteLine("---------------------------------------------");
        Console.WriteLine($"suspicious IPs   : {string.Join(" , " , report.IpAddressesAtLeastTwoFailedReq)}");

    }



}