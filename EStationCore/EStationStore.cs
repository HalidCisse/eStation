using CLib.Database.Interfaces;
using eStationCore.IManagers;
using eStationCore.Store;
using eStationCore.Store.Azure;
using eStationCore.Store.SqlLite;
using eStationCore.Store.SqlServer;
using eStationCore.Store.WebApi;
using Exceptionless;

namespace eStationCore
{
    public class EStationStore : IEstation
    {
        public EStationStore(Storage? storage, string path = default(string))
        {
            ExceptionlessClient.Default.Configuration.ApiKey = "mnPSl8rxo8I4NghVDeLg96OMlGXlfEMSttioT4hc";

            IEstation estation;

            switch (storage)
            {
                case Storage.SqlLite:
                    estation = new EstationSqlLite(path);
                    break;
                case Storage.WebApi:
                    estation = new EstationWebApi(path);
                    break;
                case Storage.Azure:
                    estation = new EstationAzure(path);
                    break;
                default:
                    estation = new EstationSqlServer(path);
                    break;
            }
           
            HumanResource = estation.HumanResource;
            Authentication = estation.Authentication;
            Economat = estation.Economat;
            Documents = estation.Documents;
            Oils = estation.Oils;
            Meta = estation.Meta;
            Sales = estation.Sales;
            Citernes = estation.Citernes;
            Fuels = estation.Fuels;
            Pompes = estation.Pompes;
            Customers = estation.Customers;
        }

        /// <summary>
        /// System de Gestion Des Achats
        /// </summary>
        public ISalesManager Sales { get; }

        /// <summary>
        /// System de Gestion Des Huiles
        /// </summary>
        public IOilManager Oils { get; }

        /// <summary>
        /// System de Gestion Des Citernes
        /// </summary>
        public ICiternesManager Citernes { get; }

        /// <summary>
        /// System de Gestion Des Carburants
        /// </summary>
        public IFuelManager Fuels { get; }

        /// <summary>
        /// System de Gestion Des Pompes
        /// </summary>
        public IPompesManager Pompes { get; }

        /// <summary>
        /// System de Gestion Des Clients
        /// </summary>
        public ICustomersManager Customers { get; }

        /// <summary>
        /// System de Gestion Des Staffs
        /// </summary>
        public IHrManager HumanResource { get; }

        /// <summary>
        /// System de Gestion de Security 
        /// </summary>
        public ISecurityManager Authentication { get; }

        /// <summary>
        /// Gestion des Ressources Financieres
        /// </summary>
        public IEconomatManager Economat { get; }

        /// <summary>
        /// Gestion des Documents Etudiants et Staffs
        /// </summary>
        public IDocumentsManager Documents { get; }

        public IMetaManager Meta { get; }

    }
}
