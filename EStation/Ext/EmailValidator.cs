using System.Globalization;
using System.Windows.Controls;
using eLib.Validation;

namespace eStation.Ext {
    internal class EmailValidator:ValidationRule {
      
        public override ValidationResult Validate (object value, CultureInfo cultureInfo) {

            var theString = ((string)value)?.Trim();

            if(string.IsNullOrEmpty(theString))
                return new ValidationResult(true, "Entrez un Email");

            return InputHelper.IsEmail(theString) ? new ValidationResult(true, null) :
                new ValidationResult(false, "Email non valide");

            //if (!InputHelper.IsEmail(theString)) return new ValidationResult(false, "Email non valide");
            //var staff = App.DataS.HumanResource.GetStaffByEmail(theString);
            //return staff != null ? new ValidationResult(false, "Email de " + staff.Person.FullName) : new ValidationResult(true, "Valide");
        }
    } 
}
