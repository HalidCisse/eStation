

using eStationCore.IManagers;
using eStationCore.Model;

namespace eStationCore.Store.SqlServer {

    /// <summary>
    /// Gestion Du Budget
    /// </summary>
    public sealed class EconomatManager : IEconomatManager
    {
       
        public EconomatManager(StationContext stationContext)
        {
            Finance = new FinanceManager(stationContext);
            PayRoll = new PayrollManager(stationContext);
        }
        
        public IFinanceManager Finance { get; }
        public IPayrollManager PayRoll { get; }
    }
}
