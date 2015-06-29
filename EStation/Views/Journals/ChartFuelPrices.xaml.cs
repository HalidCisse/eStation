using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace eStation.Views.Journals
{
    
    internal partial class FuelVariations 
    {
       
        public FuelVariations()
        {
            InitializeComponent();
        }


        public async Task Refresh(List<Guid> fuelsGuids, DateTime fromDate, DateTime toDate)
        {
            await Dispatcher.BeginInvoke(new Action(() => _BUSY_INDICATOR.IsBusy = true));
            List<eStationCore.Model.Fuel.Entity.Fuel> fuels;

            if (!fuelsGuids.Any())
            {
                fromDate = DateTime.Today.AddMonths(-11);
                toDate = DateTime.Today;
                fuels = await App.Store.Fuels.GetFuels();
            }
            else
                fuels = await App.Store.Fuels.GetFuels(fuelsGuids);

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

            foreach (var fuel in fuels)
            {
                plotModel.Series.Add(new LineSeries
                {
                    Title = fuel.Libel.ToUpper(),
                    ItemsSource =      (await App.Store.Fuels.GetPrices(fuel.FuelGuid, fromDate, toDate)).ToList(),     //Helpers.GetPoints(40,60), //    
                    DataFieldX = "Key",
                    DataFieldY = "Value",
                    LabelFormatString = "{1:C0}",
                    TrackerFormatString = "Le {2:d/MMM/yy} Prix {4:C0}",
                    TextColor = OxyColors.DimGray,
                    StrokeThickness = 2,
                    Smooth = false,
                    MarkerType = MarkerType.Circle,
                    MarkerFill = OxyColors.SteelBlue,
                    MarkerStroke = OxyColor.Parse("#FFFDFDFD"),
                    LineStyle = LineStyle.Automatic,
                });
            }

            plotModel.Axes.Add(new DateTimeAxis
            {                
                IntervalType = DateTimeIntervalType.Auto,                
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                StringFormat = "d/MMM/yy",
                MajorGridlineColor = OxyColors.Transparent,
                TicklineColor = OxyColors.WhiteSmoke,
                AxislineStyle = LineStyle.LongDash,
                AxislineThickness = 0.1,
                Position = AxisPosition.Bottom,
                TickStyle = TickStyle.Outside,
                AxislineColor = OxyColors.Gainsboro,
                IsZoomEnabled = false
            });

            plotModel.Axes.Add(new LinearAxis
            {
                MinimumPadding = 0,
                MaximumPadding = 0.06,
                AbsoluteMinimum = 0,
                AxislineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.None,
                AxislineThickness = 0.1,
                TickStyle = TickStyle.Outside,
                AxislineColor = OxyColors.Gainsboro,
                IsZoomEnabled = false
            });

            await Dispatcher.BeginInvoke(new Action(() =>
            {
                _PLOT_VIEW.Model = plotModel;
                _BUSY_INDICATOR.IsBusy = false;
            }));
        }

    }
}
