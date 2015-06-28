using eStationCore.Managers;

namespace eStationCore.Model.Common.Entity
{
    public class AboutCard
    {
        public AboutCard()
        {
            ProductName = MetaManager.ProductName;
            ProductIcon = MetaManager.ProductIcon;
            ProductDescription = MetaManager.ProductDescription;
            IsBeta = MetaManager.IsBeta;
            CurrentVersion = MetaManager.CurrentVersion + (IsBeta ? " - Beta" : "");
            DevelopperName = MetaManager.DevelopperName;
            DevelopperEmail = MetaManager.DevelopperEmail;
            CopyrightLicence = string.Join(" ", MetaManager.CopyrightLicence);
            Copyright = $"Copyright {MetaManager.CopyrightStartYear} {MetaManager.DevelopperName}";
        }

        public string ProductName { get;}

        public byte[] ProductIcon { get; }

        public string CurrentVersion { get; }

        public bool IsBeta { get; }

        public string DevelopperName { get; }

        public string DevelopperEmail { get; }

        public string ProductDescription { get; }

        public string CopyrightLicence { get;}

        public string Copyright { get; }

    }
}
