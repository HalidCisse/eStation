using System;
using System.Collections;
using System.Data.Entity;
using System.Linq;
using EStationCore.Model;
using EStationCore.Model.Fuel.Entity;
using EStationCore.Model.Fuel.Views;

namespace EStationCore.Managers
{
    public class CiternesManager
    {


        #region CRUD

        public bool Post(Citerne myCiternes)
        {
            using (var db = new StationContext())
            {                
                if (myCiternes.CiterneGuid == Guid.Empty) myCiternes.CiterneGuid = Guid.NewGuid();

                myCiternes.DateAdded = DateTime.Now;
                myCiternes.LastEditDate = DateTime.Now;

                db.Citernes.Add(myCiternes);
                return db.SaveChanges() > 0;
            }
        }

        public bool Put(Citerne myCiternes)
        {
            using (var db = new StationContext())
            {
                myCiternes.LastEditDate = DateTime.Now;

                db.Citernes.Attach(myCiternes);
                db.Entry(myCiternes).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
        }

        public bool Delete(Guid citerneGuid)
        {
            using (var db = new StationContext())
            {
                db.Citernes.Remove(db.Citernes.Find(citerneGuid));
                return db.SaveChanges() > 0;
            }
        }

        public Citerne Get(Guid citerneGuid)
        {
            using (var db = new StationContext())
                return db.Citernes.Find(citerneGuid);
        }


        #endregion


        public IEnumerable GetCiternesCards()
        {
            using (var db = new StationContext())
                return db.Citernes.ToList().Select(c=>new CiterneCard(c)).ToList();
        }


        public IEnumerable GetCiternes()
        {
            using (var db = new StationContext())
                return db.Citernes.ToList();
        }


        #region Internal Static

        internal static double GetCiterneStock(Guid citerneGuid)
        {
            return 333;
        }

        #endregion

    }
}
