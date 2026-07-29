using System.Collections.Generic;

namespace LegalHarvest.Modules
{
    public interface ICollectorModule
    {
        string ModuleName { get; }
        bool CanExecute();
        List<CollectedItem> Collect();
    }
}
