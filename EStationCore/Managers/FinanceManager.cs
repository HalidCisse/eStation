using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CLib;
using EStationCore.Model;
using EStationCore.Model.Hr.Views;
using EStationCore.Model.Sale.Entity;
using EStationCore.Model.Sale.Enums;

namespace EStationCore.Managers {

    /// <summary>
    /// Tresorerie
    /// </summary>
    public sealed class FinanceManager {

        #region CRUD

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newTransaction"></param>
        /// <returns></returns>
        public bool NewTransaction(Transaction newTransaction)
        {
            //todo security
            return StaticNewTransaction(newTransaction);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionGuid"></param>
        /// <returns></returns>
        public bool CancelTransaction(Guid transactionGuid)
        {
            using (var db = new StationContext())
            {
                var theTransaction = db.Transactions.Find(transactionGuid);
                if (theTransaction == null) throw new InvalidOperationException("CAN_NOT_FIND_REFERENCE_TRANSACTION");

                theTransaction.IsDeleted = true;
                theTransaction.DeleteDate = DateTime.Now;
                theTransaction.DeleteUserGuid = Guid.Empty;

                theTransaction.LastEditDate = DateTime.Now;
                theTransaction.LastEditUserGuid = Guid.Empty;

                db.Transactions.Attach(theTransaction);
                db.Entry(theTransaction).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
        }


        /// <summary>
        /// Nouvelle Numero de Reference
        /// </summary>
        public string GetNewTransactionReference()
        {
            string newId;
            do
                newId = "T" + RandomHelper.GetRandLetters(1) + "-" + DateTime.Today.Month +
                        DateTime.Today.Year.ToString().Substring(2) + "-" + RandomHelper.GetRandNum(4); while (
                RefTransactionExist(newId)
                );
            return newId;
        }


        /// <summary>
        /// Verifie si cette reference exist
        /// </summary>
        /// <param name="theReference"></param>
        /// <returns></returns>
        public bool RefTransactionExist(string theReference)
        {
            using (var mc = new StationContext())
                return
                    mc.Transactions.Any(
                        s => s.TransactionReference.Equals(theReference, StringComparison.CurrentCultureIgnoreCase));
        }


        /// <summary>
        /// liste des transactions
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public IEnumerable GetTransactions(DateTime? startDate, DateTime? endDate, bool includeDeleted = false)
        {
            if (includeDeleted)
                using (var db = new StationContext())
                {
                    if (startDate == null || endDate == null)
                        return
                            db.Transactions.OrderByDescending(t => t.TransactionDate)
                                .ToList()
                                .Select(t => new TransactionCard(t));

                    return db.Transactions.Where(t =>
                        t.TransactionDate >= startDate &&
                        t.TransactionDate <= endDate
                        ).OrderByDescending(t => t.TransactionDate).ToList().Select(t => new TransactionCard(t));
                }

            using (var db = new StationContext())
            {
                if (startDate == null || endDate == null)
                    return
                        db.Transactions.Where(t => !t.IsDeleted)
                            .OrderByDescending(t => t.TransactionDate)
                            .ToList()
                            .Select(t => new TransactionCard(t));

                return db.Transactions.Where(t => !t.IsDeleted &&
                                                  t.TransactionDate >= startDate &&
                                                  t.TransactionDate <= endDate
                    ).OrderByDescending(t => t.TransactionDate).ToList().Select(t => new TransactionCard(t));
            }
        }

        #endregion




        #region Views

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<double> GetTotalRecette(DateTime? startDate, DateTime? endDate)
            => await StaticGetRecette(startDate, endDate);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<double> GetTotalDepense(DateTime? startDate, DateTime? endDate)
            => await StaticGetExpense(startDate, endDate);



        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public async Task<double> GetTotalPaidSalaries(DateTime? startDate, DateTime? endDate)
            => await StaticGetPaidSalary(startDate, endDate);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public async Task<double> GetSoldeCaisse(DateTime? startDate, DateTime? endDate)
            => await StaticGetSoldeCaisse(startDate, endDate);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<double> GetSolde(DateTime? startDate, DateTime? endDate)
        {
            var solde = await StaticGetSoldeCaisse(startDate, endDate);
            solde -= await StaticGetPaidSalary(startDate, endDate);
            //solde     += await StaticGetTotalPaidSchoolFee(startDate, endDate);
            return solde;
        }

        #endregion




        #region Analytic

      
        public async Task<List<KeyValuePair<DateTime, double>>> MonthlySalary(DateTime fromDate, DateTime toDate)
        {           
            var points = new List<KeyValuePair<DateTime, double>>();
            foreach (var date in DateTimeHelper.EachMonth(new DateTime(fromDate.Year, fromDate.Month, 1), new DateTime(toDate.Year, toDate.Month, 1)))
                points.Add(new KeyValuePair<DateTime, double>(date,
                    await StaticGetPaidSalary(date.Date, date.Date.AddMonths(1).AddDays(-1))));
            return points;
        }

        public async Task<List<KeyValuePair<DateTime, double>>> MonthlyExpense(DateTime fromDate, DateTime toDate)
        {          
            var points = new List<KeyValuePair<DateTime, double>>();
            foreach (var date in DateTimeHelper.EachMonth(new DateTime(fromDate.Year, fromDate.Month, 1), new DateTime(toDate.Year, toDate.Month, 1)))
                points.Add(new KeyValuePair<DateTime, double>(date,
                    -await StaticGetExpense(date.Date, date.Date.AddMonths(1).AddDays(-1))));
            return points;
        }

        public async Task<List<KeyValuePair<DateTime, double>>> MonthlyRecette(DateTime fromDate, DateTime toDate)
        {            
            var points = new List<KeyValuePair<DateTime, double>>();
            foreach (var date in DateTimeHelper.EachMonth(new DateTime(fromDate.Year, fromDate.Month, 1), new DateTime(toDate.Year, toDate.Month, 1)))
                points.Add(new KeyValuePair<DateTime, double>(date,
                    await StaticGetRecette(date.Date, date.Date.AddMonths(1).AddDays(-1))));
            return points;
        }

        public async Task<List<KeyValuePair<DateTime, double>>> MonthlyIncome(DateTime fromDate, DateTime toDate)
        {            
            var points = new List<KeyValuePair<DateTime, double>>();
            foreach (var date in DateTimeHelper.EachMonth(new DateTime(fromDate.Year, fromDate.Month, 1), new DateTime(toDate.Year, toDate.Month, 1)))
                points.Add(new KeyValuePair<DateTime, double>(date,
                    await StaticGetRevenue(date.Date, date.Date.AddMonths(1).AddDays(-1))));
            return points;
        }

        #endregion




        #region Protected Internal Static


      
        internal async static Task<double> StaticGetRecette (DateTime? startDate, DateTime? endDate) {
            return await Task.Run(() => {
                using (var db = new StationContext()) {
                if(!db.Transactions.Any(t => !t.IsDeleted&&t.Amount>0))
                    return 0;

                if(startDate==null||endDate==null)
                    return db.Transactions.Where(t => !t.IsDeleted&&t.Amount>0).Sum(t => t.Amount);

                if(!db.Transactions.Any(t =>
                                                !t.IsDeleted&&t.Amount>0&&
                                                t.TransactionDate>=startDate&&
                                                t.TransactionDate<=endDate
                                            ))
                    return 0;

                return db.Transactions.Where(t =>
                                                !t.IsDeleted&&t.Amount>0&&
                                                t.TransactionDate>=startDate&&
                                                t.TransactionDate<=endDate
                                            ).Sum(t => t.Amount);
                }
            });
        }

   
        internal async static Task<double> StaticGetExpense (DateTime? startDate, DateTime? endDate) {
            return await Task.Run(() => {
                using (var db = new StationContext()) {

                if(!db.Transactions.Any(t => !t.IsDeleted&&t.Amount<0))
                    return 0;

                if(startDate==null||endDate==null)
                    return db.Transactions.Where(t => !t.IsDeleted&&t.Amount<0).Sum(t => t.Amount);

                if(!db.Transactions.Any(t =>
                                                !t.IsDeleted&&t.Amount<0&&
                                                t.TransactionDate>=startDate&&
                                                t.TransactionDate<=endDate
                                            ))
                    return 0;

                return db.Transactions.Where(t =>
                                                !t.IsDeleted&&t.Amount<0&&
                                                t.TransactionDate>=startDate&&
                                                t.TransactionDate<=endDate
                                            ).Sum(t => t.Amount);
            }
            });
        }      
       

        internal async static Task<double> StaticGetPaidSalary (DateTime? startDate, DateTime? endDate) {
            return await Task.Run(() => {
                using (var db = new StationContext()) {
                if(!db.Payrolls.Any(t => !t.IsDeleted&&t.IsPaid))
                    return 0;

                if(startDate==null||endDate==null)
                    return db.Payrolls.Where(t => !t.IsDeleted&&t.IsPaid).Sum(t => t.FinalPaycheck);

                return db.Payrolls.Any(t =>
                    !t.IsDeleted && t.IsPaid &&
                    t.DatePaid >= startDate &&
                    t.DatePaid <= endDate)
                    ? db.Payrolls.Where(t =>
                        !t.IsDeleted && t.IsPaid &&
                        t.DatePaid >= startDate &&
                        t.DatePaid <= endDate
                        ).Sum(t => t.FinalPaycheck)
                    : 0;
            }});
        }


        internal async static Task<double> StaticGetSoldeCaisse (DateTime? startDate, DateTime? endDate) {
            return await Task.Run(() => {
                using (var db = new StationContext()) {
                if(!db.Transactions.Any(t => !t.IsDeleted))
                    return 0;

                if(startDate==null||endDate==null)
                    return db.Transactions.Where(t => !t.IsDeleted).Sum(t => t.Amount);

                return db.Transactions.Any(t =>
                    !t.IsDeleted &&
                    t.TransactionDate >= startDate &&
                    t.TransactionDate <= endDate)
                    ? db.Transactions.Where(t =>
                        !t.IsDeleted &&
                        t.TransactionDate >= startDate &&
                        t.TransactionDate <= endDate
                        ).Sum(t => t.Amount)
                    : 0;
            }});
        }

      
        internal async static Task<double> StaticGetRevenue (DateTime startDate, DateTime endDate) {
            double solde = 0;           
            
            solde += await OilManager.GetSold(startDate, endDate);
            solde += await FuelManager.GetSold(startDate, endDate);
            solde += await SalesManager.StaticGetPurchasedSum(ProductType.Service, PurchaseState.Paid, startDate, endDate);

            solde += await StaticGetSoldeCaisse(startDate, endDate); 

            solde -= await StaticGetPaidSalary(startDate, endDate); 

            solde -= await SalesManager.StaticGetPurchasedSum(ProductType.Fuel, PurchaseState.UnPaid, startDate, endDate);
            solde -= await SalesManager.StaticGetPurchasedSum(ProductType.Oil, PurchaseState.UnPaid, startDate, endDate);

            solde -= await SalesManager.StaticGetPurchasedSum(ProductType.Fuel, PurchaseState.NotPaying, startDate, endDate);
            solde -= await SalesManager.StaticGetPurchasedSum(ProductType.Oil, PurchaseState.NotPaying, startDate, endDate);
           
            return solde;
        }

       
        internal static bool StaticNewTransaction (Transaction newTransaction) {
            
            if(TransactionExist(newTransaction))
                throw new InvalidOperationException("TRANSACTION_REFERENCE_ALREADY_EXIST");

            if(newTransaction.TransactionDate < DateTime.Today.AddDays(-1))
                throw new InvalidOperationException("TRANSACTION_DATE_NOT_VALIDE");

            using (var db = new StationContext())
            {                    
                newTransaction.TransactionGuid =newTransaction.TransactionGuid == Guid.Empty ? Guid.NewGuid() : newTransaction.TransactionGuid;
                var transDate = newTransaction.TransactionDate.GetValueOrDefault();
                newTransaction.TransactionDate = new DateTime(transDate.Year, transDate.Month, transDate.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                if (string.IsNullOrEmpty(newTransaction.Description)) newTransaction.Description =newTransaction.Designation;
              
                newTransaction.DateAdded        = DateTime.Now;
                newTransaction.AddUserGuid      = Guid.Empty;
                newTransaction.LastEditDate     = DateTime.Now;
                newTransaction.LastEditUserGuid = Guid.Empty;

                db.Transactions.Add(newTransaction);
                return db.SaveChanges()>0;
            }
        }

       
        internal static bool TransactionExist (Transaction newTransaction) {
            using (var db = new StationContext())
            {
                if(db.Transactions.Find(newTransaction.TransactionGuid)!=null)
                    return true;
                return db.Transactions.Any(t =>
                        t.TransactionReference.Equals(newTransaction.TransactionReference,
                            StringComparison.CurrentCultureIgnoreCase)) ||
                       db.Transactions.Any(t => t.PaymentMethode == newTransaction.PaymentMethode &&
                                                t.Designation.Equals(newTransaction.Designation) &&
                                                t.TransactionDate == newTransaction.TransactionDate &&
                                                Math.Abs(t.Amount - newTransaction.Amount) < .0000001);
            }
        }


        #endregion


    }
}
