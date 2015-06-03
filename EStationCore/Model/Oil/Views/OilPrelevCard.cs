using System;
using EStationCore.Model.Oil.Entity;
using Humanizer;


namespace EStationCore.Model.Oil.Views
{
    public class OilPrelevCard
    {

        public OilPrelevCard(OilPrelevement prelevement)
        {
            OilPrelevementGuid = prelevement.OilPrelevementGuid;
            Libel = $"{prelevement.Oil.Libel.ToLower().Titleize()} ({prelevement.Oil.TypeOil.ToLower().Titleize()})";
            DatePrelev = prelevement.DatePrelevement.GetValueOrDefault().ToString("dd MMM yy - HH:mm");
            TotalStock = "bidon".ToQuantity(prelevement.TotalStock);
            TotalSold = "bidon".ToQuantity(prelevement.TotalSold);

        }

        public Guid OilPrelevementGuid { get; }

        public string Libel { get; }

        public string DatePrelev { get; }
      
        public string TotalSold { get; }

        public string TotalStock { get; }

    }
}
