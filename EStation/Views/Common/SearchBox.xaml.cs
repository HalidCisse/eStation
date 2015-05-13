using System;
using System.Windows;
using System.Windows.Controls;

namespace EStation.Views.Common
{
    /// <summary>
    /// Control de Recherche Text
    /// </summary>
    public partial class SearchBox 
    {
        /// <summary>
        /// Fire when validated
        /// </summary>
        public event EventHandler OnSearch;


        /// <summary>
        /// Fire when Text Changed
        /// </summary>
        public event EventHandler TextChanged;

        /// <summary>
        /// Control de Recherche Text
        /// </summary>
        public SearchBox()
        {
            InitializeComponent();
        }


        private void _SEARCH_BUTTON_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_SEARCH_BOX.Text)) return;
          
            OnSearch?.Invoke(_SEARCH_BOX.Text, EventArgs.Empty);
        }


        private void _SEARCH_BOX_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(_SEARCH_BOX.Text)) return;

            TextChanged?.Invoke(_SEARCH_BOX.Text, EventArgs.Empty);
        }




    }
}
