using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using EStationCore.Model;
using EStationCore.Model.Fuel.Entity;
using EStationCore.Model.Fuel.Views;
using Humanizer;


namespace EStationCore.Managers
{
    public class PompesManager
    {

        #region CRUD
        
        public bool Post(Pompe myPompe)
        {
            using (var db = new StationContext())
            {
                if (!db.Citernes.Any(f => f.CiterneGuid == myPompe.CiterneGuid))
                    throw new InvalidOperationException("CITERNE_REFERENCE_NOT_FOUND");
                

                if (myPompe.PompeGuid == Guid.Empty) myPompe.PompeGuid = Guid.NewGuid();

                myPompe.DateAdded = DateTime.Now;
                myPompe.LastEditDate = DateTime.Now;

                db.Pompes.Add(myPompe);
                return db.SaveChanges() > 0;
            }
        }

        public bool Post(Prelevement myPrelevement)
        {
            using (var db = new StationContext())
            {
                if (myPrelevement.PrelevementGuid == Guid.Empty) myPrelevement.PrelevementGuid = Guid.NewGuid();

                var citerneGuid = db.Pompes.Find(myPrelevement.PompeGuid).CiterneGuid;
                if (citerneGuid != null)
                    myPrelevement.CiterneGuid = (Guid)citerneGuid;
                else
                    throw new ArgumentException("CAN_NOT_FIND_CITERNE");
               
                myPrelevement.ActualPrice = FuelManager.GetFuelCurrentPrice(db.Pompes.Find(myPrelevement.PompeGuid).Citerne.FuelGuid);

                myPrelevement.DateAdded = DateTime.Now;
                myPrelevement.LastEditDate = DateTime.Now;

                db.Set<Prelevement>().Add(myPrelevement);
                return db.SaveChanges() > 0;
            }
        }

        public bool Put(Pompe myPompe)
        {
            using (var db = new StationContext())
            {
                myPompe.LastEditDate = DateTime.Now;

                db.Pompes.Attach(myPompe);
                db.Entry(myPompe).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
        }

        public bool Put(Prelevement myPrelevement)
        {
            using (var db = new StationContext())
            {
                myPrelevement.LastEditDate = DateTime.Now;

                db.Set<Prelevement>().Attach(myPrelevement);
                db.Entry(myPrelevement).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
        }

        public bool Delete(Guid pompeGuid)
        {
            using (var db = new StationContext())
            {
                db.Pompes.Remove(db.Pompes.Find(pompeGuid));
                return db.SaveChanges() > 0;
            }
        }

        public Pompe Get(Guid pompeGuid)
        {
            using (var db = new StationContext())
                return db.Pompes.Find(pompeGuid);
        }

        public bool Post(Price newPrice)
        {
            using (var db = new StationContext())
            {
                if (!db.Fuels.Any(f => f.FuelGuid == newPrice.ProductGuid))
                    throw new InvalidOperationException("FUEL_REFERENCE_NOT_FOUND");

                if (newPrice.PriceGuid == Guid.Empty) newPrice.PriceGuid = Guid.NewGuid();

                newPrice.DateAdded = DateTime.Now;
                newPrice.LastEditDate = DateTime.Now;

                db.Set<Price>().Add(newPrice);
                return db.SaveChanges() > 0;
            }
        }



        #endregion





        #region Views



        public List<string> GetColonnes()
        {
            using (var db = new StationContext())
                return db.Pompes.Where(p=> !string.IsNullOrEmpty(p.Colonne)).Select(p=> p.Colonne.ToLower()).Distinct().ToList().Select(s=> s.Titleize()).ToList();
        }

        public IEnumerable<PrelevCard> GetPrelevCards(List<Guid> fuelsGuids, DateTime fromDate, DateTime toDate)
        {           
            using (var db = new StationContext()){
                var prelevements = new List<Prelevement>();
                foreach (var citernes in db.Fuels.Where(f => fuelsGuids.Contains(f.FuelGuid)).Select(d => d.Citernes))
                    foreach (var citerne in citernes)
                        prelevements.AddRange(citerne.Prelevements.Where(p=> p.DatePrelevement.GetValueOrDefault().Date >= fromDate && p.DatePrelevement.GetValueOrDefault().Date<= toDate));
                return prelevements.OrderByDescending(p => p.DatePrelevement).Select(p => new PrelevCard(p)).ToList();
            }
        }

        public Prelevement GetPrelevement(Guid prelevGuid)
        {
            using (var db = new StationContext())
                return db.Set<Prelevement>().Find(prelevGuid);
        }


        public Prelevement GetLastPrelevement(Guid pompeGuid)
            => StaticGetLastPrelevement(pompeGuid);

        public IEnumerable<ColonneCard> GetColonnesCard()
        {
            using (var db = new StationContext())
            {
                var cardList = new ConcurrentBag<ColonneCard>();

                var nd = new ColonneCard("");
                if (nd.Pompes.Any()) { cardList.Add(nd); }

                var cols = (db.Pompes.Where(s => !s.IsDeleted).ToList()
                    .Where(s => !string.IsNullOrEmpty(s.Colonne))
                    .Select(s => s.Colonne.ToLower())).Distinct().ToList();

                Parallel.ForEach(cols, dep => cardList.Add(new ColonneCard(dep)));

                return cardList.Any() ? cardList.OrderBy(d => d.Libel).ToList() : null;
            }
        }


        #endregion





        #region Internal Static





        internal static Prelevement StaticGetLastPrelevement(Guid pompeGuid)
        {
            using (var db = new StationContext())
                return db.Pompes.Find(pompeGuid).Prelevements.OrderByDescending(p => p.DatePrelevement).FirstOrDefault() ?? 
                    new Prelevement {Meter = db.Pompes.Find(pompeGuid).InitialMeter};
        }


        #endregion

        
    }
}
