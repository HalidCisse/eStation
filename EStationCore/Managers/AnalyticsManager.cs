using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CLib;
using EStationCore.Model;

namespace EStationCore.Managers {

    /// <summary>
    /// Gestion des Statistiques
    /// </summary>
    public sealed class AnalyticsManager {


        /// <summary>
        /// Renvoi la Liste des Absences d'un Staff ou Etudiants
        /// </summary>
        /// <param name="numberOfYear"></param>        
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, int>> YearlyRegistrations (int numberOfYear = 10) {
            using (var db = new StationContext()) {
                foreach (var year in DateTimeHelper.EachYear(DateTime.Today.AddYears(-numberOfYear), DateTime.Today))
                    yield return
                        new KeyValuePair<string, int>(year.Year.ToString(),
                            db.Customers.Count(s => s.Person.DateAdded.Value.Year == year.Year));
            }
        }


        ///// <summary>
        ///// Renvoi la Liste des Absences d'un Staff ou Etudiants
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<KeyValuePair<string, int>> EnrollementsPerClasse () {
        //    using (var db = new StationContext()) {
        //        foreach(var classe in db.Classes.Where(c=> c.Inscriptions.Any(i => i.EnrollementStatus != EnrollementStatus.Canceled &&i.SchoolYear.DateFin>DateTime.Today)))
        //            yield return
        //                new KeyValuePair<string, int>(classe.Sigle +" (" + classe.Filiere.Sigle + " )",
        //                    classe.Inscriptions.Count(i => i.EnrollementStatus!=EnrollementStatus.Canceled&& i.SchoolYear.DateFin > DateTime.Today));
        //    }
        //}


        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<KeyValuePair<string, int>> StaffPerYear (int numberOfYear = 10){
            using (var db = new StationContext()) {
                foreach(var year in DateTimeHelper.EachYear(DateTime.Today.AddYears(-numberOfYear), DateTime.Today))
                    yield return
                        new KeyValuePair<string, int>(year.Year.ToString(),
                            db.Staffs.Count(s => s.Person.DateAdded.Value.Year==year.Year));
            }
        }


        ///// <summary>
        ///// 
        ///// </summary>
        //public IEnumerable<KeyValuePair<string, double>> SchoolFeePerMonth (int numberOfMonths = 12) 
        //    => DateTimeHelper.EachMonth(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-numberOfMonths), new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1))
        //    .Select(month => new KeyValuePair<string, double>(month.ToString( "MMM"),
        //    TreasuryManager.StaticGetTotalPaidSchoolFee(month.Date, month.Date.AddMonths(1).AddDays(-1))));


        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Task<KeyValuePair<string, double>>> SalaryPerMonth (int numberOfMonths = 12) 
            => DateTimeHelper.EachMonth(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-numberOfMonths), new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1))
            .Select(async month => new KeyValuePair<string, double>(month.ToString("MMM"),
            await TreasuryManager.StaticGetTotalPaidSalaries(month.Date, month.Date.AddMonths(1).AddDays(-1))));


        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Task<KeyValuePair<string, double>>> ExpensePerMonth (int numberOfMonths = 12) 
            => DateTimeHelper.EachMonth(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-numberOfMonths), new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1))
            .Select(async month => new KeyValuePair<string, double>(month.ToString("MMM"),
            - await TreasuryManager.StaticGetTotalDepense(month.Date, month.Date.AddMonths(1).AddDays(-1))));


        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Task<KeyValuePair<string, double>>> IncomePerMonth (int numberOfMonths = 12) 
            => DateTimeHelper.EachMonth(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-numberOfMonths), new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1))
            .Select(async month => new KeyValuePair<string, double>(month.ToString("MMM"),
            await TreasuryManager.StaticGetTotalRecette(month.Date, month.Date.AddMonths(1).AddDays(-1))));


        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Task<KeyValuePair<string, double>>> TresoryPerMonth (int numberOfMonths = 12) 
            => DateTimeHelper.EachMonth(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-numberOfMonths), new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1))
            .Select(async date => new KeyValuePair<string, double>(date.Month.ToString("MMM"),
            await TreasuryManager.StaticGetSolde(date.Date, date.Date.AddMonths(1).AddDays(-1))));
    }
}
