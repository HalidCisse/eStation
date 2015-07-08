using System;
using eStationCore.Model.Hr.Enums;

namespace eStationCore.Model.Hr.Entity
{
    /// <summary>
    /// Payement des salaires
    /// </summary>
    public class Paycheck
    {

        /// <summary>
        /// Guid
        /// </summary>
        public Guid PayrollGuid { get; set; }


        /// <summary>
        /// Exp Salaire de Base
        /// </summary>
        public string Designation { get; set; }


        /// <summary>
        /// Salaire Fixe, Heures Enseignées, Heures Travailer
        /// </summary>
        public PayType PayType { get; set; }


        /// <summary>
        /// Nombre d'heures Travailer ou Enseigner
        /// </summary>
        public TimeSpan HoursWorked { get; set; }

        //public long HoursWorkedTicks { get; set; }

        //[NotMapped]
        //public TimeSpan HoursWorked
        //{
        //        get { return TimeSpan.FromTicks(HoursWorkedTicks); }
        //        set { HoursWorkedTicks = value.Ticks; }
        //}


        /// <summary>
        /// Valeur final de la Renumeration
        /// </summary>
        public double FinalPaycheck { get; set; }


        /// <summary>
        /// Numero de Reference de payement
        /// </summary>
        public string NumeroReference { get; set; }


        /// <summary>
        /// Methode de Payement (Espece, Cheque, virement ..)
        /// </summary>
        public PaymentMethode PaymentMethode { get; set; }


    }
}
