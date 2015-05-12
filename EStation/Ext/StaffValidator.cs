using System;
using System.Globalization;
using System.Windows.Controls;

namespace EStation.Ext {
    internal class StaffValidator:ValidationRule {

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public TimeSpan FromTime { get; set; }

        public TimeSpan ToTime { get; set; }

        public override ValidationResult Validate (object value, CultureInfo cultureInfo) {
            //if(value==null)
                return new ValidationResult(false, "Choisir");

            //return App.DataS.Pedagogy.Cours.IsStaffFree(((Guid)value), FromDate, ToDate, FromTime, ToTime) ?
            //    new ValidationResult(false, "Le staff n'est pas libre") : new ValidationResult(true, null);
        }
    }
}
