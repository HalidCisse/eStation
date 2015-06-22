using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CLib;
using EStation.Ext;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace EStation.Views.Journals
{
   
    internal partial class ChartFinance
    {
      

        public ChartFinance()
        {
            InitializeComponent();
        }


        public async Task Refresh(DateTime fromDate, DateTime toDate)
        {
            if (fromDate == default(DateTime))
            {
                fromDate = DateTime.Today.AddMonths(-11);
                toDate = DateTime.Today;
            }
            
            var plotModel = new PlotModel
            {
                TitleColor = OxyColors.Gray,
                TitleFontSize = 15,
                TitleFontWeight = FontWeights.Normal,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0,
                PlotAreaBorderColor = OxyColors.Gainsboro,
                PlotAreaBorderThickness = new OxyThickness(.5, 0, 0, .5),
                DefaultColors = new List<OxyColor> { OxyColors.Blue, OxyColors.Red, OxyColors.Brown, OxyColors.Orange, OxyColors.DarkRed, OxyColors.Green, OxyColors.Yellow, OxyColors.YellowGreen }
            };           

            plotModel.Series.Add(new ColumnSeries
            {
                ItemsSource = (await App.Store.Economat.Treasury.ExpensePerMonth()),
                ValueField = "Value",
                Title = "Depenses",
                FillColor = OxyColors.Brown,
                LabelPlacement = LabelPlacement.Outside,
                LabelFormatString = "{0:C0}",
                TextColor = OxyColors.DimGray,
                StrokeColor = OxyColors.White,
                StrokeThickness = 1,
                ColumnWidth = 10
            });

            plotModel.Series.Add(new ColumnSeries
            {
                ItemsSource = (await App.Store.Economat.Treasury.IncomePerMonth()),
                ValueField = "Value",
                Title = "Recettes",
                FillColor = OxyColors.CadetBlue,
                LabelPlacement = LabelPlacement.Outside,
                LabelFormatString = "{0:C0}",
                TextColor = OxyColors.DimGray,
                StrokeColor = OxyColors.White,
                StrokeThickness = 1,
                ColumnWidth = 10
            });

            plotModel.Series.Add(new ColumnSeries
            {
                ItemsSource = (await App.Store.Economat.Treasury.SalaryPerMonth()),
                ValueField = "Value",
                Title = "Salaires",
                FillColor = OxyColors.LightSteelBlue,
                LabelPlacement = LabelPlacement.Outside,
                LabelFormatString = "{0:C0}",
                TextColor = OxyColors.DimGray,
                StrokeColor = OxyColors.White,
                StrokeThickness = 1,
                ColumnWidth = 10
            });
           
            var netLine = new LineSeries
            {
                Title = "Net Revenue",
                LabelFormatString = "{1:C0}",
                TrackerFormatString = "Le {2:d/MMM/yy} Valeur {4:C0}",
                TextColor = OxyColors.DimGray,
                StrokeThickness = 2,
                Smooth = true,
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColors.SteelBlue,
                MarkerStroke = OxyColor.Parse("#FFFDFDFD"),
                LineStyle = LineStyle.Automatic
            };

            var i = 0;
            foreach (var net in (await App.Store.Economat.Treasury.TresoryPerMonth()))
            {
                netLine.Points.Add(new DataPoint(i, net.Value));
                i++;
            }

            plotModel.Series.Add(netLine);



            var axis = DateTimeHelper.EachMonth(
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
