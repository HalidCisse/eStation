using System;
using CLib;
using EStationCore.Model.Sale.Entity;
using EStationCore.Model.Sale.Enums;
using EStationCore.Properties;

namespace EStationCore.Model.Sale.Views
{
    public class PurchaseCard
    {

        public PurchaseCard(Purchase purchase)
        {
            PurchaseGuid = purchase.PurchaseGuid;
            CompanyGuid = purchase.CompanyGuid;
            Company = purchase.Company.Name;
            ProductType = purchase.ProductType;
            PurchaseDate = purchase.PurchaseDate.FriendlyDateTime();
            State = purchase.PurchaseState.GetEnumDescription();
            Sum = purchase.Sum.ToString("C0");
            Comment = purchase.Comment;            
            Description = purchase.Description;
            PurchaseState = purchase.PurchaseState;

            switch (purchase.PurchaseState)
            {
                case PurchaseState.Paid:
                    YesNoImage = ImagesHelper.ImageToByteArray(Resources.yes).Result;
                    
                    break;
                case PurchaseState.UnPaid:
                    YesNoImage = ImagesHelper.ImageToByteArray(Resources.No).Result;
                    break;
                case PurchaseState.NotPaying:
                    YesNoImage = null;
                    break;
            }            
        }

        public Guid PurchaseGuid { get;  }

        public Guid CompanyGuid { get; }

        public string Company { get;  }

        public ProductType ProductType { get;  }

        public PurchaseState PurchaseState { get; }

        public string PurchaseDate { get;  }

        public string State { get;  }

        public string Sum { get;  }

        public string Description { get;  }

        public string Comment { get;  }

        public byte[] YesNoImage { get; }
    }
}
