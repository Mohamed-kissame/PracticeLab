
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        List<int> responseTimes = new()
        {
            120, 450, 80, 600, 230,
            900, 150, 320, 700, 110
        };

        
        Console.WriteLine("=== Test 1: Normal Dataset ===");
        DisplayReport(AnalyzeResponseTimes(responseTimes));

        
        Console.WriteLine("\n=== Test 2: Empty Dataset ===");
        DisplayReport(AnalyzeResponseTimes(new List<int>()));

        
        Console.WriteLine("\n=== Test 3: Fast Responses ===");
        DisplayReport(AnalyzeResponseTimes(new List<int> { 100, 150, 200 }));


        Console.WriteLine("\n=== Test 4: Boundary (800 ms) ===");
        DisplayReport(AnalyzeResponseTimes(new List<int> { 800 }));
    }

    static ResponseTimeReport AnalyzeResponseTimes(List<int> responseTimes)
    {
        ArgumentNullException.ThrowIfNull(responseTimes);

        var slowRequests = responseTimes.Where(r => r > 300).ToList();

        var sortedResponseTimes = responseTimes.OrderByDescending(r => r).ToList();

        var topThreeSlowest = sortedResponseTimes.Take(3).ToList();

        double averageResponseTime = responseTimes.Count > 0 ? responseTimes.Average(): 0;

        int fastRequestsCount = responseTimes.Count(r => r < 200);

        bool hasVerySlowRequests = responseTimes.Any(r => r > 800);

        double slowRequestsPercentage = responseTimes.Count > 0 ? (double)slowRequests.Count / responseTimes.Count * 100 : 0;

        return new ResponseTimeReport
        {
            SlowRequests = slowRequests,
            SortedResponseTimes = sortedResponseTimes,
            TopThreeSlowest = topThreeSlowest,
            AverageResponseTime = averageResponseTime,
            FastRequestsCount = fastRequestsCount,
            HasVerySlowRequests = hasVerySlowRequests,
            SlowRequestsPercentage = slowRequestsPercentage
        };
    }

    static void DisplayReport(ResponseTimeReport report)
    {
        Console.WriteLine($"Slow requests: {string.Join(", ", report.SlowRequests)}");

        Console.WriteLine($"Sorted responses: {string.Join(", ", report.SortedResponseTimes)}");

        Console.WriteLine($"Top three slowest: {string.Join(", ", report.TopThreeSlowest)}");

        Console.WriteLine($"Average: {report.AverageResponseTime:F2} ms");

        Console.WriteLine($"Fast requests: {report.FastRequestsCount}");

        Console.WriteLine($"Has very slow requests: {report.HasVerySlowRequests}");

        Console.WriteLine( $"Slow request percentage: {report.SlowRequestsPercentage:F2}%");
    }
}

public class ResponseTimeReport
{
    public List<int> SlowRequests { get; set; } = new();

    public List<int> SortedResponseTimes { get; set; } = new();

    public List<int> TopThreeSlowest { get; set; } = new();

    public double AverageResponseTime { get; set; }

    public int FastRequestsCount { get; set; }

    public bool HasVerySlowRequests { get; set; }

    public double SlowRequestsPercentage { get; set; }
}
