using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eStationCore.Model.Fuel.Entity;
using eStationCore.Model.Fuel.Views;

namespace eStationCore.IManagers
{
    public interface IFuelManager
    {
        Task<bool> Delete(Guid fuelGuid);
        Task<bool> DeleteDelivery(Guid deliveryGuid);
        Task<Fuel> Get(Guid fuelGuid);
        Task<double> GetFuelActualPrice(Guid fuelGuid);
        Task<List<FuelCard>> GetFuelCards();
        Task<List<Fuel>> GetFuels(List<Guid> fuelsGuids);
        Task<List<Fuel>> GetFuels(DateTime date = default(DateTime));
        Task<bool> Post(Fuel myFuel);
        Task<bool> Put(Fuel myFuel);
        double GetLiterSold(List<Guid> fuelsGuids, DateTime fromDate, DateTime toDate);
        Task<double> GetLiterSoldAsync(List<Guid> fuelsGuids, DateTime fromDate, DateTime toDate);
        Task<List<KeyValuePair<DateTime, double>>> GetMonthlyIncome(DateTime fromDate, DateTime toDate);
        Task<List<KeyValuePair<DateTime, double>>> GetMonthlySales(List<Guid> fuelGuids, DateTime fromDate, DateTime toDate);
        Task<List<KeyValuePair<DateTime, double>>> GetPrices(Guid fuelGuid, DateTime fromDate, DateTime toDate);
        Task<double> GetSold(List<Guid> fuelsGuids, DateTime fromDate, DateTime toDate);
        Task<double> GetTotalDeliveryCost(List<Guid> fuelsGuids, DateTime fromDate, DateTime toDate);
        Task<double> GetTotalDeliveryLiter(List<Guid> fuelsGuids, DateTime fromDate, DateTime toDate);
        
    }
}