using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CLib;
using EStation.Ext;
using EStationCore.Model.Oil.Entity;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace EStation.Views.Journals
{
    
    internal partial class OilVariations 
    {
       
        public OilVariations()
        {
            InitializeComponent();
        }


        public async Task Refresh(List<Guid> oilsGuids, DateTime fromDate, DateTime toDate)
        {
            List<Oil> oils;

            if (!oilsGuids.Any())
            {
                fromDate = DateTime.Today.AddMonths(-11);
                toDate = DateTime.Today;
                oils = App.Store.Oils.GetOils().ToList();
            }
            else
                oils = App.Store.Oils.GetOils(oilsGuids);

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
                PlotAreaBorderThickness = new OxyThickness(.5,0,0,.5),
                DefaultColors = new List<OxyColor>{ OxyColors.Blue, OxyColors.Red, OxyColors.Brown, OxyColors.Orange, OxyColors.DarkRed, OxyColors.Green, OxyColors.Yellow, OxyColors.YellowGreen }
            };

            foreach (var oil in oils)
            {               
               var line = new LineSeries
                {
                    Title = oil.Libel.ToUpper(),   
                    ItemsSource   = (await App.Store.Oils.GetPrices(oil.OilGuid, fromDate, toDate)),
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
               };
                //line.ItemsSource = Helpers.GetPoints();
                //data.ToList().ForEach(d => line.Points.Add(new DataPoint(DateTimeAxis.ToDouble(d.Key), d.Value)));
                //GetPoints().ToList().ForEach(d => line.Points.Add(new DataPoint(DateTimeAxis.ToDouble(d.Key), d.Value)));
                plotModel.Series.Add(line);              
            }

           
            plotModel.Axes.Add(new DateTimeAxis
            {
                //IntervalLength = 30,
                //MinorIntervalType = DateTimeIntervalType.Days,
                IntervalType = DateTimeIntervalType.Months,
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                StringFormat = "MMM/yy",
                MajorGridlineColor = OxyColors.Transparent,
                TicklineColor = OxyColors.WhiteSmoke,
                AxislineStyle = LineStyle.LongDash,
                AxislineThickness = 0.1,
                Position = AxisPosition.Bottom,
                TickStyle = TickStyle.Outside,
                AxislineColor = OxyColors.Gainsboro,
                IsZoomEnabled = false,               
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
            await Dispatcher.BeginInvoke(new Action(() => _PLOT_VIEW.Model = plotModel));
        }
    }
}
