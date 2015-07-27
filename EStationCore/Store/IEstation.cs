
using CLib.Database.Interfaces;
using eStationCore.IManagers;

namespace eStationCore.Store
{
    public interface IEstation
    {
        /// <summary>
        /// System de Gestion Des Achats
        /// </summary>
        ISalesManager Sales { get; }


        /// <summary>
        /// System de Gestion Des Huiles
        /// </summary>
        IOilManager Oils { get; }


        /// <summary>
        /// System de Gestion Des Citernes
        /// </summary>
        ICiternesManager Citernes { get; }


        /// <summary>
        /// System de Gestion Des Carburants
        /// </summary>
        IFuelManager Fuels { get; }


        /// <summary>
        /// System de Gestion Des Pompes
        /// </summary>
        IPompesManager Pompes { get; }


        /// <summary>
        /// System de Gestion Des Clients
        /// </summary>
        ICustomersManager Customers { get; }


        /// <summary>
        /// System de Gestion Des Staffs
        /// </summary>
        IHrManager HumanResource { get; }


        /// <summary>
        /// System de Gestion de Security 
        /// </summary>
        ISecurityManager Authentication { get; }


        /// <summary>
        /// Gestion des Ressources Financieres
        /// </summary>
        IEconomatManager Economat { get; }


        /// <summary>
        /// Gestion des Documents Etudiants et Staffs
        /// </summary>
        IDocumentsManager Documents { get; }

        IMetaManager Meta { get; }


    }
}
