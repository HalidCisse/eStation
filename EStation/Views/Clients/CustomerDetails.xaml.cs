using System;
using System.Threading.Tasks;
using System.Windows;
using EStationCore.Model.Customer.Views;



namespace EStation.Views.Clients
{
    /// <summary>
    /// Interaction logic for CustomerDetails.xaml
    /// </summary>
    public partial class CustomerDetails 
    {
       
        private readonly CustomerCard _currentSelectedItem;
       
           
        public CustomerDetails(CustomerCard selectedItem)
        {
            _currentSelectedItem = selectedItem;
            InitializeComponent();

            _TITLE_TEXT.Text = selectedItem.FullName + " -  " + selectedItem.Company ;
        }


            
        private void Refresh() => Parallel.Invoke(
            //() => _ABSENCE_REPPORT.Refresh(_currentStudentCard.StudentGuid),
            //() => _ATTENDENCE_DETAILS.PersonGuid = _currentStudentCard.PersonGuid,

            //() => _STUDENT_SCHEDULE.Refresh(_currentStudentCard.StudentGuid),

            //() => _STUDENT_FINANCE_VIEW.StudentGuid = _currentStudentCard.StudentGuid,

            //() => _TRANSCRIPT_CARD.StudentGuid = _currentStudentCard.StudentGuid,

            //() => _STUDENT_INSCRIPTION.StudentGuid = _currentStudentCard.StudentGuid,

            //() => _STUDENT_DOCS.PersonGuid = _currentStudentCard.PersonGuid
            );

        #region EVENTS HANDLERS


        private void _MY_TAB_CONTROL_OnLoaded(object sender, RoutedEventArgs e) => Refresh();


        private void BACK_BUTTON_OnClick(object sender, RoutedEventArgs e) 
            => new Task(() => { Dispatcher.BeginInvoke(new Action(() => { NavigationService?.GoBack(); })); }).Start();


        
        #endregion


    }
}
