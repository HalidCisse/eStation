using eLib.Database.Interfaces;
using eStationCore.IManagers;

namespace eStationCore.Store.Azure
{
    public class EstationAzure : IEstation
    {
        private string _path;

        public EstationAzure(string path)
        {
            this._path = path;
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
