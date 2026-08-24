using System;
using System.Net.Mail;
using Spectre.Console;

namespace Builder.Modules;

internal sealed class Email
{
    public static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public static bool TestEmail(string recipient)
    {
        try
        {
            using (var message = new MailMessage())
            {
                message.From = new MailAddress("diahem-builder@freesmtpservers.com", "Diahem Builder");
                message.To.Add(recipient);
                message.Subject = "Diahem Builder Test Connection";
                message.Body = "Diahem builder connected successfully via Email!";

                using (var client = new SmtpClient("smtp.freesmtpservers.com", 25))
                {
                    client.Timeout = 10000;
                    client.Send(message);
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.Foreground = ConsoleColor.Green;
            AnsiConsole.WriteLine("Email >> Connection test failed: " + ex.Message);
            return false;
        }
    }
}
