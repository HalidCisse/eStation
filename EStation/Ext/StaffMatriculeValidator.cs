using System.Globalization;
using System.Windows.Controls;

namespace eStation.Ext
{
    internal class StaffMatriculeValidator : ValidationRule
    {
        public bool IsAdd { get; set; } = true;

        /// <summary>
        /// When overridden in a derived class, performs validation checks on a value.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Windows.Controls.ValidationResult"/> object.
        /// </returns>
        /// <param name="value">The value from the binding target to check.</param><param name="cultureInfo">The culture to use in this rule.</param>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!IsAdd) return new ValidationResult(true, null);
            
            var theString = ((string)value)?.Trim();
            if(string.IsNullOrEmpty(theString)||theString.Length<3)
                return new ValidationResult(false, "Min 3 caractères");

            var theMan = App.Store.HumanResource.GetStaff(theString);
            return theMan!=null ? new ValidationResult(false, theMan.Person.FullName+" dispose ce matricule") :
                new ValidationResult(true, null);
        }
    }
}
