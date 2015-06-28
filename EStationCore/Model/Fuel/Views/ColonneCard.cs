using System;
using System.Collections.Generic;
using System.Linq;

namespace eStationCore.Model.Fuel.Views
{
    public class ColonneCard
    {

        public ColonneCard(string colonne)
        {
            Libel = colonne.ToUpper();
                     
            using (var db = new StationContext())
                Pompes = string.IsNullOrEmpty(colonne)
                    ? db.Pompes.ToList().Where(s => !s.IsDeleted && string.IsNullOrEmpty(s.Colonne)).Select(p=> new PompeCard(p)).ToList()
                    : db.Pompes.ToList().Where(s => !s.IsDeleted && s.Colonne.Equals(colonne, StringComparison.InvariantCultureIgnoreCase)).Select(p=> new PompeCard(p)).ToList();
        }

       
        public string Libel { get; }
      

        public List<PompeCard> Pompes { get; set; }


    }
}
