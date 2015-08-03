namespace eStationCore.IManagers
{
    public interface IEconomatManager
    {
         IFinanceManager Finance { get; }

      
        IPayrollManager PayRoll { get; }

    }
}