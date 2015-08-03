using System;
using System.Globalization;
using eLib;
using eStationCore.Model.Sale.Entity;
using eStationCore.Properties;
using static eLib.ImagesHelper;

namespace eStationCore.Model.Hr.Views {


    /// <summary>
    /// Transaction avec ses details
    /// </summary>
    public class TransactionCard {

        /// <summary>
        /// transaction
        /// </summary>
        public TransactionCard (Transaction transaction)
        {
            TransactionGuid       = transaction.TransactionGuid;
            Designation           = transaction.Designation;
            TransactionReference  ="Ref: " + transaction.TransactionReference;
            PaymentMethodeString  = "Par " + transaction.PaymentMethode;
            TransactionDateString = transaction.TransactionDate.GetValueOrDefault().ToShortDateString();
            Description           = transaction.Description;
            TresorereName         = ""; //todo get tresorer Name

            if (transaction.Amount > 0)
            {
                AmountString=transaction.Amount.ToString("0.##", CultureInfo.CurrentCulture)+" dhs";
                UpDownImage=Resources.Down.ToByteArray();
            }
            else
            {
                AmountString= (-transaction.Amount).ToString("0.##", CultureInfo.CurrentCulture)+" dhs";
                UpDownImage=Resources.Up.ToByteArray();
            }           
        }


        /// <summary>
        /// TransactionGuid
        /// </summary>
        public Guid TransactionGuid { get; }


        /// <summary>
        /// libelle
        /// </summary>
        public string Designation { get;}


        /// <summary>
        ///TransactionNumber
        /// </summary>
        public string TransactionReference { get; }


        /// <summary>
        /// 
        /// </summary>
        public string AmountString { get;}


        /// <summary>
        /// PaymentMethode
        /// </summary>
        public string PaymentMethodeString { get;}


        /// <summary>
        /// 
        /// </summary>
        public string TransactionDateString { get;}


        /// <summary>
        /// Description
        /// </summary>
        public string Description { get;}


        /// <summary>
        /// TresorereName
        /// </summary>
        public string TresorereName { get;}


        /// <summary>
        /// UpDownImage
        /// </summary>
        public byte[] UpDownImage { get; }



    }
}
