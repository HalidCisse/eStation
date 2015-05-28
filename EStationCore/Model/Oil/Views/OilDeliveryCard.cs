

using System;
using EStationCore.Model.Oil.Entity;
using Humanizer;



namespace EStationCore.Model.Oil.Views
{
    public class OilDeliveryCard
    {

        public OilDeliveryCard(OilDelivery oilDelivery)
        {
            OilDeliveryGuid = oilDelivery.OilDeliveryGuid;
            Supplier = oilDelivery.Supplier;
            Quantity = "bidon".ToQuantity(oilDelivery.QuantityDelivered);
            Cost = oilDelivery.Cost.ToString("0.##\\ dhs");
            Date = "Le " + oilDelivery.DateAdded.GetValueOrDefault().ToShortDateString();
            Description = oilDelivery.Description;
        }


        public Guid OilDeliveryGuid { get; }

        public string Supplier { get; }

        public string Quantity { get; }

        public string Cost { get; }

        public string Date { get; }

        public string Description { get; }

    }
}
