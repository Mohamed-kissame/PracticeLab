using System.Collections.Generic;


class Program
{

    static void Main(string[] args)
    {


        List<int> StatusCodes = new List<int> { 200, 404, 200, 500, 201, 404, 403, 200, 500, 201 };

        var results = AnalyzeStatusCodes(StatusCodes);

        foreach (var result in results)
        {

            Console.WriteLine($"{result.Key}\t\t\t: {result.Value}");

        }

    }

    static Dictionary<string, int> AnalyzeStatusCodes(List<int> statusCodes)
    {
        ArgumentNullException.ThrowIfNull(statusCodes);

        var results = new Dictionary<string, int>
        {
         { "Total", statusCodes.Count },
         { "Successful", 0 },
         { "ClientErrors", 0 },
         { "ServerErrors", 0 },
         { "Status200Occurrences", 0 },
         { "DistinctStatusCodes", 0 }
        };

        HashSet<int> uniqueCodes = new();

        foreach (int code in statusCodes)
        {
            uniqueCodes.Add(code);

            string? category = code switch
            {
                >= 200 and <= 299 => "Successful",
                >= 400 and <= 499 => "ClientErrors",
                >= 500 and <= 599 => "ServerErrors",
                _ => null
            };

            if (category is not null)
            {
                results[category]++;
            }

            if (code == 200)
            {
                results["Status200Occurrences"]++;
            }
        }

        results["DistinctStatusCodes"] = uniqueCodes.Count;

        return results;
    }



}