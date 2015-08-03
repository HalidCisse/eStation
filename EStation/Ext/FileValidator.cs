using System.Globalization;
using System.IO;
using System.Windows.Controls;
using eLib.FilesHelper;

namespace eStation.Ext {

    internal class FileValidator:ValidationRule
    {

        public string FilePath = "";

        public override ValidationResult Validate (object value, CultureInfo cultureInfo) {
         
            if(string.IsNullOrEmpty(FilePath))
                return new ValidationResult(false, "Choisir un Document");

            return File.Exists(FilePath) && FilesHelper.IsDocumentFile(FilePath) ? new ValidationResult(true, null) :
                new ValidationResult(false, "Path non valide");
        }
    }
}
