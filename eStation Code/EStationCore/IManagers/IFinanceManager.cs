using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eStationCore.Model.Hr.Views;
using eStationCore.Model.Sale.Entity;

namespace eStationCore.IManagers
{
    public interface IFinanceManager
    {
        bool CancelTransaction(Guid transactionGuid);
        string GetNewTransactionReference();
        Task<double> GetRevenue(DateTime startDate, DateTime endDate);
        Task<double> GetSoldeCaisse(DateTime? startDate, DateTime? endDate);
        Task<double> GetTotalDepense(DateTime? startDate, DateTime? endDate);
        Task<double> GetTotalPaidSalaries(DateTime? startDate, DateTime? endDate);
        Task<double> GetTotalRecette(DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<TransactionCard>> GetTransactions(DateTime? startDate, DateTime? endDate, bool includeDeleted = false);
        Task<List<KeyValuePair<DateTime, double>>> MonthlyExpense(DateTime fromDate, DateTime toDate);
        Task<List<KeyValuePair<DateTime, double>>> MonthlyIncome(DateTime fromDate, DateTime toDate);
        Task<List<KeyValuePair<DateTime, double>>> MonthlyRecette(DateTime fromDate, DateTime toDate);
        Task<List<KeyValuePair<DateTime, double>>> MonthlySalary(DateTime fromDate, DateTime toDate);
        bool NewTransaction(Transaction newTransaction);
        bool RefTransactionExist(string theReference);
    }
}