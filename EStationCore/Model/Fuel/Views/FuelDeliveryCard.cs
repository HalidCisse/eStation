using System;
using CLib;
using eStationCore.Model.Fuel.Entity;

namespace eStationCore.Model.Fuel.Views
{
    public class FuelDeliveryCard
    {

        public FuelDeliveryCard(FuelDelivery fuelDelivery)
        {
            FuelDeliveryGuid = fuelDelivery.FuelDeliveryGuid;
            Supplier = fuelDelivery.Supplier;
            Quantity = fuelDelivery.QuantityDelivered.ToString("0.##\\ Litres");
            Cost = fuelDelivery.Cost.ToString("C0");
            Date = fuelDelivery.DateAdded.FriendlyDateTime();
            Description = fuelDelivery.Description;
        }


        public Guid FuelDeliveryGuid { get; }

        public string Supplier { get; }

        public string Quantity { get; }

        public string Cost { get;  }

        public string Date { get; }

        public string Description { get; }

    }
}
