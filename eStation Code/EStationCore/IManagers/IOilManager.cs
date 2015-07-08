using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using eStationCore.Model.Oil.Entity;
using eStationCore.Model.Oil.Views;

namespace eStationCore.IManagers
{
    public interface IOilManager
    {
        Task<bool> ChangePrice(Guid oilGuid, double newPrice);
        Task<bool> Delete(Guid oilGuid);
        Task<bool> DeleteDelivery(Guid deliveryGuid);
        Task<bool> DeletePrelevement(Guid oilPrelevementGuid);
        Task<Oil> Get(Guid oilGuid);
        int GetDeliveries(Guid oilGuid, DateTime fromDate, DateTime toDate);
        OilDelivery GetDelivery(Guid deliveryGuid);
        Task<int> GetGallonsSold(List<Guid> oilsGuids, DateTime fromDate, DateTime toDate);
        Task<List<KeyValuePair<DateTime, double>>> GetMonthlyIncome(List<Guid> oilGuids, DateTime fromDate, DateTime toDate);
        Task<List<KeyValuePair<DateTime, double>>> GetMonthlySales(List<Guid> oilGuids, DateTime fromDate, DateTime toDate);
        int GetOilBalance(Guid oilGuid);
        IEnumerable GetOilDeliveries(Guid oilGuid);
        Task<List<OilDeliveryCard>> GetOilDeliveries(List<Guid> oilsGuids, DateTime fromDate, DateTime toDate);
        Task<List<Oil>> GetOils();
        List<Oil> GetOils(List<Guid> oilsGuids);
        Task<List<OilCard>> GetOilsCards();
        Task<List<OilPrelevCard>> GetPrelevCards(List<Guid> oilsGuids, DateTime fromDate, DateTime toDate);
        Task<List<KeyValuePair<DateTime, double>>> GetPrices(Guid oilGuid, DateTime fromDate, DateTime toDate);
        Task<double> GetSold(List<Guid> oilsGuids, DateTime fromDate, DateTime toDate);
        Task<int> GetTotalDelivery(List<Guid> oilsGuids, DateTime fromDate, DateTime toDate);
        Task<double> GetTotalDeliveryCost(List<Guid> oilsGuids, DateTime fromDate, DateTime toDate);
        List<string> GetTypes();
        bool Post(OilDelivery myDelivery);
        bool Post(Oil myOil);
        Task<bool> Post(List<OilPrelevement> oilPrelevements, DateTime fromDate);
        bool Put(OilDelivery myDelivery);
        bool Put(Oil myOil);
    }
}