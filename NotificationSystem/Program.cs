using System;
using System.Collections.Generic;
using System.Linq;


public interface INotificationSender
{
    void Send(string message);
}


public class EmailSender : INotificationSender
{
    public void Send(string message)
    {
        throw new InvalidOperationException("Email server unavailable");
    }
}


public class SmsSender : INotificationSender
{
    public void Send(string message)
    {
        Console.WriteLine($"SMS sent: {message}");
    }
}

public class PushSender : INotificationSender
{
    public void Send(string message)
    {
        Console.WriteLine($"Push notification sent: {message}");
    }
}


public class NotificationReport
{
    public int TotalAttempts { get; set; }
    public int SuccessfulCount { get; set; }
    public int FailedCount { get; set; }

    public List<string> Errors { get; set; } = new();
}


public class NotificationService
{
    private readonly List<INotificationSender> _senders = new();

    public void RegisterSender(INotificationSender sender)
    {
        ArgumentNullException.ThrowIfNull(sender);

        
        if (_senders.Any(s => ReferenceEquals(s, sender)))
        {
            return;
        }

        _senders.Add(sender);
    }

    public NotificationReport NotifyAll(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Notification message cannot be empty.", nameof(message));
        }

        NotificationReport report = new();

        foreach (INotificationSender sender in _senders)
        {
            report.TotalAttempts++;

            try
            {
                sender.Send(message);
                report.SuccessfulCount++;
            }
            catch (Exception ex)
            {
                report.FailedCount++;

                string senderName = sender.GetType().Name;

                report.Errors.Add($"{senderName}: {ex.Message}");
            }
        }

        return report;
    }
}

class Program
{
    static void Main()
    {
        NotificationService service = new();

        // TEST 1: Empty service
        Console.WriteLine("=== TEST 1: Empty Service ===");

        var emptyReport = service.NotifyAll("Hello");
        DisplayReport(emptyReport);

        // TEST 2: Email failure, SMS and Push success
        Console.WriteLine("\n=== TEST 2: Mixed Results ===");

        EmailSender email = new();
        SmsSender sms = new();
        PushSender push = new();

        service.RegisterSender(email);
        service.RegisterSender(sms);
        service.RegisterSender(push);

        var report = service.NotifyAll("Order completed");
        DisplayReport(report);

        // TEST 3: Duplicate registration
        Console.WriteLine("\n=== TEST 3: Duplicate Sender ===");

        service.RegisterSender(email);

        var duplicateReport = service.NotifyAll("Payment received");
        DisplayReport(duplicateReport);

        // TEST 4: Null sender
        Console.WriteLine("\n=== TEST 4: Null Sender ===");

        try
        {
            service.RegisterSender(null!);
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Expected exception: {ex.GetType().Name}");
        }

        // TEST 5: All senders fail
        Console.WriteLine("\n=== TEST 5: All Senders Fail ===");

        NotificationService failingService = new();

        failingService.RegisterSender(new EmailSender());
        failingService.RegisterSender(new EmailSender());

        var failedReport = failingService.NotifyAll("Test message");
        DisplayReport(failedReport);

        // TEST 6: Empty notification message
        Console.WriteLine("\n=== TEST 6: Invalid Message ===");

        try
        {
            service.NotifyAll("");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Expected exception: {ex.GetType().Name}");
        }
    }

    static void DisplayReport(NotificationReport report)
    {
        Console.WriteLine("\n--- Notification Report ---");

        Console.WriteLine($"Total attempts : {report.TotalAttempts}");
        Console.WriteLine($"Successful     : {report.SuccessfulCount}");
        Console.WriteLine($"Failed         : {report.FailedCount}");

        if (report.Errors.Count > 0)
        {
            Console.WriteLine("\nErrors:");

            foreach (string error in report.Errors)
            {
                Console.WriteLine($"- {error}");
            }
        }
        else
        {
            Console.WriteLine("No errors recorded.");
        }

        Console.WriteLine("---------------------------");
    }
}
