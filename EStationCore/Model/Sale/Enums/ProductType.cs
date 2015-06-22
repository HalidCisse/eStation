using System.ComponentModel;

namespace EStationCore.Model.Sale.Enums
{

    public enum ProductType
    {
        [Description("Carburant")]
        Fuel,

        [Description("Huile")]
        Oil,

        [Description("Service")]
        Service

    }
}
