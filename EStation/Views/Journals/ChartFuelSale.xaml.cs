using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CLib;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace eStation.Views.Journals
{
    internal partial class ChartFuelSale 
    {
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        private bool _isFistHit = true;


        public ChartFuelSale()
        {
            InitializeComponent();
        }

        public async Task Refresh(List<Guid> fuelsGuids, DateTime fromDate, DateTime toDate)
        {           
            List<eStationCore.Model.Fuel.Entity.Fuel> fuels;

            if (_isFistHit)
            {
                fromDate = DateTime.Today.AddMonths(-11);
                toDate = DateTime.Today;
                fuels =(await App.Store.Fuels.GetFuels()).Take(4).ToList();
                _isFistHit = false;
            }
            else
            {
                if (_stopwatch.ElapsedMilliseconds < 10000 ) return;
                fuels = (await App.Store.Fuels.GetFuels(fuelsGuids)).ToList();                
            }

            var plotModel = new PlotModel
            {
                Title = "Ventes Mensuel des Carburants en litres",
                TitleColor = OxyColors.Gray,
                TitleFontSize = 15,
                TitleFontWeight = FontWeights.Normal,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0
            };

            foreach (var fuel in fuels)
            {
                var col = RandomHelper.RandomColor();               
                plotModel.Series.Add(new ColumnSeries
                {
                    ItemsSource = (await App.Store.Fuels.GetMonthlySales(new List<Guid> { fuel.FuelGuid }, fromDate, toDate)),
                    ValueField = "Value",
                    Title = fuel.Libel.ToUpper(),
                    FillColor = OxyColor.FromArgb(col.A, col.R, col.G, col.B) ,            //OxyColors.LightGreen,
                    LabelPlacement = LabelPlacement.Outside,
                    LabelFormatString = "{0:f0}",
                    TextColor = OxyColors.DimGray,
                    StrokeColor = OxyColors.White,
                    StrokeThickness = 1,
                    ColumnWidth = 5
                });
            }
            var axis =
            DateTimeHelper.EachMonth(
                new DateTime(fromDate.Year, fromDate.Month, 1),
                new DateTime(toDate.Year, toDate.Month, 1))
                .Select(month => new KeyValuePair<string, double>(month.ToString("MMM-yy"), 0)).ToList();

            plotModel.Axes.Add(new CategoryAxis
            {
                ItemsSource = axis,
                LabelField = "Key",
                AxislineStyle = LineStyle.Solid,
                AxislineThickness = 0.1,
                GapWidth = 0.15,
                Position = AxisPosition.Bottom,
                TickStyle = TickStyle.Outside,
                AxislineColor = OxyColors.Transparent,
                IsZoomEnabled = false
            });

            plotModel.Axes.Add(new LinearAxis
            {
                MinimumPadding = 0,
                MaximumPadding = 0.06,
                AbsoluteMinimum = 0,
                AxislineStyle = LineStyle.Automatic,
                AxislineThickness = 0.1,
                TickStyle = TickStyle.Outside,
                AxislineColor = OxyColors.Transparent,
                IsZoomEnabled = false
            });
            await Dispatcher.BeginInvoke(new Action(() => _PLOT_VIEW.Model = plotModel));
        }       

    }
}
