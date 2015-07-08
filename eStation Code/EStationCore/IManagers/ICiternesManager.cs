using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eStationCore.Model.Fuel.Entity;
using eStationCore.Model.Fuel.Views;

namespace eStationCore.IManagers
{
    public interface ICiternesManager
    {
        Task<bool> Post(Citerne myCiternes);
        Task<bool> Put(Citerne myCiternes);
        Task<bool> Delete(Guid citerneGuid);
        Task<Citerne> Get(Guid citerneGuid);
        Task<FuelDelivery> GetStock(Guid stockGuid);
        Task<bool> Post(FuelDelivery fuelDelivery);
        Task<bool> Put(FuelDelivery myDelivery);
        Task<List<FuelCard>> GetFuelCards();
        Task<double> GetCiterneFuelBalance(Guid citerneGuid);
        Task<List<string>> GetSuppliers();
        Task<List<CiterneCard>> GetCiternesCards();
        Task<List<Citerne>> GetCiternes();
        Task<List<FuelDeliveryCard>> GetCiterneStocks(Guid citerneGuid);
    }
}