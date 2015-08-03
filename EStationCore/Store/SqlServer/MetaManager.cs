using System.Deployment.Application;
using System.Reflection;
using eLib;
using eLib.Database.Interfaces;
using eStationCore.Model.Common.Entity;
using eStationCore.Properties;

namespace eStationCore.Store.SqlServer {
    public class MetaManager : IMetaManager
    {       
        
        string IMetaManager.ProductDescription => ProductDescription;
        byte[] IMetaManager.ProductIcon => ProductIcon;
        string IMetaManager.DevelopperEmail => DevelopperEmail;
        string IMetaManager.ProductName => ProductName;        
        public IAboutCard About { get; } = new AboutCard();
        string IMetaManager.AssemblyProductVersion => AssemblyProductVersion;
        bool IMetaManager.IsBeta => IsBeta;
        string IMetaManager.CurrentVersion => CurrentVersion;
        string IMetaManager.VersionNumber => VersionNumber;
        string IMetaManager.CompanyName => CompanyName;
        string IMetaManager.DevelopperName => DevelopperName;
        string[] IMetaManager.CopyrightLicence => CopyrightLicence;
        int IMetaManager.CopyrightStartYear => CopyrightStartYear;






        public static string ProductName { get; } = "eStation";
        public static string ProductDescription { get; } = "Logiciel de Gestion de Station Service";
        public static byte[] ProductIcon { get; } = Resources.mainicon.ToByteArray();
        public static string DevelopperEmail { get; } = "HalidCisse@gmail.com";
        public static string AssemblyProductVersion {
            get {
                var attributes = Assembly.GetExecutingAssembly()
                    .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false);
                return attributes.Length==0 ?
                    "" :
                    ((AssemblyInformationalVersionAttribute)attributes[0]).InformationalVersion;
            }
        }
        public static bool IsBeta { get; } = true;
        public static string CurrentVersion { get; } = ApplicationDeployment.IsNetworkDeployed
            ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
            : Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static string VersionNumber { get; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static string CompanyName { get; } = "Matrix Technology";        
        public static string DevelopperName { get; } = "Halidou Cisse";
        public static string[] CopyrightLicence { get; } = {
            "Licensed under the Apache License, Version 2.0 (the \"License\");",
            "you may not use this file except in compliance with the License.",
            "You may obtain a copy of the License at",
            string.Empty,
            "    http://www.apache.org/licenses/LICENSE-2.0",
            string.Empty,
            "Unless required by applicable law or agreed to in writing, software",
            "distributed under the License is distributed on an \"AS IS\" BASIS,",
            "WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.",
            "See the License for the specific language governing permissions and",
            "limitations under the License."
        };
        public static int CopyrightStartYear { get; } = 2015;
       
    }
}
