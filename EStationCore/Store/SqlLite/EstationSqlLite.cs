using CLib.Database.Interfaces;
using eStationCore.IManagers;


namespace eStationCore.Store.SqlLite
{
    public class EstationSqlLite : IEstation
    {
        private string path;

        public EstationSqlLite(string path)
        {
            this.path = path;
        }

        public ISalesManager Sales { get; }
        public IOilManager Oils { get; }
        public ICiternesManager Citernes { get; }
        public IFuelManager Fuels { get; }
        public IPompesManager Pompes { get; }
        public ICustomersManager Customers { get; }
        public IHrManager HumanResource { get; }
        public ISecurityManager Authentication { get; }
        public IEconomatManager Economat { get; }
        public IDocumentsManager Documents { get; }
        public IMetaManager Meta { get; }
    }
}
