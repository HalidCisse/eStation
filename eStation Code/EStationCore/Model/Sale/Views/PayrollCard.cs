﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Core.Managers;
using Core.Model.Hr.Entity;
using Core.Model.Sale.Enums;
using Core.Model.Shared.Views;

namespace Core.Model.Sale.Views {

    /// <summary>
    /// les donnee concernant une salaire
    /// </summary>
    public class PayrollCard {

        /// <summary>
        /// les donnee concernant une salaire
        /// </summary>
        public PayrollCard (Payroll payroll)
        {
            PayrollGuid=payroll.PayrollGuid;
            Designation=payroll.Designation;
            IsPaid = payroll.IsPaid;
            TotalSalary = 0;

            Employment employ;
            using (var db = new WinxoContext()) employ=db.Employments.Find(payroll.EmploymentGuid);

            var payrollStart = employ.SalaryRecurrence == InstallmentRecurrence.Once ? employ.StartDate.GetValueOrDefault() 
                               : payroll.PaycheckDate.GetValueOrDefault().AddMonths(-((int)employ.SalaryRecurrence));

            switch (employ.PayType)
            {
                
                //case PayType.HoursWorked:
                //    TotalSalary=(payroll.HoursWorked.TotalMinutes*(employ.HourlyPay/60));
                //    RenumerationsCards.Add(new DataCard {
                //        Info1="Heures Travaillées",
                //        Info2=payroll.HoursWorked.TotalHoursMins(),
                //        Info3=TotalSalary.ToString("0.##", CultureInfo.CurrentCulture)+" dhs"
                //    });
                //    break;
            }

            foreach (var salary in PayrollManager.StaticGetSalaries(employ.EmploymentGuid, payrollStart, payroll.PaycheckDate)) {
                RenumerationsCards.Add(new ViewCard(salary));
                TotalSalary+= salary.Remuneration;
            }

            if (payroll.PaycheckDate > DateTime.Today)
                TotalSalaryString="Estimation: "+TotalSalary.ToString("0.##", CultureInfo.CurrentCulture)+" dhs";
            else
                TotalSalaryString="Total: "+TotalSalary.ToString("0.##", CultureInfo.CurrentCulture)+" dhs";         

            if (!payroll.IsPaid) return;
            TotalSalary = payroll.FinalPaycheck;
            TotalSalaryString ="Paid: "+TotalSalary.ToString("0.##", CultureInfo.CurrentCulture)+" dhs";
            NumeroReference="Ref: "+payroll.NumeroReference;
            Observations = "Finance User ("+payroll.DatePaid.GetValueOrDefault().ToShortDateString()+")";    //+" par "+payroll.PaymentMethode.GetEnumDescription();
            YesNoColor ="Blue";
        }

        /// <summary>
        ///payrollGuid
        /// </summary>
        public Guid PayrollGuid { get; set; }

        /// <summary>
        /// Denomination
        /// </summary>
        public string Designation { get; }

        /// <summary>
        /// TotalSalary
        /// </summary>
        public string TotalSalaryString { get;  }

        /// <summary>
        /// Numero de Reference de Recue
        /// </summary>
        public string NumeroReference { get; }

        /// <summary>
        /// YesNoColor
        /// </summary>
        public string YesNoColor { get; } = "Red";
      
        /// <summary>
        /// Observations
        /// </summary>
        public string Observations { get;}

        /// <summary>
        /// YesNoColor
        /// </summary>
        public bool IsPaid { get; }

        /// <summary>
        /// Total
        /// </summary>
        public double TotalSalary { get; }

        /// <summary>
        /// 
        /// </summary>
        public List<ViewCard> RenumerationsCards { get;} = new List<ViewCard>();

    }
}
