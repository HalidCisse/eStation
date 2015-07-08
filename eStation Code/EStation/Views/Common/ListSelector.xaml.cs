using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace eStation.Views.Common
{
    /// <summary>
    /// Interaction logic for ListSelector.xaml
    /// </summary>
    public partial class ListSelector
    {
        private bool _shouldFire;
        /// <summary>
        /// Content the Key/Value to iterate over
        /// </summary>
        public Dictionary<string, string> DataDictionary
        {            
            set
            {
                if (value == null) return;                
                _THE_COMBO_BOX.ItemsSource = null;
                _THE_COMBO_BOX.ItemsSource = value;
                _shouldFire = false;
                _THE_COMBO_BOX.SelectedIndex = 0;
                _THE_LABEL.Content = _THE_COMBO_BOX.Text;
            }
        } 

        /// <summary>
        /// Fire When New Value is Selected
        /// </summary>
        public event EventHandler OnSelectionChanged;

        /// <summary>
        /// Content the Current Selceted Value
        /// </summary>
        public string SelectedValue
        {
            get { return _THE_COMBO_BOX.SelectedValue?.ToString(); }
            set
            {
                _THE_COMBO_BOX.SelectedValue = value;
                _THE_LABEL.Content = _THE_COMBO_BOX.Text;
            }
        }

        /// <summary>
        /// A Control For Iterating A Dictionary
        /// </summary>
        public ListSelector()
        {
            InitializeComponent();                      
        }
        
        private void BackwardButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_THE_COMBO_BOX.SelectedIndex < 1) return;

            _THE_COMBO_BOX.SelectedIndex = _THE_COMBO_BOX.SelectedIndex - 1;
            _THE_LABEL.Content = _THE_COMBO_BOX.Text;
        }

        private void ForwardButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_THE_COMBO_BOX.SelectedIndex + 1 >= _THE_COMBO_BOX.Items.Count) return;

            _THE_COMBO_BOX.SelectedIndex = _THE_COMBO_BOX.SelectedIndex + 1;
            _THE_LABEL.Content = _THE_COMBO_BOX.Text;
        }

        private void TheComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {                        
            if (!_shouldFire) { _shouldFire = true;}
            else OnSelectionChanged?.Invoke(SelectedValue, e);

            _BACKWARD_BUTTON.IsEnabled = _THE_COMBO_BOX.SelectedIndex > 0;
            _FORWARD_BUTTON.IsEnabled = (_THE_COMBO_BOX.SelectedIndex + 1) < _THE_COMBO_BOX.Items.Count;           
        }


       
    }
}
