using System.Globalization;
using System.Windows.Controls;

namespace eStation.Ext {
    internal class TransactionRefValidator:ValidationRule {
        public override ValidationResult Validate (object value, CultureInfo cultureInfo) {
            var theString = ((string)value)?.Trim();
            if(string.IsNullOrEmpty(theString)||theString.Length<3)
                return new ValidationResult(false, "Entrez au moins 3 caractères");

            return App.Store.Economat.Finance.RefTransactionExist(theString) ? new ValidationResult(false, "Cette reference exist deja") : new ValidationResult(true, null);
        }
    }
}
