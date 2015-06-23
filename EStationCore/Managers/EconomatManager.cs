

namespace EStationCore.Managers {

    /// <summary>
    /// Gestion Du Budget
    /// </summary>
    public sealed class EconomatManager {


        /// <summary>
        /// Gestion Des Recettes et depenses
        /// </summary>
        public FinanceManager Finance = new FinanceManager();


        /// <summary>
        /// Gestion Des Salaire
        /// </summary>
        public PayrollManager PayRoll = new PayrollManager();


    }
}
