using System;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using CLib;
using CLib.FilesHelper;
using FirstFloor.ModernUI.Windows.Controls;


namespace EStation.Views.Common {
    
    internal partial class PersonDocuments {
        private bool _isReadOnly;

        public bool IsReadOnly {
            set {
                _ADD_DOC.Visibility=value ? Visibility.Collapsed : Visibility.Visible;
                _isReadOnly=value;
            }
            get { return _isReadOnly; }
        }

        private Guid _personGuid;

        public Guid PersonGuid {
            set {
                new Task(() => Dispatcher.BeginInvoke(new Action(() => {
                    _personGuid=value;
                    _DOC_LIST.ItemsSource=App.EStation.Documents.GetPersonDocuments(value);                    
                }))).Start();
            }
        }

        public PersonDocuments () {
            InitializeComponent();
        }


        private void _ADD_DOC_OnClick(object sender, RoutedEventArgs e)
        {
            var wind = new AddDocument(_personGuid) { Owner=Window.GetWindow(this) };
            wind.ShowDialog();
            PersonGuid = _personGuid;
        }


        private void Telecharger_OnClick (object sender, RoutedEventArgs e)
        {
            var newThread = new Thread(DownloadDocument);
            newThread.Start();
        }


        private void DownloadDocument()
        {
            Dispatcher.BeginInvoke(new Action(() => {
                if(_DOC_LIST.SelectedValue==null)
                    return ;
                try {
                    var download = App.EStation.Documents.DownloadDocument((Guid)_DOC_LIST.SelectedValue);

                    var saveFileDialog1 = new SaveFileDialog
                    {
                        Filter           = "Document (*.pdf, *.doc, *.docx)|*.pdf; *.doc; *.docx",
                        FilterIndex      = 2,
                        RestoreDirectory = true,
                        FileName         =download.DocumentName
                    };

                    if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
                    var objFileStream = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);
                    objFileStream.Write(download.DataBytes, 0, download.DataBytes.Length);
                    objFileStream.Close();

                    FilesHelper.OpenFolderWithFile(saveFileDialog1.FileName);

                } catch (SecurityException) {
                    ModernDialog.ShowMessage("Permission Refusée", "ERREUR", MessageBoxButton.OK);
                } catch (Exception ex) {
                    DebugHelper.WriteException(ex);
                    ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
                }            
            }));         
        }


        private void Supprimer_OnClick (object sender, RoutedEventArgs e) {
            if(_DOC_LIST.SelectedValue==null) return;

            try
            {
                App.EStation.Documents.DeleteDocument((Guid) _DOC_LIST.SelectedValue);
            } catch (SecurityException) {
                ModernDialog.ShowMessage("Permission Refusée", "ERREUR", MessageBoxButton.OK);
                return;
            } catch (Exception ex) {
                DebugHelper.WriteException(ex);
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
                return;
            }
            ModernDialog.ShowMessage("Supprimer avec Success !", "ESchool",MessageBoxButton.OK);
            PersonGuid=_personGuid;
        }


    }
}
