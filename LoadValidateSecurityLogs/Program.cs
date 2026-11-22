using System.Text.Json;
using LoadValidateSecurityLogs;

class Program {
    static void Main(string[] args)
    {
        string filePath = Path.Combine(AppContext.BaseDirectory,"security_logs.json");

        SecurityLogLoader loader = new();

        try
        {

            SecurityLogLoadResult loadResult =
                loader.LoadSecurityLogs(filePath);

            Console.WriteLine("=== LOADING RESULTS ===");

            Console.WriteLine(
                $"Valid records    : {loadResult.ValidLogs.Count}");

            Console.WriteLine(
                $"Validation errors: {loadResult.ValidationErrors.Count}");

            // 2. Display validation errors
            if (loadResult.ValidationErrors.Count > 0)
            {
                Console.WriteLine("\n=== VALIDATION ERRORS ===");

                foreach (string error in loadResult.ValidationErrors)
                {
                    Console.WriteLine($"- {error}");
                }
            }

            // 3. Analyze only valid records
            Console.WriteLine("\n=== SECURITY ANALYSIS ===");

            SecurityLogReport report = AnalyzeSecurityLogs(loadResult.ValidLogs);

            // 4. Display analysis results
            DisplayResults(report);
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"File not found: {ex.Message}");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Invalid JSON: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"File reading error: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Access denied: {ex.Message}");
        }
    }

    static void DisplayResults(SecurityLogReport report)
    {
        ArgumentNullException.ThrowIfNull(report);

        Console.WriteLine("=== SECURITY ANALYSIS REPORT ===");

        Console.WriteLine($"Total Requests     : {report.TotalRequests}");
        Console.WriteLine($"Unique IPs         : {report.TotalUniqueIp}");
        Console.WriteLine($"Failed Requests    : {report.CountFailedRequests}");
        Console.WriteLine($"Slow Requests      : {report.CountOfSlowRequests}");
        Console.WriteLine($"Average Response   : {report.AvgResponseTime:F2} ms");

        Console.WriteLine("\n=== REQUESTS BY IP ===");

        if (report.NumberOfRequestsByIp.Count == 0)
        {
            Console.WriteLine("No requests found.");
        }
        else
        {
            foreach (var entry in report.NumberOfRequestsByIp)
            {
                Console.WriteLine($"{entry.Key} -> {entry.Value} requests");
            }
        }

        Console.WriteLine("\n=== SUSPICIOUS IP ADDRESSES ===");

        if (report.IpAddressesAtLeastTwoFailedReq.Count == 0)
        {
            Console.WriteLine("No suspicious IP addresses detected.");
        }
        else
        {
            foreach (string ip in report.IpAddressesAtLeastTwoFailedReq)
            {
                Console.WriteLine($"- {ip}");
            }
        }

        Console.WriteLine("\n=== END OF REPORT ===");
    }


    static SecurityLogReport AnalyzeSecurityLogs(List<HttpLog> logs)
    {
        ArgumentNullException.ThrowIfNull(logs);

        int totalRequests = logs.Count;

        int uniqueIpCount = logs
            .Select(log => log.IpAddress)
            .Distinct()
            .Count();

        int failedRequests = logs
            .Count(log => log.StatusCode >= 400 &&
                          log.StatusCode <= 599);

        int slowRequests = logs
            .Count(log => log.ResponseTime > 300);

        double averageResponseTime = logs.Count > 0
            ? logs.Average(log => log.ResponseTime)
            : 0;

        Dictionary<string, int> requestsByIp = logs
            .GroupBy(log => log.IpAddress)
            .ToDictionary(
                group => group.Key,
                group => group.Count()
            );

        List<string> suspiciousIps = logs
            .Where(log => log.StatusCode >= 400 &&
                          log.StatusCode <= 599)
            .GroupBy(log => log.IpAddress)
            .Where(group => group.Count() >= 2)
            .Select(group => group.Key)
            .ToList();

        return new SecurityLogReport
        {
            TotalRequests = totalRequests,
            TotalUniqueIp = uniqueIpCount,
            CountFailedRequests = failedRequests,
            CountOfSlowRequests = slowRequests,
            AvgResponseTime = averageResponseTime,
            NumberOfRequestsByIp = requestsByIp,
            IpAddressesAtLeastTwoFailedReq = suspiciousIps
        };
    }

}



