using System.Deployment.Application;
using System.Reflection;
using CLib;
using eStationCore.Model.Common.Entity;

namespace eStationCore.Managers {
    public class MetaManager
    {

        public static string ProductName => "eStation";

        public static string ProductDescription => "Logiciel de Gestion de Station Service";

        public static byte[] ProductIcon => ImagesHelper.ImageToByteArray(Properties.Resources.mainicon);

        public static string DevelopperEmail => "HalidCisse@gmail.com";

        public AboutCard About => new AboutCard();

        public static string AssemblyProductVersion {
            get {
                var attributes = Assembly.GetExecutingAssembly()
                    .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false);
                return attributes.Length==0 ?
                    "" :
                    ((AssemblyInformationalVersionAttribute)attributes[0]).InformationalVersion;
            }
        }

        public static bool IsBeta => true;

        public static string CurrentVersion => ApplicationDeployment.IsNetworkDeployed
            ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
            : Assembly.GetExecutingAssembly().GetName().Version.ToString();
     
        public static string VersionNumber => Assembly.GetExecutingAssembly().GetName().Version.ToString();


        /// <summary>
        /// Gets the company name.
        /// </summary>
        public static string CompanyName => "Matrix Technology";


        /// <summary>
        /// Gets the developper name.
        /// </summary>
        public static string DevelopperName => "Halidou Cisse";


        /// <summary>
        /// Gets the copyright banner.
        /// </summary>
        public static string[] CopyrightLicence => new[]
        {
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

        public static int CopyrightStartYear => 2015;
    }
}
