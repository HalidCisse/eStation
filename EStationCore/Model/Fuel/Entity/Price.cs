using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStationCore.Model.Fuel.Entity
{
    public class Price
    {
        [Key]
        public Guid PriceGuid { get; set; }

        public Guid FuelGuid { get; set; }

      

        public DateTime? FromDate{ get; set; }



        [ForeignKey("FuelGuid")]
        public virtual Fuel Fuel { get; set; }

        


    }
}
