﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CLib;
using EStation.Views.Fuel;
using EStationCore.Model.Fuel.Entity;
using EStationCore.Model.Fuel.Views;

namespace EStation.Views.FuelViews
{
    internal partial class CarburantView 
    {

        public event EventHandler SelectionChanged;


        private List<Guid> SelectedFuels => new List<Guid>(_CARBURANTS.SelectedItems.Cast<FuelCard>().Select(c => c.FuelGuid));


        public CarburantView()
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(new Action(async () => await Refresh()));
        }

       
        internal async Task Refresh() 
            => await Dispatcher.BeginInvoke(new Action(async ()=>
            {
                _CARBURANTS.ItemsSource = await App.Store.Fuels.GetFuelCards();
                _CARBURANTS.SelectAll();
            }));


        private async void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var wind = new AddFuel(Guid.Empty){ Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            await Refresh();
        }

        private void CARBURANTS_OnSelectionChanged(object sender, SelectionChangedEventArgs e) 
            => SelectionChanged?.Invoke(SelectedFuels, EventArgs.Empty);

        private void CARBURANTS_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_CARBURANTS.SelectedValue == null) return;
           
            var wind = new AddFuel((Guid) _CARBURANTS.SelectedValue) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            Refresh();
        }

        private void ChangeLitrage(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)e.Source;
            var menu = (ContextMenu)menuItem.Parent;
            var key =((FuelCard) (((DockPanel)menu.PlacementTarget).DataContext)).FuelGuid;

            var wind = new AddPrice(key) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            Refresh();
        }

        private void PriceBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                var card = ((FuelCard)((TextBox) sender).DataContext);

                App.Store.Pompes.Post(new Price
                {
                    ActualPrice = Convert.ToDouble(((TextBox) sender).Text),
                    ProductGuid = card.FuelGuid,
                    FromDate = DateTime.Now
                });
            }
            catch (Exception exception)
            {
                DebugHelper.WriteException(exception);
            }
            Refresh();
        }

    }
}
