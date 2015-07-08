using System;
using eStationCore.Model.Oil.Entity;
using Humanizer;

namespace eStationCore.Model.Oil.Views
{
    public class OilPrelevCard
    {

        public OilPrelevCard(OilPrelevement prelevement)
        {
            OilPrelevementGuid = prelevement.OilPrelevementGuid;
            Libel = $"{prelevement.Oil.Libel.ToLower().Titleize()} ({prelevement.Oil.TypeOil.ToUpper()})";
            DatePrelev = prelevement.DatePrelevement.GetValueOrDefault().ToString("dd MMM yy - HH:mm");
            TotalStock = $"Stock: {"bidon".ToQuantity(prelevement.Meter)}";
            TotalSold = $"Vendu: {"bidon".ToQuantity(prelevement.Result)}";
        }


        public Guid OilPrelevementGuid { get; }

        public string Libel { get; }

        public string DatePrelev { get; }
      
        public string TotalSold { get; }

        public string TotalStock { get; }

    }
}
