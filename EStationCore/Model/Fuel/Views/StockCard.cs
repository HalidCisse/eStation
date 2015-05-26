using System;
using System.Globalization;
using EStationCore.Model.Fuel.Entity;
using Humanizer;


namespace EStationCore.Model.Fuel.Views
{
    public class StockCard
    {

        public StockCard(FuelDelivery fuelDelivery)
        {
            FuelStockGuid = fuelDelivery.FuelDeliveryGuid;
            Supplier = fuelDelivery.Supplier;
            Quantity = fuelDelivery.QuantityDelivered.ToString("0.##\\L");
            Cost = fuelDelivery.Cost.ToString("0.##\\ dhs");
            Date = "Le " + fuelDelivery.DateAdded.GetValueOrDefault().ToShortDateString();
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
