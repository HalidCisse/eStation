using System.Globalization;
using System.Windows.Controls;

namespace EStation.Ext {
    internal class NotNullValidator:ValidationRule
    {
        public string ErrorMessage { get; set; } = "Selectionner";

        public override ValidationResult Validate (object value, CultureInfo cultureInfo) => value == null ? new ValidationResult(false, ErrorMessage ) : new ValidationResult(true, null);
    }
}
