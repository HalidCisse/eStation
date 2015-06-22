using System;
using System.Collections.Generic;
using CLib;


namespace EStation.Ext
{
    internal static class Helpers
    {
        public static List<KeyValuePair<DateTime, double>> GetPoints(int min =90, int max =100)
        {
            var points = new List<KeyValuePair<DateTime, double>>();
            var date = DateTime.Today.AddMonths(-4);

            for (var i = 1; i <= 8; i++)
                points.Add(new KeyValuePair<DateTime, double>(date.AddDays(i * 15), RandomHelper.Random(min, max)));
            return points;
        }







    }
}
