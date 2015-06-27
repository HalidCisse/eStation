using System;
using CLib;
using EStationCore.Model.Fuel.Entity;

namespace EStationCore.Model.Fuel.Views
{
    public class StockCard
    {

        public StockCard(FuelDelivery fuelDelivery)
        {
            FuelStockGuid = fuelDelivery.FuelDeliveryGuid;
            Supplier = fuelDelivery.Supplier;
            Quantity = fuelDelivery.QuantityDelivered.ToString("0.##\\ Litres");
            Cost = fuelDelivery.Cost.ToString("C0");
            Date = fuelDelivery.DateAdded.FriendlyDateTime();
            Description = fuelDelivery.Description;
        }


        public Guid FuelStockGuid { get; }

        public string Supplier { get; }

        public string Quantity { get; }

        public string Cost { get;  }

        public string Date { get; }

        public string Description { get; }

    }
}
