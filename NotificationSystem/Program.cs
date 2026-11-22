using System.Collections.Generic;


public interface INotificationSender
{

    public void Send(string message);
    
}

class EmailSender : INotificationSender
{
    public void Send(string message)
    {
        Console.WriteLine($"This a Notify from your Email to tell u : {message}");
    }
}

class SmsSender : INotificationSender
{

    public void Send(string message)
    {
        Console.WriteLine($"This Sms it send to u to notify u that : {message}");
    }

}

class NotificationService
{
    private readonly List<INotificationSender> _senders = new();

    public void RegisterSender(INotificationSender sender)
    {
        ArgumentNullException.ThrowIfNull(sender);

        if(_senders.Contains(sender) )
        {
            return;
        }

        _senders.Add(sender);
    }

    public void NotifyAll(string message)
    {
        foreach (var item in _senders)
        {

            item.Send(message);
            
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        NotificationService service = new();

        // Test 1: No registered senders
        Console.WriteLine("=== TEST 1: Empty Service ===");

        service.NotifyAll("Hello!");

        Console.WriteLine("Completed without errors.");


       
        Console.WriteLine("\n=== TEST 2: Register Senders ===");

        EmailSender emailSender = new();
        SmsSender smsSender = new();

        service.RegisterSender(emailSender);
        service.RegisterSender(smsSender);

        service.NotifyAll("Your order is completed.");


        
        Console.WriteLine("\n=== TEST 3: Duplicate Sender ===");

        service.RegisterSender(emailSender);

        
        service.NotifyAll("Your payment was received.");


        
        Console.WriteLine("\n=== TEST 4: Null Sender ===");

        try
        {
            service.RegisterSender(null!);
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Expected exception: {ex.GetType().Name}");
        }


      
        Console.WriteLine("\n=== TEST 5: Push Notification ===");

        service.RegisterSender(new PushSender());

        service.NotifyAll("Security alert detected!");
    }
}


class PushSender : INotificationSender
{
    public void Send(string message)
    {
        Console.WriteLine($"Push notification: {message}");
    }
}
