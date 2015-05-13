using System;
using System.Threading.Tasks;
using System.Windows;
using EStationCore.Model.Hr.Entity;
using EStationCore.Model.Security.Enums;

namespace EStation.Views.Hr
{

    /// <summary>
    /// Les details d'un Staff
    /// </summary>
    public partial class StaffDetails
    {

        /// <summary>
        /// Les details d'un Staff
        /// </summary>
        /// <param name="staffObject"></param>
        public StaffDetails (Staff staffObject) {

            InitializeComponent();

            new Task(() => Dispatcher.BeginInvoke(new Action(() =>
            {
                _TITLE_TEXT.Text               = staffObject.Person.FullName;
                _STAFF_DOCS.PersonGuid         =staffObject.Person.PersonGuid;
                _STAFF_CARD.StaffGuid          =staffObject.StaffGuid;
                _USER_CARD.ProfileGuid         =staffObject.StaffGuid;
                _USER_USPACES.ProfileGuid      =staffObject.StaffGuid;
            }))).Start();
        }


        private void BACK_BUTTON_OnClick(object sender, RoutedEventArgs e) => new Task(() => 
        { Dispatcher.BeginInvoke(new Action(() => { NavigationService?.GoBack(); })); }).Start();


       
        private void _STAFF_CARD_OnDateSelectionChanged (object sender, EventArgs e) {
            _STAFF_EMPLOY.Refresh((Guid)sender, _STAFF_CARD.FromDate, _STAFF_CARD.ToDate);
            _PAYCHECK_LIST.Refresh((Guid)sender, _STAFF_CARD.FromDate, _STAFF_CARD.ToDate);
        }


        private void _STAFF_EMPLOY_OnEmploySelectionChanged (object sender, EventArgs e) 
            => _SALARY_LIST.Refresh(_STAFF_EMPLOY.SelectedEmployGuid);


        private void _SALARY_LIST_OnSourceChanged (object sender, EventArgs e) 
            => _PAYCHECK_LIST.Refresh(_STAFF_CARD.StaffGuid, _STAFF_CARD.FromDate, _STAFF_CARD.ToDate);


        //private void _ATTENDENCE_DETAILS_OnDateSelectionChanged(object sender, EventArgs e) 
        //    => _ATTENDENCE_TICKETS.Refresh(_ATTENDENCE_DETAILS.PersonGuid, _ATTENDENCE_DETAILS.FromDate, _ATTENDENCE_DETAILS.ToDate);

        private void _USER_USPACES_OnSpaceChanged(object sender, EventArgs e) 
            => _USER_ROLES.Refresh(_USER_USPACES.ProfileGuid, (UserSpace) sender);
    }
}
