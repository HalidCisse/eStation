using CLib.Database.Interfaces;
using eStationCore.IManagers;
using eStationCore.Model;

namespace eStationCore.Store.SqlServer
{
    public class EstationSqlServer : IEstation
    {
        private string ConnectionString { get; }

        public EstationSqlServer(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public ISalesManager Sales { get; } = new SalesManager(new StationContext());
        public IOilManager Oils { get; } = new OilManager(new StationContext());
        public ICiternesManager Citernes { get; } = new CiternesManager(new StationContext());
        public IFuelManager Fuels { get; } = new FuelManager(new StationContext());
        public IPompesManager Pompes { get; } = new PompesManager(new StationContext());
        public ICustomersManager Customers { get; } = new CustomersManager(new StationContext());
        public IHrManager HumanResource { get; } = new HrManager(new StationContext());
        public ISecurityManager Authentication { get; } = new SecurityManager(new StationContext());
        public IEconomatManager Economat { get; } = new EconomatManager(new StationContext());
        public IDocumentsManager Documents { get; } = new DocumentsManager(new StationContext());
        public IMetaManager Meta { get; } = new MetaManager();
    }
}
