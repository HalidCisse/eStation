using System;
using System.Globalization;
using EStationCore.Model.Hr.Entity;

namespace EStationCore.Model.Common.Views
{
    /// <summary>
    /// 
    /// </summary>
    public class ViewCard
    {


        /// <summary>
        /// 
        /// </summary>
        public ViewCard()
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="salary"></param>
        public ViewCard(Salary salary)
        {
            Info1 = salary.Designation;
            Info3 = salary.Remuneration.ToString("0.##", CultureInfo.CurrentCulture) + " dhs";
        }

        /// <summary>
        /// Guid
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Info1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Info2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Info3 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Info4 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Date1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Bool1 { get; set; }


        public int Int1 { get; set; }


    }
}
