using System;
using System.Globalization;
using EStationCore.Model.Fuel.Entity;
using Humanizer;


namespace EStationCore.Model.Fuel.Views
{
    public class StockCard
    {

        public StockCard(FuelStock fuelStock)
        {
            FuelStockGuid = fuelStock.FuelStockGuid;
            Supplier = fuelStock.Supplier;
            Quantity = fuelStock.Quantity.ToString("0.##\\L");
            Cost = fuelStock.Cost.ToString("0.##\\ dhs");
            Date = "Le " + fuelStock.DateAdded.GetValueOrDefault().ToShortDateString();
            Description = fuelStock.Description;
        }


        public Guid FuelStockGuid { get; }

        public string Supplier { get; }

        public string Quantity { get; }

        public string Cost { get;  }

        public string Date { get; }

        public string Description { get; }

    }
}
