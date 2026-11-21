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



    }

    static Dictionary<string , int> AnalyzeIPTraffic(List<string> IpAdresses)
    {

        ArgumentNullException.ThrowIfNull(IpAdresses);

        Dictionary<string , int> results = new Dictionary<string, int>();


        foreach (var Ip in IpAdresses)
        {

            if (results.ContainsKey(Ip))
            {
                results[Ip]++;
            }
            else
            {
                results[Ip] = 1;
            }


        }

        return results;

    }

}