using System.Collections.Generic;

class Program
{

    static void Main(string[] args)
    {

      List<string> ipAddresses = new()
      {
          "192.168.1.10",
          "10.0.0.5",
          "192.168.1.10",
          "172.16.0.2",
          "10.0.0.5",
          "192.168.1.10",
          "172.16.0.2",
          "10.0.0.5",
          "10.0.0.5"
      };

         var results = AnalyzeIPTraffic(ipAddresses);

         foreach(var result in results)
        {

            Console.WriteLine($"{result.Key}    -> {result.Value}");

        }
      


    }

    static Dictionary<string , int> AnalyzeIPTraffic(List<string> IpAdresses)
    {

        ArgumentNullException.ThrowIfNull(IpAdresses);

        Dictionary<string , int> results = new Dictionary<string, int>();


        foreach (var Ip in IpAdresses)
        {

            if (results.TryGetValue(Ip , out int count))
            {
                results[Ip] = count + 1;
            }
            else
            {
                results[Ip] = 1;
            }


        }

        return results;

    }

}