using System.Globalization;
using System.Windows.Controls;
using CLib.Validation;

namespace eStation.Ext {
    internal class PhoneNumValidator:ValidationRule {
      
        public override ValidationResult Validate (object value, CultureInfo cultureInfo) {
            
            var theString = ((string)value)?.Trim();

            if (string.IsNullOrEmpty(theString))
            return new ValidationResult(true, "Entrez un Numéro");

            return InputHelper.IsValidNumber(theString) ? new ValidationResult(true, null) :
                new ValidationResult(false, "Numéro non valide");
        }
    }
}
