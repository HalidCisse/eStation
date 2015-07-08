using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eStationCore.Model.Fuel.Entity;
using eStationCore.Model.Fuel.Views;

namespace eStationCore.IManagers
{
    public interface IPompesManager
    {
        Task<bool> Delete(Guid pompeGuid);
        Pompe Get(Guid pompeGuid);
        List<string> GetColonnes();
        Task<List<ColonneCard>> GetColonnesCard();
        Task<FuelPrelevement> GetLastPrelevement(Guid pompeGuid);
        IEnumerable<PrelevCard> GetPrelevCards(List<Guid> fuelsGuids, DateTime fromDate, DateTime toDate);
        FuelPrelevement GetPrelevement(Guid prelevGuid);
        Task<bool> Post(Price newPrice);
        bool Post(Pompe myPompe);
        Task<bool> Post(FuelPrelevement myPrelevement);
        bool Put(Pompe myPompe);
        bool Put(FuelPrelevement myPrelevement);
    }
}