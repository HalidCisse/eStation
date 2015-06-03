using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CLib;
using CLib.Exceptions;
using EStationCore.Model;
using EStationCore.Model.Fuel.Entity;
using EStationCore.Model.Fuel.Views;
using EStationCore.Model.Oil.Entity;
using EStationCore.Model.Oil.Views;
using Humanizer;

namespace EStationCore.Managers
{
    public class OilManager
    {


        #region CRUD


        public bool Post(Oil myOil)
        {
            using (var db = new StationContext())
            {
                if (myOil.OilGuid == Guid.Empty) myOil.OilGuid = Guid.NewGuid();

                myOil.DateAdded = DateTime.Now;
                myOil.LastEditDate = DateTime.Now;

                db.Oils.Add(myOil);
               return db.SaveChanges() > 0;              
            }
        }

        public bool Put(Oil myOil)
        {
            using (var db = new StationContext())
            {
                myOil.LastEditDate = DateTime.Now;

                db.Oils.Attach(myOil);
                db.Entry(myOil).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
        }

        public bool Delete(Guid oilGuid)
        {
            using (var db = new StationContext())
            {
                db.Oils.Remove(db.Oils.Find(oilGuid));
                return db.SaveChanges() > 0;
            }
        }

        public Oil Get(Guid oilGuid)
        {
            using (var db = new StationContext())
                return db.Oils.Find(oilGuid);
        }

        public bool Post(OilDelivery myDelivery)
        {
            using (var db = new StationContext())
            {
                if (myDelivery.OilDeliveryGuid == Guid.Empty) myDelivery.OilDeliveryGuid = Guid.NewGuid();

                myDelivery.DateAdded = DateTime.Now;
                myDelivery.LastEditDate = DateTime.Now;

                db.OilDeliveries.Add(myDelivery);
                return db.SaveChanges() > 0;
            }
        }

        public bool Put(OilDelivery myDelivery)
        {
            using (var db = new StationContext())
            {
                myDelivery.LastEditDate = DateTime.Now;

                db.OilDeliveries.Attach(myDelivery);
                db.Entry(myDelivery).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
        }

        public bool DeleteDelivery(Guid deliveryGuid)
        {
            using (var db = new StationContext())
            {
                db.OilDeliveries.Remove(db.OilDeliveries.Find(deliveryGuid));
                return db.SaveChanges() > 0;
            }
        }

        public OilDelivery GetDelivery(Guid deliveryGuid)
        {
            using (var db = new StationContext())
                return db.OilDeliveries.Find(deliveryGuid);
        }

        public List<Oil> GetOils()
        {
            using (var db = new StationContext())
                return db.Oils.ToList();
        }

        public bool Post(List<OilPrelevement> oilPrelevements, DateTime fromDate)
        {
            using (var db = new StationContext())
            {
                foreach (var prelevement in oilPrelevements)
                {
                    if (prelevement.OilPrelevementGuid == Guid.Empty) prelevement.OilPrelevementGuid = Guid.NewGuid();
                    prelevement.ActualUnitPrice = db.Oils.Find(prelevement.OilGuid).CurrentUnitPrice;

                    var lastOne = StaticGetLastPrelevement(prelevement.OilGuid);

                    prelevement.TotalSold = lastOne.TotalStock - prelevement.TotalSold + GetDeliveries(prelevement.OilGuid, lastOne.DatePrelevement.GetValueOrDefault(), fromDate);

                    if (prelevement.TotalSold < 0)
                        throw new CoolException(
                            $"{db.Oils.Find(prelevement.OilGuid).Libel}: Le Total Vendu ne Doit Pas être Négatif");

                    if (prelevement.TotalSold > StaticGetOilBalance(prelevement.OilGuid))
                        throw new CoolException(
                            $"{db.Oils.Find(prelevement.OilGuid).Libel}: Le Total Vendu ne Doit Pas être Supérieur au Stock");

                    prelevement.DatePrelevement = fromDate;
                    prelevement.DateAdded = DateTime.Now;
                    prelevement.LastEditDate = DateTime.Now;

                    db.OilPrelevements.Add(prelevement);                   
                }                                                  
                return db.SaveChanges() > 0;
            }
        }


        #endregion




        #region Views



        public int GetDeliveries(Guid oilGuid, DateTime fromDate, DateTime toDate)
        {
            using (var db = new StationContext())
                try
                {
                    return db.Oils.Find(oilGuid)?.Deliveries
                                  .Where(d => d.DeliveryDate >= fromDate && d.DeliveryDate <= toDate)
                                  .Sum(s => s.QuantityDelivered) ?? 0;
                }
                catch (Exception exception)
                {
                    DebugHelper.WriteException(exception);
                    return 0;
                }
        }


        public IEnumerable<OilPrelevCard> GetPrelevCards(List<Guid> oilsGuids, DateTime fromDate, DateTime toDate)
        {
            using (var db = new StationContext())
            {
                var prelevements = new List<OilPrelevement>();
                foreach (var oil in db.Oils.Where(f => oilsGuids.Contains(f.OilGuid)))
                        prelevements.AddRange(oil.Prelevements.Where(p => p.DatePrelevement.GetValueOrDefault().Date >= fromDate && p.DatePrelevement.GetValueOrDefault().Date <= toDate));
                return prelevements.OrderByDescending(p => p.DatePrelevement).Select(p => new OilPrelevCard(p)).ToList();               
            }
        }


        public int GetOilBalance(Guid oilGuid) => StaticGetOilBalance(oilGuid);


        public IEnumerable<OilCard> GetOilsCards()
        {
            using (var db = new StationContext())
                return db.Oils.ToList().OrderByDescending(c => c.DateAdded).Select(c => new OilCard(c)).ToList();
        }


        public List<string> GetTypes()
        {
            using (var db = new StationContext())
                return
                    db.Oils.Where(p => !string.IsNullOrEmpty(p.TypeOil))
                        .Select(p => p.TypeOil.ToLower())
                        .Distinct()
                        .ToList()
                        .Select(s => s.Titleize())
                        .ToList();
        }


        #endregion


        #region Internal Static






        internal static OilPrelevement StaticGetLastPrelevement(Guid oilGuid)
        {
            using (var db = new StationContext())
                return db.Oils.Find(oilGuid).Prelevements.OrderByDescending(p => p.DatePrelevement).FirstOrDefault() ??
                    new OilPrelevement { TotalStock = db.Oils.Find(oilGuid).InitialStock};
        }

        public IEnumerable GetOilDeliveries(Guid oilGuid)
        {
            using (var db = new StationContext())
                return db.Oils.Find(oilGuid)?.Deliveries.OrderByDescending(s => s.DateAdded).ToList().Select(s => new OilDeliveryCard(s)).ToList();
        }

        internal static int StaticGetOilBalance(Guid oilGuid)
        {
            using (var db = new StationContext())
                try
                {
                    var stocks = db.Oils.Find(oilGuid).InitialStock + db.Oils.Find(oilGuid)?.Deliveries.Sum(s => s.QuantityDelivered) ?? 0;

                    var solds = db.Oils.Find(oilGuid)?.Prelevements.Sum(p => p.TotalSold) ?? 0;

                    return (stocks - solds);
                }
                catch (Exception exception)
                {
                    DebugHelper.WriteException(exception);
                    return 0;
                }
        }

        #endregion

        
    }
}
