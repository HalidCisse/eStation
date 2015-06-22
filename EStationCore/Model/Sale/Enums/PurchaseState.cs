using System.ComponentModel;

namespace EStationCore.Model.Sale.Enums
{
    public enum PurchaseState
    {

        [Description("Payer")]
        Paid,

        [Description("Non Payer")]
        UnPaid,

        [Description("Non Payable")]
        NotPaying

    }
}
