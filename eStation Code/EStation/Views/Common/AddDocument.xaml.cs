using System;
using System.IO;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CLib;
using CLib.Enums;
using CLib.FilesHelper;
using eStationCore.Model.Common.Entity;
using FirstFloor.ModernUI.Windows.Controls;

namespace eStation.Views.Common {
   

    public partial class AddDocument
    {
        private readonly Guid _personGuid;
        private int _errors;
        private string _documentPath;

        public AddDocument (Guid personGuid) {
            _personGuid = personGuid;
            InitializeComponent();

            if (personGuid == Guid.Empty)
            {
                ModernDialog.ShowMessage("Person Reference Invalid", "ERREUR", MessageBoxButton.OK);
                Close();
            }
            _GRID.DataContext = new Document();
        }


        private void save_Executed (object sender, ExecutedRoutedEventArgs e) {
            try
            {
                var newDoc = new Document
                {
                   DocumentName =_FILE_PATH.Text, //Path.GetFileName(_documentPath),
                   Description  =_DESC.Text,
                   PersonGuid   =_personGuid,
                   DataBytes    = File.ReadAllBytes(_documentPath),
                   DocumentGuid = Guid.NewGuid(),
                   FileType     =(Path.GetExtension(_documentPath)?.Substring(1, 1).ToUpper() + Path.GetExtension(_documentPath)?.Substring(2).ToLower()).ToEnum<DocumentType>()
                };

                App.Store.Documents.SaveDocument(newDoc);

            } catch (SecurityException) {
                ModernDialog.ShowMessage("Permission Refusée", "ERREUR", MessageBoxButton.OK);
                e.Handled=true;
                return;
            } catch (Exception ex) {
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
                return;
            }
            ModernDialog.ShowMessage("Enregistrer avec Success !", "ESchool",
                MessageBoxButton.OK);
            e.Handled=true;
            Close();
        }


        private void Save_CanExecute (object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute=_errors==0;
            e.Handled=true;
        }


        private void Validation_Error (object sender, ValidationErrorEventArgs e) {
            if(e.Action==ValidationErrorEventAction.Added)
                _errors++;
            else
                _errors--;
        }


        private void Annuler_Click (object sender, RoutedEventArgs e) => Close();


        private void OpenFile_Click (object sender, RoutedEventArgs e)
        {
            if (!FilesHelper.BrowseDocuments("Choisir un Document", _FILE_PATH)) return;
            _documentPath=_FILE_PATH.Text;
            _FILE_VALIDATOR.FilePath=_documentPath;
            _FILE_PATH.Text=Path.GetFileName(_documentPath);
        }


    }
}
