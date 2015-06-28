using System;
using System.Threading.Tasks;
using System.Windows;

namespace eStation.Views.Common {
    
    internal partial class StaffPicker {

        internal Guid SelectedStaffGuid ;

        public StaffPicker (string title) {
            InitializeComponent();

            _TITLE_TEXT.Text = title;          
            new Task(() => Dispatcher.BeginInvoke(new Action(() => {
                _STAFF.ItemsSource=App.Store.HumanResource.GetAllStaffs();
                _STAFF.SelectedIndex = 0;                
            }))).Start();            
        }
      
        private void _ENREGISTRER_OnClick(object sender, RoutedEventArgs e)
        {
            if (_STAFF.SelectedValue == null) return;
           
            SelectedStaffGuid = (Guid)_STAFF.SelectedValue;
            Close();
        }

    }
}
