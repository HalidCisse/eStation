using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using EStationCore.Model;
using EStationCore.Model.Fuel.Entity;
using EStationCore.Model.Fuel.Views;


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


        #endregion





        #region HELPERS


        public IEnumerable<ColonneCard> GetColonnesCard()
        {
            using (var db = new StationContext())
            {
                var cardList = new ConcurrentBag<ColonneCard>();

                var nd = new ColonneCard("");
                if (nd.Pompes.Any()) { cardList.Add(nd); }

                var cols = (db.Pompes.Where(s => !s.IsDeleted).ToList()
                    .Where(s => !string.IsNullOrEmpty(s.Colonne))
                    .Select(s => s.Colonne)).Distinct().ToList();

                Parallel.ForEach(cols, dep => cardList.Add(new ColonneCard(dep)));

                return cardList.Any() ? cardList.OrderBy(d => d.Libel).ToList() : null;
            }
        }



        #endregion














    }
}
