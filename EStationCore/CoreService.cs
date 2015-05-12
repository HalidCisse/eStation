

using EStationCore.Managers;

namespace EStationCore
{
    public class CoreService
    {

        /// <summary>
        /// System de Gestion Des Clients
        /// </summary>
        public CustomersManager Customers = new CustomersManager();


        /// <summary>
        /// System de Gestion Des Staffs
        /// </summary>
        public HrManager HumanResource = new HrManager();


        /// <summary>
        /// System de Gestion de Security 
        /// </summary>
        public SecurityManager Authentication = new SecurityManager();






    }
}
