using System;
using System.IO;

namespace LegalHarvest
{
    public static class LegalGuard
    {
        public static void EnsureAuthorized()
        {
            string tokenFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "legal_harvest.token");
            if (!File.Exists(tokenFile))
            {
                Console.WriteLine("[!] Fichier d'autorisation manquant.");
                Environment.Exit(1);
            }
            string token = File.ReadAllText(tokenFile).Trim();
            if (token != "MISSION_AUTHORIZED_2024")
            {
                Console.WriteLine("[!] Token invalide.");
                Environment.Exit(1);
            }
        }
    }
}
