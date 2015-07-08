using System.Globalization;
using System.Windows.Controls;

namespace eStation.Ext
{
    internal class NameValidator : ValidationRule
    {
        public bool IsNullable { get; set; } = false;

        public int Min { get; set; } = 3;
        //"Entrez au moins " + Min + " caracteres"

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var name = ((string)value)?.Trim();

            if (IsNullable && string.IsNullOrEmpty(name))
                return new ValidationResult(true, null);
          
            if (string.IsNullOrEmpty(name) || name.Length < Min) return new ValidationResult(false, "Min " + Min + " caracteres");
            return new ValidationResult(true, null);
        }
    }
}
