using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


namespace LoadValidateSecurityLogs
{
    public class SecurityLogLoader
    {

        public SecurityLogLoadResult LoadSecurityLogs(string filePath)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

            SecurityLogLoadResult result = new();

            string json = File.ReadAllText(filePath);

            using JsonDocument document = JsonDocument.Parse(json);

            if (document.RootElement.ValueKind != JsonValueKind.Array)
            {
                throw new JsonException("JSON root must be an array.");
            }

            int recordNumber = 0;

            foreach (JsonElement record in document.RootElement.EnumerateArray())
            {
                recordNumber++;

                if (record.ValueKind != JsonValueKind.Object)
                {
                    result.ValidationErrors.Add($"Record {recordNumber}: Expected a JSON object.");
                    continue;
                }

                List<string> errors = new();

                string ipAddress = string.Empty;
                int statusCode = 0;
                int responseTime = 0;

                
                if (!record.TryGetProperty("IPAddress", out JsonElement ipElement)
                    || ipElement.ValueKind != JsonValueKind.String
                    || string.IsNullOrWhiteSpace(ipElement.GetString()))
                {
                    errors.Add("Invalid or missing IPAddress");
                }
                else
                {
                    ipAddress = ipElement.GetString()!;
                }

              
                if (!record.TryGetProperty("StatusCode", out JsonElement statusElement)
                    || statusElement.ValueKind != JsonValueKind.Number
                    || !statusElement.TryGetInt32(out statusCode)
                    || statusCode < 100
                    || statusCode > 599)
                {
                    errors.Add("StatusCode must be between 100 and 599");
                }

            
                if (!record.TryGetProperty("ResponseTime", out JsonElement timeElement)
                    || timeElement.ValueKind != JsonValueKind.Number
                    || !timeElement.TryGetInt32(out responseTime)
                    || responseTime < 0)
                {
                    errors.Add("ResponseTime must be a non-negative integer");
                }

                
                if (errors.Count > 0)
                {
                    foreach (string error in errors)
                    {
                        result.ValidationErrors.Add($"Record {recordNumber}: {error}");
                    }

                    continue;
                }

              
                result.ValidLogs.Add(new HttpLog
                {
                    IpAddress = ipAddress,
                    StatusCode = statusCode,
                    ResponseTime = responseTime
                });
            }

            return result;
        }

    }
}
